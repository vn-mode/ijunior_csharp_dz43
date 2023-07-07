using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public const int SHOW_SELLER_ITEMS = 1;
    public const int SHOW_PLAYER_ITEMS = 2;
    public const int BUY_ITEM = 3;
    public const int EXIT = 4;

    public static void Main()
    {
        Player player = new Player(1000);
        Seller seller = new Seller(500);

        Item item1 = new Item("Книга");
        Item item2 = new Item("Мяч");

        seller.AddItem(item1);
        seller.AddItem(item2);

        while (true)
        {
            Console.Write($"1. Товары продавца 2. Ваши товары 3. Купить товар 4. Выход: ");

            bool isNumber = int.TryParse(Console.ReadLine(), out int choice);

            if (!isNumber)
            {
                Console.WriteLine(Messages.InvalidChoice);
                continue;
            }

            switch (choice)
            {
                case SHOW_SELLER_ITEMS:
                    seller.ShowItems();
                    break;

                case SHOW_PLAYER_ITEMS:
                    player.ShowItems();
                    break;

                case BUY_ITEM:
                    Console.Write(Messages.EnterItemName);
                    string itemName = Console.ReadLine();
                    Console.Write(Messages.EnterItemPrice);
                    bool isDecimal = decimal.TryParse(Console.ReadLine(), out decimal price);

                    if (!isDecimal)
                    {
                        Console.WriteLine(Messages.InvalidPriceInput);
                        break;
                    }
                    Item itemToBuy = seller.GetItemByName(itemName);

                    if (itemToBuy != null)
                    {
                        player.BuyItem(seller, itemToBuy, price);
                    }
                    else
                    {
                        Console.WriteLine(string.Format(Messages.SellerDoesNotHaveItemInput, itemName));
                    }
                    break;

                case EXIT:
                    return;

                default:
                    Console.WriteLine(Messages.InvalidChoice);
                    break;
            }
        }
    }
}

public abstract class Trader
{
    protected List<Item> _items;
    protected decimal _money;

    public Trader()
    {
        _items = new List<Item>();
    }

    public void ShowItems()
    {
        Console.Write(Messages.ShowItems);

        if (_items.Count == 0)
        {
            Console.WriteLine(Messages.NoItems);
        }
        else
        {
            Console.WriteLine(string.Join(", ", _items.Select(i => i.Name)));
        }
    }

    public void AddItem(Item item)
    {
        _items.Add(item);
        Console.WriteLine(Messages.ItemAdded + item.Name);
    }
}

public class Player : Trader
{
    public void BuyItem(Seller seller, Item item, decimal price)
    {
        if (_money < price)
        {
            Console.WriteLine(Messages.InsufficientMoney);
            return;
        }

        if (seller.SellItem(this, item))
        {
            _money -= price;
        }
    }

    public Player(decimal money)
    {
        _money = money;
    }
}

public class Seller : Trader
{
    public bool SellItem(Player player, Item item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
            player.AddItem(item);
            Console.WriteLine(Messages.ItemSold + item.Name);
            return true;
        }
        else
        {
            Console.WriteLine(Messages.SellerDoesNotHaveItem + item.Name);
            return false;
        }
    }

    public Seller(decimal money)
    {
        _money = money;
    }

    public Item GetItemByName(string itemName)
    {
        return _items.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());
    }
}

public class Item
{
    public string Name { get; set; }

    public Item(string name)
    {
        Name = name;
    }
}

public class Messages
{
    public const string ShowItems = "Товары: ";
    public const string NoItems = "нет товаров.";
    public const string ItemAdded = "Товар добавлен: ";
    public const string InsufficientMoney = "У вас недостаточно денег.";
    public const string ItemSold = "Товар продан: ";
    public const string SellerDoesNotHaveItem = "Продавец не имеет такого товара: ";
    public const string InvalidChoice = "Неверный выбор. Введите число от 1 до 4.";
    public const string EnterItemName = "Введите название товара: ";
    public const string EnterItemPrice = "Введите цену товара: ";
    public const string InvalidPriceInput = "Неверный ввод. Введите числовое значение для цены.";
    public const string SellerDoesNotHaveItemInput = "Товара \"{0}\" нет у продавца.";
}

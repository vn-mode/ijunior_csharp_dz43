using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public const string CommandShowSellerItems = "1";
    public const string CommandShowPlayerItems = "2";
    public const string CommandBuyItem = "3";
    public const string CommandExit = "4";

    public static void Main()
    {
        Player player = new Player(1000);
        Seller seller = new Seller(500);
        Shop shop = new Shop(seller, player);

        Item item1 = new Item("Книга", 150);
        Item item2 = new Item("Мяч", 50);

        seller.AddItem(item1);
        seller.AddItem(item2);

        bool isRunning = true;

        while (isRunning)
        {
            Console.Write(Messages.Choices);

            string choice = Console.ReadLine();

            switch (choice)
            {
                case CommandShowSellerItems:
                    seller.ShowItems();
                    break;

                case CommandShowPlayerItems:
                    player.ShowItems();
                    break;

                case CommandBuyItem:
                    shop.PurchaseItem();
                    break;

                case CommandExit:
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine(Messages.InvalidChoice);
                    break;
            }

            Console.WriteLine($"У продавца на балансе: {seller.MoneyAmount}");
            Console.WriteLine($"У вас на балансе: {player.MoneyAmount}");
        }
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
    public const string InvalidPriceInput = "Неверный ввод. Введите числовое значение для цены.";
    public const string SellerDoesNotHaveItemInput = "Товара \"{0}\" нет у продавца.";
    public const string Choices = "1. Товары продавца 2. Ваши товары 3. Купить товар 4. Выход: ";
}

public abstract class Trader
{
    protected List<Item> Items;
    private decimal _money;

    public Trader(decimal money)
    {
        Items = new List<Item>();
        _money = money;
    }

    public decimal MoneyAmount => _money;

    public void ShowItems()
    {
        Console.Write(Messages.ShowItems);

        if (Items.Count == 0)
        {
            Console.WriteLine(Messages.NoItems);
        }
        else
        {
            foreach (Item item in Items)
            {
                Console.WriteLine($"{item.Name}, стоимость: {item.Price}");
            }
        }
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
        Console.WriteLine(Messages.ItemAdded + item.Name);
    }

    public bool TryAddMoney(decimal amount)
    {
        _money += amount;
        return true;
    }

    public bool TryRemoveMoney(decimal amount)
    {
        if (_money < amount)
        {
            Console.WriteLine(Messages.InsufficientMoney);
            return false;
        }
        else
        {
            _money -= amount;
            return true;
        }
    }
}

public class Player : Trader
{
    public Player(decimal money) : base(money) { }
}

public class Seller : Trader
{
    public Seller(decimal money) : base(money) { }

    public Item GetItemByName(string itemName)
    {
        return Items.FirstOrDefault(item => item.Name.ToLower() == itemName.ToLower());
    }

    public bool TryRemoveItem(Item item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            return true;
        }
        else
        {
            Console.WriteLine(Messages.SellerDoesNotHaveItem + item.Name);
            return false;
        }
    }
}

public class Item
{
    private string _name;
    private decimal _price;

    public string Name => _name;
    public decimal Price => _price;

    public Item(string name, decimal price)
    {
        _name = name;
        _price = price;
    }
}

public class Shop
{
    private Seller _seller;
    private Player _player;

    public Shop(Seller seller, Player player)
    {
        _seller = seller;
        _player = player;
    }

    public void PurchaseItem()
    {
        Console.Write(Messages.EnterItemName);
        string itemName = Console.ReadLine();
        Item itemToBuy = _seller.GetItemByName(itemName);

        if (itemToBuy != null && _player.TryRemoveMoney(itemToBuy.Price) && _seller.TryRemoveItem(itemToBuy))
        {
            _player.AddItem(itemToBuy);
            _seller.TryAddMoney(itemToBuy.Price);
            Console.WriteLine(Messages.ItemSold + itemToBuy.Name);
        }
        else
        {
            Console.WriteLine(string.Format(Messages.SellerDoesNotHaveItemInput, itemName));
        }
    }
}

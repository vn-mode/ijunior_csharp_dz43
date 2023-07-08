using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public const int ShowSellerItems = 1;
    public const int ShowPlayerItems = 2;
    public const int BuyItem = 3;
    public const int Exit = 4;

    public static void Main()
    {
        Player player = new Player(1000);
        Seller seller = new Seller(500);

        Item item1 = new Item("Книга", 150);
        Item item2 = new Item("Мяч", 50);

        seller.AddItem(item1);
        seller.AddItem(item2);

        while (true)
        {
            Console.Write(Messages.Choices);

            bool isNumber = int.TryParse(Console.ReadLine(), out int choice);

            if (!isNumber)
            {
                Console.WriteLine(Messages.InvalidChoice);
                continue;
            }

            switch (choice)
            {
                case ShowSellerItems:
                    seller.ShowItems();
                    break;

                case ShowPlayerItems:
                    player.ShowItems();
                    break;

                case BuyItem:
                    Console.Write(Messages.EnterItemName);
                    string itemName = Console.ReadLine();
                    Item itemToBuy = seller.GetItemByName(itemName);

                    if (itemToBuy != null)
                    {
                        player.BuyItem(seller, itemToBuy);
                    }
                    else
                    {
                        Console.WriteLine(string.Format(Messages.SellerDoesNotHaveItemInput, itemName));
                    }
                    break;

                case Exit:
                    return;

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
    protected decimal Money;

    public decimal MoneyAmount => Money;

    public Trader(decimal money)
    {
        Items = new List<Item>();
        Money = money;
    }

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
}

public class Player : Trader
{
    public void BuyItem(Seller seller, Item item)
    {
        if (Money < item.Price)
        {
            Console.WriteLine(Messages.InsufficientMoney);
            return;
        }

        if (seller.SellItem(this, item))
        {
            Money -= item.Price;
        }
    }

    public Player(decimal money) : base(money) { }
}

public class Seller : Trader
{
    public bool SellItem(Player player, Item item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            player.AddItem(item);
            Console.WriteLine(Messages.ItemSold + item.Name);
            Money += item.Price;
            return true;
        }
        else
        {
            Console.WriteLine(Messages.SellerDoesNotHaveItem + item.Name);
            return false;
        }
    }

    public Seller(decimal money) : base(money) { }

    public Item GetItemByName(string itemName)
    {
        return Items.FirstOrDefault(item => item.Name.ToLower() == itemName.ToLower());
    }
}

public class Item
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Item(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

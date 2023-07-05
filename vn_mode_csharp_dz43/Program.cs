using System;
using System.Collections.Generic;

public class Player
{
    private List<Item> items;

    public Player()
    {
        items = new List<Item>();
    }

    public void ShowItems()
    {
        if (items.Count == 0)
        {
            Console.WriteLine("У вас нет товаров.");
        }
        else
        {
            Console.WriteLine("Ваши товары:");
            foreach (Item item in items)
            {
                Console.WriteLine(item.Name);
            }
        }
    }

    public void BuyItem(Seller seller, Item item)
    {
        seller.SellItem(this, item);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Console.WriteLine("Товар добавлен в ваш инвентарь: " + item.Name);
    }
}

public class Seller
{
    private List<Item> items;

    public Seller()
    {
        items = new List<Item>();
    }

    public void ShowItems()
    {
        if (items.Count == 0)
        {
            Console.WriteLine("У продавца нет товаров.");
        }
        else
        {
            Console.WriteLine("Товары продавца:");
            foreach (Item item in items)
            {
                Console.WriteLine(item.Name);
            }
        }
    }

    public void SellItem(Player player, Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            player.AddItem(item);
            Console.WriteLine("Товар продан: " + item.Name);
        }
        else
        {
            Console.WriteLine("Продавец не имеет такого товара: " + item.Name);
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Console.WriteLine("Товар добавлен продавцом: " + item.Name);
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

public class Program
{
    public static void Main()
    {
        Player player = new Player();
        Seller seller = new Seller();

        Item item1 = new Item("Книга");
        Item item2 = new Item("Мяч");

        seller.AddItem(item1);
        seller.AddItem(item2);

        Console.WriteLine("Товары продавца:");
        seller.ShowItems();

        Console.WriteLine();

        Console.WriteLine("Ваши товары:");
        player.ShowItems();

        Console.WriteLine();

        Console.WriteLine("Покупка товара...");
        player.BuyItem(seller, item1);

        Console.WriteLine();

        Console.WriteLine("Ваши товары после покупки:");
        player.ShowItems();

        Console.WriteLine();

        Console.WriteLine("Товары продавца после покупки:");
        seller.ShowItems();
    }
}

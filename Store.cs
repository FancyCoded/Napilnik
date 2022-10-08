using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Good iPhone12 = new Good("IPhone 12");
        Good iPhone11 = new Good("IPhone 11");

        Warehouse warehouse = new Warehouse();

        Shop shop = new Shop(warehouse);

        warehouse.Delive(iPhone12, 10);
        warehouse.Delive(iPhone11, 1);
        
        //Вывод всех товаров на складе с их остатком
        warehouse.DisplayStorage();

        Cart cart = shop.Cart();
        cart.Add(iPhone12, 4);
        cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

        //Вывод всех товаров в корзине
        cart.Display();
        
        Console.WriteLine(cart.Order().Paylink);
        cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары

        warehouse.DisplayStorage();
    }
}

class Cart
{
    private readonly IReadOnlyShop _shop;
    private readonly Dictionary<Good, int> _goods = new Dictionary<Good, int>();

    public IReadOnlyDictionary<Good, int> Goods => _goods;

    public Cart(IReadOnlyShop shop)
    {
        _shop = shop;
    }

    public void Add(Good good, int count)
    {
        if (_shop.Goods[good] < count)
        {
            Console.WriteLine($"На складе нет {good.Name} в количесвте - {count} шт");
            return;
        }

        if (_goods.ContainsKey(good))
            _goods[good] += count;
        else
            _goods.Add(good, count);
    }

    public Order Order() => _shop.Order(this);

    public void Display()
    {
        foreach (var good in _goods)
            Console.WriteLine($"{good.Key.Name} - {good.Value} шт");
    }
}

class Order
{
    public string Paylink { get; private set; }

    public Order()
    {
        Paylink = "paylink";
    }
}

class Warehouse : IReadOnlyWarehouse
{
    private readonly Dictionary<Good, int> _goods = new Dictionary<Good, int>();

    public IReadOnlyDictionary<Good, int> Goods => _goods;

    public void Delive(Good good, int count)
    {
        if (_goods.ContainsKey(good))
            _goods[good] += count;
        else
            _goods.Add(good, count);
    }

    public void Send(IReadOnlyDictionary<Good, int> goods)
    {
        foreach (var good in goods)
        {
            if (_goods[good.Key] > 0)
                _goods[good.Key] -= good.Value;
        }
    }

    public void DisplayStorage()
    {
        foreach (var good in _goods)
            Console.WriteLine($"{good.Key.Name} - {good.Value} шт");
    }
}

class Shop : IReadOnlyShop
{
    private readonly IReadOnlyWarehouse _warehouse;
    public IReadOnlyDictionary<Good, int> Goods => _warehouse.Goods;

    public Shop(Warehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public Order Order(Cart cart)
    {
        _warehouse.Send(cart.Goods);
        return new Order();
    }

    public Cart Cart() => new Cart(this);
}

class Cell
{
    private readonly Good _good;
    private int _count;

    public Good Good => _good;
    public int Count => _count;

    public Cell(Good good, int count)
    {
        _good = good;
        _count = count;
    }

    public void IncreaseCount(int count)
    {
        if (count < 0)
            return;

        _count += count;
    }

    public void ReduceCount(int count)
    {
        if (count < 0)
            return;

        _count -= count;
    }
}

class Good
{
    public string Name { get; private set; }

    public Good(string name)
    {
        Name = name;
    }
}

interface IReadOnlyShop
{
    IReadOnlyDictionary<Good, int> Goods { get; }
    Order Order(Cart cart);
}

interface IReadOnlyWarehouse
{
    IReadOnlyDictionary<Good, int> Goods { get; }
    void Send(IReadOnlyDictionary<Good, int> _goods);
}

using System;
using System.Text;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        IPaymentSystem paymentSystem1 = new FirstPaymentSystem();
        IPaymentSystem paymentSystem2 = new SecondPaymentSystem();
        IPaymentSystem paymentSystem3 = new ThirdPaymentSystem();
    }
}

class FirstPaymentSystem : IPaymentSystem
{
    private string _url = "pay.system1.ru/order?amount=12000RUB&hash=";

    public string Url => _url;

    public string GetPayingLink(Order order)
    {
        byte[] hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString()));

        return _url + Convert.ToBase64String(hash);
    }
}

class SecondPaymentSystem : IPaymentSystem
{
    private string _url = "order.system2.ru/pay?hash=";

    public string Url => _url;

    public string GetPayingLink(Order order)
    {
        byte[] hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString()));

        return _url + Convert.ToBase64String(hash) + order.Amount;
    }
}

class ThirdPaymentSystem : IPaymentSystem
{
    private string _url = "system3.com/pay?amount=12000&curency=RUB&hash=";
    private string _secretKey = "DSfcCeycbvxdfcfaDfcaS";

    public string Url => _url;

    public string GetPayingLink(Order order)
    {
        byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(order.Amount.ToString()));

        return _url + Convert.ToBase64String(hash) + order.Id + _secretKey;
    }
}

class Order
{
    public readonly int Id;
    public readonly int Amount;

    public Order(int id, int amount) => (Id, Amount) = (id, amount);
}

interface IPaymentSystem
{
    public string GetPayingLink(Order order);
    public string Url { get; }
}

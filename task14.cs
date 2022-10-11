using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<int, IPaymentSystem> paymentSystems = new Dictionary<int, IPaymentSystem>();

        PaymentSystemFabric paymentSystemFabric = new PaymentSystemFabric();
        IPaymentHandler paymentHandler = new PaymentHandler();

        PaymentSystem qiwi = paymentSystemFabric.Create(1, "QIWI");
        PaymentSystem webMoney = paymentSystemFabric.Create(2, "WebMoney");
        PaymentSystem card = paymentSystemFabric.Create(3, "Card");

        paymentSystems.Add(qiwi.SystemId, qiwi);
        paymentSystems.Add(webMoney.SystemId, webMoney);
        paymentSystems.Add(card.SystemId, card);

        var orderForm = new OrderForm(paymentSystems);
        int systemId;

        orderForm.ShowForm(out systemId);

        if (paymentSystems.ContainsKey(systemId))
            paymentHandler.ShowPaymentResult(paymentSystems[systemId]);
    }
}

public class OrderForm : IOrderForm
{
    private readonly Dictionary<int, IPaymentSystem> _paymentSystems;

    public OrderForm(Dictionary<int, IPaymentSystem> paymentSystems) =>
        _paymentSystems = paymentSystems;

    public void ShowForm(out int systemId)
    {
        Console.WriteLine("Мы принимаем: ");

        foreach (var paymentSystem in _paymentSystems)
            Console.Write($"{paymentSystem.Value.Name}\n");

        //симуляция веб интерфейса
        Console.WriteLine("Какое системой вы хотите совершить оплату?");
        systemId = Convert.ToInt32(Console.ReadLine());
    }
}

public class PaymentSystem : IPaymentSystem
{
    private readonly int _systemId;
    private readonly string _name;

    public PaymentSystem(int systemId, string name) =>
        (_systemId, _name) = (systemId, name);

    public int SystemId => _systemId;
    public string Name => _name;
}

public class PaymentHandler : IPaymentHandler
{
    public void ShowPaymentResult(IPaymentSystem paymentSystem)
    {
        Console.WriteLine($"Вы оплатили с помощью {paymentSystem.SystemId}");
        Console.WriteLine($"Проверка платежа через {paymentSystem.Name}...");
        Console.WriteLine($"Перевод на страницу {paymentSystem.Name}...");
        Console.WriteLine("Оплата прошла успешно!");
    }
}

public class PaymentSystemFabric : IPaymentSystemFabric
{
    public PaymentSystem Create(int systemId, string name) =>
        new PaymentSystem(systemId, name);
}

public interface IPaymentSystem
{
    int SystemId { get; }
    string Name { get; }
}

public interface IPaymentSystemFabric
{
    PaymentSystem Create(int systemId, string name);
}

public interface IPaymentHandler
{
    void ShowPaymentResult(IPaymentSystem paymentSystem);
}

public interface IOrderForm
{
    void ShowForm(out int systemId);
}
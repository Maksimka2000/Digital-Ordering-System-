using System.ComponentModel.Design;
using Newtonsoft.Json;

namespace DigitalOrdering;

public abstract class Order
{
    // class extent 
    // private static List<Order> _orders = new List<Order>(); // no class extent as is abstract

    // class fields
    private static int IdCounter = 0;
    private static double _service = 0.1;

    // class field setter validation
    public static double Service
    {
        get => _service * 100;
        private set
        {
            ValidateService(value);
            _service = value / 100;
        }
    }

    // fields 
    public int Id { get; }
    public double OrderPrice { get; private set; }
    public double TotalPrice { get; private set; } = 0;
    private int _numberOfPeople;
    public TimeSpan? StartTime { get; protected set; }

    // fields setter validation
    public int NumberOfPeople
    {
        get => _numberOfPeople;
        private set
        {
            ValidateInteger(value);
            _numberOfPeople = value;
        }
    }

    //constructor
    [JsonConstructor]
    protected Order(int numberOfPeople)
    {
        Id = ++IdCounter;
        OrderPrice = 0;
        NumberOfPeople = numberOfPeople;
        // CalculateTotalPrice();
    }

    // validation 
    private static void ValidateService(double value)
    {
        if (value < 0 || value > 100) throw new ArgumentException("Service must be between 0 and 100.");
    }
    private static void ValidateInteger(int value)
    {
        if (value <= 0) throw new ArgumentException("Number of people must be greater than zero.");
    }
    
    // crud
    public static void ChangeService(double service)
    {
        Service = service;
    }

    //methods
    public void CalculateTotalPrice()
    {
        TotalPrice = OrderPrice * (1 - _service);
    }
}
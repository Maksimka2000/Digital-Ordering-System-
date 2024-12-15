using Newtonsoft.Json;
using DidgitalOrdering;

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
    [JsonIgnore]
    public int Id { get; }
    [JsonIgnore]
    public double OrderPrice { get; private set; }
    [JsonIgnore]
    public double TotalPrice { get; private set; } = 0;
    protected int _numberOfPeople;
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
    protected Order(int numberOfPeople, RegisteredClient? registeredClient = null)
    {
        Id = ++IdCounter;
        OrderPrice = 0;
        NumberOfPeople = numberOfPeople;
        if(registeredClient != null) AddRegisteredClient(registeredClient); 
        // CalculateTotalPrice();
    }
    
    // association wiht registered client
    private RegisteredClient _registeredClient;
    public RegisteredClient RegisteredClient => _registeredClient;
    public void AddRegisteredClient(RegisteredClient registeredClient) // public till the period when order is not finiliezed.
    {
        if(registeredClient == null) throw new NullReferenceException("RegisteredClient is null in the AddRegisteredClient method");
        if(_registeredClient == registeredClient) return;
        if (_registeredClient == null)
        {
            _registeredClient = registeredClient;
            registeredClient.AddOrder(this);
        }
    }
    
    // association with attribute MenuItem => OrderList => Order
    private List<OrderList> _menuItems = [];
    // association getters
    [JsonIgnore]
    public List<OrderList> MenuItems => [.._menuItems];
    // association methods
    public void AddMenuItem(MenuItem menuItem, int quantity = 1)
    {
        if(quantity <= 0) throw new ArgumentException($"quantity must be greater than zero");
        if(menuItem == null) throw new ArgumentNullException($" {this}: MenuItem in AddMenuItem can't be null");
        new OrderList(menuItem, this, quantity);
    }
    public void AddOrderList(OrderList orderList)
    {
        if (orderList == null) throw new ArgumentNullException($" {this}: OrderList can't be null in AddOrderList()");
        if (orderList.Order != this) throw new ArgumentException($"You trying to add the wrong orderList to the Order in AddOrderList()");
        if (!_menuItems.Contains(orderList)) _menuItems.Add(orderList);
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



using Newtonsoft.Json;
using DidgitalOrdering;

namespace DigitalOrdering;

public abstract class Order
{
    // class extent 
    // private static List<Order> _orders = new List<Order>(); // no class extent as is abstract

    // class fields
    private static int IdCounter = 0;
    private static double _service = 10;

    // class field setter validation
    public static double Service
    {
        get => _service;
        private set
        {
            ValidateService(value);
            _service = value;
        }
    }

    // fields 
    [JsonIgnore]
    public int Id { get; }
    [JsonIgnore]
    public double OrderPrice { get; private set; } = 0.0;
    [JsonIgnore]
    public double TotalPrice { get; private set; }
    [JsonIgnore]
    public double ServicePrice { get; private set; }
    [JsonIgnore]
    public double DiscountAmount { get; private set; } = 0.0;
    
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
    
    //virtual property
    public virtual Table? Table => null;

    //constructor
    [JsonConstructor]
    protected Order(int numberOfPeople, Dictionary<MenuItem, int>? menuItemsWithQuantities = null, RegisteredClient? registeredClient = null)
    {
        Id = ++IdCounter;
        NumberOfPeople = numberOfPeople;
        if(registeredClient != null) AddRegisteredClient(registeredClient);
        
        if (menuItemsWithQuantities != null)
        {
            foreach (var entry in menuItemsWithQuantities)
            {
                var menuItem = entry.Key;
                var quantity = entry.Value;
                AddMenuItemToOrder(menuItem, quantity);
            }
        }
    }
    
    // association with registered client
    private RegisteredClient _registeredClient;
    public RegisteredClient RegisteredClient => _registeredClient;
    public void AddRegisteredClient(RegisteredClient registeredClient) // public till the period when order is not finiliezed.
    {
        if(registeredClient == null) throw new NullReferenceException("RegisteredClient is null in the AddRegisteredClient method");
        if(_registeredClient == null){
            _registeredClient = registeredClient;
            registeredClient.AddOrder(this);
        }
    }
    protected void RemoveRegisteredClient()
    {
        _registeredClient.RemoveOrder(this);
        _registeredClient = null;
    }
    
    // association with attribute MenuItem => OrderList => Order
    protected List<OrderList> _menuItems = [];
    // association getters
    [JsonIgnore]
    public List<OrderList> MenuItems => [.._menuItems];
    // association methods
    public void AddMenuItemToOrder(MenuItem menuItem, int quantity = 1)
    {
        if(quantity <= 0) throw new ArgumentException($"quantity must be greater than zero");
        if(menuItem == null) throw new ArgumentNullException($" {this}: MenuItem in AddMenuItem can't be null");
        // check if MenuItem belong to the specific restaurant the order is placed
        if(menuItem.Restaurant != this.Table.Restaurant ) throw new ArgumentException($"MenuItem you are trying to add to the order doesn't belong to the restaurant in which you made the order"); 
        // validate the day and time of the setOfMenuItem and validate if it is the OnlineOrder
        if (menuItem is SetOfMenuItem setOfMenuItem)
        {
            if (this is OnlineOrder onlineOrder) throw new ArgumentException($"MenuItem you are trying to add to the order is SetOfMenuItems with name: {setOfMenuItem.Name} and The order you created is OnlineOrder with id: {onlineOrder.Id}. You can't add setOfMenuItems to the OnlineOrder is it polices of restaurant.");
            // ValidateSetOfMenuItem(setOfMenuItem); // reveal soon if needed
        }
        new OrderList(menuItem, this, quantity);
    }
    public void AddOrderList(OrderList orderList)
    {
        if (orderList == null) throw new ArgumentNullException($" {this}: OrderList can't be null in AddOrderList()");
        if (orderList.Order != this) throw new ArgumentException($"You trying to add the wrong orderList to the Order in AddOrderList()");
        if (!_menuItems.Contains(orderList)) _menuItems.Add(orderList);
        MakeCalculationOfPrice(orderList);
    }
    public void RemoveMenuItemFromOrder(OrderList orderList)
    {
        if(orderList == null) throw new ArgumentException($" Order.cs: OrderList can't be null in RemoveOrderList()");
        if(orderList.Order != this) throw new ArgumentException($"You trying to remove the wrong orderList from the Order in RemoveOrderList()");
        if (_menuItems.Contains(orderList))
        {
            _menuItems.Remove(orderList);
            orderList.RemoveOrderList();
        }
    }
    public void DecrementQuantityInOrderList(OrderList orderList){
        if(orderList == null) throw new ArgumentException($"Order list can't be null in DecrementQuantityInOrderList() Order.cs");
        if(orderList.Order != this) throw new AggregateException($"you are trying to modify the wrong orderList from the MenuItem in AddOrderList()");
        orderList.DecrementQuantity();
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

    
    //CRUD on obj
    public abstract void RemoveOrder();
    
    // crud
    public static void ChangeService(double service)
    {
        Service = service;
    }

    //methods
    private void MakeCalculationOfPrice(OrderList orderList)
    {
        for (var quantity = 1; quantity <= orderList.Quantity; quantity++)
        {
            var priceOfMenuItem = orderList.MenuItem.Price;
            OrderPrice += priceOfMenuItem;
            if (orderList.MenuItem.Promotion != null)
            {
                DiscountAmount += priceOfMenuItem * (orderList.MenuItem.Promotion.DiscountPercent/100);
            }
        }   
        ServicePrice = (OrderPrice - DiscountAmount) * (Service/100);
        TotalPrice =  (OrderPrice - DiscountAmount) + ServicePrice;
    }

    
}



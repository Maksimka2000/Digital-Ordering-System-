using Newtonsoft.Json;
using DidgitalOrdering;

namespace DigitalOrdering;

public abstract class Order
{
    
    // Multi-Aspect: Finilized and StandBy roles
    public enum OrderRole
    {
        StandBy,
        Finalized
    }
    
    // Finilized 
    private string? _coupon;
    public string? Coupon
    {
        get => _coupon;
        private set
        {
            // validation
            _coupon = value;
        }
    }
    private int _bonusesUsed = 0;
    public int BonusesUsed
    {
        get => _bonusesUsed;
        private set
        {
            ValidateMandatoryInteger(value);
            _bonusesUsed = value;
        }
    }
    public bool IsPayed { get; private set; }
    public TimeSpan? TimeEnd { get; private set; }
    public void PrintBill()
    {
        if(Role != OrderRole.Finalized) throw new InvalidOperationException("You cannot subtract loyalty points. Role isn't Finilized");
        Console.WriteLine("\n \n");
        Console.WriteLine("============================================");
    Console.WriteLine($"Bill for Order ID: {Id}");
    Console.WriteLine("============================================");
    Console.WriteLine($"Start Time: {StartTime}");
    Console.WriteLine($"End Time: {TimeEnd}");
    Console.WriteLine($"Number of People: {NumberOfPeople}");
    Console.WriteLine("============================================");
    Console.WriteLine("Items Ordered:");
    Console.WriteLine("--------------------------------------------");
    
    foreach (var orderList in MenuItems)
    {
        var menuItem = orderList.MenuItem;
        var quantity = orderList.Quantity;
        var itemPrice = menuItem.Price;
        var totalItemPrice = itemPrice * quantity;

        var discountPercent = menuItem.Promotion?.DiscountPercent ?? 0;
        var discountAmount = (itemPrice * discountPercent / 100) * quantity;

        Console.WriteLine($"{menuItem.Name} (x{quantity})");
        Console.WriteLine($"  Unit Price: ${itemPrice:F2}");
        Console.WriteLine($"  Discount: {discountPercent}% (-${discountAmount:F2})");
        Console.WriteLine($"  Total Price (after discount): ${totalItemPrice - discountAmount:F2}");
        Console.WriteLine("--------------------------------------------");
    }
    
    Console.WriteLine("Summary:");
    Console.WriteLine("--------------------------------------------");
    Console.WriteLine($"Subtotal (Before Discounts): ${OrderPrice:F2}");
    Console.WriteLine($"Discount Amount: -${DiscountAmount:F2}");
    Console.WriteLine($"Service Fee ({Service}%): ${ServicePrice:F2}");
    Console.WriteLine($"Total Price (Before Bonuses): ${TotalPrice + BonusesUsed:F2}");
    Console.WriteLine($"Bonuses Used: -${BonusesUsed:F2}");
    Console.WriteLine($"Total Price (After Bonuses): ${TotalPrice:F2}");
    Console.WriteLine("============================================");
    
    if (!string.IsNullOrEmpty(Coupon))
        Console.WriteLine($"Coupon Applied: {Coupon}");
    else
        Console.WriteLine("No Coupon Applied.");
    
    Console.WriteLine("============================================");
    Console.WriteLine("Thank you for dining with us!");
    Console.WriteLine("\n \n");
    }
    public void MarkAsPayed()
    {
        if (Role != OrderRole.Finalized) throw new InvalidOperationException("Order is not StandBy");
        IsPayed = true;
    }
    
    
    
    // StandBy
    public void FinalizeOrder(int bonusesUsed = 0, string? coupon = null)
    {
        //change role
        if(Role != OrderRole.StandBy) throw new InvalidOperationException("Order is not StandBy");
        Role = OrderRole.Finalized;
        
        IsPayed = false;
        TimeEnd = DateTime.Now.TimeOfDay;
        
        //coupone logic
        Coupon = coupon;
        
        if (RegisteredClient != null)
        {
            //logic of price update and loyalty points 
            if(RegisteredClient.Bonus < bonusesUsed) throw new ArgumentException("Bonuses used cant be more than Registered Client's bonuses");
            if (bonusesUsed > OrderPrice) throw new ArgumentException($"Bonuses used can't be more than OrderPrice, so maximum is {OrderPrice}");
            BonusesUsed = bonusesUsed;
            RegisteredClient.SubtractLoyaltyPoints(bonusesUsed);
            TotalPrice -= bonusesUsed;
            
            // Add order to the registered clietn if exist for the order the history and delete temporary association.
            AddFinalizedOrderToRegisteredClient(RegisteredClient);
            RemoveRegisteredClient();    
        }
        
        // after finilization of the order table becomes free and unOccupied.
        Table.MakeTableUnoccupied();
    }
    public void SendOrderToTheKitchen()
    {
        if (this is OnlineOrder onlineOrder && !onlineOrder.IsGuestsArrived) throw new InvalidOperationException("You cannot send order to the kitchen as guests haven't arrived yet");
        if (Role != OrderRole.StandBy) throw new InvalidOperationException("Order must be in the 'StandBy' state to send it to the kitchen.");
        if (!Table.IsOccupied) throw new InvalidOperationException("Table is not Occupied");
        Console.WriteLine("\n \n");
    Console.WriteLine("============================================");
    Console.WriteLine($"Order Sent to Kitchen - ID: {Id}");
    Console.WriteLine("============================================");
    Console.WriteLine($"Start Time: {StartTime}");
    Console.WriteLine("============================================");
    Console.WriteLine("Items Ordered:");
    Console.WriteLine("--------------------------------------------");

    foreach (var orderList in MenuItems)
    {
        var menuItem = orderList.MenuItem;
        var quantity = orderList.Quantity;
        Console.WriteLine($"{menuItem.Name} (x{quantity}):");
        if (menuItem is SetOfMenuItem setOfMenuItem)
        {
            foreach (var beverage in setOfMenuItem.Beverages)
            {
                Console.WriteLine($"            [{beverage.Name} (x{quantity})]");
            }
            foreach (var food in setOfMenuItem.Foods)
            {
                Console.WriteLine($"            [{food.Name} (x{quantity})]");
            }
        }
        Console.WriteLine("--------------------------------------------");
    }
    Console.WriteLine("\n \n");
    }

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
    public OrderRole Role { get; private set; }
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
        protected set
        {
            ValidateInteger(value);
            _numberOfPeople = value;
        }
    }
    
    //constructor
    [JsonConstructor]
    protected Order(int numberOfPeople, Dictionary<MenuItem, int>? menuItemsWithQuantities = null, RegisteredClient? registeredClient = null)
    {
        Id = ++IdCounter;
        NumberOfPeople = numberOfPeople;
        if(registeredClient != null) AddRegisteredClient(registeredClient);
        
        
        // MOVED OT THE TableOrder.cs and OnlineOrder.cs // because we first need the table be assigne and that this code
        // if (menuItemsWithQuantities != null)
        // {
        //     foreach (var entry in menuItemsWithQuantities)
        //     {
        //         var menuItem = entry.Key;
        //         var quantity = entry.Value;
        // AddMenuItemToOrder(menuItem, quantity);
        //     }
        // }
    }
    
    // association with Table (REVERSE)
    protected Table _table = null;
    // association getter
    public Table Table => _table;
    // association methods
    protected abstract void AddTable(Table table);
    protected abstract void RemoveTable();

 
    
    //association with RegisteredClient stores finalized orders (REVERSE)
    private RegisteredClient _registeredClientOrderHistory;
    public RegisteredClient RegisteredClientOrderHistory => _registeredClientOrderHistory;
    public void AddFinalizedOrderToRegisteredClient(RegisteredClient registeredClient)
    {
        if (Role != OrderRole.Finalized) throw new InvalidOperationException("Order has different role, need Finalized");
        if(registeredClient == null) throw new ArgumentException($" registered client: in the AddFinalizedOrderToRegisteredClient method in OnlineOrder cant be null");
        if (_registeredClientOrderHistory == null)
        {
            _registeredClientOrderHistory = registeredClient;
            registeredClient.AddFinalizedOrder(this);
        }
    }
    

    // association with registered client
    private RegisteredClient _registeredClient;
    public RegisteredClient RegisteredClient => _registeredClient;
    public void AddRegisteredClient(RegisteredClient registeredClient) // public till the period when order is not finiliezed.
    {
        if(Role != OrderRole.StandBy) throw new InvalidOperationException("Order has different role, need StandBy");
        if(registeredClient == null) throw new NullReferenceException("RegisteredClient is null in the AddRegisteredClient method");
        if(_registeredClient == null){
            _registeredClient = registeredClient;
            registeredClient.AddOrder(this);
        }
    }
    public void RemoveRegisteredClient()
    {
        if (_registeredClient != null)
        {
            var temp = _registeredClient;
            _registeredClient = null;
            temp.RemoveOrder(this);
        }
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
        MakeCalculationOfPrice(orderList); // Adjust price after adding menuItem
    }
    public void RemoveMenuItemFromOrder(OrderList orderList)
    {
        if(orderList == null) throw new ArgumentException($" Order.cs: OrderList can't be null in RemoveOrderList()");
        if(orderList.Order != this) throw new ArgumentException($"You trying to remove the wrong orderList from the Order in RemoveOrderList()");
        if (_menuItems.Contains(orderList))
        {
            AdjustPriceOnRemoval(orderList); // Adjust price after removing the menuItems
            _menuItems.Remove(orderList);
            orderList.RemoveOrderList();
        }
    }
    public void DecrementQuantityInOrderList(OrderList orderList){
        if(orderList == null) throw new ArgumentException($"Order list can't be null in DecrementQuantityInOrderList() Order.cs");
        if(orderList.Order != this) throw new AggregateException($"you are trying to modify the wrong orderList from the MenuItem in AddOrderList()");
        if (orderList.Quantity > 1)
        {
            orderList.DecrementQuantity();
            AdjustPriceOnDecrement(orderList); // Adjust price after decrementation
        }
        else
        {
            RemoveMenuItemFromOrder(orderList);
        }
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
    private void ValidateMandatoryInteger(int value)
    {
        if(value < 0) throw new ArgumentException("Value must be greater than or equal to 0.");
    }

    
    //CRUD on obj
    public abstract void RemoveOrder();
    
    // crud
    public static void UpdateServicePercentage(double service)
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
    private void AdjustPriceOnRemoval(OrderList orderList)
    {
        for (var quantity = 1; quantity <= orderList.Quantity; quantity++)
        {
            var priceOfMenuItem = orderList.MenuItem.Price;
            OrderPrice -= priceOfMenuItem;

            if (orderList.MenuItem.Promotion != null)
            {
                DiscountAmount -= priceOfMenuItem * (orderList.MenuItem.Promotion.DiscountPercent / 100);
            }
        }
        RecalculateServiceAndTotalPrice();
    }
    private void AdjustPriceOnDecrement(OrderList orderList)
    {
        var priceOfMenuItem = orderList.MenuItem.Price;
        OrderPrice -= priceOfMenuItem;

        if (orderList.MenuItem.Promotion != null)
        {
            DiscountAmount -= priceOfMenuItem * (orderList.MenuItem.Promotion.DiscountPercent / 100);
        }
        RecalculateServiceAndTotalPrice();
    }
    private void RecalculateServiceAndTotalPrice()
    {
        ServicePrice = (OrderPrice - DiscountAmount) * (Service / 100);
        TotalPrice = (OrderPrice - DiscountAmount) + ServicePrice;
    }

}



using DidgitalOrdering;
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class OnlineOrder : Order
{
    //class extent
    private static List<OnlineOrder> _onlineOrders = [];

    //fields
    private DateTime _dateAndTime;
    private string? _description;
    private TimeSpan _duration;
    public bool IsGuestsArrived { get; private set; }
    public NonRegisteredClient NonRegisteredClient { get; private set; }
    
    // fields setter validation
    public TimeSpan Duration
    {
        get => _duration;
        private set
        {
            ValidateDuration(value);
            _duration = value;
        }
    }
    public string? Description
    {
        get => _description;
        private set
        {
            ValidateStringOptional(value, "description");
            _description = value;
        }
    }
    public DateTime DateAndTime
    {
        get => _dateAndTime;
        private set
        {
            ValidateDateAndTime(value);
            _dateAndTime = value;
        }
    }
    public new TimeSpan? StartTime
    {
        get => base.StartTime;
        set
        {
            base.StartTime = value;
        }
    }

    // Constructor
    [JsonConstructor]
    public OnlineOrder(Restaurant restaurant, int numberOfPeople, DateTime dateAndTime, TimeSpan? duration = null, string? description = null, Dictionary<MenuItem, int>? menuItemsWithQuantities = null, RegisteredClient? registeredClient = null, NonRegisteredClient? nonRegisteredClient = null) : base(numberOfPeople, menuItemsWithQuantities, registeredClient)
    {
        DateAndTime = dateAndTime;
        Description = description;
        IsGuestsArrived = false;
        if (duration == null) Duration = new TimeSpan(2, 0, 0);
        else Duration = (TimeSpan)duration;
        StartTime = null;
        AddTable(restaurant);
        AddRestaurant(restaurant);
        ValidateAuthorization(registeredClient, nonRegisteredClient);
        if(nonRegisteredClient != null) NonRegisteredClient = nonRegisteredClient;
        if(registeredClient != null) AddOnlineOrderToRegisteredClient(registeredClient);
        
        
        if (menuItemsWithQuantities != null)
        {
            foreach (var entry in menuItemsWithQuantities)
            {
                var menuItem = entry.Key;
                var quantity = entry.Value;
                AddMenuItemToOrder(menuItem, quantity);
            }
        }
        
        AddOnlineOrder(this);
    }
    
    //association with Registered client stores online orders (REVERSE)
    private RegisteredClient _registeredClientMadeOnlineOrder;
    public RegisteredClient RegisteredClientMadeOnlineOrder => _registeredClientMadeOnlineOrder;
    public void AddOnlineOrderToRegisteredClient(RegisteredClient registeredClient)
    {
        if(registeredClient == null) throw new ArgumentException($" registered client: in the AddOnlineOrderToRegisteredClient method in OnlineOrder cant be null");
        if (_registeredClientMadeOnlineOrder == null)
        {
            _registeredClientMadeOnlineOrder = registeredClient;
            registeredClient.AddOnlineOrder(this);
        }
    }
    public void RemoveOnlineOrderFromRegisteredClient()
    {
        if (_registeredClientMadeOnlineOrder != null)
        {
            var temp = _registeredClientMadeOnlineOrder;
            _registeredClientMadeOnlineOrder = null;
            temp.RemoveOnlineOrder(this);
        }
    }
    
    //association with table (REVERSE)
    //association getter
    //association methods
    protected override void AddTable(Table table)
    {
        if(table == null) throw new ArgumentNullException($"Table can't be null in AddTable() while addin to the OnlineOrder");
        _table = table;
        table.AddOrder(this);
    }
    protected override void RemoveTable()
    {
        _table.RemoveOrder(this);
        _table = null;   
    }
    private static readonly Random _random = new Random();
    private void AddTable(Restaurant restaurant)
    {
        //conversion 
        var day = _dateAndTime.DayOfWeek;
        var time = _dateAndTime.TimeOfDay;
        // check restaurant open for that time.
        if(!(restaurant.IsRestaurantOpen(day, time + _duration) && restaurant.IsRestaurantOpen(day, time))) throw new ArgumentException($"restaurant {restaurant.Name} is closed for for the time of reservation make sure order start and finish time is in scopes of the open time fo restaurant. Time restaurant opened that day: {restaurant.GetOpenHour(day)}  ");
        // check tables with appropriate numb of seates
        var tables = restaurant.Tables.Where(table => table.Capacity == _numberOfPeople || table.Capacity == _numberOfPeople+1).ToList();
        if(tables.Count == 0) throw new KeyNotFoundException($"There are no available tables for such amount of people in restaurant: {_numberOfPeople}");
        // chekc available table for specific time.
        tables = tables.Where(table => table.IsAvailableForOnlineOrder(_dateAndTime, _duration)).ToList(); // list of tables available for the reservation
        if(tables.Count == 0) throw new KeyNotFoundException($"No tables available for day {day} and time {time}, everything is booked, sorry, Choose different day and time.");
        // select table randomly
        var table = tables[_random.Next(tables.Count)];
        //assign table for OnlineOrder and Table
        AddTable(table);
    }
    
    
    //association with restaurant (REVESRE)
    private Restaurant _restaurant;
    //association getter
    public Restaurant Restaurant => _restaurant;
    //association methods
    public void AddRestaurant(Restaurant restaurant)
    {
        if(restaurant is null) throw new ArgumentNullException($"Argument {nameof(restaurant)} cannot be null in  AddRestaurant()");
        if (_restaurant == null)
        {
            _restaurant = restaurant;
            restaurant.AddOnlineOrder(this);    
        }
    }
    public void RemoveRestaurant()
    {
        if (_restaurant != null)
        {
            var temp = _restaurant;
            _restaurant = null;    
            temp.RemoveOnlineOrder(this);
            
        }
    }
    
    // validations
    private void ValidateAuthorization(RegisteredClient? registeredClient, NonRegisteredClient? nonRegisteredClient)
    {
        if(!(registeredClient == null ^ nonRegisteredClient == null)) throw new ArgumentException("You cannot have both registered and non-registered clients or you can't have non of them");
    }
    private static void ValidateStringOptional(string? value, string propertyName)
    {
        if (value == string.Empty)  throw new ArgumentException($"{propertyName} cannot be empty");
    }
    private static void ValidateDuration(TimeSpan value)
    {
        if(value < new TimeSpan (1, 0, 0)) throw new ArgumentException ("Duration must be greater than 1 hour");
        if(value > new TimeSpan(6, 0, 0)) throw new ArgumentException ("Duration must be less than 6 hours");
    }
    private static void ValidateDateAndTime(DateTime value)
    {
        if( value < DateTime.Now.AddHours(3)) throw new ArgumentException("Date and time must be in the future and more than 3 hours ahead.");
        if (value > DateTime.Now.AddDays(14)) throw new ArgumentException("Restaurant don't accept reservation for more that 2 week from today");
    }
    
    
    //CRUD
    private static void AddOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder == null) throw new ArgumentException("Online order cannot be null");
        _onlineOrders.Add(onlineOrder);
    }
    public static List<OnlineOrder> GetOnlineOrders()
    {
        return [.._onlineOrders];
    }
    public override void RemoveOrder()
    {
        if (_onlineOrders.Contains(this))
        {
            // remove online order
            _onlineOrders.Remove(this);
            
            // remove association with table
            RemoveTable(); // _table = null
            
            // remove association with registered client (_registeredClientMadeOnlineOrder)
            RemoveOnlineOrderFromRegisteredClient();

            // remove association with registered client 2
            RemoveRegisteredClient();
            
            // remove association with restaurant
            RemoveRestaurant();
            
            // remove association with the MenuItem
            foreach (var orderList in _menuItems)
            {
                RemoveMenuItemFromOrder(orderList);
            }
            
        }
    }
    
    //methods
    public void UpdateOnlineOrder(DateTime? dateAndTime = null, TimeSpan? duration = null, int? numberOfPeople = null, string? description = null  )
    {
        if(dateAndTime == null) dateAndTime = _dateAndTime;
        if(duration == null) duration = _duration;
        if(numberOfPeople == null) numberOfPeople = _numberOfPeople;
        if(description == null) description = _description;
        
        DateAndTime = (DateTime)dateAndTime;
        Duration = (TimeSpan)duration;
        NumberOfPeople = (int)numberOfPeople;
        Description = description;
        
        AddTable(_restaurant);
    }
    public void MarkAsGuestsArrived()
    {
        if(Table.IsOccupied) throw new InvalidOperationException("This table is already occupied, try to change the table manualy");
        IsGuestsArrived = true;
        StartTime = new TimeSpan(DateTime.Now.Ticks);
        Table.MakeTableOccupied();
        RemoveRestaurant();
        RemoveOnlineOrderFromRegisteredClient();
    }
}
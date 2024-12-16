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
    public bool HaveGuestsArrived { get; private set; }
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
    public OnlineOrder(Restaurant restaurant, int numberOfPeople, DateTime dateAndTime, TimeSpan? duration = null, string? description = null, RegisteredClient? registeredClient = null, NonRegisteredClient? nonRegisteredClient = null) : base(numberOfPeople, registeredClient)
    {
        DateAndTime = dateAndTime;
        Description = description;
        HaveGuestsArrived = false;
        if (duration == null) Duration = new TimeSpan(2, 0, 0);
        else Duration = (TimeSpan)duration;
        StartTime = null;
        AddTable(restaurant);
        AddRestaurant(restaurant);
        AddOnlineOrder(this);
        ValidateAuthorization(registeredClient, nonRegisteredClient);
        if(nonRegisteredClient != null) NonRegisteredClient = nonRegisteredClient;
        if(registeredClient != null) AddOnlineOrderToRegisteredClient(registeredClient);
    }
    
    //association with Registered client stores online orders.
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
    
    //association with table 
    private Table _table;
    //association getter
    public override Table Table => _table;
    //association methods
    private void AddTable(Table table)
    {
        if(table == null) throw new ArgumentNullException($"Table can't be null in AddTable() while addin to the OnlineOrder");
        _table = table;
        table.AddOnlineOrder(this);
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
    
    //association with restaurant
    private Restaurant _restaurant;
    //association getter
    public Restaurant Restaurant => _restaurant;
    //association methods
    private void AddRestaurant(Restaurant restaurant)
    {
        if(restaurant is null) throw new ArgumentNullException($"Argument {nameof(restaurant)} cannot be null in  AddRestaurant()");
        _restaurant = restaurant;
        restaurant.AddOnlineOrder(this);
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
    
    //methods
    public void MarkAsGuestsArrived()
    {
        HaveGuestsArrived = true;
        StartTime = new TimeSpan(DateTime.Now.Ticks);
    }
}
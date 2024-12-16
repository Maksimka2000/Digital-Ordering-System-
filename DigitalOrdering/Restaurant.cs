using DidgitalOrdering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class Restaurant
{
    // class extent
    private static List<Restaurant> _restaurants = [];

    // static fields
    private static int IdCounter = 0;

    // fields
    public int Id { get; }
    private string _name;
    private Address _location;
    private List<OpenHour> _openHours;

    // setters validation
    public List<OpenHour> OpenHours => [.._openHours];
    public string Name
    {
        get => _name;
        private set
        {
            ValidateStringMandatory(value, "Restaurant Name");
            _name = value;
        }
    }
    
    public Address Location
    {
        get => _location.Clone();
        private set
        {
            ValidateLocation(value);
            _location = value;
        }
    }

    //constructor
    [JsonConstructor]
    public Restaurant(string name, Address location, List<OpenHour> openHours)
    {
        Id = ++IdCounter;
        Name = name;
        Location = location;
        UpdateOpenHours(openHours);
        AddRestaurant(this);
    }
    
    //association with MenuItem
    private List<MenuItem> _menu = [];
    public List<MenuItem> Menu => [.._menu];
    public void AddMenuItemToMenu(string name, double price, string description,
        Beverage.BeverageType beverageT, bool isAlcohol,
        List<Ingredient>? ingredients = null, Promotion? promotion = null, bool isAvailable = true)
    {
        new Beverage(this, name, price, description, beverageT, isAlcohol);
    }
    public void AddMenuItemToMenu(string name, double price, string description,
        Food.FoodType foodT,
        List<Ingredient>? ingredients,
        List<Food.DietaryPreferencesType>? dietaryPreference = null, Promotion? promotion = null, bool isAvailable = true)
    {
        new Food(this, name, price, description, foodT, ingredients, dietaryPreference, promotion, isAvailable);
    }
    public void AddMenuItemToMenu(string name, double price, string description, List<Food>? foods = null,
        List<Beverage>? beverages = null, List<DayOfWeek>? days = null, TimeSpan? startTime = null,
        TimeSpan? endTime = null, bool isAvailable = true)
    {
        new SetOfMenuItem(this, name, price, description, foods, beverages, days, startTime, endTime, isAvailable);
    }
    public void AddMenuItemToMenu(MenuItem menuItem)
    {
        if(menuItem == null) throw new ArgumentNullException($"Menu item is null in AddMenuItemToMenu() Restaurant");
        if(menuItem.Restaurant != this) throw new AggregateException($"Menu item you are trying to add belong to other Restaurant: {menuItem.Restaurant.Name}");
        if(!_menu.Contains(menuItem)) _menu.Add(menuItem);
    }
    
    
    // association with tables
    private List<Table> _tables = [];
    // association getter
    [JsonIgnore]
    public List<Table> Tables => [.._tables];
    // associaiton methods
    public void AddTable(int capacity, string? alias = null, string? description = null)
    {
        var table = new Table(this, capacity, description, alias);
    }
    public void AddTable(Table table)
    {
        if(table == null) throw new ArgumentNullException($"Table is null in the AddTable method");
        if(table.Restaurant != this) throw new ArgumentException($"Table can belong only to one Restaurant which is: {table.Restaurant.Name}");
        if(!_tables.Contains(table)) _tables.Add(table);     
    }
    
    
    //association with OnlineOrder 
    private List<OnlineOrder> _onlineOrders = [];
    //association getter
    public List<OnlineOrder> OnlineOrders => _onlineOrders;
    //association methods
    public void MakeOnlineOrder(int numberOfPeople, DateTime dateAndTime, TimeSpan? duration = null, string? description = null)
    {
        new OnlineOrder(this, numberOfPeople, dateAndTime, duration, description);
    }
    public void AddOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder == null) throw new ArgumentNullException($"onlineOrder is null in the AddOnlineOrder()");
        if(onlineOrder.Restaurant != this) throw new ArgumentException($"online order can belong only to one Restaurant which is: {onlineOrder.Restaurant.Name}");
        if (!_onlineOrders.Contains(onlineOrder)) _onlineOrders.Add(onlineOrder);    
    }
    
    // multi-value attribute methods
    public void UpdateOpenHours(List<OpenHour> newOpenHours)
    {
        ValidateWorkHours(newOpenHours);
        ValidateWorkHoursDays(newOpenHours);
        _openHours = newOpenHours;
    }

    // validation
    private static void ValidateStringMandatory(string value, string text)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{text} cannot be null or empty");
    }

    private static void ValidateWorkHours(List<OpenHour> workHours)
    {
        if (workHours == null) throw new ArgumentException("work hours is null");
    }

    private static void ValidateWorkHoursDays(List<OpenHour> workHours)
    {
        if (workHours.Count != 7) throw new ArgumentException("Work hours cannot be less than 7");
        if (workHours.Select(wh => wh.Day).Distinct().Count() != 7)
            throw new ArgumentException("not all days specified.");
    }

    private static void ValidateLocation(Address value)
    {
        if (value == null) throw new ArgumentNullException(" address cannot be null");
    }

    // get, delete, add, update.
    public static List<Restaurant> GetRestaurants()
    {
        return [.._restaurants];
    }

    private static void AddRestaurant(Restaurant restaurant)
    {
        _restaurants.Add(restaurant);
    }

    public void UpdateName(string newName)
    {
        Name = newName;
    }

    // methods
    public bool IsRestaurantOpen(DayOfWeek dayOfWeek, TimeSpan currentTime)
    {
        var openHoursDay = GetOpenHour(dayOfWeek);
        return openHoursDay.IsOpen && openHoursDay.OpenTime <= currentTime && openHoursDay.CloseTime >= currentTime;
    }
    public void UpdateOpenHour(DayOfWeek dayOfWeek, TimeSpan? openTime, TimeSpan? closeTime)
    {
        var day = GetOpenHour(dayOfWeek);
        day.UpdateTime(openTime, closeTime);
        Console.WriteLine($"time succesfully updated: {(day.IsOpen ? $"from {openTime} to {closeTime} on {dayOfWeek}" : $"closed on {dayOfWeek}")}");
    }
    public OpenHour GetOpenHour(DayOfWeek dayOfWeek)
    {
        var openHour = OpenHours.FirstOrDefault(openHour => openHour.Day == dayOfWeek);
        if ( openHour == null ) throw new KeyNotFoundException($"No open hours found for day {dayOfWeek}");
        return openHour;
    }
    
}

// are custom objects, Complex attributes
[Serializable]
public class OpenHour
{
    [JsonConverter(typeof(StringEnumConverter))]
    public DayOfWeek Day { get; private set; }

    public TimeSpan? OpenTime { get; private set; }
    public TimeSpan? CloseTime { get; private set; }
    public bool IsOpen { get; private set; }

    [JsonConstructor]
    public OpenHour(DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        Day = day;
        UpdateTime(openTime, closeTime);
    }

    public void UpdateTime(TimeSpan? newOpenTime, TimeSpan? newCloseTime)
    {
        if (newOpenTime.HasValue && newCloseTime.HasValue)
            ValidateTime(newOpenTime, newCloseTime);

        if (newOpenTime.HasValue ^ newCloseTime.HasValue)
            throw new ArgumentException($" both of them can be null or not null.");

        OpenTime = newOpenTime;
        CloseTime = newCloseTime;
        IsOpen = !(OpenTime == null && CloseTime == null);
    }

    private static void ValidateTime(TimeSpan? openTime, TimeSpan? closeTime)
    {
        if (openTime >= closeTime) throw new ArgumentException("Closing time must be later than opening time");
    }

    public override string ToString()
    {
        return $"{Day}: {(IsOpen ? (OpenTime + " : " + CloseTime)  : "closed" )}";
    }
}

[Serializable]
public class Address
{
    private string _street;
    private string _city;
    private string _streetNumber;

    public string Street
    {
        get => _street;
        private set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Street cannot be null or empty.");
            _street = value;
        }
    }
    public string City
    {
        get => _city;
        private set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("City cannot be null or empty.");
            _city = value;
        }
    }
    public string StreetNumber
    {
        get => _streetNumber;
        private set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Street Number cannot be null or empty.");
            _streetNumber = value;
        }
    }

    [JsonConstructor]
    public Address(string street, string city, string streetNumber)
    {
        Street = street;
        City = city;
        StreetNumber = streetNumber;
    }

    public override string ToString()
    {
        return $"adress: {Street} {StreetNumber}, {City}";
    }

    public Address Clone()
    {
        return new Address(Street, City, StreetNumber);
    }
}
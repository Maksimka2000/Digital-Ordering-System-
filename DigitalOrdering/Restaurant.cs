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

    public static void AddRestaurant(Restaurant restaurant)
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
    private OpenHour GetOpenHour(DayOfWeek dayOfWeek)
    {
        var openHour = OpenHours.FirstOrDefault(openHour => openHour.Day == dayOfWeek);
        if ( openHour == null ) throw new KeyNotFoundException($"No open hours found for day {dayOfWeek}");
        return openHour;
    }


    //  serialized and deserialized 
    public static void SaveRestaurantJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_restaurants, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File Restaurant saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving Restaurant file: {e.Message}");
        }
    }

    public static void LoadRestaurantJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(json);
                // foreach (var restaurant in restaurants) {new Restaurant(restaurant.Name, restaurant.Location, restaurant.OpeningHours);}
                Console.WriteLine($"File Restaurant loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading Restaurant file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading Restaurant file: {e.Message}");
        }
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
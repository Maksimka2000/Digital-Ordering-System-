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
    public Address Location { get; } // as assigned not changed never immutable 
    private List<OpenHour> _openHours;

    // setters validation
    public string Name
    {
        get => _name;
        private set
        {
            ValidateStringMandatory(value, "Restaurant Name");
            _name = value;
        }
    }
    public List<OpenHour> OpenHours
    {
        get => _openHours;
        private set
        {
            ValidateWorkHours(value);
            _openHours = value;
        }
    }
    
    
    //constructor
    [JsonConstructor]
    public Restaurant(string name, Address location, List<OpenHour> openHours)
    {
        Id = ++IdCounter;
        Name = name;
        Location = location;
        OpenHours = openHours;
    }
    
    // validation
    private static void ValidateStringMandatory(string value, string text)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{text} cannot be null or empty");
    }
    private static void ValidateWorkHours(List<OpenHour> workHours)
    {
        if(workHours == null) throw new ArgumentException("work hours is null");
        if (workHours == null || workHours.Count != 7) throw new ArgumentException("Work hours cannot be null or less than 7.");
        if (workHours.Select(wh => wh.Day).Distinct().Count() != 7) throw new ArgumentException("not all days specified.");
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
        Name = newName;  // Calls setter validation
    }
    
    // methods
    public bool IsRestaurantOpen(DayOfWeek dayOfWeek, TimeSpan currentTime)
    {
        var openHoursDay = OpenHours.FirstOrDefault(openHour => openHour.Day == dayOfWeek);
        return openHoursDay.IsOpen && openHoursDay.OpenTime <= currentTime && openHoursDay.CloseTime >= currentTime;
    }

    public void UpdateOpenHours(DayOfWeek dayOfWeek, TimeSpan? openTime, TimeSpan? closeTime)
    {
        var day = OpenHours.FirstOrDefault(openHour => openHour.Day == dayOfWeek);
        day.UpdateTime(openTime, closeTime);
        Console.WriteLine($"time successfull updated: {(day.IsOpen ? $"from {openTime} to {closeTime} on {dayOfWeek}" : $"closed on {dayOfWeek}" )}");
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
    public bool IsOpen {get; private set;}
    
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
        
        OpenTime = newOpenTime;
        CloseTime = newCloseTime;
        IsOpen = !(OpenTime == null && CloseTime == null);
    }
    
    private static void ValidateTime(TimeSpan? openTime, TimeSpan? closeTime)
    {
        if(openTime >= closeTime) throw new ArgumentException("Closing time must be later than opening time");
    }
}

[Serializable]
public class Address
{
    public string Street { get; } // unmodified after creation (no setter)
    public string City { get; } // unmodified after creation

    [JsonConstructor]
    public Address(string street, string city)
    {
        Street = street;
        City = city; 
    }
}
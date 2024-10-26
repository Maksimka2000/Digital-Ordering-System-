using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class Restaurant
{
    
    private static List<Restaurant> _restaurants = new List<Restaurant>();
    
    public string Name { get; set; }
    public Address Location { get; set; }
    public List<OpenHours> WorkHours { get; set; }

    
    [JsonConstructor]
    public Restaurant(string name, Address location, List<OpenHours> openHours)
    {
        ValidateStringMandatory(name, "Restaurant name");
        Name = name;
        Location = location;
        WorkHours = openHours;
    }

    public bool IsRestaurantOpen(DayOfWeek dayOfWeek, TimeSpan currentTime)
    {
        OpenHours openHours = WorkHours.FirstOrDefault(openHour => openHour.Day == dayOfWeek);
        return openHours != null && openHours.OpenTime <= currentTime && openHours.CloseTime >= currentTime;
    }
    

    // validation
    private static void ValidateStringMandatory(string value, string text)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{text} cannot be null or empty");
    }
    
   

    

    
    // get, delete, add, update.
    public static List<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>(_restaurants);
    }

    public static void AddRestaurant(Restaurant restaurant)
    {
        _restaurants.Add(restaurant);
    }
    
    
    
    // ================================================================ serialized and deserialized 
    public static void SaveRestaurantJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_restaurants, Formatting.Indented);

            File.WriteAllText(path, json);
            Console.WriteLine($"File saved successfully at {path}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving file: {e.Message}");
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
                // foreach (var restaurant in restaurants)
                // {
                //     new Restaurant(restaurant.Name, restaurant.Location, restaurant.OpeningHours);
                // }
            }
            else
            {
                throw new Exception($"Load Promotion JSON problem");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading file: {e.Message}");
        }
        
    }
    
    
}



// are custom objects, Complex attributes
[Serializable]
public class OpenHours
{
    
    [JsonConverter(typeof(StringEnumConverter))]
    public DayOfWeek Day { get; private set; }
    public TimeSpan OpenTime { get;}
    public TimeSpan CloseTime { get;}

    [JsonConstructor]
    public OpenHours(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
    {
        ValidateTime(openTime, closeTime);
        Day = day;
        OpenTime = openTime;
        CloseTime = closeTime;
    }
    

    private static void ValidateTime(TimeSpan openTime, TimeSpan closeTime)
    {
        if(openTime >= closeTime) throw new ArgumentException("Closing time must be later than opening time");
    }
}

[Serializable]
public class Address
{
    public string Street { get; }
    public string City { get; }

    [JsonConstructor]
    public Address(string street, string city)
    {
        Street = street;
        City = city; 
    }
}
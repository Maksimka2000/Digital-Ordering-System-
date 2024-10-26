using Newtonsoft.Json;

namespace DigitalOrdering;

public class Restaurant
{
    
    private static List<Restaurant> _restaurants = new List<Restaurant>();
    
    public string Name { get; set; }
    public string Location { get; set; }
    public string OpeningHours { get; set; }

    
    [System.Text.Json.Serialization.JsonConstructor]
    public Restaurant() {}
    
    public Restaurant(string name, string location, string openingHours)
    {
        Name = name;
        Location = location;
        OpeningHours = openingHours;
    }

    public static List<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>(_restaurants);
    }
    
    
    
    // ================================================================ serialized and deserialized 
    public static void SavePromotionJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_restaurants, Formatting.Indented);

            File.WriteAllText(path, json);
            Console.WriteLine($"File saved successfully at {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    public static void LoadPromotionJSON(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(json);
            foreach (var restaurant in restaurants)
            {
                new Restaurant(restaurant.Name, restaurant.Location, restaurant.OpeningHours);
            }
        }
        else
        {
            throw new Exception($"Load Promotion JSON problem");
        }
    }
    
    
}
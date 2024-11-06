using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class Food : MenuItem
{
    
    // enums
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DietaryPreferencesType
    {
        GlutenFree = 0,
        Vegan = 1,
        Vegetarian = 2,
        LactoseFree = 3,
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FoodType
    {
        Pasta = 0,
        Desert = 1,
        Snack = 2
    }

    // class extent
    private static List<Food> _foods = [];
    
    // fields
    [JsonConverter(typeof(StringEnumConverter))]
    public DietaryPreferencesType? DietaryPreference { get; private set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public FoodType FoodT { get; private set; }

    // constructor
    [JsonConstructor]
    public Food(string name, double price, string description,
        List<Ingredient>? ingredients, Promotion? promotion, FoodType foodT,
        DietaryPreferencesType? dietaryPreference = null)
        : base(name, price, description, ingredients, promotion)
    {
        DietaryPreference = dietaryPreference;
        FoodT = foodT;
    }
    
    // Get, Add, Delete, Update CRUD
    public static void AddFood(Food food)
    {
        if(food == null) throw new ArgumentException("food cannot be null");
        else _foods.Add(food);
    }
    public static List<Food> GetFoods()
    {
        return new List<Food>(_foods);
    }
    public static void DeleteFood(Food food)
    {
        _foods.Remove(food);
    }

    
    // serialized and deserialized
    public static void SaveFoodJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_foods, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File Food saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving Food file: {e.Message}");
        }
    }
    public static void LoadFoodJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _foods = JsonConvert.DeserializeObject<List<Food>>(json);
                Console.WriteLine($"File Food loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading Food file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading Food file: {e.Message}");
        }
    }
}
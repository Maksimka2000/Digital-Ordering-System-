using DigitalOrdering;
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
    private DietaryPreferencesType? _dietaryPreferences;
    public DietaryPreferencesType? DietaryPreference
    {
        get => _dietaryPreferences;
        private set
        {
            if(value != null) {ValidateDietaryPreference(value);}
            _dietaryPreferences = value;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    private FoodType _foodType;
    public FoodType FoodT
    {
        get => _foodType;
        private set
        {
            ValidateFoodType(value);
            _foodType = value;
        }
    }

    // constructor
    [JsonConstructor]
    public Food(string name, double price, string description,
        List<Ingredient>? ingredients, Promotion? promotion, FoodType foodT,
        DietaryPreferencesType? dietaryPreference = null)
        : base(name, price, description, ingredients, promotion)
    {
        FoodT = foodT;
        DietaryPreference = dietaryPreference;
    }
    
    
    // validation 
    private static void ValidateDietaryPreference (DietaryPreferencesType? dietaryPreference)
    {
        if (dietaryPreference != null && !Enum.IsDefined(typeof(DietaryPreferencesType), dietaryPreference)) throw new ArgumentException($"Invalid beverage type: {dietaryPreference}");
        
    }
    private static void ValidateFoodType(FoodType value)
    {
        if (!Enum.IsDefined(typeof(FoodType), value)) throw new ArgumentException($"Invalid food type {value}");
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
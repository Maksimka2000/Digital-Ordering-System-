using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class Beverage : MenuItem
{
    //enums
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BeverageType
    {
        Cafeteria = 0,
        Cocktails = 1,
        Drinks = 2
    }

    // class extent
    private static List<Beverage> _beverages = [];

    // fields
    public bool IsAlcohol { get; private set; }

    [JsonConverter(typeof(StringEnumConverter))]
    private BeverageType _beveragesType;

    public BeverageType BeverageT
    {
        get => _beveragesType;
        private set
        {
            ValidateBeverageType(value);
            _beveragesType = value;
        }
    }

    // constructor
    [JsonConstructor]
    public Beverage(string name, double price, string description,
        BeverageType beverageT, bool isAlcohol,
        List<Ingredient>? ingredients = null, Promotion? promotion = null) : base(name, price,
        description, ingredients, promotion)
    {
        IsAlcohol = isAlcohol;
        BeverageT = beverageT;
    }

    //validation 
    private static void ValidateBeverageType(BeverageType beverageType)
    {
        if (!Enum.IsDefined(typeof(BeverageType), beverageType))
            throw new ArgumentException($"Invalid beverage type: {beverageType}");
    }

    // Get, Add, Delete, Update
    public static void AddBeverage(Beverage beverage)
    {
        if (beverage == null) throw new ArgumentException("Game cannot be null");
        _beverages.Add(beverage);
    }

    public static List<Beverage> GetBeverages()
    {
        return [.._beverages];
    }

    public static void DeleteBeverage(Beverage beverage)
    {
        _beverages.Remove(beverage);
    }

    //  serialized and deserialized
    public static void SaveBeverageJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_beverages, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File Beverage saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving Beverage file: {e.Message}");
        }
    }

    public static void LoadBeverageJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _beverages = JsonConvert.DeserializeObject<List<Beverage>>(json);
                Console.WriteLine($"File Beverage loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading Beverage file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading Beverage file: {e.Message}");
        }
    }
}
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class SetOfMenuItem : MenuItem
{
    
    // class extent
    private static List<SetOfMenuItem> _businessLunches = new List<SetOfMenuItem>();
    
    // static fields
    private static int MaxNumberOfFood = 4;
    private static int MinNumberOfFood = 2;
    private static int MaxNumberOfBeverage = 1;
    private static int MinNumberOfBeverage = 1;
    
    // dependencies
    public List<Food> Foods { get; private set; } = new List<Food>();
    public List<Beverage> Beverages { get; private set; } = new List<Beverage>();

    // constructor
    [JsonConstructor]
    public SetOfMenuItem(string name, double price, string description, List<Food> foods, List<Beverage> beverages) : base(name, price, description)
    {
        ValidateFoodsInput(foods);
        Foods = foods;
        ValidateBeveragesInput(beverages);
        Beverages = beverages;
    }

    // validation 
    private static void  ValidateFoodsInput(List<Food> foods)
    {
        if (!(foods.Count >= MinNumberOfFood && foods.Count <= MaxNumberOfFood)) throw new ArgumentException($"The number of foods must be between {MinNumberOfFood} and {MaxNumberOfFood}");
    }
    private static void ValidateBeveragesInput(List<Beverage> beverages)
    {
        if (!(beverages.Count >= MinNumberOfBeverage && beverages.Count <= MaxNumberOfBeverage))
            throw new ArgumentException($"Bevarage should be min {MinNumberOfBeverage} and max {MaxNumberOfBeverage}");
    }
    
    // Get, Delete, Add, Update
    public static void AddBusinessLunch(SetOfMenuItem setOfMenuItem)
    {
        if(setOfMenuItem == null)throw new ArgumentException("businessLunch cannot be null");
        _businessLunches.Add(setOfMenuItem);
    }
    public static List<SetOfMenuItem> GetBusinessLunches()
    {
        return new List<SetOfMenuItem>(_businessLunches);
    }
    public static void DeleteBusinessLunch(SetOfMenuItem setOfMenuItem)
    {
        _businessLunches.Remove(setOfMenuItem);
    }
    public void UpdateFood(int index, Food newFood)
    {
        if (newFood == null) throw new ArgumentNullException($"newFood is null");
        if (index < 0 || index >= Foods.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        Foods[index] = newFood;
        ValidateFoodsInput(Foods);
    }
    
    public void UpdateBeverage(int index, Beverage newBeverage)
    {
        if (newBeverage == null) throw new ArgumentNullException($"newBeverage is null");
        if (index < 0 || index >= Beverages.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        Beverages[index] = newBeverage;
        ValidateBeveragesInput(Beverages);
    }
    

    
    // ================================================================ serialized and deserialized
    public static void SaveBusinessLunchJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_businessLunches, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File BusinessLunch saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving BusinessLunch file: {e.Message}");
        }
    }

    public static void LoadBusinessLunchJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _businessLunches = JsonConvert.DeserializeObject<List<SetOfMenuItem>>(json);
                Console.WriteLine($"File BusinessLunch loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading BusinessLunch file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading BusinessLunch file: {e.Message}");
        }
        
        
    }

    
}
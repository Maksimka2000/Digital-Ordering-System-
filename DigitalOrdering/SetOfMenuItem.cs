using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class SetOfMenuItem : MenuItem
{
    
    // class extent
    private static List<SetOfMenuItem> _businessLunches = new List<SetOfMenuItem>();
    
    // static fields
    private static int _maxNumberOfFood = 4;
    private static int _minNumberOfFood = 2;
    private static int _maxNumberOfBeverage = 1;
    private static int _minNumberOfBeverage = 1;
    
    // seeter adn getter for static fileds validation 
    public static int MaxNumberOfFood
    {
        get => _maxNumberOfFood;
        private set
        {
            ValidateIntMaxNumberOfFood(value, "MaxNumberOfFood");
            _maxNumberOfFood = value;
        }
    }
    public static int MinNumberOfFood
    {
        get => _minNumberOfFood;
        private set
        {
            ValidateIntMinNumberOfFood(value, "MinNumberOfFood");
            _minNumberOfFood = value;
        }
    }
    public static int MaxNumberOfBeverage
    {
        get => _maxNumberOfBeverage;
        private set
        {
            ValidateIntMaxNumberOfBeverage(value, "MaxNumberOfBeverage");
            _maxNumberOfBeverage = value;
        }
    }
    public static int MinNumberOfBeverage
    {
        get => _minNumberOfBeverage;
        private set
        {
            ValidateIntMinNumberOfBeverage(value, "MinNumberOfBeverage");
            _minNumberOfBeverage = value;
        }
    }

    
    
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
    private static void ValidateIntMaxNumberOfBeverage(int value, string text)
    {
        if (value < 0 || value < MinNumberOfBeverage) throw new ArgumentOutOfRangeException("The value must be not less that 0 and " + MinNumberOfBeverage);
    }
    private static void ValidateIntMinNumberOfBeverage(int value, string minnumberoffood)
    {
        if (value < 0 || value > MaxNumberOfBeverage) throw new ArgumentOutOfRangeException("The value must be not less that 0 and must be less than" + MaxNumberOfBeverage);
    }
    private static void ValidateIntMaxNumberOfFood(int value, string text)
    {
        if (value < 0 || value < MinNumberOfFood) throw new ArgumentOutOfRangeException("The value must be not less that 0 and " + MinNumberOfFood);
    }
    private static void ValidateIntMinNumberOfFood(int value, string minnumberoffood)
    {
        if (value < 0 || value > MaxNumberOfFood) throw new ArgumentOutOfRangeException("The value must be not less that 0 and must be less than" + MaxNumberOfFood);
    }
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

    public static void UpdateMaxNumberOfBeverage(int value)
    {
        MaxNumberOfBeverage = value;
    }
    public static void UpdateMinNumberOfBeverage(int value)
    {
        MinNumberOfBeverage = value;
    }
    public static void UpdateMaxNumberOfFood(int value)
    {
        MaxNumberOfFood = value;
    }
    public static void UpdateMinNumberOfFood(int value)
    {
        MinNumberOfFood = value;
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
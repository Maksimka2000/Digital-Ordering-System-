using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class BusinessLunch : MenuItem
{
    private static List<BusinessLunch> _businessLunches = new List<BusinessLunch>();
    
    public List<Food> Foods { get; } = new List<Food>();
    public List<Beverage> Beverages { get; } = new List<Beverage>();

    [JsonConstructor]
    public BusinessLunch(string name, double price, string description, bool hasChangableIngredients, List<Food> foods, List<Beverage> beverages) : base(name, price, description,
        hasChangableIngredients)
    {
        ValidateFoodsInput(foods);
        Foods = foods;
        ValidateBeveragesInput(beverages);
        Beverages = beverages;
    }

    // validation 
    private static void  ValidateFoodsInput(List<Food> foods)
    {
        if (!(foods.Count >= 2 && foods.Count <=4)) throw new ArgumentException("The number of foods must be between 2 and 4");
    }
    private static void ValidateBeveragesInput(List<Beverage> beverages)
    {
        if (beverages.Count != 1) throw new ArgumentException("Bevarage should be min and max: 1");
        
    }
    
    // Get, Delete, Add, Update
    public static void AddBusinessLunch(BusinessLunch businessLunch)
    {
        if(businessLunch == null)throw new ArgumentException("Game cannot be null");
        _businessLunches.Add(businessLunch);
    }
    public static List<BusinessLunch> GetBusinessLunches()
    {
        return new List<BusinessLunch>(_businessLunches);
    }
    public static void DeleteBusinessLunch(BusinessLunch businessLunch)
    {
        _businessLunches.Remove(businessLunch);
    }
    
    

    
    // ================================================================ serialized and deserialized
    public static void SaveBusinessLunchJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_businessLunches, Formatting.Indented);
        
            File.WriteAllText(path, json);
            Console.WriteLine($"File saved successfully at {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    public static void LoadBusinessLunchJSON(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _businessLunches = JsonConvert.DeserializeObject<List<BusinessLunch>>(json);
            // foreach (var businessLunch in businessLunches)
            // {
            //
            //     Food[] foods = new Food[3];
            //     for (int i = 0; i < businessLunch.Foods.Length; i++)
            //     {
            //         foods[i] = Food.GetFoods().FirstOrDefault(b => b.Id == businessLunch.Foods[i].Id);;
            //     }
            //     Beverage[] baverages = new Beverage[1];
            //     for (int i = 0; i < businessLunch.Beverages.Length; i++)
            //     {
            //         baverages[i] = Beverage.GetBeverages().FirstOrDefault(b => b.Id == businessLunch.Beverages[i].Id);
            //     }
            //     
            //     new BusinessLunch(businessLunch.Name, businessLunch.Price, businessLunch.Description, businessLunch.hasChangableIngredients, foods, baverages);
            //     
            // }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    
}
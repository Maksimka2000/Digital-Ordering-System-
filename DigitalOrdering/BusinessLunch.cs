using Newtonsoft.Json;

namespace DigitalOrdering;

public class BusinessLunch : MenuItem
{
    private static List<BusinessLunch> _businessLunches = new List<BusinessLunch>();

    public Food[] Foods = new Food[3];
    public Beverage[] Beverages = new Beverage[1];

    [JsonConstructor]
    public BusinessLunch():base(){}
    public BusinessLunch(string name, double price, string description, bool hasChangableIngredients, Food[] foods, Beverage[] beverages) : base(name, price, description,
        hasChangableIngredients, null, null)
    {
        Foods = validateFoodsInput(foods);
        Beverages = validateBeveragesInput(beverages);
        _businessLunches.Add(this);
    }

    private Food[] validateFoodsInput(Food[] foods)
    {
        if (foods.Length != 3) throw new NotImplementedException();
        return foods;
    }

    private Beverage[] validateBeveragesInput(Beverage[] beverages)
    {
        if (beverages.Length != 1) throw new NotImplementedException();
        return beverages;
    }

    
    public static List<BusinessLunch> GetBusinessLunches()
    {
        return new List<BusinessLunch>(_businessLunches);
    }
    
    
    public override bool Delete()
    {
        return _businessLunches.Remove(this);
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
            var businessLunches = JsonConvert.DeserializeObject<List<BusinessLunch>>(json);
            foreach (var businessLunch in businessLunches)
            {

                Food[] foods = new Food[3];
                for (int i = 0; i < businessLunch.Foods.Length; i++)
                {
                    foods[i] = Food.GetFoods().FirstOrDefault(b => b.Id == businessLunch.Foods[i].Id);;
                }
                Beverage[] baverages = new Beverage[1];
                for (int i = 0; i < businessLunch.Beverages.Length; i++)
                {
                    baverages[i] = Beverage.GetBeverages().FirstOrDefault(b => b.Id == businessLunch.Beverages[i].Id);
                }
                
                new BusinessLunch(businessLunch.Name, businessLunch.Price, businessLunch.Description, businessLunch.hasChangableIngredients, foods, baverages);
                
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class Beverage : MenuItem
{
    private static List<Beverage> _beverages = new List<Beverage>();

    public bool IsAlcohol { get;  set; }

    public enum BeverageType 
    {
        Cafeteria = 0,
        Cocktails = 1,
        Drinks = 2
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public BeverageType BeverageT { get;  set; }

    [JsonConstructor]
    public Beverage(string name, double price, string description, bool hasChangableIngredients,
        List<Ingredient>? ingredients, Promotion? promotion, bool isAlcohol, BeverageType beverageT) : base(name, price, description,
        hasChangableIngredients, ingredients, promotion)
    {
        IsAlcohol = isAlcohol;
        BeverageT = beverageT;
    }

    
    
    // Get, Add, Delete, Update
    public static void AddBeverage(Beverage beverage)
    {
        if(beverage == null) throw new ArgumentException("Game cannot be null");
        else _beverages.Add(beverage);
    }
    public static List<Beverage> GetBeverages()
    {
        return new List<Beverage>(_beverages);
    }
    public static void DeleteBeverage(Beverage beverage)
    {
        _beverages.Remove(beverage);
    }
    
    
    
    // ================================================================ serialized and deserialized
    public static void LoadBeverageJSON(string path) 
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _beverages = JsonConvert.DeserializeObject<List<Beverage>>(json);
            // foreach (var beverage in beverages)
            // {
            //     
            //     
            //     Promotion? promotion = beverage.Promotion == null ? null : Promotion.GetPromotions().Find(pro => pro.Id == beverage.Promotion.Id);
            //     Beverage.BeverageType beverageType = (Beverage.BeverageType)beverage.BeverageT;
            //     
            //     
            //     List<Ingredient?>? ingredients = new List<Ingredient>();
            //     if (beverage.Ingredients != null)
            //     {
            //         foreach (var ingredient in beverage.Ingredients)
            //         {
            //             ingredients.Add(DigitalOrdering.Ingredient.GetIngredients().FirstOrDefault(p => p.Id == ingredient.Id));
            //         }
            //         
            //         new Beverage(beverage.Name, beverage.Price, beverage.Description, beverage.hasChangableIngredients, ingredients, promotion, beverage.IsAlcohol, beverage.BeverageT);
            //     }
            //     else
            //     {
            //         new Beverage(beverage.Name, beverage.Price, beverage.Description, beverage.hasChangableIngredients, null, promotion, beverage.IsAlcohol, beverage.BeverageT);
            //     }
            // }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public static void SaveBeverageJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_beverages, Formatting.Indented);
        
            File.WriteAllText(path, json);
            Console.WriteLine($"File saved successfully at {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }
}
using Newtonsoft.Json;

namespace DigitalOrdering;

public class Food : MenuItem
{
    public enum DietaryPreferencesType
    {
        GlutenFree = 0,
        Vegan = 1,
        Vegetarian = 2,
        LactoseFree = 3,
        NoPreference = 4
    }

    public enum FoodType
    {
        Pasta = 0,
        Desert = 1,
        Snack = 2
    }


    private static List<Food> _foods = new List<Food>();
    public DietaryPreferencesType DietaryPreference { get; set; }
    public FoodType FoodT { get; set; }

    [JsonConstructor]
    public Food():base(){}
    public Food(string name, double price, string description, bool hasChangableIngredients,
        List<Ingredient>? ingredients, Promotion? promotion, DietaryPreferencesType dietaryPreference, FoodType foodT)
        : base(name, price, description, hasChangableIngredients, ingredients, promotion)
    {
        DietaryPreference = dietaryPreference;
        FoodT = foodT;
        _foods.Add(this);
    }
    
    public override bool Delete()
    {
        return _foods.Remove(this);
    }
    

    public static List<Food> GetFoods()
    {
        return new List<Food>(_foods);
    }

    
    // ================================================================ serialized and deserialized
    public static void SaveFoodJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_foods, Formatting.Indented);
        
            File.WriteAllText(path, json);
            Console.WriteLine($"File saved successfully at {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    public static void LoadFoodJSON(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var foods = JsonConvert.DeserializeObject<List<Food>>(json);
            foreach (var food in foods)
            {
                Promotion? promotion = food.Promotion == null ? null : Promotion.GetPromotions().Find(pro => pro.Id == food.Promotion.Id);
                Food.DietaryPreferencesType dietaryPreference = (Food.DietaryPreferencesType)food.DietaryPreference;
                Food.FoodType foodType = (Food.FoodType)food.FoodT;
                List<Ingredient?>? ingredients = new List<Ingredient>();
                if (food.Ingredients != null)
                {
                    foreach (var ingredient in food.Ingredients)
                    {
                        ingredients.Add(DigitalOrdering.Ingredient.GetIngredients().FirstOrDefault(p => p.Id == ingredient.Id));
                    }
                    
                    new Food(food.Name, food.Price, food.Description, food.hasChangableIngredients, ingredients, promotion, dietaryPreference, foodType);
                }
                else
                {
                    new Food(food.Name, food.Price, food.Description, food.hasChangableIngredients, null, promotion, dietaryPreference, foodType);
                }
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }


    
}
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
    private List<DietaryPreferencesType> _dietaryPreferences = [];
    private FoodType _foodType;
    
    // setters and getters
    public List<DietaryPreferencesType> DietaryPreferences => _dietaryPreferences; // don't work inf [.._dietaryPreferences] somewhy
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
        FoodType foodT,
        List<Ingredient>? ingredients,
        List<DietaryPreferencesType>? dietaryPreference = null, Promotion? promotion = null)
        : base(name, price, description, ingredients, promotion)
    {
        FoodT = foodT;
        if(dietaryPreference != null) UpdateDietaryPreferencesType(dietaryPreference);
    }

    // methods for the multi-value attribute
    public void AddDietaryPreferencesType(DietaryPreferencesType dietaryPreference)
    {
        ValidateDietaryPreference(dietaryPreference);
        if (!_dietaryPreferences.Contains(dietaryPreference))
            _dietaryPreferences.Add(dietaryPreference);
    }
    public void RemoveDietaryPreferencesType(DietaryPreferencesType dietaryPreference)
    {
        if (_dietaryPreferences.Contains(dietaryPreference))
            _dietaryPreferences.Remove(dietaryPreference);
    }
    public void UpdateDietaryPreferencesType(List<DietaryPreferencesType>? dietaryPreferences)
    {
        if (dietaryPreferences != null && dietaryPreferences.Count > 0)
        {
            if(_dietaryPreferences.Count > 0)
                foreach (DietaryPreferencesType dietaryPreference in _dietaryPreferences)
                    RemoveDietaryPreferencesType(dietaryPreference);
            else
                foreach (DietaryPreferencesType dietaryPreference in dietaryPreferences)
                    AddDietaryPreferencesType(dietaryPreference);
        } else throw new ArgumentNullException($"epmty lists and null is not allowed in UpdateDietaryPreferencesType().");
    }
    
    // associations reverse 
    private List<SetOfMenuItem> _foodInSetOfMenuItems = new();
    // associations reverse getter
    [JsonIgnore]
    public List<SetOfMenuItem> FoodInSetOfMenuItems => [.._foodInSetOfMenuItems];
    // associations reverse methods
    public void AddSetOfMenuItemsToFood(SetOfMenuItem setOfMenuItem)
    {
        if (setOfMenuItem == null) throw new ArgumentNullException($"{this.Name}. SetOfMenuItems can not be null in the AddSetOfMenuItems method.");
        if (!_foodInSetOfMenuItems.Contains(setOfMenuItem))
        {
            _foodInSetOfMenuItems.Add(setOfMenuItem);
            setOfMenuItem.AddFood(this);
        }
    }
    public void RemoveSetOfMenuItemsFromFood(SetOfMenuItem setOfMenuItem)
    {
        if (setOfMenuItem == null) throw new ArgumentNullException($"{this.Name}. SetOfMenuItems can not be null in the RemoveSetOfMenuItemFromFood method.");
        if (_foodInSetOfMenuItems.Contains(setOfMenuItem))
        {
            _foodInSetOfMenuItems.Remove(setOfMenuItem);
            setOfMenuItem.RemoveFood(this);
        }
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
    
    // methods on Object (add,remove,delete,modify)
    public static void AddFood(Food food)
    {
        if(food == null) throw new ArgumentException("food cannot be null");
        _foods.Add(food);
    }
    public static List<Food> GetFoods()
    {
        return new List<Food>(_foods);
    }
    public static void DeleteFood(Food food)
    {
        if (_foods.Contains(food))
        {
            if (food._foodInSetOfMenuItems.Count > 0)
            {
                foreach (var setOfMenuItem in food._foodInSetOfMenuItems)
                {
                    // food.RemoveSetOfMenuItemsFromFood(setOfMenuItem);
                    setOfMenuItem.RemoveFood(food);
                    Console.WriteLine($"Set of menu items named: {setOfMenuItem.Name} id: {setOfMenuItem.Id} was modified by RemoveFood. So mind of the {food.Name}  does not exist in SetOfMenuItem anymore, modify you SetOfMenuItem as soon as possible.");
                }
            }
            if (food.Ingredients.Count > 0)
            {
                foreach (var ingredient in food._ingredients)
                { 
                    // food.RemoveIngredient(ingredient);
                    ingredient.RemoveMenuItemFromIngredient(food);
                }
            }
            
            _foods.Remove(food);
        }
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
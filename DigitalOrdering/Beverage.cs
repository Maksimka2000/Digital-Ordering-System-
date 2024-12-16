using Newtonsoft.Json;


namespace DigitalOrdering;

[Serializable]
public class Beverage : MenuItem
{
    //enums
    // [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
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
    private BeverageType _beveragesType;

    //getters and setters
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
    public Beverage(Restaurant restaurant, string name, double price, string description,
        BeverageType beverageT, bool isAlcohol,
        List<Ingredient>? ingredients = null, Promotion? promotion = null, bool isAvailable = true) : base(restaurant, name, price,
        description, isAvailable, ingredients, promotion)
    {
        IsAlcohol = isAlcohol;
        BeverageT = beverageT;
        AddBeverage(this);
    }
    
    //association reverse
    private List<SetOfMenuItem> _beverageInSetOfMenuItems = [];
    //association reverse getter
    [JsonIgnore]
    public List<SetOfMenuItem> BeverageInSetOfMenuItems => [.._beverageInSetOfMenuItems];
    //association reverse methods
    public void AddSetOfMenuItemsToBeverage(SetOfMenuItem setOfMenuItem)
    {
        if(setOfMenuItem == null) throw new ArgumentNullException($"{this.Name}, SetOfMenuItem can't be null in AddBeverageInSetOfMenuItem method");
        if (!_beverageInSetOfMenuItems.Contains(setOfMenuItem))
        {
            _beverageInSetOfMenuItems.Add(setOfMenuItem);
            setOfMenuItem.AddBeverage(this);
        }
    }
    public void RemoveSetOfMenuItemsFromBeverage(SetOfMenuItem setOfMenuItem)
    {
        if(setOfMenuItem == null) throw new ArgumentNullException($"{this.Name}, SetOfMenuItem can't be null in RemoveSetOfMenuItemsFromBeverage method");
        if (_beverageInSetOfMenuItems.Contains(setOfMenuItem))
        {
            _beverageInSetOfMenuItems.Remove(setOfMenuItem);
            setOfMenuItem.RemoveBeverage(this);
        }
    }

    //validation 
    private static void ValidateBeverageType(BeverageType beverageType)
    {
        if (!Enum.IsDefined(typeof(BeverageType), beverageType))
            throw new ArgumentException($"Invalid beverage type: {beverageType}");
    }

    // methods on Object
    private static void AddBeverage(Beverage beverage)
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
        if (_beverages.Contains(beverage))
        {
            if (beverage._beverageInSetOfMenuItems.Count > 0)
            {
                foreach (var setOfMenuItem in beverage._beverageInSetOfMenuItems)
                {
                    setOfMenuItem.RemoveBeverage(beverage);
                    Console.WriteLine($"Set of menu items named: {setOfMenuItem.Name} id: {setOfMenuItem.Id} was modified by RemoveBeverage. So mind of the {beverage.Name}  does not exist in SetOfMenuItem anymore, modify you SetOfMenuItem as soon as possible.");
                }
            }
            if (beverage._ingredients.Count > 0)
            {
                foreach (var ingredient in beverage._ingredients)
                {
                    ingredient.RemoveMenuItemFromIngredient(beverage);
                }
            }
        }
        _beverages.Remove(beverage);
    }
    
}
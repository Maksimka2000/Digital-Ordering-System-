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
    public Food(Restaurant restaurant, string name, double price, string description,
        FoodType foodT,
        List<Ingredient>? ingredients,
        List<DietaryPreferencesType>? dietaryPreference = null, Promotion? promotion = null, bool isAvailable = true)
        : base(restaurant, name, price, description, isAvailable, ingredients, promotion)
    {
        FoodT = foodT;
        if(dietaryPreference != null && dietaryPreference.Count > 0) UpdateDietaryPreferencesType(dietaryPreference);
        AddFood(this);
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
    
    // associations  with setOfMenuItems  (REVERSE)
    private List<SetOfMenuItem> _foodInSetOfMenuItems = [];
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
    private static void AddFood(Food food)
    {
        if(food == null) throw new ArgumentException("food cannot be null");
        _foods.Add(food);
    }
    public static List<Food> GetFoods()
    {
        return new List<Food>(_foods);
    }
    public override void RemoveMenuItem()
    {
        if (_foods.Contains(this))
        {
            
            // verify removing association with the setOFmenuItems
            if (_foodInSetOfMenuItems.Count > 0) throw new ArgumentException($"Food Can not be deleted as it is in the SetOfMenuItems: {_foodInSetOfMenuItems.Count}, first modify the  setOfmenuItems");
                
            // verify removing association with the ingredients
            if (_ingredients.Count > 0)
            {
                foreach (var ingredient in _ingredients)
                {
                    ingredient.RemoveMenuItemFromIngredient(this);
                }
            }
             
            // check if menu item is in any other table orders or online Orders if it is in any TableOrder(Change in future to the StandBY) or in the ONlineOrders
            foreach (var orderList in _orders)
            {
                if(orderList.Order is OnlineOrder || orderList.Order is TableOrder) throw new Exception($"You can't delete orders that are on reservations or in table orders");
            }
            
            // association with attribute MenuItem => OrderList => Order
            if (_orders.Count > 0)
            {
                foreach (var order in _orders)
                {
                    RemoveOrderFromMenuItem(order);
                }
            }
            
            // remove menuItem
            _foods.Remove(this);
            
            // verify removing association with restaurant
            _restaurant.RemoveMenuItemFromMenu(this);
            RemoveRestaurant();
        }
    }
}
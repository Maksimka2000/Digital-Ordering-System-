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
        List<Ingredient>? ingredients = null, Promotion? promotion = null, bool isAvailable = true) : base(restaurant,
        name, price,
        description, isAvailable, ingredients, promotion)
    {
        IsAlcohol = isAlcohol;
        BeverageT = beverageT;
        AddBeverage(this);
    }

    //association (REVERSE)
    private List<SetOfMenuItem> _beverageInSetOfMenuItems = [];

    //association reverse getter
    [JsonIgnore] public List<SetOfMenuItem> BeverageInSetOfMenuItems => [.._beverageInSetOfMenuItems];

    //association reverse methods
    public void AddSetOfMenuItemsToBeverage(SetOfMenuItem setOfMenuItem)
    {
        if (setOfMenuItem == null)
            throw new ArgumentNullException(
                $"{this.Name}, SetOfMenuItem can't be null in AddBeverageInSetOfMenuItem method");
        if (!_beverageInSetOfMenuItems.Contains(setOfMenuItem))
        {
            _beverageInSetOfMenuItems.Add(setOfMenuItem);
            setOfMenuItem.AddBeverage(this);
        }
    }
    public void RemoveSetOfMenuItemsFromBeverage(SetOfMenuItem setOfMenuItem)
    {
        if (setOfMenuItem == null) throw new ArgumentNullException($"{this.Name}, SetOfMenuItem can't be null in RemoveSetOfMenuItemsFromBeverage method");
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

    public override void RemoveMenuItem()
    {
        if (_beverages.Contains(this))
        {
            // verify removing association with the setOFmenuItems
            if (_beverageInSetOfMenuItems.Count > 0) throw new ArgumentException($"Beverage Can be deleted as it is in the SetOfMenuItems: {_beverageInSetOfMenuItems.Count}, first modify the  setOfmenuItems");
            
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
            
            // verify removing association with restaurant
            RemoveRestaurant();
            _restaurant.RemoveMenuItemFromMenu(this);
            
            // remove menuItem
            _beverages.Remove(this);
        }
        
    }
}
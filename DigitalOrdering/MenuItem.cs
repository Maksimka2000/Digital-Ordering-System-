using Newtonsoft.Json;
using DidgitalOrdering;

namespace DigitalOrdering;

[Serializable]
public abstract class MenuItem
{
    // static fields 
    private static int IdCounter = 0;

    // fields
    [JsonIgnore]
    public int Id { get; }
    private string _name;
    private double _price;
    private string _description;
    private Promotion? _promotion;
    public bool IsAvailable { get; private set; }

    //setters and getters    
    public string Name
    {
        get => _name;
        private set
        {
            ValidateStringMandatory(value, "Name in MenuItem");
            _name = value;
        }
    }
    public double Price
    {
        get => _price;
        private set
        {
            ValidatePrice(value, "MenuItem");
            _price = value;
        }
    }
    public string Description
    {
        get => _description;
        private set
        {
            ValidateStringMandatory(value, "Description in MenuItem");
            _description = value;
        }
    }
    public Promotion? Promotion
    {
        get => _promotion;
        private set => _promotion = value;
    }

    // constructor
    [JsonConstructor]
    protected MenuItem(Restaurant restaurant, string name, double price, string description, bool isAvailable = true, List<Ingredient>? ingredients = null, Promotion? promotion = null)
    {
        Id = ++IdCounter;
        Name = name;
        Price = price;
        Description = description;
        if (ingredients != null && ingredients.Count > 0) UpdateIngredients(ingredients);
        Promotion = promotion;
        IsAvailable = isAvailable;
        AddRestaurant(restaurant);
    }
    
    //association with Restaurant
    private Restaurant _restaurant;
    public Restaurant Restaurant => _restaurant;
    private void AddRestaurant(Restaurant restaurant)
    {
        if(restaurant == null) throw new NullReferenceException("Restaurant cannot be null  in AddRestaurant() MenuItem");
        _restaurant = restaurant;
        restaurant.AddMenuItemToMenu(this);
    }
    
    
    // association with ingredient
    protected List<Ingredient> _ingredients = [];
    // association getter 
    public List<Ingredient> Ingredients => [.._ingredients];
    // association methods
    public void AddIngredient(Ingredient ingredient)
    {
        if(ingredient == null) throw new ArgumentNullException($" {this.Name}: Ingredient in AddIngredient can't be null");
        if (!_ingredients.Contains(ingredient))
        {
            _ingredients.Add(ingredient);
            // reverse connection
            ingredient.AddMenuItemToIngredient(this);
        }
    }
    public void RemoveIngredient(Ingredient ingredient)
    {
        if(ingredient == null) throw new ArgumentNullException($" {this.Name}: Ingredient in RemoveIngredient can't be null");
        if (_ingredients.Contains(ingredient))
        {
            _ingredients.Remove(ingredient);
            // reverse connection
            ingredient.RemoveMenuItemFromIngredient(this);
        }
    }
    public void UpdateIngredients(List<Ingredient>? ingredients)
    {
        if (ingredients != null && ingredients.Count != 0)
        {
            if (_ingredients.Count > 0) 
                foreach (Ingredient ingredient in _ingredients) 
                    RemoveIngredient(ingredient);
            foreach (Ingredient ingredient in ingredients) AddIngredient(ingredient);
        }
    }
    
    // association with attribute MenuItem => OrderList => Order
    private List<OrderList> _orders = [];
    // association getters
    [JsonIgnore]
    public List<OrderList> Orders => [.._orders];
    // association methods
    public void AddToOrder(Order order, int quantity = 1)
    {
        if(quantity <= 0) throw new ArgumentException($"quantity must be greater than zero");
        if(order == null) throw new ArgumentNullException($" {this.Name}: Order in AddToOrder can't be null"); 
        // check if MenuItem belong to the specific restaurant the order is placed
        if(order.Table.Restaurant != this.Restaurant) throw new AggregateException($"MenuItem {this.Name}: you are trying to asign to the order doesn't exist in the restaurant in which the order was opened. Name of restaurant: {order.Table.Restaurant.Name}");
        // validate the day and time of the setOfMenuItem
        if (this is SetOfMenuItem setOfMenuItem)
        {
            if(order is OnlineOrder onlineOrder)  throw new ArgumentException($"MenuItem you are trying to add to the order is SetOfMenuItems with name: {setOfMenuItem.Name} and The order you created is OnlineOrder with id: {onlineOrder.Id}. You can't add setOfMenuItems to the OnlineOrder is it polices of restaurant");
            // ValidateSetOfMenuItem(setOfMenuItem); // reveal soon if needed
        }
        new OrderList(this, order, quantity);
    }
    public void AddOrderList(OrderList orderList)
    {
        if (orderList == null) throw new ArgumentNullException($" {this}: OrderList can't be null in AddOrderList()");
        if (orderList.MenuItem != this) throw new AggregateException($"you are trying to add the wrong orderList to the MenuItem in AddOrderList()");
        if (!_orders.Contains(orderList)) _orders.Add(orderList);
    }
    
    // validation
    private static void ValidateStringMandatory(string name, string text)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{text} cannot be null or empty");
    }
    private static void ValidatePrice(double price, string text)
    {
        if (price <= 0) throw new ArgumentException($"{text} Price must be greater than zero");
    }

    // CRUD
    public void UpdateName(string newName)
    {
        Name = newName;
    }
    public void UpdatePrice(double newPrice)
    {
        Price = newPrice;
    }
    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }
    
    //methods
    public void MakeUnavailable()
    {
        IsAvailable = false;
    }
    public void MakeAvailable()
    {
        IsAvailable = true;
    }
    
    // other methods
    public void AddPromotion(Promotion promotion)
    {
        Promotion = promotion;
    }
    public void RemovePromotion()
    {
        Promotion = null;
    }

    // methods updating the promotion 
    public void UpdateDiscountPercentPromotion(double newDiscountPromotion)
    {
        if (_promotion != null) _promotion.UpdateDiscountPercent(newDiscountPromotion);
        else Console.WriteLine("No Promotion to update");
    }
    public void UpdateNamePromotion(string newName)
    {
        if (_promotion != null) _promotion.UpdateName(newName);
        else Console.WriteLine("No Promotion to update");
    }
    public void UpdateDescriptionPromotion(string newDescription)
    {
        if (_promotion != null) _promotion.UpdateDescription(newDescription);
        else Console.WriteLine("No Promotion to update");
    }
    public void RemoveDescriptionPromotion()
    {
        if (_promotion != null) _promotion.RemoveDescription();
        else Console.WriteLine("No Promotion to update");
    }
    public void UpdateTypePromotion(Promotion.PromotionType newPromotionType)
    {
        if (_promotion != null) _promotion.UpdateType(newPromotionType);
        else Console.WriteLine("No Promotion to update");
    }
    
    
    
    
}
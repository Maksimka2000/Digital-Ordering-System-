using System.Collections.Immutable;
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public abstract class MenuItem
{
    // static fields 
    private static int IdCounter = 0;

    // fields
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
        get => _promotion == null ? null : _promotion.Clone();
        private set
        {
            _promotion = value;
        }
    }

    // constructor
    [JsonConstructor]
    protected MenuItem(string name, double price, string description, List<Ingredient>? ingredients = null, Promotion? promotion = null)
    {
        Id = ++IdCounter;
        Name = name;
        Price = price;
        Description = description;
        if (ingredients != null) foreach (Ingredient ingredient in ingredients) AddIngredient(ingredient);
        Promotion = promotion;
        IsAvailable = true;
    }
    
    // association
    private List<Ingredient> _ingredients = new();
    // association getter 
    public List<Ingredient> Ingredients => [.._ingredients];
    // association methods
    public void AddIngredient(Ingredient ingredient)
    {
        if(ingredient == null) throw new ArgumentNullException($"Ingredient: '{ingredient.Name}' in AddIngredient can't be null");
        if (!_ingredients.Contains(ingredient))
        {
            _ingredients.Add(ingredient);
            // reverse connection
            ingredient.AddMenuItemToIngredient(this);
        }
    }
    public void RemoveIngredient(Ingredient ingredient)
    {
        if(ingredient == null) throw new ArgumentNullException($"Ingredient: '{ingredient.Name}' in RemoveIngredient can't be null");
        if (_ingredients.Contains(ingredient))
        {
            _ingredients.Remove(ingredient);
            // reverse connection
            ingredient.RemoveMenuItemFromIngredient(this);
        }
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
    public void Unavailable()
    {
        IsAvailable = false;
    }

    public void Available()
    {
        IsAvailable = true;
    }
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
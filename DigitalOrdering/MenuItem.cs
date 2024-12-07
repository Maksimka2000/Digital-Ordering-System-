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
    public void AddPromotion(Promotion promotion)
    {
        Promotion = promotion;
    }
    public void RemovePromotion()
    {
        Promotion = null;
    }
    
    
    
}
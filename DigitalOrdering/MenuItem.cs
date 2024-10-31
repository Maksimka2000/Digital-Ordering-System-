using System.Collections.Immutable;
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public abstract class MenuItem
{
    
    // class extent
    
    // static fields 
    private static int IdCounter = 0;
    
    // fields
    public int Id { get;  }
    public string _name;
    public double _price;
    public string _description;
    
    //setters validation
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

    // dependencies
    public List<Ingredient>? _ingredients;
    public Promotion? _promotion;
    
    //dependencies setter validation
    public List<Ingredient>? Ingredients
    {
        get => _ingredients;
        private set
        {
            ValidateIngredientsList(value);
            _ingredients = value;
        }
    }
    
    public Promotion? Promotion
    {
        get => _promotion;
        private set => _promotion = value; // Allows setting Promotion to null if necessary
    }

    // constructor
    [JsonConstructor]
    protected MenuItem(string name, double price, string description, List<Ingredient>? ingredients = null, Promotion? promotion = null)
    {
        Id = ++IdCounter;
        Name = name;
        Price = price;
        Description = description;
        Ingredients = ingredients;
        Promotion = promotion;
    }
    
    // validation
    private static void ValidateIngredientsList(List<Ingredient>? ingredients)
    {
        if (ingredients != null && ingredients.Count == 0) throw new ArgumentException($"Ingrediesnts list cannot be empty");
    }
    private static void ValidateStringMandatory(string name, string text)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{text} cannot be null or empty");
    }
    private static void ValidatePrice(double price, string text)
    {
        if (price <= 0) throw new ArgumentException($"{text} Price must be greater than zero");
    }
    
    // CRUD
    public virtual List<Ingredient> GetIngredients()
    {
        return new List<Ingredient>(Ingredients); 
    }
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
    public void UpdateIngredients(List<Ingredient>? newIngredients)
    {
        Ingredients = newIngredients;
    }
    public void UpdatePromotion(Promotion? newPromotion)
    {
        Promotion = newPromotion;
    }
    
}
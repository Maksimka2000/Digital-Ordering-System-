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
    public int Id { get; }
    public string _name;
    public double _price;
    public string _description;

    //setters and getters implemention
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
    
    // constructor
    [JsonConstructor]
    protected MenuItem(string name, double price, string description, List<Ingredient>? ingredients = null, Promotion? promotion = null)
    {
        Id = ++IdCounter;
        Name = name;
        Price = price;
        Description = description;
        if (ingredients != null) UpdateIngredients(ingredients);
        if (promotion != null) AddPromotion(promotion);
    }
    
    // association filed
    private List<Ingredient> _ingredients = new();
    private Promotion? _promotion = null;
    // associations getters and setters
    public List<Ingredient> Ingredients
    {
        get => [.._ingredients];
    }
    public Promotion? Promotion
    {
        get => _promotion;
    }

    // associations methods
    public void AddPromotion(Promotion promotion)
    {
        if (promotion == null) throw new ArgumentException("Promotion in MenuItem while AddPromotionToMenuItem method called obj is null");
        if (_promotion == promotion){ Console.WriteLine($"There is  already a promotion: {promotion.Name} in MenuItem: {this.Name}"); return; }
        if (_promotion != null){ Console.WriteLine($"There is  already some different promotion: {this.Promotion?.Name} in MenuItem: {this.Name}, first delete previous and add new"); return; }
        _promotion = promotion;
        promotion.AddMenuItemToPromotion(this);
    }
    public void RemovePromotion(Promotion promotion)
    {
        if (promotion == null) throw new ArgumentException("Promotion in MenuItem while RemovePromotionToMenuItem method called obj is null");
        if (_promotion != promotion) { Console.WriteLine($"Promotion {promotion.Name} you want to remove is not in the MenuItem {this.Name}"); return; }
        _promotion = null;
        promotion.RemoveMenuItemFromPromotion(this);
    }
    public void AddIngredient(Ingredient ingredient)
    {
        //validate
        if ( ingredient == null) throw new ArgumentException("Ingredient in MenuItem while AddIngredient method called obj is null");
        //duplication validation
        if ( _ingredients.Contains(ingredient)){ Console.WriteLine($"Ingredient: {ingredient.Name} already added to MenuItem: {this.Name}"); return;}
        //
        _ingredients.Add(ingredient);
        ingredient.AddMenuItemToIngredient(this);
    }
    public void RemoveIngredient(Ingredient ingredient)
    {
        // validate 
        if ( ingredient == null) throw new ArgumentException("Ingredient in MenuItem while RemoveIngredient method called obj is null");
        // existance validation
        if ( !_ingredients.Contains(ingredient)){ Console.WriteLine($"Ingredient: {ingredient.Name} doesn't exist in MenuItem: {this.Name}"); return;}
        //
        _ingredients.Remove(ingredient);
        ingredient.RemoveMenuItemFromIngredient(this);
    }
    public void UpdateIngredients(List<Ingredient> ingredients)
    {
        if (ingredients == null || ingredients.Count == 0) throw new ArgumentException($"Ingredients list cannot be empty and null in the {nameof(MenuItem)}, {nameof(UpdateIngredients)} method ");
        if (_ingredients.ToList().Count > 0) foreach (var ingredient in _ingredients.ToList()) RemoveIngredient(ingredient);
        foreach (var ingredient in ingredients) AddIngredient(ingredient);
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
}
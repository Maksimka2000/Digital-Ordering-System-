using System.Collections.Immutable;
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public abstract class MenuItem
{
    
    
    private static int IdCounter = 0;
    public int Id { get;  private set; }
    public string Name { get;  private set; }
    public double Price { get;  private set; }
    public string Description { get; private set; }
    public bool HasChangableIngredients { get; private set; }

    public List<Ingredient>? Ingredients { get;  private set; }
    public Promotion? Promotion { get; private set; }

    
    
    [JsonConstructor]
    protected MenuItem(string name, double price, string description, bool hasChangableIngredients)
    {
        Id = ++IdCounter;
        ValidateStringMandatory(name, "Name in MenuItem");
        Name = name;
        ValidatePrice(price, "MenuItem");
        Price = price;
        ValidateStringMandatory(description, "Description in MenuItem");
        Description = description;
        HasChangableIngredients = hasChangableIngredients;
    }
    [JsonConstructor]
    protected MenuItem(string name, double price, string description, bool hasChangableIngredients,
        List<Ingredient>? ingredients, Promotion? promotion) : this(name, price, description, hasChangableIngredients)
    {
        ValidateListIngredietns(ingredients);
        Ingredients = ingredients;
        Promotion = promotion;
    }

    


    // validation
    private static void ValidateListIngredietns(List<Ingredient>? ingredients)
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
    
    
}
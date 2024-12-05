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
    private string _name;
    private double _price;
    private string _description;

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
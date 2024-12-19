using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class Promotion
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromotionType
    {
        Regular = 0
    }

    // Class/static fields/attributes
    private const double MaxDiscountPercent = 99.99;
    private const double MinDiscountPercent = 0.01;

    // Fields/attributes
    private double _discountPercent;
    private string _name;
    private string? _description;
    private PromotionType _type;

    // setters validation 
    [JsonConverter(typeof(StringEnumConverter))]
    public PromotionType Type
    {
        get => _type;
        private set
        {
            if (!Enum.IsDefined(typeof(PromotionType), value))
                throw new ArgumentException("Promotion type is not defined in Promotion class.");
            _type = value;
        }
    }

    public double DiscountPercent
    {
        get => _discountPercent;
        private set
        {
            ValidateDiscountPercentage(value);
            _discountPercent = value;
        }
    }

    public string Name
    {
        get => _name;
        private set
        {
            ValidateStringMandatory(value, "Name in Promotion");
            _name = value;
        }
    }

    public string? Description
    {
        get => _description;
        private set
        {
            if (value == null || !string.IsNullOrEmpty(value))
            {
                _description = value;
            }
        }
    }


    // constructor
    [JsonConstructor]
    public Promotion(double discountPercent, string name, string? description = null,
        PromotionType type = PromotionType.Regular)
    {
        DiscountPercent = discountPercent;
        Name = name;
        Description = description;
        Type = type;
    }

    // validation methods
    private static void ValidateDiscountPercentage(double discountPercent)
    {
        if (!(discountPercent >= MinDiscountPercent && discountPercent <= MaxDiscountPercent))
            throw new Exception($"Discount must be from 0.01 min to 99.99 max");
    }

    private static void ValidateStringMandatory(string name, string text)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{text} cannot be null or empty");
    }

    private static void ValidateStringOptional(string value, string text)
    {
        if (value == string.Empty) throw new ArgumentException($"{text} cannot be empty");
    }

    // get, add, delete, set  on class
    public void UpdateDiscountPercent(double newDiscountPercent)
    {
        DiscountPercent = newDiscountPercent;
    }

    public void UpdateName(string newName)
    {
        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }

    public void RemoveDescription()
    {
        _description = null;
    }

    public void UpdateType(PromotionType newType)
    {
        Type = newType;
    }

    // other
    public override string ToString()
    {
        return $"name: {Name}, description: {Description}, discount: {DiscountPercent}, type: {Type}";
    }

    public Promotion Clone()
    {
        return new Promotion(DiscountPercent, Name, Description, Type);
    }
}
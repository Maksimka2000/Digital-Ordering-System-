using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class Promotion
{
    
    // class extent
    private static List<Promotion> _promotions = [];
    
    // Class/static fields/attributes
    private static int IdCounter = 0;  // add id counter
    private const int MaxDiscountPercent = 99;
    private const int MinDiscountPercent = 1;
    
    // Fields/attributes
    public int Id { get; } // no set! As assigned can't be changed 
    private int _discountPercent; // no set and get
    private string _name;
    private string _description;
    
    // setters validation 
    public int DiscountPercent
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
    public string Description
    {
        get => _description;
        private set
        {
            ValidateStringMandatory(value, "Description in Promotion");
            _description = value;
        }
    }
    
    // constructor
    [JsonConstructor]
    public Promotion(int discountPercent, string name, string description)
    {
        Id = ++IdCounter;
        DiscountPercent = discountPercent;
        Name = name;
        Description = description;
    }
    
    // assosiation field
    // assosiation field reverse
    private MenuItem? _promotionInMenuItem;

    public MenuItem? GetPromotionInMenuItem()
    {
        return _promotionInMenuItem;
    }

    public void AddPromotionInMenuItem(MenuItem menuItem)
    {
        if ( menuItem == null) throw new ArgumentException("MenuItem is null");
        if ( _promotionInMenuItem == menuItem) return;
        
        _promotionInMenuItem = menuItem;
        menuItem.AddMenuItemPromotion(this);
    }
    
    // validation methods
    private static void ValidateDiscountPercentage(int discountPercent)
    {
        if (!(discountPercent >= MinDiscountPercent && discountPercent <= MaxDiscountPercent)) throw new Exception($"Discount must be from 1 to 99 max");
    }
    private static void ValidateStringMandatory(string name, string text)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{text} cannot be null or empty");
    }
    private static void ValidateNameDuplication(Promotion promotion)
    {
        if (_promotions.FirstOrDefault(i => i.Name == promotion.Name) == null) ;
        else throw new ArgumentException($"promotion {promotion.Name} already exists");
    }
    
    // get, add, delete, set  on class
    public static void AddPromotion(Promotion promotion)
    {
        if (promotion == null) throw new ArgumentException("Promotion cannot be null");
        // ValidateNameDuplication(promotion);
        _promotions.Add(promotion);
    }
    public static List<Promotion> GetPromotions()
    {
        return [.._promotions];
    }
    public static void DeletePromotion(Promotion promotion)
    {
        if(promotion == null) throw new ArgumentException("Promotion cannot be null");
        _promotions.Remove(promotion);
    }
    
    
    public void UpdateDiscountPercent(int newDiscountPercent)
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
    
    // Serialized and deserialized 
    public static void SavePromotionJson(string path)
    {
        try
        {
            var json = JsonConvert.SerializeObject(_promotions, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File Promotion saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving Promotion file: {e.Message}");
        }
    }

    public static void LoadPromotionJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _promotions = JsonConvert.DeserializeObject<List<Promotion>>(json);
                // foreach (var promotion in promotions) { new Promotion(promotion.DiscountPercent, promotion.Name, promotion.Description); }
                Console.WriteLine($"File Promotion loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading Promotion file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading Promotion file: {e.Message}");
        }
    }
}
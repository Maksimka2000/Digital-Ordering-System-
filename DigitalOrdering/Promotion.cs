using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class Promotion
{
    
    // class extent
    private static List<Promotion> _promotions = new List<Promotion>();
    
    // Class/static fileds/attributes
    private static int IdCounter = 0;  // add id counter
    private static int MaxDiscountPercent = 99;
    private static int MinDiscountPercent = 1;
    
    // Fields/attributesx
    public int Id { get; } // no set! As asigned can't be changed 
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
    
    // validation meethods
    private static void ValidateDiscountPercentage(int discountPercent)
    {
        if (!(discountPercent >= MinDiscountPercent && discountPercent <= MaxDiscountPercent)) throw new Exception($"Discount must be from 5 to 95 max");
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
        ValidateNameDuplication(promotion);
        _promotions.Add(promotion);
    }
    public static List<Promotion> GetPromotions()
    {
        return new List<Promotion>(_promotions);
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
    public static void SavePromotionJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_promotions, Formatting.Indented);
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
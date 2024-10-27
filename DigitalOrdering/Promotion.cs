using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class Promotion
{
    private static List<Promotion> _promotions = new List<Promotion>();
    private static int IdCounter = 0;
    
    public int Id { get; }
    public int DiscountPercent { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    private const int MaxDiscountPercent = 95;
    private const int MinDiscountPercent = 5;

    
    [JsonConstructor]
    public Promotion(int discountPercent, string name, string description)
    {
        Id = ++IdCounter;
        ValidateDiscountPercentage(discountPercent);
        DiscountPercent = discountPercent;
        ValidateStringMandatory(name, "Name in Promotion");
        Name = name;
        ValidateStringMandatory(description, "Description in Promotion");
        Description = description;
    }
    

    // validation 
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
    
    // get, add, delete
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
        if(promotion == null) throw new ArgumentException("Game cannot be null");
        _promotions.Remove(promotion);
    }
    
    
    
    // ================================================================ serialized and deserialized 
    public static void SavePromotionJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_promotions, Formatting.Indented);

            File.WriteAllText(path, json);
            Console.WriteLine($"File saved successfully at {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    public static void LoadPromotionJSON(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _promotions = JsonConvert.DeserializeObject<List<Promotion>>(json);
            // foreach (var promotion in promotions)
            // {
            //     new Promotion(promotion.DiscountPercent, promotion.Name, promotion.Description);
            // }
        }
        else
        {
            throw new Exception($"Load Promotion JSON problem");
        }
    }
}
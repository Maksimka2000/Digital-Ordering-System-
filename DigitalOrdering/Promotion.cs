using Newtonsoft.Json;

namespace DigitalOrdering;

public class Promotion
{
    private static List<Promotion> _promotions = new List<Promotion>();

    private static int IdCounter = 0;
    public int Id { get; private set; }
    public int DiscountPercent { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public const int MaxDiscountPercent = 50;
    public const int MinDiscountPercent = 5;

    
    [JsonConstructor]
    public Promotion(int discountPercent, string name, string description)
    {
        Id = ++IdCounter;
        DiscountPercent = validateDiscountPercentage(discountPercent);
        Name = name;
        Description = description;
    }


    public static void AddPromotion(Promotion promotion)
    {
        if (promotion == null) throw new ArgumentException("Game cannot be null");
        _promotions.Add(promotion);
    }

    private void ValidateDuplication(string name)
    {
        if (GetPromotionByName(name) != null) throw new Exception($"The promotion with name {name} has already been added.");
    }

    private static Promotion? GetPromotionByName(string name)
    {
        return _promotions.FirstOrDefault(p => p.Name == name);
    }

    private int validateDiscountPercentage(int discountPercent)
    {
        if (discountPercent >= MinDiscountPercent && discountPercent <= MaxDiscountPercent) return discountPercent;
        throw new Exception($"Discount must be from 5 to 50 max");
    }

    public static List<Promotion> GetPromotions()
    {
        return new List<Promotion>(_promotions);
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
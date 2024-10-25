using Newtonsoft.Json;

namespace DigitalOrdering;

public class Promotion
{
    private static List<Promotion> _promotions = new List<Promotion>();

    public int Id { get; set; }
    public int DiscountPercent { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public const int MaxDiscountPercent = 50;
    public const int MinDiscountPercent = 5;
    
    
    
    [JsonConstructor]
    public Promotion(){}
    public Promotion(int discountPercent, string name, string description)
    {
        
            Id = _promotions.Count;
            DiscountPercent = validateDiscountPercentage(discountPercent);
            Name = name;
            Description = description;

            if (GetPromotionByName(name) == null)
            {
                _promotions.Add(this);
            }
            else
            {
                throw new Exception($"The promotion with name {name} has already been added.");
            }        
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
            var promotions = JsonConvert.DeserializeObject<List<Promotion>>(json);
            foreach (var promotion in promotions)
            {
                new Promotion(promotion.DiscountPercent, promotion.Name, promotion.Description);
            }
           
        }
        else
        {
            throw new Exception($"Load Promotion JSON problem");
        }
    }
}
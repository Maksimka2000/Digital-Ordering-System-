using Newtonsoft.Json;

namespace DigitalOrdering;

public class Ingredient
{
    
    private static List<Ingredient> _ingredients = new List<Ingredient>();
    
    public int Id { get;  set; }
    public string Name { get; set; }


    [JsonConstructor]
    public Ingredient() {}
    public Ingredient(string name)
    {
        Id = _ingredients.Count;
        Name = name;
        if (GetIngredientByName(name) == null)
        {
            _ingredients.Add(this);
        }
        else
        {
            throw new Exception($"Ingredient already exists: {name}");
        }
    }

    public static Ingredient? GetIngredientByName(string name)
    {
        return _ingredients.FirstOrDefault(n => n.Name == name);
    }
    
    public static List<Ingredient> GetIngredients()
    {
        return new List<Ingredient>(_ingredients);
    }
    
    
    // ================================================================ serialized and deserialized 
    public static void SaveIngredientJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_ingredients, Formatting.Indented);
        
            File.WriteAllText(path, json);
            Console.WriteLine($"File saved successfully at {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    public static void LoadIngredientJSON(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(json);
            foreach (var ingredient in ingredients)
            {
                new Ingredient(ingredient.Name);
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    
}
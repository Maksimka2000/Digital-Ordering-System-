using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class Ingredient
{
    
    private static List<Ingredient> _ingredients = new List<Ingredient>();
    
    private static int IdCounter = 0;
    public int Id { get; }
    public string Name { get; private set; }


    [JsonConstructor]
    public Ingredient(string name)
    {
        Id = ++IdCounter;
        ValidateStringMandatory(name, "Name in Ingredient");
        Name = name;
    }
    
    // validation
    private static void ValidateStringMandatory(string name, string text)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{text} cannot be null or empty");
    }
    private static void ValidateNameDuplication(Ingredient ingredient)
    {
        if (_ingredients.FirstOrDefault(i => i.Name == ingredient.Name) == null); 
        else throw new ArgumentException($"ingredient {ingredient.Name} already exists");
    }
    
    
    
    // get, delete, add, update
    public static void AddIngredient(Ingredient ingredient)
    {        
        if(ingredient == null) throw new ArgumentException("Ingredient cannot be null");
        ValidateNameDuplication(ingredient);
        _ingredients.Add(ingredient);
    }

    public static List<Ingredient> GetIngredients()
    {
        return new List<Ingredient>(_ingredients);
    }
    public static void DeleteIngredient(Ingredient ingredient)
    {
        _ingredients.Remove(ingredient);
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
            _ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(json);
            // foreach (var ingredient in ingredients)
            // {
            //     new Ingredient(ingredient.Name);
            // }
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    
}
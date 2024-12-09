using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class Ingredient
{
    // clas extent
    private static List<Ingredient> _ingredients = [];

    // static/class fields/attributes
    private static int IdCounter = 0;

    // fields
    public int Id { get; }
    private string _name;

    // setter validation 
    public string Name
    {
        get => _name;
        private set
        {
            ValidateStringMandatory(value, "Name in Ingredient");
            _name = value;
        }
    }

    // constructor
    [JsonConstructor]
    public Ingredient(string name)
    {
        Id = ++IdCounter;
        Name = name;
    }
    
    //association reverse 
    private List<MenuItem> _ingredientInMenuItems = new();
    //associateion reverse getter
    [JsonIgnore]
    public List<MenuItem> IngredientInMenuItems => [.._ingredientInMenuItems];
    //association reverse methods
    public void AddMenuItemToIngredient(MenuItem menuItem)
    {
        if(menuItem == null) throw new ArgumentNullException($"MenuItem: '{menuItem.Name}' in AddMenuItemToIngredient Ingredient: '{Name}' is null");
        if (!_ingredientInMenuItems.Contains(menuItem))
        {
            _ingredientInMenuItems.Add(menuItem);
            menuItem.AddIngredient(this);
        }
    }
    public void RemoveMenuItemFromIngredient(MenuItem menuItem)
    {
        if (menuItem == null) throw new ArgumentNullException($"MenuItem: '{menuItem.Name}' in RemoveMenuItemFromIngredient Ingredient: '{Name}' is null");
        if (_ingredientInMenuItems.Contains(menuItem))
        {
            _ingredientInMenuItems.Remove(menuItem);
            menuItem.RemoveIngredient(this);
        }
    }
    
    // validation
    private static void ValidateStringMandatory(string name, string text)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{text} cannot be null or empty");
    }

    private static void ValidateNameDuplication(Ingredient ingredient)
    {
        if (_ingredients.FirstOrDefault(i => i.Name == ingredient.Name) == null) ;
        else throw new ArgumentException($"ingredient {ingredient.Name} already exists");
    }

    // get, delete, add, update CRUD 
    public static void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null) throw new ArgumentException("Ingredient cannot be null");
        _ingredients.Add(ingredient);
    }

    public static List<Ingredient> GetIngredients()
    {
        return [.._ingredients];
    }

    public static void DeleteIngredient(Ingredient ingredient)
    {
        if (ingredient == null) throw new ArgumentException("ingredient cannot be null");
        _ingredients.Remove(ingredient);
    }

    public void UpdateName(string newName)
    {
        Name = newName;
    }


    //  serialized and deserialized 
    public static void SaveIngredientJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_ingredients, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File Ingredient saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving Ingredient file: {e.Message}");
        }
    }

    public static void LoadIngredientJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(json);
                // foreach (var ingredient in ingredients) { new Ingredient(ingredient.Name); }
                Console.WriteLine($"File Ingredient loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading Ingredient file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading Ingredient file: {e.Message}");
        }
    }
}
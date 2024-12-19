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
    [JsonIgnore]
    public int Id { get; }
    private string _name;

    // setter validation 
    public string Name
    {
        get => _name;
        private set
        {
            // if (_ingredients.FirstOrDefault(i => i.Name == value) != null) return;
            ValidateStringMandatory(value, "Name in Ingredient");
            _name = value;
        }
    }

    // constructor
    [JsonConstructor]
    public Ingredient(string name)
    {
        Id = ++IdCounter;
        ValidateStringMandatory(name, "Name in Ingredient");
        ValidateNameDuplication(name);
        Name = name;
        AddIngredient(this);
    }
    
    //association reverse
    private List<MenuItem> _ingredientInMenuItems = [];
    //association reverse getter
    [JsonIgnore]
    public List<MenuItem> IngredientInMenuItems => _ingredientInMenuItems;
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
    private static void ValidateNameDuplication(string name)
    {
        if (_ingredients.Any(i => i.Name == name))
        {
            throw new ArgumentException($"Ingredient with name '{name}' already exists.");
        }
    }

    // get, delete, add, update CRUD 
    private static void AddIngredient(Ingredient ingredient)
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
}
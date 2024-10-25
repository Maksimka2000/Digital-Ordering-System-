using System.Collections.Immutable;
using Newtonsoft.Json;

namespace DigitalOrdering;

public abstract class MenuItem
{
    public int Id { get;  set; }
    public string Name { get;  set; }
    public double Price { get;  set; }
    public string Description { get; set; }
    public bool hasChangableIngredients { get; set; }

    public List<Ingredient>? Ingredients { get;  set; }
    public Promotion? Promotion { get; set; }

    public abstract bool Delete();
    
    [JsonConstructor]
    protected MenuItem(){}
    protected MenuItem(string name, double price, string description, bool hasChangableIngredients,
        List<Ingredient>? ingredients, Promotion? promotion)
    {
        Id = Beverage.GetBeverages().Count + Food.GetFoods().Count + BusinessLunch.GetBusinessLunches().Count;
        Name = name;
        Price = price;
        Description = description;
        this.hasChangableIngredients = hasChangableIngredients;
        Ingredients = validateIngredientsInput(ingredients);
        Promotion = promotion;
    }



    public void AddIngredient(Ingredient ingredient)
    {
        if (Ingredients == null) Ingredients = new List<Ingredient>();
        Ingredients!.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        Ingredients!.Remove(ingredient);
    }


  

    private List<Ingredient>? validateIngredientsInput(List<Ingredient>? ingredients)
    {
        if (ingredients is null || ingredients.Count == 0) return null;
        else
        {
            return ingredients;
        }
    }


    


    public void ChangeName(string newName)
    {
        this.Name = newName;
    }


}
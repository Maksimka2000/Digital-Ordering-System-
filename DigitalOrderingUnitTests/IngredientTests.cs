using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class IngredientTests
{
    public IngredientTests()
    {
        typeof(Ingredient).GetField("_ingredients",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, new List<Ingredient>());
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "Tomato";

        var ingredient = new Ingredient(name);

        Assert.Equal(name, ingredient.Name);
        Assert.True(ingredient.Id > 0);
    }

    [Fact]
    public void Name_Getter_ReturnsCorrectValue()
    {
        var ingredient = new Ingredient("Basil");
        Assert.Equal("Basil", ingredient.Name);
    }

    [Fact]
    public void Id_Getter_ReturnsUniqueValue()
    {
        var ingredient1 = new Ingredient("Oregano");
        var ingredient2 = new Ingredient("Pepper");
        Assert.NotEqual(ingredient1.Id, ingredient2.Id);
    }


    [Fact]
    public void AddIngredient_AddsIngredientToList()
    {
        var ingredient = new Ingredient("Lettuce");

        Ingredient.AddIngredient(ingredient);
        var ingredients = Ingredient.GetIngredients();

        Assert.Contains(ingredient, ingredients);
    }

    [Fact]
    public void GetIngredients_ReturnsCorrectListOfIngredients()
    {
        var ingredient1 = new Ingredient("Onion");
        var ingredient2 = new Ingredient("Garlic");
        Ingredient.AddIngredient(ingredient1);
        Ingredient.AddIngredient(ingredient2);

        var ingredients = Ingredient.GetIngredients();

        Assert.Equal(2, ingredients.Count);
        Assert.Contains(ingredient1, ingredients);
        Assert.Contains(ingredient2, ingredients);
    }

    [Fact]
    public void DeleteIngredient_RemovesIngredientFromList()
    {
        var ingredient = new Ingredient("Cheese");
        Ingredient.AddIngredient(ingredient);

        Ingredient.DeleteIngredient(ingredient);
        var ingredients = Ingredient.GetIngredients();

        Assert.DoesNotContain(ingredient, ingredients);
    }

    // [Fact]
    // public void UpdateName_ChangesIngredientName()
    // {
    //     var ingredient = new Ingredient("Basil");
    //
    //     ingredient.UpdateName("Fresh Basil");
    //
    //     Assert.Equal("Fresh Basil", ingredient.Name);
    // }

    [Fact]
    public void SaveIngredientJSON_SavesIngredientsToFile()
    {
        var ingredient = new Ingredient("Butter");
        Ingredient.AddIngredient(ingredient);
        const string path = "test_ingredients.json";

        Ingredient.SaveIngredientJSON(path);

        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadIngredientJSON_LoadsIngredientsFromFile()
    {
        const string path = "test_ingredients.json";
        var ingredient = new Ingredient("Salt");
        Ingredient.AddIngredient(ingredient);
        Ingredient.SaveIngredientJSON(path);
        Ingredient.DeleteIngredient(ingredient);

        Ingredient.LoadIngredientJSON(path);
        var ingredients = Ingredient.GetIngredients();

        Assert.Single(ingredients);
        Assert.Equal(ingredient.Name, ingredients[0].Name);

        File.Delete(path);
    }
    
    [Fact]
    public void ExceptionTests()
    {
        var ingredient = new Ingredient("Salt");
        Ingredient.AddIngredient(ingredient);
        var duplicateIngredient = new Ingredient("Salt");
        Assert.Throws<ArgumentException>(() => Ingredient.AddIngredient(duplicateIngredient));

        Assert.Throws<ArgumentException>(() => Ingredient.AddIngredient(null));
    }
}
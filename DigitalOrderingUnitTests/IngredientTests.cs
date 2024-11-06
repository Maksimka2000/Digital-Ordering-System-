using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class IngredientTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "Tomato";

        var ingredient = new Ingredient(name);

        Assert.Equal(name, ingredient.Name);
        Assert.True(ingredient.Id > 0);
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
    public void GetIngredients_ReturnsListOfIngredients()
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

    [Fact]
    public void UpdateName_ChangesIngredientName()
    {
        var ingredient = new Ingredient("Basil");

        ingredient.UpdateName("Fresh Basil");

        Assert.Equal("Fresh Basil", ingredient.Name);
    }

    [Fact]
    public void AddIngredient_ThrowsExceptionForDuplicateName()
    {
        var ingredient1 = new Ingredient("Olive Oil");
        Ingredient.AddIngredient(ingredient1);
        var ingredient2 = new Ingredient("Olive Oil");

        Assert.Throws<ArgumentException>(() => Ingredient.AddIngredient(ingredient2));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidArguments()
    {
        Assert.Throws<ArgumentException>(() => new Ingredient(null));
        Assert.Throws<ArgumentException>(() => new Ingredient(""));
    }

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
}
using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class IngredientTests
{
    public IngredientTests()
    {
        ResetStaticIngredients();
    }

    private void ResetStaticIngredients()
    {
        typeof(Ingredient)
            .GetField("_ingredients", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<Ingredient>());
    }

    private static Restaurant CreateTestRestaurant()
    {
        return new Restaurant("Testaurant", new Address("Main St", "Test City", "123"), new List<OpenHour>
        {
            new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(22, 0, 0)),
            new OpenHour(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(23, 0, 0)),
            new OpenHour(DayOfWeek.Sunday, new TimeSpan(10, 0, 0), new TimeSpan(20, 0, 0))
        });
    }

    private static Food CreateTestFood(Restaurant restaurant, string name = "Pizza")
    {
        return new Food(restaurant, name, 10.0, "Delicious food", Food.FoodType.Snack, null);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var ingredient = new Ingredient("Tomato");
        Assert.Equal("Tomato", ingredient.Name);
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
    public void GetIngredients_ReturnsCorrectListOfIngredients()
    {
        var ingredient1 = new Ingredient("Onion");
        var ingredient2 = new Ingredient("Garlic");

        var ingredients = Ingredient.GetIngredients();

        Assert.Equal(2, ingredients.Count);
        Assert.Contains(ingredient1, ingredients);
        Assert.Contains(ingredient2, ingredients);
    }

    [Fact]
    public void DeleteIngredient_RemovesIngredientFromList()
    {
        var ingredient = new Ingredient("Cheese");

        ingredient.RemoveIngredient();

        Assert.DoesNotContain(ingredient, Ingredient.GetIngredients());
    }

    [Fact]
    public void UpdateName_ChangesIngredientName()
    {
        var ingredient = new Ingredient("Basil");

        ingredient.UpdateName("Fresh Basil");

        Assert.Equal("Fresh Basil", ingredient.Name);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForNullOrEmptyName()
    {
        Assert.Throws<ArgumentException>(() => new Ingredient(null));
        Assert.Throws<ArgumentException>(() => new Ingredient(""));
    }

    [Fact]
    public void AddMenuItemToIngredient_AddsAssociation()
    {
        var ingredient = new Ingredient("Tomato");
        var restaurant = CreateTestRestaurant();
        var menuItem = CreateTestFood(restaurant);

        ingredient.AddMenuItemToIngredient(menuItem);

        Assert.Contains(menuItem, ingredient.IngredientInMenuItems);
        Assert.Contains(ingredient, menuItem.Ingredients);
    }

    [Fact]
    public void RemoveMenuItemFromIngredient_RemovesAssociation()
    {
        var ingredient = new Ingredient("Cheese");
        var restaurant = CreateTestRestaurant();
        var menuItem = CreateTestFood(restaurant);

        ingredient.AddMenuItemToIngredient(menuItem);
        ingredient.RemoveMenuItemFromIngredient(menuItem);

        Assert.DoesNotContain(menuItem, ingredient.IngredientInMenuItems);
        Assert.DoesNotContain(ingredient, menuItem.Ingredients);
    }
}

using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class FoodTests
{
    private readonly Restaurant _restaurant;

    public FoodTests()
    {
        ResetStaticFoods();
        _restaurant = CreateRestaurant();
    }

    private void ResetStaticFoods()
    {
        typeof(Food).GetField("_foods", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, new List<Food>());
    }

    private static Restaurant CreateRestaurant()
    {
        var address = new Address("Main St", "Test City", "123");
        var openHours = new List<OpenHour>
        {
            new(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(22, 0, 0)),
            new(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(23, 0, 0)),
            new(DayOfWeek.Sunday, new TimeSpan(10, 0, 0), new TimeSpan(20, 0, 0))
        };
        return new Restaurant("Testaurant", address, openHours);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "Pasta Carbonara";
        const double price = 12.5;
        const string description = "A classic Italian pasta dish";
        var ingredients = new List<Ingredient> { new("Onion") };
        const Food.FoodType foodType = Food.FoodType.Pasta;
        var dietaryPreferences = new List<Food.DietaryPreferencesType> { Food.DietaryPreferencesType.GlutenFree };

        var food = new Food(_restaurant, name, price, description, foodType, ingredients, dietaryPreferences);

        Assert.Equal(name, food.Name);
        Assert.Equal(price, food.Price);
        Assert.Equal(description, food.Description);
        Assert.Equal(foodType, food.FoodT);
        Assert.Contains(Food.DietaryPreferencesType.GlutenFree, food.DietaryPreferences);
    }

    [Fact]
    public void AddFood_AddsFoodToList()
    {
        var food = new Food(_restaurant, "Salad", 8.0, "Fresh garden salad", Food.FoodType.Snack, null);
        Assert.Contains(food, Food.GetFoods());
    }

    [Fact]
    public void GetFoods_ReturnsCorrectListOfFoods()
    {
        var food1 = new Food(_restaurant, "Soup", 5.0, "Tomato soup", Food.FoodType.Snack, null);
        var food2 = new Food(_restaurant, "Cake", 3.5, "Chocolate cake", Food.FoodType.Desert, null);

        var foods = Food.GetFoods();

        Assert.Equal(2, foods.Count);
        Assert.Contains(food1, foods);
        Assert.Contains(food2, foods);
    }

    [Fact]
    public void DeleteFood_RemovesFoodFromList()
    {
        var food = new Food(_restaurant, "Pizza", 10.0, "Cheese pizza", Food.FoodType.Snack, null);

        food.RemoveMenuItem();

        Assert.DoesNotContain(food, Food.GetFoods());
    }

    [Fact]
    public void AddDietaryPreferencesType_AddsPreference()
    {
        var food = new Food(_restaurant, "Vegan Salad", 9.0, "Healthy salad", Food.FoodType.Snack, null);

        food.AddDietaryPreferencesType(Food.DietaryPreferencesType.Vegan);

        Assert.Contains(Food.DietaryPreferencesType.Vegan, food.DietaryPreferences);
    }

    [Fact]
    public void RemoveDietaryPreferencesType_RemovesPreference()
    {
        var food = new Food(
            _restaurant, "Vegan Salad", 9.0, "Healthy salad", Food.FoodType.Snack, null,
            new List<Food.DietaryPreferencesType> { Food.DietaryPreferencesType.Vegan });

        food.RemoveDietaryPreferencesType(Food.DietaryPreferencesType.Vegan);

        Assert.DoesNotContain(Food.DietaryPreferencesType.Vegan, food.DietaryPreferences);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidArguments()
    {
        Assert.Throws<ArgumentException>(() =>
            new Food(_restaurant, null, 10.0, "Test", Food.FoodType.Snack, null));

        Assert.Throws<ArgumentException>(() =>
            new Food(_restaurant, "Test", -5.0, "Test", Food.FoodType.Snack, null));
    }

    [Fact]
    public void AddSetOfMenuItemsToFood_AddsAssociation()
    {
        var setOfMenuItem = new SetOfMenuItem(_restaurant, "Lunch Combo", 15.0, "Lunch special");
        var food = new Food(_restaurant, "Pasta", 12.0, "Italian pasta", Food.FoodType.Pasta, null);

        food.AddSetOfMenuItemsToFood(setOfMenuItem);

        Assert.Contains(setOfMenuItem, food.FoodInSetOfMenuItems);
        Assert.Contains(food, setOfMenuItem.Foods);
    }

    [Fact]
    public void RemoveSetOfMenuItemsFromFood_RemovesAssociation()
    {
        var setOfMenuItem = new SetOfMenuItem(_restaurant, "Dinner Combo", 25.0, "Dinner special");
        var food = new Food(_restaurant, "Pizza", 10.0, "Cheese pizza", Food.FoodType.Snack, null);

        food.AddSetOfMenuItemsToFood(setOfMenuItem);
        food.RemoveSetOfMenuItemsFromFood(setOfMenuItem);

        Assert.DoesNotContain(setOfMenuItem, food.FoodInSetOfMenuItems);
        Assert.DoesNotContain(food, setOfMenuItem.Foods);
    }
}

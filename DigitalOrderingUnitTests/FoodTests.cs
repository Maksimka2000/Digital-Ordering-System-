using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class FoodTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "Pasta Carbonara";
        const double price = 12.5;
        const string description = "A classic Italian pasta dish";
        var ingredients = new List<Ingredient>();
        Promotion? promotion = null;
        const Food.FoodType foodType = Food.FoodType.Pasta;
        Food.DietaryPreferencesType? dietaryPreference = Food.DietaryPreferencesType.GlutenFree;

        var food = new Food(name, price, description, ingredients, promotion, foodType, dietaryPreference);

        Assert.Equal(name, food.Name);
        Assert.Equal(price, food.Price);
        Assert.Equal(description, food.Description);
        Assert.Equal(ingredients, food.Ingredients);
        Assert.Equal(promotion, food.Promotion);
        Assert.Equal(foodType, food.FoodT);
        Assert.Equal(dietaryPreference, food.DietaryPreference);
    }

    [Fact]
    public void AddFood_AddsFoodToList()
    {
        var food = new Food("Salad", 8.0, "Fresh garden salad", null, null, Food.FoodType.Snack);

        Food.AddFood(food);
        var foods = Food.GetFoods();

        Assert.Contains(food, foods);
    }

    [Fact]
    public void GetFoods_ReturnsListOfFoods()
    {
        var food1 = new Food("Soup", 5.0, "Tomato soup", null, null, Food.FoodType.Snack);
        var food2 = new Food("Cake", 3.5, "Chocolate cake", null, null, Food.FoodType.Desert);
        Food.AddFood(food1);
        Food.AddFood(food2);

        var foods = Food.GetFoods();

        Assert.Equal(2, foods.Count);
        Assert.Contains(food1, foods);
        Assert.Contains(food2, foods);
    }

    [Fact]
    public void DeleteFood_RemovesFoodFromList()
    {
        var food = new Food("Pizza", 10.0, "Cheese pizza", null, null, Food.FoodType.Snack);
        Food.AddFood(food);

        Food.DeleteFood(food);
        var foods = Food.GetFoods();

        Assert.DoesNotContain(food, foods);
    }

    [Fact]
    public void SaveFoodJSON_SavesFoodsToFile()
    {
        var food = new Food("Burger", 7.5, "Beef burger", null, null, Food.FoodType.Snack);
        Food.AddFood(food);
        const string path = "test_food.json";

        Food.SaveFoodJSON(path);

        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadFoodJSON_LoadsFoodsFromFile()
    {
        const string path = "test_food.json";
        var food = new Food("Sandwich", 6.0, "Ham sandwich", null, null, Food.FoodType.Snack);
        Food.AddFood(food);
        Food.SaveFoodJSON(path);
        Food.DeleteFood(food);

        Food.LoadFoodJSON(path);
        var foods = Food.GetFoods();

        Assert.Single(foods);
        Assert.Equal(food.Name, foods[0].Name);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidArguments()
    {
        Assert.Throws<ArgumentException>(() => new Food(null, 10.0, "Test", null, null, Food.FoodType.Snack));
        Assert.Throws<ArgumentException>(() => new Food("Test", -5.0, "Test", null, null, Food.FoodType.Snack));
    }

    [Fact]
    public void ExtentPersistence_ChecksStorageAndRetrieval()
    {
        const string path = "test_food_persistence.json";
        var food1 = new Food("Salad", 6.0, "Healthy salad", null, null, Food.FoodType.Snack);
        var food2 = new Food("Soup", 4.0, "Hot soup", null, null, Food.FoodType.Snack);

        Food.AddFood(food1);
        Food.AddFood(food2);
        Food.SaveFoodJSON(path);

        Food.GetFoods().Clear();

        Food.LoadFoodJSON(path);
        var foods = Food.GetFoods();

        Assert.Equal(2, foods.Count);
        Assert.Contains(foods, f => f.Name == "Salad" && f.Description == "Healthy salad");
        Assert.Contains(foods, f => f.Name == "Soup" && f.Description == "Hot soup");

        File.Delete(path);
    }
    //
    // [Fact]
    // public void ExtentIntegrity_ChecksListCopying()
    // {
    //     var food1 = new Food("Salad", 6.0, "Healthy salad", null, null, Food.FoodType.Snack);
    //     var food2 = new Food("Soup", 4.0, "Hot soup", null, null, Food.FoodType.Snack);
    //
    //     Food.AddFood(food1);
    //     Food.AddFood(food2);
    //
    //     var foodsCopy = Food.GetFoods();
    //     foodsCopy.Clear();
    //
    //     var originalFoods = Food.GetFoods();
    //
    //     Assert.Equal(2, originalFoods.Count);
    //     Assert.Contains(food1, originalFoods);
    //     Assert.Contains(food2, originalFoods);
    // }
}
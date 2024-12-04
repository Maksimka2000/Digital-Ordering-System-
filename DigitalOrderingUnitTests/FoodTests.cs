using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class FoodTests
{
    public FoodTests()
    {
        typeof(Food).GetField("_foods",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, new List<Food>());
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "Pasta Carbonara";
        const double price = 12.5;
        const string description = "A classic Italian pasta dish";
        var ingredients = new List<Ingredient>
        {
            new("Onion")
        };
        var promotion = new Promotion(15, "Winter Sale", "Discount for winter items");
        const Food.FoodType foodType = Food.FoodType.Pasta;
        Food.DietaryPreferencesType? dietaryPreference = Food.DietaryPreferencesType.GlutenFree;

        var food = new Food(name, price, description, ingredients, promotion, foodType, dietaryPreference);

        Assert.Equal(name, food.Name);
        Assert.Equal(price, food.Price);
        Assert.Equal(description, food.Description);
        // Assert.Equal(ingredients, food.Ingredients);
        // Assert.Equal(promotion, food.Promotion);
        Assert.Equal(foodType, food.FoodT);
        Assert.Equal(dietaryPreference, food.DietaryPreference);
    }

    [Fact]
    public void FoodType_Getter_ReturnsCorrectValue()
    {
        var food = new Food("Salad", 8.0, "Fresh garden salad", null, null, Food.FoodType.Snack);
        Assert.Equal(Food.FoodType.Snack, food.FoodT);
    }

    [Fact]
    public void DietaryPreference_Getter_ReturnsCorrectValue()
    {
        var food = new Food("Vegan Burger", 10.0, "Delicious vegan burger", null, null, Food.FoodType.Snack,
            Food.DietaryPreferencesType.Vegan);
        Assert.Equal(Food.DietaryPreferencesType.Vegan, food.DietaryPreference);
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
    public void GetFoods_ReturnsCorrectListOfFoods()
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
}
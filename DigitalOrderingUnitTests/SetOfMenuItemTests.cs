using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class SetOfMenuItemTests
{
    public SetOfMenuItemTests()
    {
        typeof(SetOfMenuItem)
            .GetField("_businessLunches", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<SetOfMenuItem>());
    }

    private static List<Food> CreateFoodList(int count)
    {
        var foods = new List<Food>();
        for (var i = 0; i < count; i++)
        {
            foods.Add(new Food("Food " + i, 10.0, "Test Food", null, null, Food.FoodType.Snack));
        }

        return foods;
    }

    private static List<Beverage> CreateBeverageList(int count)
    {
        var beverages = new List<Beverage>();
        for (var i = 0; i < count; i++)
        {
            beverages.Add(new Beverage("Beverage " + i, 5.0, "Test Beverage", null, null, false,
                Beverage.BeverageType.Drinks));
        }

        return beverages;
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var foods = CreateFoodList(SetOfMenuItem.MinNumberOfFood);
        var beverages = CreateBeverageList(SetOfMenuItem.MinNumberOfBeverage);

        var businessLunch = new SetOfMenuItem("Lunch Set", 20.0, "Business Lunch", foods, beverages);

        Assert.Equal("Lunch Set", businessLunch.Name);
        Assert.Equal(20.0, businessLunch.Price);
        Assert.Equal("Business Lunch", businessLunch.Description);
        Assert.Equal(foods, businessLunch.Foods);
        Assert.Equal(beverages, businessLunch.Beverages);
    }

    [Fact]
    public void MaxNumberOfFood_Getter_ReturnsCorrectValue()
    {
        Assert.Equal(4, SetOfMenuItem.MaxNumberOfFood);
    }

    [Fact]
    public void MinNumberOfFood_Getter_ReturnsCorrectValue()
    {
        Assert.Equal(2, SetOfMenuItem.MinNumberOfFood);
    }

    [Fact]
    public void MaxNumberOfBeverage_Getter_ReturnsCorrectValue()
    {
        Assert.Equal(1, SetOfMenuItem.MaxNumberOfBeverage);
    }

    [Fact]
    public void MinNumberOfBeverage_Getter_ReturnsCorrectValue()
    {
        Assert.Equal(1, SetOfMenuItem.MinNumberOfBeverage);
    }

    [Fact]
    public void AddBusinessLunch_AddsRestaurantToList()
    {
        var businessLunch =
            new SetOfMenuItem("Lunch Set", 20.0, "Business Lunch", CreateFoodList(2), CreateBeverageList(1));
        SetOfMenuItem.AddBusinessLunch(businessLunch);

        var lunches = SetOfMenuItem.GetBusinessLunches();
        Assert.Contains(businessLunch, lunches);
    }

    [Fact]
    public void GetBusinessLunches_ReturnsCorrectList()
    {
        var lunch1 = new SetOfMenuItem("Lunch Set 1", 20.0, "Business Lunch 1", CreateFoodList(2),
            CreateBeverageList(1));
        var lunch2 = new SetOfMenuItem("Lunch Set 2", 25.0, "Business Lunch 2", CreateFoodList(3),
            CreateBeverageList(1));

        SetOfMenuItem.AddBusinessLunch(lunch1);
        SetOfMenuItem.AddBusinessLunch(lunch2);

        var lunches = SetOfMenuItem.GetBusinessLunches();

        Assert.Equal(2, lunches.Count);
        Assert.Contains(lunch1, lunches);
        Assert.Contains(lunch2, lunches);
    }

    [Fact]
    public void SaveBusinessLunchJson_SavesToFile()
    {
        var businessLunch =
            new SetOfMenuItem("Lunch Set", 20.0, "Business Lunch", CreateFoodList(2), CreateBeverageList(1));
        SetOfMenuItem.AddBusinessLunch(businessLunch);
        const string path = "test_business_lunches.json";

        SetOfMenuItem.SaveBusinessLunchJson(path);
        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadBusinessLunchJson_LoadsFromFile()
    {
        const string path = "test_business_lunches.json";
        var businessLunch =
            new SetOfMenuItem("Lunch Set", 20.0, "Business Lunch", CreateFoodList(2), CreateBeverageList(1));
        SetOfMenuItem.AddBusinessLunch(businessLunch);
        SetOfMenuItem.SaveBusinessLunchJson(path);
        SetOfMenuItem.GetBusinessLunches().Clear();

        SetOfMenuItem.LoadBusinessLunchJson(path);
        var lunches = SetOfMenuItem.GetBusinessLunches();

        Assert.Single(lunches);
        Assert.Equal(businessLunch.Name, lunches[0].Name);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidFoodCount()
    {
        var invalidFoods = CreateFoodList(5);
        var beverages = CreateBeverageList(1);
        Assert.Throws<ArgumentException>(() =>
            new SetOfMenuItem("Lunch Set", 20.0, "Business Lunch", invalidFoods, beverages));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidBeverageCount()
    {
        var foods = CreateFoodList(2);
        var invalidBeverages = CreateBeverageList(2);
        Assert.Throws<ArgumentException>(() =>
            new SetOfMenuItem("Lunch Set", 20.0, "Business Lunch", foods, invalidBeverages));
    }
}
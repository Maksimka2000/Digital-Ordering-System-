using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class BeverageTests
{
    public BeverageTests()
    {
        typeof(Beverage).GetField("_beverages",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, new List<Beverage>());
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "Mojito";
        const double price = 10.5;
        const string description = "A refreshing cocktail with mint and lime";
        var ingredients = new List<Ingredient>
        {
            new("Tomato"),
            new("Cheese"),
            new("Basil"),
            new("Olive Oil")
        };
        var promotion = new Promotion(15, "Sale", "Product sale");
        const bool isAlcohol = true;
        var beverageType = Beverage.BeverageType.Cocktails;

        var beverage = new Beverage(name, price, description, beverageType, isAlcohol, ingredients, promotion);

        Assert.Equal(name, beverage.Name);
        Assert.Equal(price, beverage.Price);
        Assert.Equal(description, beverage.Description);
        // Assert.Equal(ingredients, beverage.Ingredients);
        // Assert.Equal(promotion, beverage.Promotion);
        Assert.Equal(isAlcohol, beverage.IsAlcohol);
        Assert.Equal(beverageType, beverage.BeverageT);
    }

    [Fact]
    public void BeverageType_Getter_ReturnsCorrectValue()
    {
        var beverage = new Beverage("Espresso", 2.5, "Strong coffee",
            Beverage.BeverageType.Cafeteria, false, null, null);
        Assert.Equal(Beverage.BeverageType.Cafeteria, beverage.BeverageT);
    }

    [Fact]
    public void IsAlcohol_Getter_ReturnsCorrectValue()
    {
        var beverage = new Beverage("Beer", 4.0, "Refreshing beer", Beverage.BeverageType.Drinks, true, null, null);
        Assert.True(beverage.IsAlcohol);
    }

    [Fact]
    public void AddBeverage_AddsBeverageToList()
    {
        var beverage = new Beverage("Espresso", 2.5, "Strong coffee",
            Beverage.BeverageType.Cafeteria, false, null, null);

        Beverage.AddBeverage(beverage);
        var beverages = Beverage.GetBeverages();

        Assert.Contains(beverage, beverages);
    }

    [Fact]
    public void GetBeverages_ReturnsCorrectList()
    {
        var beverage1 = new Beverage("Espresso", 2.5, "Strong coffee",
            Beverage.BeverageType.Cafeteria, false, null, null);
        var beverage2 = new Beverage("Whiskey", 12.0, "Smooth whiskey", Beverage.BeverageType.Drinks, true, null, null);
        Beverage.AddBeverage(beverage1);
        Beverage.AddBeverage(beverage2);

        var beverages = Beverage.GetBeverages();

        Assert.Equal(2, beverages.Count);
        Assert.Contains(beverage1, beverages);
        Assert.Contains(beverage2, beverages);
    }

    [Fact]
    public void DeleteBeverage_RemovesBeverageFromList()
    {
        var beverage = new Beverage("Tea", 1.5, "Green tea", Beverage.BeverageType.Cafeteria, false, null, null);
        Beverage.AddBeverage(beverage);

        Beverage.DeleteBeverage(beverage);
        var beverages = Beverage.GetBeverages();

        Assert.DoesNotContain(beverage, beverages);
    }

    // [Fact]
    // public void SaveBeverageJSON_SavesBeveragesToFile()
    // {
    //     var beverage = new Beverage("Coca Cola", 1.0, "Classic cola drink",
    //         Beverage.BeverageType.Drinks, false, null, null);
    //     Beverage.AddBeverage(beverage);
    //     const string path = "test_beverages.json";
    //
    //     Beverage.SaveBeverageJSON(path);
    //
    //     Assert.True(File.Exists(path));
    //
    //     File.Delete(path);
    // }

    // [Fact]
    // public void LoadBeverageJSON_LoadsBeveragesFromFile()
    // {
    //     const string path = "test_beverages.json";
    //     var beverage = new Beverage("Pepsi", 1.0, "Classic cola drink",
    //         Beverage.BeverageType.Drinks, false, null, null);
    //     Beverage.AddBeverage(beverage);
    //     Beverage.SaveBeverageJSON(path);
    //     Beverage.DeleteBeverage(beverage);
    //
    //     Beverage.LoadBeverageJSON(path);
    //     var beverages = Beverage.GetBeverages();
    //
    //     Assert.Single(beverages);
    //     Assert.Equal(beverage.Name, beverages[0].Name);
    //
    //     File.Delete(path);
    // }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidArguments()
    {
        Assert.Throws<ArgumentException>(() =>
            new Beverage(null, 10.0, "Test", Beverage.BeverageType.Drinks, false, null, null));
        Assert.Throws<ArgumentException>(() =>
            new Beverage("Test", -5.0, "Test", Beverage.BeverageType.Drinks, false, null, null));
        Assert.Throws<ArgumentException>(() =>
            new Beverage("Test", 5.0, "", Beverage.BeverageType.Drinks, false, null, null));
    }
}
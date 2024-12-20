using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class BeverageTests
{
    private readonly Restaurant _restaurant;

    public BeverageTests()
    {
        ResetBeverages();
        _restaurant = CreateRestaurant();
    }

    private static void ResetBeverages()
    {
        typeof(Beverage)
            .GetField("_beverages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, new List<Beverage>());
    }

    private static Restaurant CreateRestaurant()
    {
        var address = new Address("Main St", "Test City", "123");
        var openHours = new List<OpenHour>
        {
            new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(22, 0, 0)),
            new OpenHour(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(23, 0, 0)),
            new OpenHour(DayOfWeek.Sunday, new TimeSpan(10, 0, 0), new TimeSpan(20, 0, 0))
        };
        return new Restaurant("Testaurant", address, openHours);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var ingredients = CreateIngredients("Mint", "Lime", "Rum");
        var beverage = CreateBeverage("Mojito", 10.5, "A refreshing cocktail with mint and lime", Beverage.BeverageType.Cocktails, true, ingredients);

        AssertBeverageProperties(beverage, "Mojito", 10.5, "A refreshing cocktail with mint and lime", Beverage.BeverageType.Cocktails, true);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidBeverageType()
    {
        Assert.Throws<ArgumentException>(() =>
            new Beverage(_restaurant, "InvalidBeverage", 5.0, "Test", (Beverage.BeverageType)999, false));
    }

    [Fact]
    public void AddSetOfMenuItemsToBeverage_AddsAssociation()
    {
        var setOfMenuItem = CreateSetOfMenuItem("Lunch Combo", 15.0, "Lunch special");
        var beverage = CreateBeverage("Coke", 2.0, "Soda", Beverage.BeverageType.Drinks, false);

        beverage.AddSetOfMenuItemsToBeverage(setOfMenuItem);

        Assert.Contains(setOfMenuItem, beverage.BeverageInSetOfMenuItems);
        Assert.Contains(beverage, setOfMenuItem.Beverages);
    }

    [Fact]
    public void RemoveSetOfMenuItemsFromBeverage_RemovesAssociation()
    {
        var setOfMenuItem = CreateSetOfMenuItem("Dinner Combo", 25.0, "Dinner special");
        var beverage = CreateBeverage("Sprite", 2.5, "Soda", Beverage.BeverageType.Drinks, false);

        beverage.AddSetOfMenuItemsToBeverage(setOfMenuItem);
        beverage.RemoveSetOfMenuItemsFromBeverage(setOfMenuItem);

        Assert.DoesNotContain(setOfMenuItem, beverage.BeverageInSetOfMenuItems);
        Assert.DoesNotContain(beverage, setOfMenuItem.Beverages);
    }

    [Fact]
    public void AddBeverage_AddsBeverageToList()
    {
        var beverage = CreateBeverage("Fanta", 3.0, "Orange soda", Beverage.BeverageType.Drinks, false);

        var beverages = Beverage.GetBeverages();
        Assert.Contains(beverage, beverages);
    }

    [Fact]
    public void GetBeverages_ReturnsCorrectList()
    {
        var beverage1 = CreateBeverage("Tea", 1.5, "Hot tea", Beverage.BeverageType.Cafeteria, false);
        var beverage2 = CreateBeverage("Mojito", 10.0, "Cocktail", Beverage.BeverageType.Cocktails, true);

        var beverages = Beverage.GetBeverages();

        Assert.Equal(2, beverages.Count);
        Assert.Contains(beverage1, beverages);
        Assert.Contains(beverage2, beverages);
    }

    [Fact]
    public void DeleteBeverage_RemovesBeverageFromList()
    {
        var beverage = CreateBeverage("Water", 1.0, "Mineral water", Beverage.BeverageType.Cafeteria, false);

        beverage.RemoveMenuItem();
        var beverages = Beverage.GetBeverages();

        Assert.DoesNotContain(beverage, beverages);
    }

    [Fact]
    public void AddSetOfMenuItemsToBeverage_ThrowsForNull()
    {
        var beverage = CreateBeverage("Coke", 2.0, "Soda", Beverage.BeverageType.Drinks, false);
        Assert.Throws<ArgumentNullException>(() => beverage.AddSetOfMenuItemsToBeverage(null));
    }

    [Fact]
    public void RemoveSetOfMenuItemsFromBeverage_ThrowsForNull()
    {
        var beverage = CreateBeverage("Sprite", 2.5, "Soda", Beverage.BeverageType.Drinks, false);
        Assert.Throws<ArgumentNullException>(() => beverage.RemoveSetOfMenuItemsFromBeverage(null));
    }

    private Beverage CreateBeverage(string name, double price, string description, Beverage.BeverageType type, bool isAlcohol, List<Ingredient>? ingredients = null)
    {
        return new Beverage(_restaurant, name, price, description, type, isAlcohol, ingredients);
    }

    private SetOfMenuItem CreateSetOfMenuItem(string name, double price, string description)
    {
        return new SetOfMenuItem(_restaurant, name, price, description);
    }

    private List<Ingredient> CreateIngredients(params string[] ingredientNames)
    {
        return ingredientNames.Select(name => new Ingredient(name)).ToList();
    }

    private void AssertBeverageProperties(Beverage beverage, string name, double price, string description, Beverage.BeverageType type, bool isAlcohol)
    {
        Assert.Equal(name, beverage.Name);
        Assert.Equal(price, beverage.Price);
        Assert.Equal(description, beverage.Description);
        Assert.Equal(type, beverage.BeverageT);
        Assert.Equal(isAlcohol, beverage.IsAlcohol);
    }
}

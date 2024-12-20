using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class TestMenuItem : MenuItem
{
    private static List<TestMenuItem> _menuItems = new();

    public TestMenuItem(
        Restaurant restaurant,
        string name,
        double price,
        string description,
        bool isAvailable = true,
        List<Ingredient>? ingredients = null,
        Promotion? promotion = null
    ) : base(restaurant, name, price, description, isAvailable, ingredients, promotion)
    {
    }

    public static void AddMenuItem(TestMenuItem item) => _menuItems.Add(item);

    public static List<TestMenuItem> GetMenuItems() => new(_menuItems);
    public override void RemoveMenuItem()
    {
        throw new NotImplementedException();
    }
}

public class MenuItemTests
{
    private readonly Restaurant _restaurant;

    public MenuItemTests()
    {
        ResetStaticFields();
        _restaurant = CreateTestRestaurant();
    }

    private void ResetStaticFields()
    {
        typeof(TestMenuItem).GetField("_menuItems", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, new List<TestMenuItem>());
        typeof(Ingredient).GetField("_ingredients", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, new List<Ingredient>());
    }

    private Restaurant CreateTestRestaurant()
    {
        var address = new Address("Main St", "Test City", "123");
        var openHours = new List<OpenHour>
        {
            new(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0)),
            new(DayOfWeek.Sunday, new TimeSpan(10, 0, 0), new TimeSpan(20, 0, 0))
        };
        return new Restaurant("Testaurant", address, openHours);
    }

    private TestMenuItem CreateMenuItem(string name = "Sample Item", double price = 5.0, string description = "Sample Description", bool isAvailable = true)
    {
        return new TestMenuItem(_restaurant, name, price, description, isAvailable);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var ingredients = new List<Ingredient> { new("Cheese"), new("Bread") };
        var promotion = new Promotion(15, "Sale", "Discounted item");

        var menuItem = new TestMenuItem(_restaurant, "Grilled Cheese", 4.5, "A classic grilled cheese sandwich", true, ingredients, promotion);

        Assert.Equal("Grilled Cheese", menuItem.Name);
        Assert.Equal(4.5, menuItem.Price);
        Assert.Equal("A classic grilled cheese sandwich", menuItem.Description);
        Assert.True(menuItem.Id > 0);
        Assert.Equal(ingredients.Count, menuItem.Ingredients.Count);
        Assert.Equal(promotion, menuItem.Promotion);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidArguments()
    {
        Assert.Throws<ArgumentException>(() => new TestMenuItem(_restaurant, null, 10.0, "Test Description"));
        Assert.Throws<ArgumentException>(() => new TestMenuItem(_restaurant, "Test", -5.0, "Test Description"));
        Assert.Throws<ArgumentException>(() => new TestMenuItem(_restaurant, "Test", 10.0, null));
    }

    [Fact]
    public void AddIngredient_AddsIngredientToMenuItem()
    {
        var menuItem = CreateMenuItem("Pizza", 12.0, "Cheese Pizza");
        var ingredient = new Ingredient("Cheese");

        menuItem.AddIngredient(ingredient);

        Assert.Contains(ingredient, menuItem.Ingredients);
        Assert.Contains(menuItem, ingredient.IngredientInMenuItems);
    }

    [Fact]
    public void RemoveIngredient_RemovesIngredientFromMenuItem()
    {
        var menuItem = CreateMenuItem("Pizza", 12.0, "Cheese Pizza");
        var ingredient = new Ingredient("Cheese");

        menuItem.AddIngredient(ingredient);
        menuItem.RemoveIngredient(ingredient);

        Assert.DoesNotContain(ingredient, menuItem.Ingredients);
        Assert.DoesNotContain(menuItem, ingredient.IngredientInMenuItems);
    }

    [Fact]
    public void AddMenuItem_AddsItemToList()
    {
        var menuItem = CreateMenuItem("Burger", 7.5, "Beef Burger");
        TestMenuItem.AddMenuItem(menuItem);

        Assert.Contains(menuItem, TestMenuItem.GetMenuItems());
    }

    [Fact]
    public void GetMenuItems_ReturnsCorrectListOfItems()
    {
        var item1 = CreateMenuItem("Burger", 7.5, "Beef Burger");
        var item2 = CreateMenuItem("Fries", 2.5, "Crispy French Fries");
        TestMenuItem.AddMenuItem(item1);
        TestMenuItem.AddMenuItem(item2);

        var items = TestMenuItem.GetMenuItems();

        Assert.Equal(2, items.Count);
        Assert.Contains(item1, items);
        Assert.Contains(item2, items);
    }

    [Fact]
    public void UpdatePromotion_UpdatesPromotionCorrectly()
    {
        var menuItem = CreateMenuItem("Salad", 5.0, "Fresh Salad");
        var promotion = new Promotion(20, "Discount", "20% Off");

        menuItem.AddPromotion(promotion);
        Assert.Equal(promotion, menuItem.Promotion);

        menuItem.RemovePromotion();
        Assert.Null(menuItem.Promotion);
    }

    [Fact]
    public void MakeAvailable_MakesMenuItemAvailable()
    {
        var menuItem = CreateMenuItem("Soup", 4.0, "Tomato Soup", false);

        menuItem.MakeAvailable();

        Assert.True(menuItem.IsAvailable);
    }

    [Fact]
    public void MakeUnavailable_MakesMenuItemUnavailable()
    {
        var menuItem = CreateMenuItem("Soup", 4.0, "Tomato Soup", true);

        menuItem.MakeUnavailable();

        Assert.False(menuItem.IsAvailable);
    }
}

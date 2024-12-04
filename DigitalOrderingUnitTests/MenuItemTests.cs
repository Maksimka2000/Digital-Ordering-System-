using System.Reflection;
using DigitalOrdering;
using Newtonsoft.Json;
using Xunit;

namespace DigitalOrderingUnitTests;

// We can't create a object of class MenuItem because it's abstract so we created this class for testing purposes
public class TestMenuItem(
    string name,
    double price,
    string description,
    List<Ingredient>? ingredients = null,
    Promotion? promotion = null)
    : MenuItem(name, price, description, ingredients, promotion)
{
    private static List<TestMenuItem> _menuItems = [];

    public static void AddMenuItem(TestMenuItem item)
    {
        _menuItems.Add(item);
    }

    public static List<TestMenuItem> GetMenuItems()
    {
        return [.._menuItems];
    }

    public static void ClearMenuItems()
    {
        _menuItems.Clear();
    }

    public static void SaveMenuItemsJson(string path)
    {
        var json = JsonConvert.SerializeObject(_menuItems, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public static void LoadMenuItemsJson(string path)
    {
        if (!File.Exists(path)) return;
        var json = File.ReadAllText(path);
        _menuItems = JsonConvert.DeserializeObject<List<TestMenuItem>>(json) ?? new List<TestMenuItem>();
    }
}

public class MenuItemTests
{
    public MenuItemTests()
    {
        TestMenuItem.ClearMenuItems();
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "Grilled Cheese";
        const double price = 4.5;
        const string description = "A classic grilled cheese sandwich";
        var ingredients = new List<Ingredient> { new("Cheese"), new("Bread") };
        var promotion = new Promotion(15, "Sale", "Product sale");

        var menuItem = new TestMenuItem(name, price, description, ingredients, promotion);

        Assert.Equal(name, menuItem.Name);
        Assert.Equal(price, menuItem.Price);
        Assert.Equal(description, menuItem.Description);
        // Assert.Equal(ingredients, menuItem.Ingredients);
        // Assert.Equal(promotion, menuItem.Promotion);
        Assert.True(menuItem.Id > 0);
    }


    [Fact]
    public void Id_Getter_ReturnsCorrectValue()
    {
        var menuItem = new TestMenuItem("Salad", 3.5, "Green Salad");
        Assert.True(menuItem.Id > 0);
    }

    [Fact]
    public void Name_Getter_ReturnsCorrectValue()
    {
        var menuItem = new TestMenuItem("Soup", 5.0, "Tomato Soup");
        Assert.Equal("Soup", menuItem.Name);
    }

    [Fact]
    public void Price_Getter_ReturnsCorrectValue()
    {
        var menuItem = new TestMenuItem("Pasta", 8.0, "Pasta with Marinara Sauce");
        Assert.Equal(8.0, menuItem.Price);
    }

    [Fact]
    public void Description_Getter_ReturnsCorrectValue()
    {
        var menuItem = new TestMenuItem("Pizza", 10.0, "Cheese Pizza");
        Assert.Equal("Cheese Pizza", menuItem.Description);
    }

    [Fact]
    public void AddMenuItem_AddsItemToList()
    {
        var menuItem = new TestMenuItem("Burger", 7.5, "Beef Burger");
        TestMenuItem.AddMenuItem(menuItem);

        var items = TestMenuItem.GetMenuItems();
        Assert.Contains(menuItem, items);
    }

    [Fact]
    public void GetMenuItems_ReturnsCorrectListOfItems()
    {
        var item1 = new TestMenuItem("Burger", 7.5, "Beef Burger");
        var item2 = new TestMenuItem("Fries", 2.5, "Crispy French Fries");
        TestMenuItem.AddMenuItem(item1);
        TestMenuItem.AddMenuItem(item2);

        var items = TestMenuItem.GetMenuItems();

        Assert.Equal(2, items.Count);
        Assert.Contains(item1, items);
        Assert.Contains(item2, items);
    }

    [Fact]
    public void SaveMenuItemsJSON_SavesItemsToFile()
    {
        var menuItem = new TestMenuItem("Burger", 7.5, "Beef Burger");
        TestMenuItem.AddMenuItem(menuItem);
        const string path = "test_menuitems.json";

        TestMenuItem.SaveMenuItemsJson(path);
        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadMenuItemsJSON_LoadsItemsFromFile()
    {
        const string path = "test_menuitems.json";
        var menuItem = new TestMenuItem("Pizza", 10.0, "Cheese Pizza");
        TestMenuItem.AddMenuItem(menuItem);
        TestMenuItem.SaveMenuItemsJson(path);
        TestMenuItem.ClearMenuItems(); // Clear list to test reloading

        TestMenuItem.LoadMenuItemsJson(path);
        var items = TestMenuItem.GetMenuItems();

        Assert.Single(items);
        Assert.Equal(menuItem.Name, items[0].Name);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidArguments()
    {
        Assert.Throws<ArgumentException>(() => new TestMenuItem(null, 10.0, "Test Description"));
        Assert.Throws<ArgumentException>(() => new TestMenuItem("Test", -5.0, "Test Description"));
        Assert.Throws<ArgumentException>(() => new TestMenuItem("Test", 10.0, null));
    }
    //
    // [Fact]
    // public void UpdateIngredients_ThrowsExceptionForEmptyIngredientsList()
    // {
    //     var menuItem = new TestMenuItem("Salad", 3.0, "Green Salad");
    //     Assert.Throws<ArgumentException>(() => menuItem.UpdateIngredients(new List<Ingredient>()));
    // }
}
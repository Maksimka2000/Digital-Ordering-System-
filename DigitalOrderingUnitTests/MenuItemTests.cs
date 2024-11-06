using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

// We can't create a object of class MenuItem because it's abstract so we created this class for testing purposes
public class TestMenuItem(
    string name,
    double price,
    string description,
    List<Ingredient>? ingredients = null,
    Promotion? promotion = null)
    : MenuItem(name, price, description, ingredients, promotion);

public class MenuItemTests
{
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
        Assert.Equal(ingredients, menuItem.Ingredients);
        Assert.Equal(promotion, menuItem.Promotion);
        Assert.True(menuItem.Id > 0);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidArguments()
    {
        Assert.Throws<ArgumentException>(() => new TestMenuItem(null, 10.0, "Test Description"));
        Assert.Throws<ArgumentException>(() => new TestMenuItem("Test", -5.0, "Test Description"));
        Assert.Throws<ArgumentException>(() => new TestMenuItem("Test", 10.0, null));
    }

    [Fact]
    public void UpdateName_ChangesMenuItemName()
    {
        var menuItem = new TestMenuItem("Burger", 5.0, "Classic Burger");

        menuItem.UpdateName("Cheeseburger");

        Assert.Equal("Cheeseburger", menuItem.Name);
    }

    [Fact]
    public void UpdatePrice_ChangesMenuItemPrice()
    {
        var menuItem = new TestMenuItem("Pizza", 8.0, "Cheese Pizza");

        menuItem.UpdatePrice(9.0);

        Assert.Equal(9.0, menuItem.Price);
    }

    [Fact]
    public void UpdateDescription_ChangesMenuItemDescription()
    {
        var menuItem = new TestMenuItem("Soda", 1.5, "Refreshing Soda");

        menuItem.UpdateDescription("Chilled Soda");

        Assert.Equal("Chilled Soda", menuItem.Description);
    }

    [Fact]
    public void UpdateIngredients_ChangesMenuItemIngredients()
    {
        var initialIngredients = new List<Ingredient> { new("Milk") };
        var newIngredients = new List<Ingredient> { new("Sugar"), new("Vanilla") };
        var menuItem = new TestMenuItem("Ice Cream", 2.5, "Vanilla Ice Cream", initialIngredients);

        menuItem.UpdateIngredients(newIngredients);

        Assert.Equal(newIngredients, menuItem.Ingredients);
    }

    [Fact]
    public void UpdatePromotion_ChangesMenuItemPromotion()
    {
        var promotion = new Promotion(10, "Big sale", "Good promo");
        var menuItem = new TestMenuItem("Coffee", 2.0, "Hot Coffee");

        menuItem.UpdatePromotion(promotion);

        Assert.Equal(promotion, menuItem.Promotion);
    }

    [Fact]
    public void UpdateIngredients_ThrowsExceptionForEmptyIngredientsList()
    {
        var menuItem = new TestMenuItem("Salad", 3.0, "Green Salad");
        Assert.Throws<ArgumentException>(() => menuItem.UpdateIngredients(new List<Ingredient>()));
    }

    [Fact]
    public void GetIngredients_ReturnsCorrectIngredients()
    {
        var ingredients = new List<Ingredient> { new("Tomato"), new("Lettuce") };
        var menuItem = new TestMenuItem("Salad", 3.0, "Fresh Salad", ingredients);

        var returnedIngredients = menuItem.GetIngredients();

        Assert.Equal(ingredients, returnedIngredients);
    }
}
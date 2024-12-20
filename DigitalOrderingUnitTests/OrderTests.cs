using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class TestOrder : Order
{
    private static List<TestOrder> _orders = [];

    public TestOrder(int numberOfPeople) : base(numberOfPeople)
    {
        var ingredientsField = typeof(Ingredient).GetField("_ingredients",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (ingredientsField != null)
        {
            var currentIngredients = (List<Ingredient>)ingredientsField.GetValue(null);
            if (currentIngredients != null)
            {
                currentIngredients.Clear(); 
            }
            else
            {
                ingredientsField.SetValue(null, new List<Ingredient>());
            }
        }

        AddOrder(this);
    }


    public static void AddOrder(TestOrder order)
    {
        _orders.Add(order);
    }

    public static List<TestOrder> GetOrders()
    {
        return [.._orders];
    }

    public override void RemoveOrder()
    {
        throw new NotImplementedException();
    }
}

public class OrderTests
{
    private readonly Restaurant _restaurant;
    private readonly MenuItem _menuItemWithPromo;
    private readonly MenuItem _menuItemWithoutPromo;

    public OrderTests()
    {
        typeof(Ingredient)
            .GetField("_ingredients", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, new List<Ingredient>());

        RestoreStaticOrders();

        var address = new Address("Main St", "Test City", "123");
        var openHours = new List<OpenHour>
        {
            new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0)),
            new OpenHour(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0)),
            new OpenHour(DayOfWeek.Sunday, new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0))
        };
        _restaurant = new Restaurant("Testaurant", address, openHours);

        var ingredients = new List<Ingredient>
        {
            new Ingredient("Cheese"),
            new Ingredient("Tomato Sauce")
        };

        var promotion = new Promotion(10, "Discount", "10% off");

        _menuItemWithPromo = new Food(_restaurant, "Pizza", 20.0, "Cheese Pizza", Food.FoodType.Pasta, ingredients,
            null, promotion, true);
        _menuItemWithoutPromo =
            new Beverage(_restaurant, "Soda", 10.0, "Coca-Cola", Beverage.BeverageType.Drinks, false);
    }

    private void RestoreStaticOrders()
    {
        typeof(Order)
            .GetField("_orders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, new List<Order>());
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const int numberOfPeople = 3;
        var order = new TestOrder(numberOfPeople);

        Assert.Equal(numberOfPeople, order.NumberOfPeople);
        Assert.Equal(0, order.OrderPrice);
        Assert.Equal(0, order.TotalPrice);
        Assert.NotEqual(0, order.Id);
        Assert.Null(order.StartTime); 

        var orders = TestOrder.GetOrders();
        Assert.Contains(order, orders); 
        Assert.Single(orders); 
    }


    [Fact]
    public void AddMenuItem_ThrowsExceptionForInvalidQuantity()
    {
        var order = new TestOrder(2);
        Assert.Throws<ArgumentException>(() => order.AddMenuItemToOrder(_menuItemWithPromo, 0));
        Assert.Throws<ArgumentException>(() => order.AddMenuItemToOrder(_menuItemWithPromo, -1));
    }

    [Fact]
    public void AddMenuItem_ThrowsExceptionForInvalidMenuItem()
    {
        var order = new TestOrder(3);
        Assert.Throws<ArgumentNullException>(() => order.AddMenuItemToOrder(null, 1));
    }


    [Fact]
    public void ChangeService_UpdatesServiceChargeCorrectly()
    {
        Order.ChangeService(15);
        Assert.Equal(15, Order.Service);

        Order.ChangeService(0); 
        Assert.Equal(0, Order.Service);
    }

    [Fact]
    public void ChangeService_ThrowsExceptionForInvalidServiceCharge()
    {
        Assert.Throws<ArgumentException>(() => Order.ChangeService(-10));
        Assert.Throws<ArgumentException>(() => Order.ChangeService(150));
    }
}
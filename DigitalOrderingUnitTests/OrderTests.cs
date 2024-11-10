using DigitalOrdering;
using Newtonsoft.Json;
using Xunit;

namespace DigitalOrderingUnitTests;

// We can't create a object of class MenuItem because it's abstract so we created this class for testing purposes
public class TestOrder : Order
{
    private static List<TestOrder> _orders = [];

    public TestOrder(int numberOfPeople) : base(numberOfPeople)
    {
        CalculateTotalPrice();
    }

    // Extent management methods
    public static void AddOrder(TestOrder order)
    {
        _orders.Add(order);
    }

    public static List<TestOrder> GetOrders()
    {
        return [.._orders];
    }

    public static void ClearOrders()
    {
        _orders.Clear();
    }

    // File persistence methods
    public static void SaveOrdersJSON(string path)
    {
        var json = JsonConvert.SerializeObject(_orders, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public static void LoadOrdersJSON(string path)
    {
        if (!File.Exists(path)) return;
        var json = File.ReadAllText(path);
        _orders = JsonConvert.DeserializeObject<List<TestOrder>>(json) ?? new List<TestOrder>();
    }
}

public class OrderTests
{
    public OrderTests()
    {
        TestOrder.ClearOrders();
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
    }

    [Fact]
    public void Id_Getter_ReturnsCorrectValue()
    {
        var order = new TestOrder(2);
        Assert.True(order.Id > 0);
    }

    [Fact]
    public void NumberOfPeople_Getter_ReturnsCorrectValue()
    {
        var order = new TestOrder(5);
        Assert.Equal(5, order.NumberOfPeople);
    }

    [Fact]
    public void AddOrder_AddsOrderToList()
    {
        var order = new TestOrder(4);
        TestOrder.AddOrder(order);

        var orders = TestOrder.GetOrders();
        Assert.Contains(order, orders);
    }

    [Fact]
    public void GetOrders_ReturnsCorrectListOfOrders()
    {
        var order1 = new TestOrder(3);
        var order2 = new TestOrder(5);
        TestOrder.AddOrder(order1);
        TestOrder.AddOrder(order2);

        var orders = TestOrder.GetOrders();

        Assert.Equal(2, orders.Count);
        Assert.Contains(order1, orders);
        Assert.Contains(order2, orders);
    }

    [Fact]
    public void SaveOrdersJSON_SavesOrdersToFile()
    {
        var order = new TestOrder(3);
        TestOrder.AddOrder(order);
        const string path = "test_orders.json";

        TestOrder.SaveOrdersJSON(path);
        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadOrdersJSON_LoadsOrdersFromFile()
    {
        const string path = "test_orders.json";
        var order = new TestOrder(2);
        TestOrder.AddOrder(order);
        TestOrder.SaveOrdersJSON(path);
        TestOrder.ClearOrders();

        TestOrder.LoadOrdersJSON(path);
        var orders = TestOrder.GetOrders();

        Assert.Single(orders);
        Assert.Equal(order.NumberOfPeople, orders[0].NumberOfPeople);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidNumberOfPeople()
    {
        Assert.Throws<ArgumentException>(() => new TestOrder(0));
        Assert.Throws<ArgumentException>(() => new TestOrder(-5));
    }

    [Fact]
    public void ChangeService_ThrowsExceptionForInvalidServiceCharge()
    {
        Assert.Throws<ArgumentException>(() => Order.ChangeService(-10));
        Assert.Throws<ArgumentException>(() => Order.ChangeService(150));
    }
}
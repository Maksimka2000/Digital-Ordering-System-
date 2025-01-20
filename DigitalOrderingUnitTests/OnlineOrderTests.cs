using DigitalOrdering;
using Xunit;
using System.Reflection;
using DidgitalOrdering;

namespace DigitalOrderingUnitTests;

public class OnlineOrderTests
{
    private readonly Restaurant _restaurant;
    private readonly DateTime _validOpenHours;

    public OnlineOrderTests()
    {
        ResetStaticOrders();
        _restaurant = InitializeRestaurant();
        InitializeTables();
        _validOpenHours = DateTime.Now.AddDays(3).Date + new TimeSpan(13, 0, 0);
    }

    private void ResetStaticOrders()
    {
        typeof(OnlineOrder)
            .GetField("_onlineOrders", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<OnlineOrder>());
    }

    private Restaurant InitializeRestaurant()
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
            new(DayOfWeek.Sunday, new TimeSpan(9, 0, 0), new TimeSpan(21, 0, 0))
        };
        return new Restaurant("Testaurant", address, openHours);
    }

    private void InitializeTables()
    {
        new Table(_restaurant, 1, "Small Table 1", "Single-person table");
        new Table(_restaurant, 2, "Medium Table 1", "Two-person table");
        new Table(_restaurant, 4, "Large Table 1", "Four-person table");
    }

    private OnlineOrder CreateOrder(int numberOfPeople, string description, NonRegisteredClient? client = null,
        TimeSpan? duration = null)
    {
        return new OnlineOrder(
            _restaurant,
            numberOfPeople,
            _validOpenHours,
            duration,
            description,
            null,
            null,
            new NonRegisteredClient("John Doe", "+48 123 456 789")
        );
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var client = new NonRegisteredClient("John Doe", "+48 123 456 789");
        const int numberOfPeople = 4;
        const string description = "Birthday Party";

        var onlineOrder = CreateOrder(numberOfPeople, description, client);

        Assert.Equal(numberOfPeople, onlineOrder.NumberOfPeople);
        Assert.Equal(_validOpenHours, onlineOrder.DateAndTime);
        Assert.Equal(description, onlineOrder.Description);
        Assert.False(onlineOrder.IsGuestsArrived);
        Assert.Null(onlineOrder.StartTime);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidDateAndTime()
    {
        var client = new NonRegisteredClient("Jane Doe", "+48 123 456 789");

        Assert.Throws<ArgumentException>(() =>
            new OnlineOrder(_restaurant, 2, DateTime.Now.AddDays(3).Date + new TimeSpan(22, 0, 0), null, "Too late",
                null, null, client));
    }

    [Fact]
    public void MarkAsGuestsArrived_SetsHaveGuestsArrivedToTrue()
    {
        var onlineOrder = CreateOrder(2, "Lunch");

        onlineOrder.MarkAsGuestsArrived();

        Assert.True(onlineOrder.IsGuestsArrived);
        Assert.NotNull(onlineOrder.StartTime);
    }

    [Fact]
    public void AddOnlineOrder_AddsOrderToList()
    {
        var onlineOrder = CreateOrder(1, "Dinner");

        var orders = OnlineOrder.GetOnlineOrders();

        Assert.Contains(onlineOrder, orders);
    }

    [Fact]
    public void GetOnlineOrders_ReturnsCorrectList()
    {
        var client1 = new NonRegisteredClient("Alice", "+48 987 654 321");
        var client2 = new NonRegisteredClient("Bob", "+48 654 321 987");

        var order1 = CreateOrder(4, "Party", client1);
        var order2 = CreateOrder(2, "Dinner", client2);

        var orders = OnlineOrder.GetOnlineOrders();

        Assert.Equal(2, orders.Count);
        Assert.Contains(order1, orders);
        Assert.Contains(order2, orders);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidDuration()
    {
        var client = new NonRegisteredClient("John Doe", "+48 123 456 789");

        Assert.Throws<ArgumentException>(() =>
            CreateOrder(2, "Too short", client, new TimeSpan(0, 30, 0)));
    }
    
    [Fact]
    public void UpdateOnlineOrder_ShouldUpdateFieldsCorrectly()
    {
        var initialDateAndTime = DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0);
        var initialDuration = TimeSpan.FromHours(2);
        
        var updatedDateAndTime = DateTime.Now.AddDays(3).Date + new TimeSpan(12, 0, 0);
        var updatedDuration = TimeSpan.FromHours(3);

        var onlineOrder = new OnlineOrder(
            restaurant: _restaurant,
            numberOfPeople: 2,
            dateAndTime: initialDateAndTime,
            duration: initialDuration,
            description: "Initial Description",
            null,
            null,
            new NonRegisteredClient("John Doe", "+48 123 456 789")
        );

        // Act
        onlineOrder.UpdateOnlineOrder(
            dateAndTime: updatedDateAndTime,
            duration: updatedDuration,
            numberOfPeople: 3,
            description: "Updated Description"
        );

        // Assert
        Assert.Equal(updatedDateAndTime, onlineOrder.DateAndTime);
        Assert.Equal(updatedDuration, onlineOrder.Duration);
        Assert.Equal(3, onlineOrder.NumberOfPeople);
        Assert.Equal("Updated Description", onlineOrder.Description);
    }
}
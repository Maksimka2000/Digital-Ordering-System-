using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class OnlineOrderTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const int numberOfPeople = 4;
        var dateAndTime = DateTime.Now.AddHours(1);
        const string description = "Birthday Party";

        var onlineOrder = new OnlineOrder(numberOfPeople, dateAndTime, description);

        Assert.Equal(numberOfPeople, onlineOrder.NumberOfPeople);
        Assert.Equal(dateAndTime, onlineOrder.DateAndTime);
        Assert.Equal(description, onlineOrder.Description);
        Assert.False(onlineOrder.HaveGuestsArrived);
        Assert.Null(onlineOrder.StartTime);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectlyWithoutDescription()
    {
        const int numberOfPeople = 2;
        var dateAndTime = DateTime.Now.AddHours(2);

        var onlineOrder = new OnlineOrder(numberOfPeople, dateAndTime);

        Assert.Equal(numberOfPeople, onlineOrder.NumberOfPeople);
        Assert.Equal(dateAndTime, onlineOrder.DateAndTime);
        Assert.Null(onlineOrder.Description);
    }

    [Fact]
    public void AddOnlineOrder_AddsOrderToList()
    {
        var onlineOrder = new OnlineOrder(3, DateTime.Now.AddHours(1), "Lunch Reservation");

        OnlineOrder.AddOnlineOrder(onlineOrder);
        var onlineOrders = OnlineOrder.GetOnlineOrders();

        Assert.Contains(onlineOrder, onlineOrders);
    }

    [Fact]
    public void GetOnlineOrders_ReturnsListOfOrders()
    {
        var onlineOrder1 = new OnlineOrder(2, DateTime.Now.AddHours(2), "Dinner Reservation");
        var onlineOrder2 = new OnlineOrder(4, DateTime.Now.AddHours(3), "Event Reservation");
        OnlineOrder.AddOnlineOrder(onlineOrder1);
        OnlineOrder.AddOnlineOrder(onlineOrder2);

        var onlineOrders = OnlineOrder.GetOnlineOrders();

        Assert.Equal(2, onlineOrders.Count);
        Assert.Contains(onlineOrder1, onlineOrders);
        Assert.Contains(onlineOrder2, onlineOrders);
    }

    [Fact]
    public void MarkAsGuestsArrived_SetsHaveGuestsArrivedAndStartTime()
    {
        var onlineOrder = new OnlineOrder(5, DateTime.Now.AddHours(1), "Conference Booking");

        onlineOrder.MarkAsGuestsArrived();

        Assert.True(onlineOrder.HaveGuestsArrived);
        Assert.NotNull(onlineOrder.StartTime);
    }

    [Fact]
    public void SaveOnlineOrderJSON_SavesOrdersToFile()
    {
        var onlineOrder = new OnlineOrder(3, DateTime.Now.AddHours(1), "Meeting");
        OnlineOrder.AddOnlineOrder(onlineOrder);
        const string path = "test_online_orders.json";

        OnlineOrder.SaveOnlineOrderJSON(path);

        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadOnlineOrderJSON_LoadsOrdersFromFile()
    {
        const string path = "test_online_orders.json";
        var onlineOrder = new OnlineOrder(2, DateTime.Now.AddHours(2), "Dinner for Two");
        OnlineOrder.AddOnlineOrder(onlineOrder);
        OnlineOrder.SaveOnlineOrderJSON(path);
        OnlineOrder.GetOnlineOrders().Clear(); 

        OnlineOrder.LoadOnlineOrderJSON(path);
        var onlineOrders = OnlineOrder.GetOnlineOrders();

        Assert.Single(onlineOrders);
        Assert.Equal(onlineOrder.Description, onlineOrders[0].Description);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForEmptyDescription()
    {
        Assert.Throws<ArgumentException>(() => new OnlineOrder(2, DateTime.Now.AddHours(1), ""));
    }
}
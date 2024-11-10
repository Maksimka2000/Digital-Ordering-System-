using DigitalOrdering;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DigitalOrderingUnitTests;

public class OnlineOrderTests
{
    public OnlineOrderTests()
    {
        typeof(OnlineOrder)
            .GetField("_onlineOrders", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<OnlineOrder>());
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const int numberOfPeople = 4;
        var dateAndTime = DateTime.Now.AddHours(4);
        const string description = "Birthday Party";

        var onlineOrder = new OnlineOrder(numberOfPeople, dateAndTime, description);

        Assert.Equal(numberOfPeople, onlineOrder.NumberOfPeople);
        Assert.Equal(dateAndTime, onlineOrder.DateAndTime);
        Assert.Equal(description, onlineOrder.Description);
        Assert.False(onlineOrder.HaveGuestsArrived);
        Assert.Null(onlineOrder.StartTime);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly_WithOptionalDescription()
    {
        const int numberOfPeople = 2;
        var dateAndTime = DateTime.Now.AddHours(4);

        var onlineOrder = new OnlineOrder(numberOfPeople, dateAndTime);

        Assert.Equal(numberOfPeople, onlineOrder.NumberOfPeople);
        Assert.Equal(dateAndTime, onlineOrder.DateAndTime);
        Assert.Null(onlineOrder.Description);
    }

    [Fact]
    public void DateAndTime_Getter_ReturnsCorrectValue()
    {
        var dateAndTime = DateTime.Now.AddHours(5);
        var onlineOrder = new OnlineOrder(3, dateAndTime, "Dinner");

        Assert.Equal(dateAndTime, onlineOrder.DateAndTime);
    }

    [Fact]
    public void HaveGuestsArrived_IsInitiallyFalse()
    {
        var onlineOrder = new OnlineOrder(3, DateTime.Now.AddHours(5), "Dinner");

        Assert.False(onlineOrder.HaveGuestsArrived);
    }

    [Fact]
    public void Description_Getter_ReturnsCorrectValue()
    {
        var onlineOrder = new OnlineOrder(2, DateTime.Now.AddHours(5), "Lunch");

        Assert.Equal("Lunch", onlineOrder.Description);
    }

    [Fact]
    public void StartTime_Getter_IsInitiallyNull()
    {
        var onlineOrder = new OnlineOrder(2, DateTime.Now.AddHours(5), "Lunch");

        Assert.Null(onlineOrder.StartTime);
    }

    [Fact]
    public void AddOnlineOrder_AddsOrderToList()
    {
        var onlineOrder = new OnlineOrder(3, DateTime.Now.AddHours(5), "Dinner Party");
        OnlineOrder.AddOnlineOrder(onlineOrder);

        var orders = OnlineOrder.GetOnlineOrders();
        Assert.Contains(onlineOrder, orders);
    }

    [Fact]
    public void GetOnlineOrders_ReturnsCorrectListOfOrders()
    {
        var order1 = new OnlineOrder(4, DateTime.Now.AddHours(5), "Birthday Celebration");
        var order2 = new OnlineOrder(2, DateTime.Now.AddHours(6), "Anniversary Dinner");

        OnlineOrder.AddOnlineOrder(order1);
        OnlineOrder.AddOnlineOrder(order2);

        var orders = OnlineOrder.GetOnlineOrders();

        Assert.Equal(2, orders.Count);
        Assert.Contains(order1, orders);
        Assert.Contains(order2, orders);
    }

    [Fact]
    public void SaveOnlineOrderJSON_SavesOrdersToFile()
    {
        var onlineOrder = new OnlineOrder(3, DateTime.Now.AddHours(5), "Team Meeting");
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
        var onlineOrder = new OnlineOrder(2, DateTime.Now.AddHours(5), "Family Dinner");
        OnlineOrder.AddOnlineOrder(onlineOrder);
        OnlineOrder.SaveOnlineOrderJSON(path);
        OnlineOrder.GetOnlineOrders().Clear();

        OnlineOrder.LoadOnlineOrderJSON(path);
        var orders = OnlineOrder.GetOnlineOrders();

        Assert.Single(orders);
        Assert.Equal(onlineOrder.Description, orders[0].Description);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForEmptyDescription()
    {
        Assert.Throws<ArgumentException>(() => new OnlineOrder(2, DateTime.Now.AddHours(5), ""));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForDateAndTimeInPast()
    {
        Assert.Throws<ArgumentException>(() => new OnlineOrder(2, DateTime.Now.AddHours(1), "Past Event"));
    }
}
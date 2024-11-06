using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

// We can't create a object of class MenuItem because it's abstract so we created this class for testing purposes
public class TestOrder : Order
{
    public TestOrder(int numberOfPeople) : base(numberOfPeople)
    {
        CalculateTotalPrice();
    }
}

public class OrderTests
{
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
    public void Constructor_ThrowsExceptionForInvalidNumberOfPeople()
    {
        Assert.Throws<ArgumentException>(() => new TestOrder(0));
        Assert.Throws<ArgumentException>(() => new TestOrder(-5));
    }

    [Fact]
    public void ChangeService_SetsServiceChargeCorrectly()
    {
        const double service = 15;
        Order.ChangeService(service);
        var order = new TestOrder(2);

        order.CalculateTotalPrice();

        Assert.Equal(85, order.TotalPrice);
    }

    [Fact]
    public void ChangeService_ThrowsExceptionForInvalidServiceCharge()
    {
        Assert.Throws<ArgumentException>(() => Order.ChangeService(-10));
        Assert.Throws<ArgumentException>(() => Order.ChangeService(150));
    }

    [Fact]
    public void CalculateTotalPrice_CalculatesCorrectlyWithDefaultServiceCharge()
    {
        var order = new TestOrder(2);

        order.CalculateTotalPrice();

        Assert.Equal(180, order.TotalPrice);
    }

    [Fact]
    public void CalculateTotalPrice_CalculatesCorrectlyAfterServiceChange()
    {
        var order = new TestOrder(2);
        Order.ChangeService(20);

        order.CalculateTotalPrice();

        Assert.Equal(160, order.TotalPrice);
    }
}
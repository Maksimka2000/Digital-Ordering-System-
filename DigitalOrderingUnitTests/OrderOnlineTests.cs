using Xunit;
using DigitalOrdering;

public class OrderOnlineTests
{
    [Fact]
    public void ApplyDiscount()
    {
        var order = new OrderOnline();
        order.ApplyDiscount(10.0);
        Assert.Equal(10.0, order.Discount);
    }
}
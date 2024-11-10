using DidgitalOrdering;
using Xunit;
using DigitalOrdering;

namespace DigitalOrderingUnitTests;

public class NonRegisteredClientTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "John Doe";
        const string phoneNumber = "+48 123 456 789";

        var client = new NonRegisteredClient(name, phoneNumber);

        Assert.Equal(name, client.Name);
        Assert.Equal(phoneNumber, client.PhoneNumber);
    }

    [Fact]
    public void Name_Getter_ReturnsCorrectValue()
    {
        var client = new NonRegisteredClient("Jane Doe", "+48 987 654 321");
        Assert.Equal("Jane Doe", client.Name);
    }

    [Fact]
    public void PhoneNumber_Getter_ReturnsCorrectValue()
    {
        var client = new NonRegisteredClient("Client Name", "+48 123 456 789");
        Assert.Equal("+48 123 456 789", client.PhoneNumber);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidName()
    {
        Assert.Throws<ArgumentException>(() => new NonRegisteredClient("", "+48 123 456 789"));
        Assert.Throws<ArgumentException>(() => new NonRegisteredClient(null, "+48 123 456 789"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidPhoneNumber()
    {
        Assert.Throws<ArgumentException>(() => new NonRegisteredClient("John Doe", ""));
        Assert.Throws<ArgumentException>(() => new NonRegisteredClient("John Doe", "invalid-phone"));
    }
}

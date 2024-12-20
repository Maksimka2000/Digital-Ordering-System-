using DidgitalOrdering;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class NonRegisteredClientTests
{
    private const string ValidName = "John Doe";
    private const string ValidPhoneNumber = "+48 123 456 789";

    private NonRegisteredClient CreateClient(string name = ValidName, string phoneNumber = ValidPhoneNumber)
    {
        return new NonRegisteredClient(name, phoneNumber);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var client = CreateClient();

        Assert.Equal(ValidName, client.Name);
        Assert.Equal(ValidPhoneNumber, client.PhoneNumber);
    }

    [Fact]
    public void Name_Getter_ReturnsCorrectValue()
    {
        var client = CreateClient(name: "Jane Doe");

        Assert.Equal("Jane Doe", client.Name);
    }

    [Fact]
    public void PhoneNumber_Getter_ReturnsCorrectValue()
    {
        var client = CreateClient(phoneNumber: "+48 987 654 321");

        Assert.Equal("+48 987 654 321", client.PhoneNumber);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForEmptyName()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(name: ""));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForNullName()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(name: null));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForEmptyPhoneNumber()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(phoneNumber: ""));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidPhoneNumber()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(phoneNumber: "invalid-phone"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForNullPhoneNumber()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(phoneNumber: null));
    }

    [Fact]
    public void Constructor_AcceptsValidPhoneNumberFormat1()
    {
        var client = CreateClient(phoneNumber: "+48 123 456 789");

        Assert.Equal("+48 123 456 789", client.PhoneNumber);
    }

    [Fact]
    public void Constructor_AcceptsValidPhoneNumberFormat2()
    {
        var client = CreateClient(phoneNumber: "123 456 789");

        Assert.Equal("123 456 789", client.PhoneNumber);
    }

    [Fact]
    public void Constructor_AcceptsValidPhoneNumberFormat3()
    {
        var client = CreateClient(phoneNumber: "+48123456789");

        Assert.Equal("+48123456789", client.PhoneNumber);
    }

    [Fact]
    public void Constructor_AcceptsValidPhoneNumberFormat4()
    {
        var client = CreateClient(phoneNumber: "123-456-789");

        Assert.Equal("123-456-789", client.PhoneNumber);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForShortPhoneNumber()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(phoneNumber: "12345"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidPhoneNumberFormat()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(phoneNumber: "+12 345 6789"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForNonNumericPhoneNumber()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(phoneNumber: "abcd-efg-hijk"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForIncompletePhoneNumber()
    {
        Assert.Throws<ArgumentException>(() => CreateClient(phoneNumber: "1234-567"));
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var client = CreateClient();

        Assert.Equal($"Name: {ValidName}, PhoneNumber: {ValidPhoneNumber}", client.ToString());
    }
}

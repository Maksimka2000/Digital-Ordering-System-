using DigitalOrdering;
using Xunit;
using System.Reflection;

namespace DigitalOrderingUnitTests;

public class RegisteredClientTests
{
    public RegisteredClientTests()
    {
        ResetRegisteredClients();
    }

    private void ResetRegisteredClients()
    {
        typeof(RegisteredClient)
            .GetField("_registeredClients", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<RegisteredClient>());
    }

    private RegisteredClient CreateClient(
        string name = "John",
        string password = "P@ssw0rd1!",
        string email = "john.doe@example.com",
        string phoneNumber = "+48 123 456 789",
        string surname = "Doe")
    {
        return new RegisteredClient(name, password, email, phoneNumber, surname);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var client = CreateClient();

        Assert.Equal("John", client.Name);
        Assert.Equal("P@ssw0rd1!", client.Password);
        Assert.Equal("Doe", client.Surname);
        Assert.Equal("john.doe@example.com", client.Email);
        Assert.Equal("+48 123 456 789", client.PhoneNumber);
        Assert.Equal(0, client.Bonus);
        Assert.True(client.Id > 0);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForEmptyPassword()
    {
        Assert.Throws<ArgumentException>(() => new RegisteredClient("User", "", "user@example.com"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForEmptyEmail()
    {
        Assert.Throws<ArgumentException>(() => new RegisteredClient("User", "P@ssw0rd1!", ""));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForNullEmailAndPhoneNumber()
    {
        Assert.Throws<NullReferenceException>(() => new RegisteredClient("User", "P@ssw0rd1!"));
    }

    [Fact]
    public void UpdatePassword_UpdatesPasswordCorrectly()
    {
        var client = CreateClient();
        client.UpdatePassword("NewP@ssw0rd!2");

        Assert.Equal("NewP@ssw0rd!2", client.Password);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForShortPassword()
    {
        Assert.Throws<ArgumentException>(() => new RegisteredClient("User", "pass", "user@example.com"));
    }

    [Fact]
    public void UpdateEmail_UpdatesEmailCorrectly()
    {
        var client = CreateClient();
        client.UpdateEmail("newuser@example.com");

        Assert.Equal("newuser@example.com", client.Email);
    }

    [Fact]
    public void UpdateEmail_ThrowsExceptionWhenEmailAndPhoneAreNull()
    {
        var client = CreateClient();
        client.UpdateEmail(null);

        Assert.Throws<NullReferenceException>(() => client.UpdatePhoneNumber(null));
    }

    [Fact]
    public void UpdatePhoneNumber_UpdatesPhoneNumberCorrectly()
    {
        var client = CreateClient();
        client.UpdatePhoneNumber("+48 987 654 321");

        Assert.Equal("+48 987 654 321", client.PhoneNumber);
    }

    [Fact]
    public void AddRegisteredClient_AddsClientToList()
    {
        var client = CreateClient();
        RegisteredClient.AddRegisteredClient(client);

        var clients = RegisteredClient.GetRegisteredClients();
        Assert.Contains(client, clients);
    }

    [Fact]
    public void GetRegisteredClients_ReturnsAllClients()
    {
        var client1 = CreateClient("Alice", "P@ssw0rd1!", "alice@example.com");
        var client2 = CreateClient("Bob", "P@ssw0rd2@", "bob@example.com");

        var clients = RegisteredClient.GetRegisteredClients();

        Assert.Equal(2, clients.Count);
        Assert.Contains(client1, clients);
        Assert.Contains(client2, clients);
    }

    [Fact]
    public void UpdateName_UpdatesNameCorrectly()
    {
        var client = CreateClient();
        client.UpdateName("NewName");

        Assert.Equal("NewName", client.Name);
    }

    [Fact]
    public void UpdateSurname_UpdatesSurnameCorrectly()
    {
        var client = CreateClient(surname: "OldSurname");
        client.UpdateSurname("NewSurname");

        Assert.Equal("NewSurname", client.Surname);
    }

    [Fact]
    public void UpdateSurname_ThrowsExceptionForShortSurname()
    {
        var client = CreateClient(surname: "ValidSurname");
        Assert.Throws<ArgumentException>(() => client.UpdateSurname("A"));
    }
}
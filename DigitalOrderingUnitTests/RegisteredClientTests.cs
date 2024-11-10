using DigitalOrdering;
using Xunit;
using System.Reflection;

namespace DigitalOrderingUnitTests;

public class RegisteredClientTests
{
    public RegisteredClientTests()
    {
        typeof(RegisteredClient)
            .GetField("_registeredClients", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<RegisteredClient>());
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var client = new RegisteredClient("John", "P@ssw0rd", "john.doe@example.com", "+48 123 456 789", "Doe");

        Assert.Equal("John", client.Name);
        Assert.Equal("P@ssw0rd", client.Password);
        Assert.Equal("Doe", client.Surname);
        Assert.Equal("john.doe@example.com", client.Email);
        Assert.Equal("+48 123 456 789", client.PhoneNumber);
        Assert.Equal(0, client.Bonus);
        Assert.True(client.Id > 0);
    }

    [Fact]
    public void Id_Getter_ReturnsCorrectValue()
    {
        var client = new RegisteredClient("Alice", "P@ssw0rd", "john.doe@example.com", "+48 123 456 789", "Doe");
        Assert.True(client.Id > 0);
    }

    [Fact]
    public void Password_Getter_ReturnsCorrectValue()
    {
        var client = new RegisteredClient("Bob", "P@ssw0rd1!", "john.doe@example.com", "+48 123 456 789", "Doe");
        Assert.Equal("P@ssw0rd1!", client.Password);
    }

    [Fact]
    public void Surname_Getter_ReturnsCorrectValue()
    {
        var client = new RegisteredClient("Charlie", "P@ssw0rd", "charlie.doe@example.com", null, "Doe");
        Assert.Equal("Doe", client.Surname);
    }

    [Fact]
    public void Email_Getter_ReturnsCorrectValue()
    {
        var client = new RegisteredClient("Dave", "P@ssw0rd", "dave@example.com");
        Assert.Equal("dave@example.com", client.Email);
    }

    [Fact]
    public void PhoneNumber_Getter_ReturnsCorrectValue()
    {
        var client = new RegisteredClient("Eve", "P@ssw0rd", null, "+48 123 456 789");
        Assert.Equal("+48 123 456 789", client.PhoneNumber);
    }

    [Fact]
    public void AddRegisteredClient_AddsClientToList()
    {
        var client = new RegisteredClient("Ivy", "P@ssw0rd", "ivy@example.com");
        RegisteredClient.AddRegisteredClient(client);

        var clients = RegisteredClient.GetRegisteredClients();
        Assert.Contains(client, clients);
    }

    [Fact]
    public void GetRegisteredClients_ReturnsCorrectListOfClients()
    {
        var client1 = new RegisteredClient("Jack", "P@ssw0rd", "jack@example.com");
        var client2 = new RegisteredClient("Jill", "P@ssw0rd1", "jill@example.com");

        RegisteredClient.AddRegisteredClient(client1);
        RegisteredClient.AddRegisteredClient(client2);

        var clients = RegisteredClient.GetRegisteredClients();

        Assert.Equal(2, clients.Count);
        Assert.Contains(client1, clients);
        Assert.Contains(client2, clients);
    }

    [Fact]
    public void SaveRegisteredClientJSON_SavesClientsToFile()
    {
        var client = new RegisteredClient("Kevin", "P@ssw0rd", "kevin@example.com");
        RegisteredClient.AddRegisteredClient(client);
        const string path = "test_registered_clients.json";

        RegisteredClient.SaveRegisteredClientJSON(path);
        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadRegisteredClientJSON_LoadsClientsFromFile()
    {
        const string path = "test_registered_clients.json";
        var client = new RegisteredClient("Lily", "P@ssw0rd", "lily@example.com");
        RegisteredClient.AddRegisteredClient(client);
        RegisteredClient.SaveRegisteredClientJSON(path);
        RegisteredClient.GetRegisteredClients().Clear();

        RegisteredClient.LoadRegisteredClientJSON(path);
        var clients = RegisteredClient.GetRegisteredClients();

        Assert.Single(clients);
        Assert.Equal(client.Name, clients[0].Name);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForMissingEmailAndPhoneNumber()
    {
        Assert.Throws<NullReferenceException>(() => new RegisteredClient("George", "P@ssw0rd"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidEmailFormat()
    {
        Assert.Throws<ArgumentException>(() => new RegisteredClient("Mike", "P@ssw0rd", "invalid-email"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidPasswordFormat()
    {
        Assert.Throws<ArgumentException>(() => new RegisteredClient("Nancy", "password"));
    }
}
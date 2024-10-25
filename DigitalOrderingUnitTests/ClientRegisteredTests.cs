using Xunit;
using DigitalOrdering;

public class ClientRegisteredTests
{
    [Fact]
    public void AddBonus()
    {
        var client = new ClientRegistered();
        var initialBonus = ClientRegistered.BonusCounter;

        client.AddBonus(20);

        Assert.Equal(initialBonus + 20, ClientRegistered.BonusCounter);
    }
}
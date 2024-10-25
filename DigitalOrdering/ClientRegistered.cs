namespace DigitalOrdering;

public class ClientRegistered : Client
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string Surname { get; set; }
    public static int BonusCounter = 0;

    public void AddBonus(int bonus)
    {
        BonusCounter += bonus;
    }
}
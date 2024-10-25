namespace DigitalOrdering;

public class Reservation
{
    public DateTime DateTime { get; set; }
    public int Duration { get; set; }
    public int PeopleCount { get; set; }
    public string? Description { get; set; }
    public Restaurant Restaurant { get; set; }
    
    public void ExtendReservation(int extraMinutes)
    {
        Duration += extraMinutes;
    }
}
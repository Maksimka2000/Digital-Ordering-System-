namespace DigitalOrdering;

public class Restaurant
{
    
    public string Name { get; set; }
    public string Location { get; set; }
    public string OpeningHours { get; set; }

    public Restaurant(string name, string location, string openingHours)
    {
        Name = name;
        Location = location;
        OpeningHours = openingHours;
    }
    
    
}
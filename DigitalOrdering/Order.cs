namespace DigitalOrdering;

public abstract class Order
{
    
    private const double Service = 0.1;
    
    public int Id { get; set; }
    public double OrderPrice { get; set; }
    public double ServicePrice { get; set; }
    public double TotalPrice { get; set; }
    
    
    public List<MenuItem> MenuItems { get; set; }
    
}
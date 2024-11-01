namespace DigitalOrdering;

public abstract class Order
{
    
    private const double Service = 0.1;
    private const int IdCounter = 0;
    
    public int Id { get; set; }
    public double OrderPrice { get; set; }
    public double ServicePrice { get; set; }
    public double TotalPrice { get; set; }
    
    
    
}
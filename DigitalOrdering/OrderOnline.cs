namespace DigitalOrdering;

public class OrderOnline
{
    public double? Discount { get; set; }
    public Reservation Reservation { get; set; }   
    
    public void ApplyDiscount(double discountPercentage)
    {
        Discount = discountPercentage;
    }
}
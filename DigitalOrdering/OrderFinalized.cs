namespace DigitalOrdering;

public class OrderFinalized : Order
{

    public string? Coupon { get; set; }
    private enum CardType
    {
        Visa,
        MasterCard,
    }
    public int? Bonused { get; set; }

    
}
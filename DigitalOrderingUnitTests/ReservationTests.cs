using Xunit;
using DigitalOrdering;

public class ReservationTests
{
    [Fact]
    public void ExtendReservation()
    {
        var reservation = new Reservation { Duration = 60 };
        reservation.ExtendReservation(30);
        Assert.Equal(90, reservation.Duration);
    }
}
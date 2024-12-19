using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class PromotionTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var promotion = CreatePromotion(20, "Spring Sale", "Discount on all items for spring");

        AssertPromotionProperties(promotion, 20, "Spring Sale", "Discount on all items for spring", Promotion.PromotionType.Regular);
    }

    [Fact]
    public void Constructor_SetsDefaultTypeCorrectly()
    {
        var promotion = CreatePromotion(15, "Winter Sale");

        Assert.Equal(Promotion.PromotionType.Regular, promotion.Type);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidDiscountPercent()
    {
        Assert.Throws<Exception>(() => CreatePromotion(0, "Invalid Discount"));
        Assert.Throws<Exception>(() => CreatePromotion(100, "Invalid Discount"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidNameOrDescription()
    {
        Assert.Throws<ArgumentException>(() => CreatePromotion(10, ""));
        Assert.Throws<ArgumentException>(() => CreatePromotion(10, null));
    }

    [Fact]
    public void UpdateDiscountPercent_UpdatesValueCorrectly()
    {
        var promotion = CreatePromotion(20, "Spring Sale", "Initial discount");

        promotion.UpdateDiscountPercent(30);

        Assert.Equal(30, promotion.DiscountPercent);
    }

    [Fact]
    public void UpdateDiscountPercent_ThrowsExceptionForInvalidValue()
    {
        var promotion = CreatePromotion(20, "Spring Sale", "Initial discount");

        Assert.Throws<Exception>(() => promotion.UpdateDiscountPercent(0));
        Assert.Throws<Exception>(() => promotion.UpdateDiscountPercent(100));
    }

    [Fact]
    public void UpdateName_ChangesNameCorrectly()
    {
        var promotion = CreatePromotion(10, "Old Name");

        promotion.UpdateName("New Name");

        Assert.Equal("New Name", promotion.Name);
    }

    [Fact]
    public void UpdateDescription_ChangesDescriptionCorrectly()
    {
        var promotion = CreatePromotion(15, "Test Promo", "Old description");

        promotion.UpdateDescription("New description");

        Assert.Equal("New description", promotion.Description);
    }

    [Fact]
    public void RemoveDescription_SetsDescriptionToNull()
    {
        var promotion = CreatePromotion(10, "Test Promo", "Temporary description");

        promotion.RemoveDescription();

        Assert.Null(promotion.Description);
    }

    [Fact]
    public void UpdateType_ChangesPromotionTypeCorrectly()
    {
        var promotion = CreatePromotion(15, "Test Promo");

        promotion.UpdateType(Promotion.PromotionType.Regular);

        Assert.Equal(Promotion.PromotionType.Regular, promotion.Type);
    }
    
    private Promotion CreatePromotion(double discountPercent, string name, string description = null)
    {
        return new Promotion(discountPercent, name, description);
    }

    private void AssertPromotionProperties(Promotion promotion, double discountPercent, string name, string description, Promotion.PromotionType type)
    {
        Assert.Equal(discountPercent, promotion.DiscountPercent);
        Assert.Equal(name, promotion.Name);
        Assert.Equal(description, promotion.Description);
        Assert.Equal(type, promotion.Type);
    }
}

using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class PromotionTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const int discountPercent = 20;
        const string name = "Spring Sale";
        const string description = "Discount on all items for spring";

        var promotion = new Promotion(discountPercent, name, description);

        Assert.Equal(discountPercent, promotion.DiscountPercent);
        Assert.Equal(name, promotion.Name);
        Assert.Equal(description, promotion.Description);
        Assert.True(promotion.Id > 0);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidDiscountPercent()
    {
        Assert.Throws<Exception>(() => new Promotion(0, "New Year Sale", "Discount on New Year's Day"));
        Assert.Throws<Exception>(() => new Promotion(100, "Holiday Sale", "Special discount for the holidays"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForEmptyNameOrDescription()
    {
        Assert.Throws<ArgumentException>(() => new Promotion(10, "", "Discount for members"));
        Assert.Throws<ArgumentException>(() => new Promotion(10, "Member Discount", ""));
    }

    [Fact]
    public void AddPromotion_AddsPromotionToList()
    {
        var promotion = new Promotion(15, "Summer Sale", "Discount on all summer items");

        Promotion.AddPromotion(promotion);
        var promotions = Promotion.GetPromotions();

        Assert.Contains(promotion, promotions);
    }

    [Fact]
    public void GetPromotions_ReturnsListOfPromotions()
    {
        var promotion1 = new Promotion(10, "Black Friday", "Discount on Black Friday");
        var promotion2 = new Promotion(5, "Cyber Monday", "Discount on Cyber Monday");
        Promotion.AddPromotion(promotion1);
        Promotion.AddPromotion(promotion2);

        var promotions = Promotion.GetPromotions();

        Assert.Equal(2, promotions.Count);
        Assert.Contains(promotion1, promotions);
        Assert.Contains(promotion2, promotions);
    }

    [Fact]
    public void DeletePromotion_RemovesPromotionFromList()
    {
        var promotion = new Promotion(20, "Halloween Sale", "Discount on Halloween items");
        Promotion.AddPromotion(promotion);

        Promotion.DeletePromotion(promotion);
        var promotions = Promotion.GetPromotions();

        Assert.DoesNotContain(promotion, promotions);
    }

    [Fact]
    public void AddPromotion_ThrowsExceptionForDuplicateName()
    {
        var promotion1 = new Promotion(10, "Exclusive Offer", "Special exclusive discount");
        Promotion.AddPromotion(promotion1);
        var promotion2 = new Promotion(20, "Exclusive Offer", "Duplicate promotion name test");

        Assert.Throws<ArgumentException>(() => Promotion.AddPromotion(promotion2));
    }

    [Fact]
    public void UpdateDiscountPercent_ChangesDiscountPercent()
    {
        var promotion = new Promotion(10, "Seasonal Discount", "Limited-time offer");

        promotion.UpdateDiscountPercent(25);

        Assert.Equal(25, promotion.DiscountPercent);
    }

    [Fact]
    public void UpdateName_ChangesPromotionName()
    {
        var promotion = new Promotion(15, "Weekend Deal", "Discount valid on weekends");

        promotion.UpdateName("Weekend Special");

        Assert.Equal("Weekend Special", promotion.Name);
    }

    [Fact]
    public void UpdateDescription_ChangesPromotionDescription()
    {
        var promotion = new Promotion(30, "Flash Sale", "Discount on select items");

        promotion.UpdateDescription("Limited-time flash sale");

        Assert.Equal("Limited-time flash sale", promotion.Description);
    }

    [Fact]
    public void SavePromotionJSON_SavesPromotionsToFile()
    {
        var promotion = new Promotion(5, "New Customer Discount", "Discount for new customers");
        Promotion.AddPromotion(promotion);
        const string path = "test_promotions.json";

        Promotion.SavePromotionJSON(path);

        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadPromotionJSON_LoadsPromotionsFromFile()
    {
        const string path = "test_promotions.json";
        var promotion = new Promotion(10, "Holiday Discount", "Discount for the holiday season");
        Promotion.AddPromotion(promotion);
        Promotion.SavePromotionJSON(path);
        Promotion.GetPromotions().Clear();

        Promotion.LoadPromotionJSON(path);
        var promotions = Promotion.GetPromotions();

        Assert.Single(promotions);
        Assert.Equal(promotion.Name, promotions[0].Name);

        File.Delete(path);
    }
}
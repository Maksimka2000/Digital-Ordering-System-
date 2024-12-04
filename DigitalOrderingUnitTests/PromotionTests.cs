using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class PromotionTests
{
    public PromotionTests()
    {
        typeof(Promotion)
            .GetField("_promotions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, new List<Promotion>());
    }

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
    public void DiscountPercent_Getter_ReturnsCorrectValue()
    {
        var promotion = new Promotion(15, "Winter Sale", "Discount for winter items");
        Assert.Equal(15, promotion.DiscountPercent);
    }

    [Fact]
    public void Name_Getter_ReturnsCorrectValue()
    {
        var promotion = new Promotion(10, "Holiday Sale", "Seasonal discount");
        Assert.Equal("Holiday Sale", promotion.Name);
    }

    [Fact]
    public void Description_Getter_ReturnsCorrectValue()
    {
        var promotion = new Promotion(25, "Flash Sale", "Limited time discount");
        Assert.Equal("Limited time discount", promotion.Description);
    }

    [Fact]
    public void AddPromotion_AddsPromotionToList()
    {
        var promotion = new Promotion(15, "Summer Sale", "Discount on summer items");
        Promotion.AddPromotion(promotion);

        var promotions = Promotion.GetPromotions();
        Assert.Contains(promotion, promotions);
    }

    [Fact]
    public void GetPromotions_ReturnsCorrectListOfPromotions()
    {
        var promotion1 = new Promotion(10, "New Year Sale", "Discount for the new year");
        var promotion2 = new Promotion(20, "Valentine's Day Sale", "Discount for Valentine's Day");
        Promotion.AddPromotion(promotion1);
        Promotion.AddPromotion(promotion2);

        var promotions = Promotion.GetPromotions();

        Assert.Equal(2, promotions.Count);
        Assert.Contains(promotion1, promotions);
        Assert.Contains(promotion2, promotions);
    }

    [Fact]
    public void SavePromotionJSON_SavesPromotionsToFile()
    {
        var promotion = new Promotion(30, "Autumn Sale", "Discount on autumn items");
        Promotion.AddPromotion(promotion);
        const string path = "test_promotions.json";

        Promotion.SavePromotionJson(path);
        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadPromotionJSON_LoadsPromotionsFromFile()
    {
        const string path = "test_promotions.json";
        var promotion = new Promotion(5, "Weekend Sale", "Discount on weekends");
        Promotion.AddPromotion(promotion);
        Promotion.SavePromotionJson(path);
        
        Promotion.GetPromotions().Clear();

        Promotion.LoadPromotionJSON(path);
        var promotions = Promotion.GetPromotions();

        Assert.Single(promotions);
        Assert.Equal(promotion.Name, promotions[0].Name);

        File.Delete(path);
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
}
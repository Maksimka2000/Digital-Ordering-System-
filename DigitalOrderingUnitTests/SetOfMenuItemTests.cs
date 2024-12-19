using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class SetOfMenuItemTests
{
    public SetOfMenuItemTests()
    {
        ResetStaticSetOfMenuItems();
    }

    private void ResetStaticSetOfMenuItems()
    {
        typeof(SetOfMenuItem)
            .GetField("_setOfMenuItems", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<SetOfMenuItem>());
    }

    private static Restaurant CreateRestaurant()
    {
        return new Restaurant("Test Restaurant", new Address("Main St", "Test City", "123"),
            new List<OpenHour>
            {
                new OpenHour(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
                new OpenHour(DayOfWeek.Tuesday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
                new OpenHour(DayOfWeek.Wednesday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
                new OpenHour(DayOfWeek.Thursday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
                new OpenHour(DayOfWeek.Friday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
                new OpenHour(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0)),
                new OpenHour(DayOfWeek.Sunday, null, null)
            });
    }

    private static List<Food> CreateFoodList(int count, Restaurant restaurant)
        => Enumerable.Range(1, count)
            .Select(i => new Food(restaurant, $"Food {i}", 10.0, "Test Food", Food.FoodType.Snack, null))
            .ToList();

    private static List<Beverage> CreateBeverageList(int count, Restaurant restaurant)
        => Enumerable.Range(1, count)
            .Select(i => new Beverage(restaurant, $"Beverage {i}", 5.0, "Test Beverage", Beverage.BeverageType.Drinks, false))
            .ToList();

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var restaurant = CreateRestaurant();
        var foods = CreateFoodList(2, restaurant);
        var beverages = CreateBeverageList(1, restaurant);

        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch set", foods, beverages);

        Assert.Equal("Lunch Set", set.Name);
        Assert.Equal(25.0, set.Price);
        Assert.Equal("Business lunch set", set.Description);
        Assert.Equal(foods.Count, set.Foods.Count);
        Assert.Equal(beverages.Count, set.Beverages.Count);
        Assert.NotNull(set.Days);
        Assert.NotEmpty(set.Days);
    }

    [Fact]
    public void AddFood_AddsFoodToSet()
    {
        var restaurant = CreateRestaurant();
        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch");
        var food = new Food(restaurant, "Soup", 5.0, "Tomato soup", Food.FoodType.Snack, null);

        set.AddFood(food);

        Assert.Contains(food, set.Foods);
    }

    [Fact]
    public void RemoveFood_RemovesFoodFromSet()
    {
        var restaurant = CreateRestaurant();
        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch");
        var food = new Food(restaurant, "Soup", 5.0, "Tomato soup", Food.FoodType.Snack, null);

        set.AddFood(food);
        set.RemoveFood(food);

        Assert.DoesNotContain(food, set.Foods);
    }

    [Fact]
    public void AddBeverage_AddsBeverageToSet()
    {
        var restaurant = CreateRestaurant();
        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch");
        var beverage = new Beverage(restaurant, "Cola", 5.0, "Cold drink", Beverage.BeverageType.Drinks, false);

        set.AddBeverage(beverage);

        Assert.Contains(beverage, set.Beverages);
    }

    [Fact]
    public void RemoveBeverage_RemovesBeverageFromSet()
    {
        var restaurant = CreateRestaurant();
        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch");
        var beverage = new Beverage(restaurant, "Cola", 5.0, "Cold drink", Beverage.BeverageType.Drinks, false);

        set.AddBeverage(beverage);
        set.RemoveBeverage(beverage);

        Assert.DoesNotContain(beverage, set.Beverages);
    }

    [Fact]
    public void UpdateDays_UpdatesAvailableDays()
    {
        var restaurant = CreateRestaurant();
        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch");

        var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday };
        set.UpdateDays(days);

        Assert.Equal(days.Count, set.Days.Count);
        Assert.All(days, day => Assert.Contains(day, set.Days));
    }

    [Fact]
    public void UpdateTime_UpdatesStartAndEndTime()
    {
        var restaurant = CreateRestaurant();
        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch");

        var startTime = new TimeSpan(11, 0, 0);
        var endTime = new TimeSpan(15, 0, 0);
        set.UpdateTime(startTime, endTime);

        Assert.Equal(startTime, set.StartTime);
        Assert.Equal(endTime, set.EndTime);
    }

    [Fact]
    public void UpdateTime_ThrowsExceptionForInvalidTimeRange()
    {
        var restaurant = CreateRestaurant();
        var set = new SetOfMenuItem(restaurant, "Lunch Set", 25.0, "Business lunch");

        Assert.Throws<ArgumentException>(() => set.UpdateTime(new TimeSpan(15, 0, 0), new TimeSpan(11, 0, 0)));
    }
}

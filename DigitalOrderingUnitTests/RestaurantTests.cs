using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class RestaurantTests
{
    public RestaurantTests()
    {
        ResetStaticRestaurants();
    }

    private void ResetStaticRestaurants()
    {
        typeof(Restaurant)
            .GetField("_restaurants", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<Restaurant>());
    }

    private static Restaurant CreateRestaurant(string name = "Testaurant", Address? location = null, List<OpenHour>? openHours = null)
    {
        location ??= new Address("Main Street", "Metropolis", "10");
        openHours ??= CreateOpenHours();
        return new Restaurant(name, location, openHours);
    }

    private static List<OpenHour> CreateOpenHours(bool closedOnSunday = true)
    {
        return new List<OpenHour>
        {
            new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(14, 0, 0)),
            new OpenHour(DayOfWeek.Sunday, closedOnSunday ? null : new TimeSpan(10, 0, 0), closedOnSunday ? null : new TimeSpan(14, 0, 0))
        };
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "La Pizzeria";
        var location = new Address("Main Street", "Metropolis", "10");
        var openHours = CreateOpenHours();

        var restaurant = CreateRestaurant(name, location, openHours);

        Assert.Equal(name, restaurant.Name);
        Assert.Equal(location.ToString(), restaurant.Location.ToString());
        Assert.Equal(openHours.Count, restaurant.OpenHours.Count);
        Assert.True(restaurant.Id > 0);
    }

    [Theory]
    [InlineData("", "Main Street", "Metropolis", "10")]
    public void Constructor_ThrowsExceptionForInvalidName(string name, string street, string city, string number)
    {
        var location = new Address(street, city, number);
        Assert.Throws<ArgumentException>(() => CreateRestaurant(name, location));
    }

    [Fact]
    public void GetRestaurants_ReturnsAllRestaurants()
    {
        var restaurant1 = CreateRestaurant("Testaurant1", new Address("Street1", "City1", "10"));
        var restaurant2 = CreateRestaurant("Testaurant2", new Address("Street2", "City2", "20"));

        var restaurants = Restaurant.GetRestaurants();

        Assert.Equal(2, restaurants.Count);
        Assert.Contains(restaurant1, restaurants);
        Assert.Contains(restaurant2, restaurants);
    }

    [Fact]
    public void AddTable_AddsTableToRestaurant()
    {
        var restaurant = CreateRestaurant();
        var table = new Table(restaurant, 4);

        restaurant.AddTable(table);

        Assert.Contains(table, restaurant.Tables);
        Assert.Single(restaurant.Tables);
    }

    [Fact]
    public void AddTable_ThrowsExceptionForNullTable()
    {
        var restaurant = CreateRestaurant();
        Assert.Throws<ArgumentNullException>(() => restaurant.AddTable(null));
    }

    [Fact]
    public void AddMenuItemToMenu_AddsMenuItem()
    {
        var restaurant = CreateRestaurant();
        var menuItem = new Beverage(restaurant, "Cola", 5.0, "Cold drink", Beverage.BeverageType.Drinks, false);

        restaurant.AddMenuItemToMenu(menuItem);

        Assert.Contains(menuItem, restaurant.Menu);
        Assert.Single(restaurant.Menu);
    }

    [Fact]
    public void AddMenuItemToMenu_ThrowsExceptionForNullMenuItem()
    {
        var restaurant = CreateRestaurant();
        Assert.Throws<ArgumentNullException>(() => restaurant.AddMenuItemToMenu(null));
    }

    [Fact]
    public void IsRestaurantOpen_ReturnsTrueForOpenHours()
    {
        var restaurant = CreateRestaurant();
        var result = restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(10, 0, 0));

        Assert.True(result);
    }

    [Fact]
    public void UpdateOpenHours_UpdatesWorkHours()
    {
        var restaurant = CreateRestaurant();
        var newOpenHours = CreateOpenHours(closedOnSunday: false);

        restaurant.UpdateOpenHours(newOpenHours);

        Assert.True(restaurant.GetOpenHour(DayOfWeek.Sunday).IsOpen);
    }

    [Fact]
    public void UpdateName_ChangesRestaurantName()
    {
        var restaurant = CreateRestaurant();
        restaurant.UpdateName("Updated Name");

        Assert.Equal("Updated Name", restaurant.Name);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, 8, 18)]
    public void UpdateOpenHour_ChangesOpenAndCloseTimes(DayOfWeek day, int openHour, int closeHour)
    {
        var restaurant = CreateRestaurant();
        restaurant.UpdateOpenHour(day, new TimeSpan(openHour, 0, 0), new TimeSpan(closeHour, 0, 0));

        var openHourData = restaurant.GetOpenHour(day);
        Assert.Equal(new TimeSpan(openHour, 0, 0), openHourData.OpenTime);
        Assert.Equal(new TimeSpan(closeHour, 0, 0), openHourData.CloseTime);
    }

    [Fact]
    public void GetOpenHour_ThrowsExceptionForInvalidDay()
    {
        var restaurant = CreateRestaurant();
        Assert.Throws<KeyNotFoundException>(() => restaurant.GetOpenHour((DayOfWeek)8));
    }
}

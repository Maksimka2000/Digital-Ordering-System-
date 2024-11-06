using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class RestaurantTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var address = new Address("123 Main St", "Springfield");
        var openHours = CreateDefaultOpenHours();
        var restaurant = new Restaurant("The Bistro", address, openHours);

        Assert.Equal("The Bistro", restaurant.Name);
        Assert.Equal(address, restaurant.Location);
        Assert.Equal(openHours, restaurant.OpenHours);
        Assert.True(restaurant.Id > 0);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidName()
    {
        var address = new Address("123 Main St", "Springfield");
        var openHours = CreateDefaultOpenHours();

        Assert.Throws<ArgumentException>(() => new Restaurant("", address, openHours));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidOpenHours()
    {
        var address = new Address("123 Main St", "Springfield");
        var incompleteOpenHours = new List<OpenHour>
            { new(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)) };

        Assert.Throws<ArgumentException>(() => new Restaurant("The Bistro", address, incompleteOpenHours));
    }

    [Fact]
    public void AddRestaurant_AddsRestaurantToList()
    {
        var address = new Address("123 Main St", "Springfield");
        var openHours = CreateDefaultOpenHours();
        var restaurant = new Restaurant("The Diner", address, openHours);

        Restaurant.AddRestaurant(restaurant);
        var restaurants = Restaurant.GetRestaurants();

        Assert.Contains(restaurant, restaurants);
    }

    [Fact]
    public void GetRestaurants_ReturnsListOfRestaurants()
    {
        var address1 = new Address("123 Main St", "Springfield");
        var address2 = new Address("456 Elm St", "Springfield");
        var restaurant1 = new Restaurant("Restaurant 1", address1, CreateDefaultOpenHours());
        var restaurant2 = new Restaurant("Restaurant 2", address2, CreateDefaultOpenHours());

        Restaurant.AddRestaurant(restaurant1);
        Restaurant.AddRestaurant(restaurant2);

        var restaurants = Restaurant.GetRestaurants();

        Assert.Equal(2, restaurants.Count);
        Assert.Contains(restaurant1, restaurants);
        Assert.Contains(restaurant2, restaurants);
    }

    [Fact]
    public void UpdateName_ChangesRestaurantName()
    {
        var address = new Address("123 Main St", "Springfield");
        var openHours = CreateDefaultOpenHours();
        var restaurant = new Restaurant("Old Name", address, openHours);

        restaurant.UpdateName("New Name");

        Assert.Equal("New Name", restaurant.Name);
    }

    [Fact]
    public void UpdateOpenHours_ChangesOpenHoursForSpecificDay()
    {
        var address = new Address("123 Main St", "Springfield");
        var openHours = CreateDefaultOpenHours();
        var restaurant = new Restaurant("The Grill", address, openHours);

        restaurant.UpdateOpenHours(DayOfWeek.Monday, new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0));

        var updatedHours = restaurant.OpenHours.Find(hour => hour.Day == DayOfWeek.Monday);
        Assert.Equal(new TimeSpan(10, 0, 0), updatedHours.OpenTime);
        Assert.Equal(new TimeSpan(22, 0, 0), updatedHours.CloseTime);
    }

    [Fact]
    public void IsRestaurantOpen_ReturnsCorrectStatusBasedOnTime()
    {
        var address = new Address("123 Main St", "Springfield");
        var openHours = CreateDefaultOpenHours();
        var restaurant = new Restaurant("Café", address, openHours);

        Assert.True(restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(12, 0, 0))); // Within open hours
        Assert.False(restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(8, 0, 0))); // Before open hours
        Assert.False(restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(20, 0, 0))); // After open hours
    }

    [Fact]
    public void SaveRestaurantJSON_SavesRestaurantsToFile()
    {
        var address = new Address("123 Main St", "Springfield");
        var restaurant = new Restaurant("File Restaurant", address, CreateDefaultOpenHours());
        Restaurant.AddRestaurant(restaurant);
        const string path = "test_restaurants.json";

        Restaurant.SaveRestaurantJSON(path);

        Assert.True(File.Exists(path));

        File.Delete(path);
    }

    [Fact]
    public void LoadRestaurantJSON_LoadsRestaurantsFromFile()
    {
        const string path = "test_restaurants.json";
        var address = new Address("123 Main St", "Springfield");
        var restaurant = new Restaurant("File Load Restaurant", address, CreateDefaultOpenHours());
        Restaurant.AddRestaurant(restaurant);
        Restaurant.SaveRestaurantJSON(path);
        Restaurant.GetRestaurants().Clear();

        Restaurant.LoadRestaurantJSON(path);
        var restaurants = Restaurant.GetRestaurants();

        Assert.Single(restaurants);
        Assert.Equal(restaurant.Name, restaurants[0].Name);

        File.Delete(path);
    }

    private static List<OpenHour> CreateDefaultOpenHours()
    {
        return
        [
            new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)),
            new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)),
            new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)),
            new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)),
            new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)),
            new OpenHour(DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)),
            new OpenHour(DayOfWeek.Sunday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0))
        ];
    }
}

public class OpenHourTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var openHour = new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0));

        Assert.Equal(DayOfWeek.Monday, openHour.Day);
        Assert.Equal(new TimeSpan(9, 0, 0), openHour.OpenTime);
        Assert.Equal(new TimeSpan(17, 0, 0), openHour.CloseTime);
        Assert.True(openHour.IsOpen);
    }

    [Fact]
    public void UpdateTime_ChangesOpenAndCloseTime()
    {
        var openHour = new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0));

        openHour.UpdateTime(new TimeSpan(10, 0, 0), new TimeSpan(20, 0, 0));

        Assert.Equal(new TimeSpan(10, 0, 0), openHour.OpenTime);
        Assert.Equal(new TimeSpan(20, 0, 0), openHour.CloseTime);
        Assert.True(openHour.IsOpen);
    }

    [Fact]
    public void UpdateTime_SetsIsOpenToFalseForNullTimes()
    {
        var openHour = new OpenHour(DayOfWeek.Monday);

        Assert.False(openHour.IsOpen);
    }
}

public class AddressTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var address = new Address("456 Elm St", "Springfield");

        Assert.Equal("456 Elm St", address.Street);
        Assert.Equal("Springfield", address.City);
    }
}
using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class RestaurantTests
{
    public RestaurantTests()
    {
        typeof(Restaurant)
            .GetField("_restaurants", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<Restaurant>());
    }

    private static List<OpenHour> CreateOpenHours(bool closedOnSunday = true)
    {
        return
        [
            new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
            new OpenHour(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(14, 0, 0)),
            new OpenHour(DayOfWeek.Sunday, closedOnSunday ? null : new TimeSpan(10, 0, 0),
                closedOnSunday ? null : new TimeSpan(14, 0, 0))
        ];
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string name = "La Pizzeria";
        var location = new Address("Main Street 10", "Metropolis", "22");
        var openHours = CreateOpenHours();

        var restaurant = new Restaurant(name, location, openHours);

        Assert.Equal(name, restaurant.Name);
        Assert.Equal(location, restaurant.Location);
        Assert.Equal(openHours, restaurant.OpenHours);
        Assert.True(restaurant.Id > 0);
    }

    [Fact]
    public void Id_Getter_ReturnsCorrectValue()
    {
        var restaurant = new Restaurant("Test Restaurant", new Address("Main St", "Test City", "22"), CreateOpenHours());
        Assert.True(restaurant.Id > 0);
    }


    [Fact]
    public void Name_Getter_ReturnsCorrectValue()
    {
        var restaurant = new Restaurant("Ocean Diner", new Address("Bay Ave 5", "Seaside", "22"), CreateOpenHours());
        Assert.Equal("Ocean Diner", restaurant.Name);
    }

    [Fact]
    public void Location_Getter_ReturnsCorrectValue()
    {
        var location = new Address("High St 22", "Riverside", "22");
        var restaurant = new Restaurant("Bistro Riverside", location, CreateOpenHours());
        Assert.Equal(location, restaurant.Location);
    }

    [Fact]
    public void AddRestaurant_AddsRestaurantToList()
    {
        var restaurant =
            new Restaurant("Cafe Good Day", new Address("Sunset Blvd 15", "Sunnyville", "22"), CreateOpenHours());
        Restaurant.AddRestaurant(restaurant);

        var restaurants = Restaurant.GetRestaurants();
        Assert.Contains(restaurant, restaurants);
    }

    [Fact]
    public void GetRestaurants_ReturnsCorrectListOfRestaurants()
    {
        var restaurant1 = new Restaurant("Deli Delight", new Address("Market St 10", "Townsville", "22"), CreateOpenHours());
        var restaurant2 = new Restaurant("Bakery Bliss", new Address("Hill Rd 1", "Greenville", "22"), CreateOpenHours());

        Restaurant.AddRestaurant(restaurant1);
        Restaurant.AddRestaurant(restaurant2);

        var restaurants = Restaurant.GetRestaurants();

        Assert.Equal(2, restaurants.Count);
        Assert.Contains(restaurant1, restaurants);
        Assert.Contains(restaurant2, restaurants);
    }

    [Fact]
    public void SaveRestaurantJSON_SavesRestaurantsToFile()
    {
        var restaurant = new Restaurant("Cafe Sunrise", new Address("Lake St 7", "Hilltown", "22"), CreateOpenHours());
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
        var restaurant = new Restaurant("Moonlight Cafe", new Address("Night St 12", "Moon City", "22"), CreateOpenHours());
        Restaurant.AddRestaurant(restaurant);
        Restaurant.SaveRestaurantJSON(path);
        Restaurant.GetRestaurants().Clear();

        Restaurant.LoadRestaurantJSON(path);
        var restaurants = Restaurant.GetRestaurants();

        Assert.Single(restaurants);
        Assert.Equal(restaurant.Name, restaurants[0].Name);

        File.Delete(path);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidNameOrLocation()
    {
        Assert.Throws<ArgumentException>(() =>
            new Restaurant("", new Address("Lake St 7", "Hilltown", "22"), CreateOpenHours()));
        Assert.Throws<ArgumentNullException>(() => new Restaurant("Cafe Sunset", null, CreateOpenHours()));
    }
}

public class OpenHourTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var day = DayOfWeek.Monday;
        var openTime = new TimeSpan(9, 0, 0);
        var closeTime = new TimeSpan(17, 0, 0);

        var openHour = new OpenHour(day, openTime, closeTime);

        Assert.Equal(day, openHour.Day);
        Assert.Equal(openTime, openHour.OpenTime);
        Assert.Equal(closeTime, openHour.CloseTime);
        Assert.True(openHour.IsOpen);
    }

    [Fact]
    public void Constructor_SetsClosedDayCorrectly()
    {
        var day = DayOfWeek.Sunday;

        var openHour = new OpenHour(day);

        Assert.Equal(day, openHour.Day);
        Assert.Null(openHour.OpenTime);
        Assert.Null(openHour.CloseTime);
        Assert.False(openHour.IsOpen);
    }

    [Fact]
    public void UpdateTime_ThrowsExceptionForInvalidTimes()
    {
        var openHour = new OpenHour(DayOfWeek.Wednesday);

        var invalidOpenTime = new TimeSpan(18, 0, 0);
        var invalidCloseTime = new TimeSpan(9, 0, 0);

        Assert.Throws<ArgumentException>(() => openHour.UpdateTime(invalidOpenTime, invalidCloseTime));
    }

    [Fact]
    public void UpdateTime_ThrowsExceptionForOneNullTime()
    {
        var openHour = new OpenHour(DayOfWeek.Thursday);

        var openTime = new TimeSpan(10, 0, 0);

        Assert.Throws<ArgumentException>(() => openHour.UpdateTime(openTime, null));
    }
}

public class AddressTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const string street = "Main St";
        const string city = "Metropolis";
        const string streetNumber = "22";

        var address = new Address(street, city, streetNumber);

        Assert.Equal(street, address.Street);
        Assert.Equal(city, address.City);
        Assert.Equal(streetNumber, address.StreetNumber);
    }

    [Fact]
    public void Street_Getter_ReturnsCorrectValue()
    {
        var address = new Address("Broadway", "New York", "22");
        Assert.Equal("Broadway", address.Street);
    }

    [Fact]
    public void City_Getter_ReturnsCorrectValue()
    {
        var address = new Address("Elm St", "Gotham", "22");
        Assert.Equal("Gotham", address.City);
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidStreet()
    {
        Assert.Throws<ArgumentException>(() => new Address("", "Metropolis", "22"));
        Assert.Throws<ArgumentException>(() => new Address(null, "Metropolis", "22"));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidCity()
    {
        Assert.Throws<ArgumentException>(() => new Address("Main St", "", "22"));
        Assert.Throws<ArgumentException>(() => new Address("Main St", null, "22"));
    }
}
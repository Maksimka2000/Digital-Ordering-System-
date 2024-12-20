using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class TableTests
{
    private readonly DateTime _validOpenHours = DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0);

    public TableTests()
    {
        ResetStaticTables();
    }

    private void ResetStaticTables()
    {
        typeof(Table)
            .GetField("_tables", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<Table>());
    }

    private static Restaurant CreateRestaurant()
    {
        var address = new Address("Main St", "Test City", "123");
        var openHours = new List<OpenHour>
        {
            new(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
            new(DayOfWeek.Tuesday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
            new(DayOfWeek.Wednesday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
            new(DayOfWeek.Thursday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
            new(DayOfWeek.Friday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0)),
            new(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0)),
            new(DayOfWeek.Sunday, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0))
        };

        return new Restaurant("Test Restaurant", address, openHours);
    }

    private Table CreateTable(Restaurant restaurant, int capacity, string alias = "Table Alias", string description = "Description")
        => new Table(restaurant, capacity, alias, description);

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var restaurant = CreateRestaurant();
        const int capacity = 4;
        const string alias = "Window Table";
        const string description = "A table by the window";

        var table = CreateTable(restaurant, capacity, alias, description);

        Assert.Equal(capacity, table.Capacity);
        Assert.Equal(alias, table.Alias);
        Assert.Equal(description, table.Description);
        Assert.False(table.IsLocked);
        Assert.True(table.Id > 0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_ThrowsExceptionForInvalidCapacity(int invalidCapacity)
    {
        var restaurant = CreateRestaurant();
        Assert.Throws<ArgumentException>(() => CreateTable(restaurant, invalidCapacity));
    }

    [Fact]
    public void UpdateAlias_UpdatesAliasCorrectly()
    {
        var restaurant = CreateRestaurant();
        var table = CreateTable(restaurant, 4, "Original Alias");

        table.UpdateAlias("Updated Alias");

        Assert.Equal("Updated Alias", table.Alias);
    }

    [Fact]
    public void UpdateAlias_ThrowsExceptionForEmptyAlias()
    {
        var restaurant = CreateRestaurant();
        var table = CreateTable(restaurant, 4);

        Assert.Throws<ArgumentException>(() => table.UpdateAlias(""));
    }

    [Fact]
    public void LockTable_SetsIsLockedToTrue()
    {
        var restaurant = CreateRestaurant();
        var table = CreateTable(restaurant, 4);

        table.LockTable();

        Assert.True(table.IsLocked);
    }

    [Fact]
    public void AddTable_AddsTableToList()
    {
        var restaurant = CreateRestaurant();
        var table = CreateTable(restaurant, 4);
        

        Assert.Contains(table, Table.GetTables());
    }

    [Fact]
    public void DeleteTable_RemovesTableFromList()
    {
        ResetStaticTables();
        var restaurant = CreateRestaurant();
        var table = CreateTable(restaurant, 6, "VIP Table");



        Assert.DoesNotContain(table, Table.GetTables());
    }

    [Fact]
    public void GetTables_ReturnsCorrectListOfTables()
    {
        ResetStaticTables();
        var restaurant = CreateRestaurant();
        var table1 = CreateTable(restaurant, 2, "Table 1");
        var table2 = CreateTable(restaurant, 4, "Table 2");

        var tables = Table.GetTables();

        Assert.Equal(2, tables.Count);
        Assert.Contains(table1, tables);
        Assert.Contains(table2, tables);
    }
}

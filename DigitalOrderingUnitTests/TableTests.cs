using System.Reflection;
using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests;

public class TableTests
{
    public TableTests()
    {
        typeof(Table)
            .GetField("_tables", BindingFlags.NonPublic | BindingFlags.Static)
            ?.SetValue(null, new List<Table>());
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        const int capacity = 4;
        const string alias = "Window Table";

        var table = new Table(capacity, alias);

        Assert.Equal(capacity, table.Capacity);
        Assert.Equal(alias, table.Alias);
        Assert.False(table.IsLocked);
        Assert.True(table.Id > 0);
    }

    [Fact]
    public void Id_Getter_ReturnsCorrectValue()
    {
        var table = new Table(2, "Center Table");
        Assert.True(table.Id > 0);
    }

    [Fact]
    public void Alias_Getter_ReturnsCorrectValue()
    {
        var table = new Table(4, "Corner Table");
        Assert.Equal("Corner Table", table.Alias);
    }

    [Fact]
    public void Capacity_Getter_ReturnsCorrectValue()
    {
        var table = new Table(6, "Outdoor Table");
        Assert.Equal(6, table.Capacity);
    }

    [Fact]
    public void IsLocked_Getter_IsInitiallyFalse()
    {
        var table = new Table(3, "Round Table");
        Assert.False(table.IsLocked);
    }

    [Fact]
    public void AddTable_AddsTableToList()
    {
        var table = new Table(4, "Side Table");
        Table.AddTable(table);

        var tables = Table.GetTables();
        Assert.Contains(table, tables);
    }

    [Fact]
    public void GetTables_ReturnsCorrectListOfTables()
    {
        var table1 = new Table(2, "Table 1");
        var table2 = new Table(4, "Table 2");

        Table.AddTable(table1);
        Table.AddTable(table2);

        var tables = Table.GetTables();

        Assert.Equal(2, tables.Count);
        Assert.Contains(table1, tables);
        Assert.Contains(table2, tables);
    }

    // [Fact]
    // public void SaveTableJSON_SavesTablesToFile()
    // {
    //     var table = new Table(4, "Patio Table");
    //     Table.AddTable(table);
    //     const string path = "test_tables.json";
    //
    //     Table.SaveTableJSON(path);
    //     Assert.True(File.Exists(path));
    //
    //     File.Delete(path);
    // }
    //
    // [Fact]
    // public void LoadTableJSON_LoadsTablesFromFile()
    // {
    //     const string path = "test_tables.json";
    //     var table = new Table(6, "VIP Table");
    //     Table.AddTable(table);
    //     Table.SaveTableJSON(path);
    //     Table.GetTables().Clear(); 
    //     
    //     Table.LoadTableJSON(path);
    //     var tables = Table.GetTables();
    //
    //     Assert.Single(tables);
    //     Assert.Equal(table.Alias, tables[0].Alias);
    //
    //     File.Delete(path);
    // }

    [Fact]
    public void UpdateAlias_ThrowsExceptionForEmptyAlias()
    {
        var table = new Table(4, "Table Alias");

        Assert.Throws<ArgumentException>(() => table.UpdateAlias(" "));
    }

    [Fact]
    public void Constructor_ThrowsExceptionForInvalidCapacity()
    {
        Assert.Throws<ArgumentException>(() => new Table(0));
        Assert.Throws<ArgumentException>(() => new Table(-5));
    }
}
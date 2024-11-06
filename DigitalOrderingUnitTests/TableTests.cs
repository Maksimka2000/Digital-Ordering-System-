using DigitalOrdering;
using Xunit;

namespace DigitalOrderingUnitTests
{
    public class TableTests
    {
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
        public void Constructor_SetsPropertiesCorrectlyWithoutAlias()
        {
            const int capacity = 6;

            var table = new Table(capacity);

            Assert.Equal(capacity, table.Capacity);
            Assert.Null(table.Alias);
        }

        [Fact]
        public void Constructor_ThrowsExceptionForInvalidCapacity()
        {
            Assert.Throws<ArgumentException>(() => new Table(0));
            Assert.Throws<ArgumentException>(() => new Table(-5));
        }

        [Fact]
        public void AddTable_AddsTableToList()
        {
            var table = new Table(4, "Corner Table");

            Table.AddTable(table);
            var tables = Table.GetTables();

            Assert.Contains(table, tables);
        }

        [Fact]
        public void GetTables_ReturnsListOfTables()
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

        [Fact]
        public void DeleteTable_RemovesTableFromList()
        {
            var table = new Table(4, "Table to Delete");
            Table.AddTable(table);

            Table.DeleteTable(table);
            var tables = Table.GetTables();

            Assert.DoesNotContain(table, tables);
        }

        [Fact]
        public void UpdateAlias_ChangesTableAlias()
        {
            var table = new Table(4, "Initial Alias");

            table.UpdateAlias("Updated Alias");

            Assert.Equal("Updated Alias", table.Alias);
        }

        [Fact]
        public void UpdateCapacity_ChangesTableCapacity()
        {
            var table = new Table(4);

            table.UpdateCapacity(6);

            Assert.Equal(6, table.Capacity);
        }

        [Fact]
        public void LockTable_SetsIsLockedToTrue()
        {
            var table = new Table(4);

            table.LockTable();

            Assert.True(table.IsLocked);
        }

        [Fact]
        public void UnLockTable_SetsIsLockedToFalse()
        {
            var table = new Table(4);
            table.LockTable();

            table.UnLockTable();

            Assert.False(table.IsLocked);
        }

        [Fact]
        public void SaveTableJSON_SavesTablesToFile()
        {
            var table = new Table(4, "File Table");
            Table.AddTable(table);
            const string path = "test_tables.json";

            Table.SaveTableJSON(path);

            Assert.True(File.Exists(path));

            File.Delete(path);
        }

        [Fact]
        public void LoadTableJSON_LoadsTablesFromFile()
        {
            const string path = "test_tables.json";
            var table = new Table(4, "Loaded Table");
            Table.AddTable(table);
            Table.SaveTableJSON(path);
            Table.GetTables().Clear();

            Table.LoadTableJSON(path);
            var tables = Table.GetTables();

            Assert.Single(tables);
            Assert.Equal(table.Alias, tables[0].Alias);

            File.Delete(path);
        }

        [Fact]
        public void UpdateAlias_ThrowsExceptionForEmptyAlias()
        {
            var table = new Table(4, "Table Alias");

            Assert.Throws<ArgumentException>(() => table.UpdateAlias(" "));
        }
    }
}
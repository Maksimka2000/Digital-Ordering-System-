using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class Table
{
    // class extent
    private static List<Table> _tables = [];

    private static int IdCounter = 0;
    public int Id { get; }
    private string? _alias;
    private int _capacity;
    public bool IsLocked { get; private set; }

    // fields setter validation
    public int Capacity
    {
        get => _capacity;
        private set
        {
            ValidateCapacity(value);
            _capacity = value;
        }
    }

    public string? Alias
    {
        get => _alias;
        private set
        {
            ValidateAlias(value);
            _alias = value;
        }
    }

    // constructor
    [JsonConstructor]
    public Table(int capacity, string? alias = null)
    {
        Id = ++IdCounter;
        Alias = alias;
        Capacity = capacity;
        IsLocked = false;
    }

    // validation
    private static void ValidateCapacity(int value)
    {
        if (value <= 0) throw new ArgumentException("Capacity must be greater than zero.");
    }

    private static void ValidateAlias(string value)
    {
        if (value == string.Empty) throw new ArgumentException("Alias cant be empty");
    }


    // get, delete, add, update CRUD 
    public static void AddTable(Table table)
    {
        if (table == null) throw new ArgumentException("Tabale cannot be null");
        _tables.Add(table);
    }

    public static List<Table> GetTables()
    {
        return [.._tables];
    }

    public static void DeleteTable(Table table)
    {
        if (table == null) throw new ArgumentException("table cannot be null");
        _tables.Remove(table);
    }

    public void UpdateAlias(string alias)
    {
        Alias = alias;
    }

    public void UpdateCapacity(int capacity)
    {
        Capacity = capacity;
    }

    // methods
    public void LockTable()
    {
        IsLocked = true;
    }

    public void UnLockTable()
    {
        IsLocked = false;
    }

    // // ================================================================ serialized and deserialized 
    // public static void SaveTableJSON(string path)
    // {
    //     try
    //     {
    //         string json = JsonConvert.SerializeObject(_tables, Formatting.Indented);
    //         File.WriteAllText(path, json);
    //         Console.WriteLine($"File Table saved successfully at {path}");
    //     }
    //     catch (Exception e)
    //     {
    //         throw new ArgumentException($"Error saving Table file: {e.Message}");
    //     }
    // }
    //
    // public static void LoadTableJSON(string path)
    // {
    //     try
    //     {
    //         if (File.Exists(path))
    //         {
    //             string json = File.ReadAllText(path);
    //             _tables = JsonConvert.DeserializeObject<List<Table>>(json);
    //             Console.WriteLine($"File Table loaded successfully at {path}");
    //         }
    //         else throw new ArgumentException($"Error loading Table file: path: {path} doesn't exist ");
    //     }
    //     catch (Exception e)
    //     {
    //         throw new ArgumentException($"Error loading Table file: {e.Message}");
    //     }
    // }
}
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class Table
{
    // class extent
    private static List<Table> _tables = [];

    private static int IdCounter = 0;
    [JsonIgnore]
    public int Id { get; }
    private string? _description;
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
    public string? Description
    {
        get => _description;
        private set  {
            ValidateOptionalString(value, "Description");
            _description = value;
        }
    }
    public string? Alias
    {
        get => _alias;
        private set
        {
            ValidateOptionalString(value, "Alias");
            _alias = value;
        }
    }

    // constructor
    [JsonConstructor]
    public Table(Restaurant restaurant, int capacity, string? alias = null, string? description = null)
    {
        Id = ++IdCounter;
        Capacity = capacity;
        Description = description;
        Alias = alias;
        IsLocked = false;
        AddToRestaurant(restaurant);
        AddTable(this);
    }
    
    // association reverse with restaurant 
    private Restaurant _restaurant;
    //associaiton reverse getter and setter
    public Restaurant Restaurant => _restaurant;
    // association reverse methods.
    private void AddToRestaurant(Restaurant restaurant)
    {
        if(restaurant is null) throw new ArgumentNullException($"Restaurant can't be null in AddRelationToRestaurant()");
        _restaurant = restaurant;
        restaurant.AddTable(this);
    }
    
    //associatin with OnlineOrder
    private List<OnlineOrder> _onlineOrders = [];
    //association getter
    public List<OnlineOrder> OnlineOrders => [.._onlineOrders];
    //association methods
    public void AddOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder is null) throw new ArgumentNullException($"Online order can't be null in AddOnlineOrder()");
        if(onlineOrder.Table != this) throw new ArgumentException($"Online order belong to table id: {onlineOrder.Table.Id}. And this table id: {this.Id} AddOnlineOrder()");
        if (!_onlineOrders.Contains(onlineOrder)) _onlineOrders.Add(onlineOrder);
    }
    
    //association with TableOrder
    private List<TableOrder> _tableOrders = [];
    //association getter
    public List<TableOrder> TableOrder => [.._tableOrders];
    // association methods
    public void AddTableOrder(TableOrder tableOrder)
    {
        if(tableOrder is null) throw new ArgumentNullException($"Table order can't be null in AddTableOrder()");
        if(tableOrder.Table != this) throw new ArgumentException($"Table order belong to table id: {tableOrder.Table.Id}. And this table id: {this.Id} AddTableOrder()");
        if(!_tableOrders.Contains(tableOrder)) _tableOrders.Add(tableOrder);
    }
    
    // validation
    private static void ValidateCapacity(int value)
    {
        if (value <= 0) throw new ArgumentException("Capacity must be greater than zero.");
    }

    private static void ValidateOptionalString(string value, string text)
    {
        if (value == string.Empty) throw new ArgumentException($"{text} while creating Table cant be empty");
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
    
    //methods for the Making OnlineOrder 
    public bool IsAvailableForOnlineOrder(DateTime dateAndTime, TimeSpan? duration)
    {
        return _onlineOrders.All(onlineOrder => dateAndTime + duration <= onlineOrder.DateAndTime ||
                                 dateAndTime >= onlineOrder.DateAndTime + onlineOrder.Duration);
    }
}
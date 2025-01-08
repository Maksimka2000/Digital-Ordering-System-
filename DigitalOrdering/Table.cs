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
    
    // association  with restaurant  (REVERSE)
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
    private void RemoveFromRestaurant()
    {
        _restaurant.RemoveTable(this);
        _restaurant = null;
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
    public void RemoveOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder is null) throw new ArgumentNullException($"Online order can't be null in RemoveOnlineOrder()");
        if (onlineOrder.Table != this) throw new AggregateException($"You are trying to delete an onlineOrder which doesn't even belong to this: {Id} table order belong to {onlineOrder.Table.Id} table");
        if (_onlineOrders.Contains(onlineOrder))
        {
            _onlineOrders.Remove(onlineOrder);
            onlineOrder.RemoveOrder();
        }
    }
    
    //association with TableOrder
    private List<TableOrder> _tableOrders = [];
    //association getter
    public List<TableOrder> TableOrders => [.._tableOrders];
    // association methods
    public void AddTableOrder(TableOrder tableOrder)
    {
        if(tableOrder is null) throw new ArgumentNullException($"Table order can't be null in AddTableOrder()");
        if(tableOrder.Table != this) throw new ArgumentException($"Table order belong to table id: {tableOrder.Table.Id}. And this table id: {this.Id} AddTableOrder()");
        if(!_tableOrders.Contains(tableOrder)) _tableOrders.Add(tableOrder);
    }
    public void RemoveTableOrder(TableOrder tableOrder)
    {
        if(tableOrder is null) throw new ArgumentNullException($"Table order can't be null in RemoveTableOrder()");
        if (tableOrder.Table != this) throw new AggregateException($"You are trying to delete an tableOrder which doesn't even belong to this: {Id} table order belong to {tableOrder.Table.Id} table");
        if (_tableOrders.Contains(tableOrder))
        {
            _tableOrders.Remove(tableOrder);
            tableOrder.RemoveOrder();
        }
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
    private static void AddTable(Table table)
    {
        if (table == null) throw new ArgumentException("Tabale cannot be null");
        _tables.Add(table);
    }
    public static List<Table> GetTables()
    {
        return [.._tables];
    }
    public void DeleteTable()
    {
        if (_onlineOrders.Count > 0 && _tableOrders.Count > 0) throw new ArgumentException($" You can't delete the table as it has active reservations or have orders"); //// modify this later to the STAND BY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (_tables.Contains(this))
        {
            _tables.Remove(this);
            RemoveFromRestaurant(); // _restaurant = null;    
        }
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
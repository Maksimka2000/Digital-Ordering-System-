using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class TableOrder : Order
{
    // class extent 
    private static List<TableOrder> _tableOrders = [];

    //fileds 
    public TimeSpan? QRCodeScanTime { get; } // accessed the webpage from the same device and made an order.

    //field setter validation
    public new TimeSpan? StartTime
    {
        get => base.StartTime;
        set { base.StartTime = value; }
    }

    // Constructor
    [JsonConstructor]
    public TableOrder(Table table, int numberOfPeople, Dictionary<MenuItem, int>? menuItemsWithQuantities = null, RegisteredClient? registeredClient = null) : base(numberOfPeople, menuItemsWithQuantities, registeredClient)
    {
        QRCodeScanTime = new TimeSpan(DateTime.Now.Ticks); // change soon. We assign the value here when the user first requested the web page and taking when the user sent the order form and then if the device is the same asingn value to hte qr code
        StartTime = new TimeSpan(DateTime.Now.Ticks);
        AddTable(table);
        AddTableOrder(this);
        
        if (menuItemsWithQuantities != null)
        {
            foreach (var entry in menuItemsWithQuantities)
            {
                var menuItem = entry.Key;
                var quantity = entry.Value;
                AddMenuItemToOrder(menuItem, quantity);
            }
        }
    }
    
    // association with Table (REVERSE)
    private Table _table;
    // association getter
    public override Table Table => _table;
    // association methods
    private void AddTable(Table table)
    {
        if (table == null) throw new ArgumentNullException($"Table can't be null in AddTable() while addin to the TableOrder");
        _table = table;
        table.AddTableOrder(this);
    }
    private void RemoveTable()
    {
        _table.RemoveTableOrder(this);
        _table = null;
    }
    


    // validation

    //crud
    private static void AddTableOrder(TableOrder table)
    {
        if (table == null) throw new ArgumentException("Table can't be null");
        _tableOrders.Add(table);
    }
    public override void RemoveOrder()
    {
        if (_tableOrders.Contains(this))
        {
            // remove online order
            _tableOrders.Remove(this);
            
            // remove association with table
            RemoveTable(); // _table = null

            // remove association with registered client 2
            RemoveRegisteredClient();
            
            // remove association with the MenuItem
            foreach (var orderList in _menuItems)
            {
                RemoveMenuItemFromOrder(orderList);
            }
            
        }
    }

    public static List<TableOrder> GetTableOrders()
    {
        return new List<TableOrder>(_tableOrders);
    }
}
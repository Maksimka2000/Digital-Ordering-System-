using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class TableOrder : Order
{
    // class extent 
    private static List<TableOrder> _tableOrders = new List<TableOrder>();

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
    public TableOrder(int numberOfPeople) : base(numberOfPeople)
    {
        QRCodeScanTime =
            new TimeSpan(DateTime.Now
                .Ticks); // change soon. We assign the value here when the user first requested the web page and taking when the user sent the order form and then if the device is the same asingn value to hte qr code
        StartTime = new TimeSpan(DateTime.Now.Ticks);
    }

    // validation

    //crud
    public static void AddTableOrder(TableOrder table)
    {
        if (table == null) throw new ArgumentException("Table can't be null");
        _tableOrders.Add(table);
    }

    public static List<TableOrder> GetTableOrders()
    {
        return new List<TableOrder>(_tableOrders);
    }

    //  serialized and deserialized 
    public static void SaveTableOrderJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_tableOrders, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File TableOrder saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving TableOrder file: {e.Message}");
        }
    }

    public static void LoadTableOrderJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _tableOrders = JsonConvert.DeserializeObject<List<TableOrder>>(json);
                Console.WriteLine($"File TableOrder loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading TableOrder file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading TableOrder file: {e.Message}");
        }
    }
}
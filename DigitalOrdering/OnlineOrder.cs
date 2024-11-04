using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class OnlineOrder : Order
{
    //class extent
    private static List<OnlineOrder> _onlineOrders = new List<OnlineOrder>();
    
    // static fields
    private static TimeSpan Duration = new TimeSpan(2, 0, 0);
    
    //fields
    private DateTime _dateAndTime;
    private string? _description;
    public bool IsGuestesArrived { get; private set; }
    
    // fields setter validation
    public string? Description
    {
        get => _description;
        private set
        {
            ValidateStringOptional(value, "description");
            _description = value;
        }
    }
    public DateTime DateAndTime
    {
        get => _dateAndTime;
        private set
        {
            // future validation
            _dateAndTime = value;
        }
    }
    public new TimeSpan? StartTime
    {
        get => base.StartTime;
        set
        {
            base.StartTime = value;
        }
    }

    // Constructor
    [JsonConstructor]
    public OnlineOrder(int numberOfPeople, DateTime dateAndTime, string? description = null) : base(numberOfPeople)
    {
        DateAndTime = dateAndTime;
        Description = description;
        IsGuestesArrived = false;
        StartTime = null;
    }
    

    // validations
    private static void ValidateStringOptional(string? value, string propertyName)
    {
        if (value == string.Empty)  throw new ArgumentException($"{propertyName} cannot be empty");
    }
    
    
    //CRUD
    public static void AddOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder == null) throw new ArgumentException("Online order cannot be null");
        _onlineOrders.Add(onlineOrder);
    }
    public static List<OnlineOrder> GetOnlineOrders()
    {
        return new List<OnlineOrder>(_onlineOrders);
    }
    
    //methods
    public void MarkAsGuestesArrived()
    {
        IsGuestesArrived = true;
        StartTime = new TimeSpan(DateTime.Now.Ticks);
    }
    
    //  serialized and deserialized 
    public static void SaveOnlineOrderJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_onlineOrders, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File OnlineOrder saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving OnlineOrder file: {e.Message}");
        }
    }
    public static void LoadOnlineOrderJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _onlineOrders = JsonConvert.DeserializeObject<List<OnlineOrder>>(json);
                Console.WriteLine($"File OnlineOrder loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading OnlineOrder file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading OnlineOrder file: {e.Message}");
        }
    }
}
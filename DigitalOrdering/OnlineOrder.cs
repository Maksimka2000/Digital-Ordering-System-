using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class OnlineOrder : Order
{
    //class extent
    private static List<OnlineOrder> _onlineOrders = [];
    
    // static fields
    private static TimeSpan _duration = new TimeSpan(2, 0, 0);
    public static TimeSpan Duration
    {
        get => _duration;
        private set
        {
            ValidateDuration(value);
            _duration = value;
        }
    }

    //fields
    private DateTime _dateAndTime;
    private string? _description;
    public bool HaveGuestsArrived { get; private set; }
    
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
            ValidateDateAndTime(value);
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
        HaveGuestsArrived = false;
        StartTime = null;
    }
    

    // validations
    private static void ValidateStringOptional(string? value, string propertyName)
    {
        if (value == string.Empty)  throw new ArgumentException($"{propertyName} cannot be empty");
    }
    private static void ValidateDuration(TimeSpan value)
    {
        if(value < new TimeSpan (1, 0, 0)) throw new ArgumentException ("Duration must be greater than 1 hour");
    }
    private static void ValidateDateAndTime(DateTime value)
    {
        if( value < DateTime.Now.AddHours(3)) throw new ArgumentException("Date and time must be in the future and more than 3 hours ahead.");
    }
    
    
    //CRUD
    public static void AddOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder == null) throw new ArgumentException("Online order cannot be null");
        _onlineOrders.Add(onlineOrder);
    }
    public static List<OnlineOrder> GetOnlineOrders()
    {
        return [.._onlineOrders];
    }
    
    //methods
    public void MarkAsGuestsArrived()
    {
        HaveGuestsArrived = true;
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
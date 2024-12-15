using System.Diagnostics;
using System.Text.RegularExpressions;
using DidgitalOrdering;
using DigitalOrdering;
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class RegisteredClient
{
    
    //class extent
    private static List<RegisteredClient> _registeredClients = [];
    
    //static fields
    private static int IdCounter = 0;
    
    //fields
    public int Id { get; }
    private string _name;
    private string _password;
    private string? _surname;
    private string? _email;
    private string? _phoneNumber;
    public int Bonus {get; private set;}
    
    //fields setters validation 
    public string Password
    {
        get => _password;
        private set
        {
            ValidatePassword(value);
            ValidatePasswordRegex(value);
            _password = value;
        }
    }
    public string? Surname
    {
        get => _surname;
        private set
        {
            ValidateSurname(value);
            _surname = value;
        }
    }
    public string? Email
    {
        get => _email;
        private set
        {
            if (value != null)
            {
                ValidateEmail(value);
                ValidateEmailRegex(value);
            }
            _email = value;
        }
    }
    public string? PhoneNumber
    {
        get => _phoneNumber;
        private set
        {
            ValidatePhoneNumber(value);
            ValidatePhoneNumberRegex(value);
            _phoneNumber = value;
        }
    }
    public string Name
    {
        get => _name;
        private set
        {
            ValidateName(value);
            _name = value;
        }
    }

    // constructor 
    [JsonConstructor]
    public RegisteredClient(string name, string password, string? email = null, string? phoneNumber = null, string? surname = null)
    {
        Id = ++IdCounter;
        Name = name;
        Password = password;
        Surname = surname;
        ValidateEmailAndPhoneNumberInput(email, phoneNumber);
        Email = email;
        PhoneNumber = phoneNumber;
        Bonus = 0;
    }
    
    //association with OnlineOrder
    private Dictionary<int, OnlineOrder> _onlineOrders = [];
    public Dictionary<int, OnlineOrder> OnlineOrders => _onlineOrders;
    public void AddOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder == null) throw new ArgumentNullException($"Parameter {nameof(onlineOrder)} cannot be null in AddOnlineOrder() RegisteredClient");
        if(onlineOrder.RegisteredClient == null) throw new ArgumentException($"you can't add the online order directly to the user, it is done automatically after creation of the online order");
        if (onlineOrder.RegisteredClient != this) throw new AggregateException($"online order you are trying to add belong to different client");
        if (!_onlineOrders.ContainsKey(onlineOrder.Id))
        {
            _onlineOrders[onlineOrder.Id] = onlineOrder;
            onlineOrder.AddOnlineOrderToRegisteredClient(this);
        }
    }
    
    //association with Order reverse
    private List<Order> _orders = [];
    // public List<Order> Orders => [.._orders];
    public void AddOrder(Order order)
    {
        if (order == null) throw new ArgumentException($"Order can be null in the AddOrder() Registered client");
        if(order.RegisteredClient != null && order.RegisteredClient != this) throw new ArgumentException($"Order {order.Id} has already client asigned to it: {order.RegisteredClient.Id}");
        if (!_orders.Contains(order))
        {
            _orders.Add(order);
            order.AddRegisteredClient(this);
        }
    }
    
    // validation methods
    private static void ValidateEmailAndPhoneNumberInput(string? email, string? phoneNumber)
    { 
        if(email is null && phoneNumber is null) throw new NullReferenceException("Email and phone number cannot be null");
    }
    private static void ValidateSurname(string? value)
    {
        if(value == string.Empty) throw new ArgumentException($"Surname cannot be empty");
        if(value is { Length: < 2 }) throw new ArgumentException($"Surname must be at least 2 characters");
    }
    private static void ValidatePassword(string value)
    {
        if(string.IsNullOrEmpty(value)) throw new ArgumentException($"Password cannot be empty");
    }
    private static void ValidatePasswordRegex(string value)
    {
        var passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
        if(!passwordRegex.IsMatch(value)) throw new ArgumentException($"Password is not valid");
        if(value.Length < 8) throw new ArgumentException($"Password must be at least 8 characters");
    }
    private static void ValidateEmail(string? value)
    {
        if(value == string.Empty) throw new ArgumentException($"Email cannot be empty");
    }
    private static void ValidateEmailRegex(string value)
    {
        var emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        if(value != null && !emailRegex.IsMatch(value)) throw new ArgumentException($"Email is not valid");
        if(value is { Length: < 8 }) throw new ArgumentException($"Email must be at least 8 characters");
    }
    private void ValidatePhoneNumber(string? phoneNumber)
    {
        if (phoneNumber == string.Empty)  throw new ArgumentException($"PhoneNumber cannot be empty");
    }
    private static void ValidatePhoneNumberRegex(string? phoneNumber)
    {
        if (phoneNumber != null &&
            !(new System.Text.RegularExpressions.Regex(@"^(\+48\s?)?(\d{3}[\s-]?\d{3}[\s-]?\d{3})$").IsMatch(
                phoneNumber)))
            throw new ArgumentException(
                "Invalid phoneNumber. Try examples: 455540400, 345 654 456, +48545346345, +48 563 954 944");
    }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"Name cannot be null or empty");
    }
    
    // crud
    public static void AddRegisteredClient(RegisteredClient client)
    {
        if(client == null) throw new ArgumentException("client cannot be null");
        _registeredClients.Add(client);
    }
    public static List<RegisteredClient> GetRegisteredClients()
    {
        return [.._registeredClients];
    }
    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
    }
    public void UpdateName(string newName)
    {
        Name = newName;
    }
    public void UpdateSurname(string newSurname)
    {
        Surname = newSurname;
    }

    public void UpdateEmail(string? newEmail)
    {
        var oldEmail = Email;
        Email = newEmail;
        if (newEmail is null && PhoneNumber is null)
        {
            Email = oldEmail;
            throw new NullReferenceException("Email cannot be null");
        }   
    }
    public void UpdatePhoneNumber(string? newPhoneNumber)
    {
        var oldPhoneNumber = PhoneNumber;
        PhoneNumber = newPhoneNumber;
        if (newPhoneNumber is null && Email is null)
        {
            PhoneNumber = oldPhoneNumber;
            throw new NullReferenceException("Email cannot be null");
        }
    }
    
    
}
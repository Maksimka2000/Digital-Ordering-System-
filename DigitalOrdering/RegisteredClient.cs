using System.Diagnostics;
using System.Text.RegularExpressions;
using DidgitalOrdering;
using DigitalOrdering;
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class RegisteredClient : NonRegisteredClient
{
    
    //class extent
    private static List<RegisteredClient> _registeredClients = [];
    
    //static fields
    private static int IdCounter = 0;
    
    //fields
    public int Id { get; }
    private string _password;
    private string? _surname;
    private string? _email;
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
    public new string? PhoneNumber
    {
        get => base.PhoneNumber;
        private set
        {
            ValidatePhoneNumber(value);
            ValidatePhoneNumberRegex(value);
            base.PhoneNumber = value;
        }
    }

    // constructor 
    [JsonConstructor]
    public RegisteredClient(string name, string password, string? email = null, string? phoneNumber = null, string? surname = null) : base(name, phoneNumber)
    {
        Id = ++IdCounter;
        Password = password;
        Surname = surname;
        ValidateEmailAndPhoneNumberInput(email, phoneNumber);
        Email = email;
        PhoneNumber = phoneNumber;
        Bonus = 0;
    }
    
    //association with Order
    private List<Order> _orders = [];
    public List<Order> Orders => [.._orders];
    public void AddOrder(Order order)
    {
        if (order == null) throw new ArgumentException($"Order can be null in the AddOrder() Registered client");
        if(_orders.Contains(order)) return;
        if (order.RegisteredClient == null || order.RegisteredClient == this)
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
    protected override void ValidatePhoneNumber(string? phoneNumber)
    {
        if (phoneNumber == string.Empty)  throw new ArgumentException($"PhoneNumber cannot be empty");
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
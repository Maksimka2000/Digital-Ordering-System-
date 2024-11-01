using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class RegisteredClient : NonRegisteredClient
{
    
    //class extent
    private static List<RegisteredClient> _registeredClients = new List<RegisteredClient>();
    
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
            _password = value;
        }
    }
    public new string Name
    {
        get => base.Name;
        set
        {
            ValidateName(value);
            base.Name = value;
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
            ValidateEmail(value);
            _email = value;
        }
    }
    public new string? PhoneNumber
    {
        get => base.PhoneNumber;
        set
        {
            ValidatePhoneNumber(value);
            base.PhoneNumber = value;
        }
    }

    // constructor 
    [JsonConstructor]
    public RegisteredClient(string name, string password, string? surname = null, string? email = null, string? phoneNumber = null) : base(name, phoneNumber)
    {
        Id = ++IdCounter;
        // name
        Password = password;
        Surname = surname;
        Email = email;
        ValidateEmailAndPhoneNumberInput(email, phoneNumber);
        Bonus = 0;
    }
    
    // validation methods
    private static void ValidateEmailAndPhoneNumberInput(string? email, string? phoneNumber)
    { 
        if(email is null && phoneNumber is null) throw new NullReferenceException("Email and phone number cannot be null");
    }
    private static void ValidateSurname(string? value)
    {
        if(value == string.Empty) throw new ArgumentException($"Surname cannot be empty");
        if(value != null && value.Length < 2) throw new ArgumentException($"Surname must be at least 2 characters");
    }
    private static void ValidatePassword(string value)
    {
        Regex PasswordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
        if(string.IsNullOrEmpty(value)) throw new ArgumentException($"Password cannot be empty");
        if(!PasswordRegex.IsMatch(value)) throw new ArgumentException($"Password is not valid");
        if(value.Length < 8) throw new ArgumentException($"Password must be at least 8 characters");
    }

    private static void ValidateEmail(string? value)
    {
        Regex EmailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        if(value == string.Empty) throw new ArgumentException($"Email cannot be empty");
        if(value != null && !EmailRegex.IsMatch(value)) throw new ArgumentException($"Email is not valid");
    }
    
    // crud
    public static void AddRegisteredClient(RegisteredClient client)
    {
        if(client == null) throw new ArgumentException("client cannot be null");
        _registeredClients.Add(client);
    }
    public static List<RegisteredClient> GetRegisteredClients()
    {
        return new List<RegisteredClient>(_registeredClients);
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
        Email = newEmail;
        if(newEmail is null && PhoneNumber is null) throw new NullReferenceException("Email cannot be null");   
    }
    public void UpdatePhoneNumber(string? newPhoneNumber)
    {
        PhoneNumber = newPhoneNumber;
        if(newPhoneNumber is null && Email is null) throw new NullReferenceException("Email cannot be null");
    }
    
    //  serialized and deserialized 
    public static void SaveRegisteredClientJSON(string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_registeredClients, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File RegisteredClient saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving RegisteredClient file: {e.Message}");
        }
    }

    public static void LoadRegisteredClientJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _registeredClients = JsonConvert.DeserializeObject<List<RegisteredClient>>(json);
                Console.WriteLine($"File RegisteredClient loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading RegisteredClient file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading RegisteredClient file: {e.Message}");
        }
    }
}
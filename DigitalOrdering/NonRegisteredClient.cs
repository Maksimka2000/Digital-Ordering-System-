using Newtonsoft.Json;

namespace DidgitalOrdering;

public class NonRegisteredClient
{
    // class extent
    // there is no need for class extent as we don't need to store information of it; 
    // client object will be stored inside the order, number and name will be stored in the order field like a complex attribute
    // private static List<NonRegisteredClient> _clients = new List<NonRegisteredClient>();

    //fields
    private string _name;
    private string? _phoneNumber;
    
    //setter validation
    public string? PhoneNumber
    {
        get => _phoneNumber;
        protected set
        {
            ValidatePhoneNumber(value);
            ValidatePhoneNumberRegex(value);
            _phoneNumber = value;
        }
    }

    public string Name
    {
        get => _name;
        protected set
        {
            ValidateName(value);
            _name = value;
        }
    }

    // constructor
    [JsonConstructor]
    public NonRegisteredClient(string name, string? phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    // validation methods
    private static void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"Name cannot be null or empty");
    }

    protected virtual void ValidatePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) throw new ArgumentException($"PhoneNumber cannot be null or empty");
    }

    protected static void ValidatePhoneNumberRegex(string? phoneNumber)
    {
        if (phoneNumber != null &&
            !(new System.Text.RegularExpressions.Regex(@"^(\+48\s?)?(\d{3}[\s-]?\d{3}[\s-]?\d{3})$").IsMatch(
                phoneNumber)))
            throw new ArgumentException(
                "Invalid phoneNumber. Try examples: 455540400, 345 654 456, +48545346345, +48 563 954 944");
    }
}
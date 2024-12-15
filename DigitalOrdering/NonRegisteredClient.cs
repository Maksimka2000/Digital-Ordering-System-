using Newtonsoft.Json;

namespace DidgitalOrdering;

public class NonRegisteredClient
{
    //fields
    private string _name;
    private string? _phoneNumber;
    
    //setter validation
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
    public NonRegisteredClient(string name, string? phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public override string ToString()
    {
        return $"Name: {Name}, PhoneNumber: {PhoneNumber}";
    }

    // validation methods
    private static void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"Name cannot be null or empty");
    }

    private void ValidatePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) throw new ArgumentException($"PhoneNumber cannot be null or empty");
    }

    private static void ValidatePhoneNumberRegex(string? phoneNumber)
    {
        if (phoneNumber != null &&
            !(new System.Text.RegularExpressions.Regex(@"^(\+48\s?)?(\d{3}[\s-]?\d{3}[\s-]?\d{3})$").IsMatch(
                phoneNumber)))
            throw new ArgumentException(
                "Invalid phoneNumber. Try examples: 455540400, 345 654 456, +48545346345, +48 563 954 944");
    }
}
using Newtonsoft.Json;

namespace DigitalOrdering;

[Serializable]
public class NonRegisteredClient
{
    
    // class extent
    // there is no need for class extent as we don't need to store information of it; 
    // client object will be stored inside the order, number and name will be stored in the order field like a complex attribute
    private static List<NonRegisteredClient> _clients = new List<NonRegisteredClient>();
    
    //fields
    public string Name { get; protected set; }
    public string? PhoneNumber { get; protected set; }
    
    //setter validation
    // no need as we don't plan to change it latter

    // constructor
    [JsonConstructor]
    public NonRegisteredClient(string name, string? phoneNumber)
    {
        ValidateName(name);
        Name = name;
        ValidatePhoneNumber(phoneNumber);
        PhoneNumber = phoneNumber;
    }

    // validation methods
    protected  static void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException($"Name cannot be null or empty");
    }
    protected  static void ValidatePhoneNumber(string? phoneNumber)
    {
        if (phoneNumber == string.Empty)  throw new ArgumentException($"PhoneNumber cannot be null or empty");
        if (phoneNumber != null && !(new System.Text.RegularExpressions.Regex(@"^(\+48\s?)?(\d{3}[\s-]?\d{3}[\s-]?\d{3})$").IsMatch(phoneNumber)))
            throw new ArgumentException("Invalid phoneNumber. Try examples: 455540400, 345 654 456, +48545346345, +48 563 954 944");
    }
    
}
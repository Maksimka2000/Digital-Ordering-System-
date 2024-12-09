using System.Collections;
using DigitalOrdering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class SetOfMenuItem : MenuItem
{
    // class extent
    private static List<SetOfMenuItem> _setOfMenuItems = [];
    
    //fields
    private List<DayOfWeek> _days = [];
    public TimeSpan? StartTime { get; private set;}
    public TimeSpan? EndTime { get; private set;}
    
    // getters and setters
    public List<DayOfWeek> Days => [.._days];

    // constructor
    [JsonConstructor]
    public SetOfMenuItem(string name, double price, string description, List<Food>? foods = null, List<Beverage>? beverages = null, List<DayOfWeek>? days = null, TimeSpan? startTime = null, TimeSpan? endTime = null) :
        base(name, price, description)
    {
        UpdateDays(days);
        UpdateTime(startTime, endTime);
        ValidateFoodAndBeverage(foods, beverages);
        UpdateFoods(foods);
        UpdateBeverages(beverages);
    }

    // methods for the multi-value attribute
    public void AddDay(DayOfWeek day)
    {
        if (!_days.Contains(day)) _days.Add(day);
    }
    public void RemoveDay(DayOfWeek day)
    {
        if (_days.Contains(day)) _days.Remove(day);
    }
    public void UpdateDays(List<DayOfWeek>? days)
    {
        if (days != null && days.Count > 0)
        {
            if(days.Distinct().Count() != days.Count) throw new ArgumentException("Duplicate of Days are not permitted in SetOfMenuItem");
            if (_days.Count > 0)
                foreach (var day in _days)
                    RemoveDay(day);
            else
                foreach (var day in days)
                    AddDay(day);
        }
        else if (days == null)
        {
            _days = Enum.GetValues<DayOfWeek>().ToList();
        }
        else
            throw new ArgumentException("You can't pass empty list to UpdateDays()");
        
    }

    // associations 
    private List<Food> _foods = [];
    private List<Beverage> _beverages = [];
    
    // associations getters
    public List<Food> Foods => [.._foods];
    public List<Beverage> Beverages => [.._beverages];
    
    // asoosciations methods
    public void AddFood(Food food)
    {
        if(food is null)  throw new ArgumentNullException($"{this.Name}. Food cannot be null in AddFood method.");
        if (!_foods.Contains(food))
        {
            _foods.Add(food);
            food.AddSetOfMenuItemsToFood(this);
        }
    }
    public void RemoveFood(Food food)
    {
        if(food is null)  throw new ArgumentNullException($"{this.Name}. Food cannot be null in Delete method.");
        if (_foods.Contains(food))
        {
            _foods.Remove(food);
            food.RemoveSetOfMenuItemsFromFood(this);
        }
    }
    public void UpdateFoods(List<Food>? foods)
    {
        if (foods != null && foods.Count != 0)
        {
            if(_foods.Count > 0) foreach(Food food in _foods) RemoveFood(food);
            foreach(Food food in foods) AddFood(food);    
        }
    }

    public void AddBeverage(Beverage beverage)
    {
        if(beverage is null) throw new ArgumentNullException($"{this.Name}. Beverages cannot be null in AddBeverage method.");
        if (!_beverages.Contains(beverage))
        {
            _beverages.Add(beverage);
            beverage.AddSetOfMenuItemsToBeverage(this);
        }
    }
    public void RemoveBeverage(Beverage beverage)
    {
        if(beverage is null) throw new ArgumentNullException($"{this.Name}. Beverages cannot be null in RemoveBeverage method.");
        if (_beverages.Contains(beverage))
        {
            _beverages.Remove(beverage);
            beverage.RemoveSetOfMenuItemsFromBeverage(this);
        }
    }
    public void UpdateBeverages(List<Beverage>? beverages)
    {
        if (beverages != null && beverages.Count != 0)
        {
            if(_beverages.Count > 0) foreach(Beverage beverage in _beverages) RemoveBeverage(beverage);
            foreach(Beverage beverage in beverages) AddBeverage(beverage);    
        }
    }

    // validation 
    private void ValidateTime(TimeSpan? startTime, TimeSpan? endTime)
    {
        if((startTime.HasValue ^ endTime.HasValue)) throw new ArgumentException("Start time and end time must be specified or nothing");
        if(startTime.HasValue && endTime.HasValue && startTime >= endTime) throw new ArgumentException("Start time must be before end time");
    }
    private void ValidateFoodAndBeverage(List<Food>? foods, List<Beverage>? beverages)
    {
        if((foods == null || foods.Count == 0) && (beverages == null || beverages.Count == 0)) throw new ArgumentException("at least one element from the Food or Beverage is required");
    }

    // Methods for the Object
    public static void AddSetOfMenuItems(SetOfMenuItem setOfMenuItem)
    {
        if (setOfMenuItem == null) throw new ArgumentException("businessLunch cannot be null");
        _setOfMenuItems.Add(setOfMenuItem);
    }
    public static List<SetOfMenuItem> GetSetOfMenuItems()
    {
        return [.._setOfMenuItems];
    }
    public static void DeleteSetOfMenuItems(SetOfMenuItem setOfMenuItem)
    {
        _setOfMenuItems.Remove(setOfMenuItem);
    }

    //  Attribute StartTime and EndTime Methods 
    public void UpdateTime(TimeSpan? startTime, TimeSpan? endTime)
    {
        ValidateTime(startTime, endTime);
        if (startTime == null && endTime == null)
        {
            StartTime = new TimeSpan(0, 0, 0);
            EndTime = new TimeSpan(23, 59, 59);
        }
        else
        {
            StartTime = startTime;
            EndTime = endTime;
        }
        
    }
    public void RemoveTime()
    {
        UpdateTime(null, null);
    }

    
    

    // ================================================================ serialized and deserialized
    public static void SaveSetOfMenuItemsJson(string path)
    {
        try
        {
            var json = JsonConvert.SerializeObject(_setOfMenuItems, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine($"File SetOfMenuItem saved successfully at {path}");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving SetOfMenuItem file: {e.Message}");
        }
    }

    public static void LoadSetOfMenuItemsJson(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _setOfMenuItems = JsonConvert.DeserializeObject<List<SetOfMenuItem>>(json);
                Console.WriteLine($"File BusinessLunch loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading SetOfMenuItem file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading SetOfMenuItem file: {e.Message}");
        }
    }
}
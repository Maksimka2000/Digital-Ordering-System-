using DidgitalOrdering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalOrdering;

[Serializable]
public class Restaurant
{
    // class extent
    private static List<Restaurant> _restaurants = [];

    // static fields
    private static int IdCounter = 0;

    // fields
    public int Id { get; }
    private string _name;
    private Address _location;
    private List<OpenHour> _openHours;
    
    // setters validation
    public List<OpenHour> OpenHours => [.._openHours];
    public string Name
    {
        get => _name;
        private set
        {
            ValidateStringMandatory(value, "Restaurant Name");
            _name = value;
        }
    }
    
    public Address Location
    {
        get => _location.Clone();
        private set
        {
            ValidateLocation(value);
            _location = value;
        }
    }

    //constructor
    [JsonConstructor]
    public Restaurant(string name, Address location, List<OpenHour> openHours, Table? table = null, MenuItem? menuItem = null)
    {
        Id = ++IdCounter;
        Name = name;
        Location = location;
        UpdateOpenHours(openHours);
        AddRestaurant(this);

        //default values tables, menuItem as a part of the composition association
        if (table == null) AddTable( 99, null, "Default table");
        if (menuItem == null) AddMenuItemToMenu("Default MenuItem", 99, "Default menu item");
    }
        
    //association with MenuItem 
    private List<MenuItem> _menu = [];
    public List<MenuItem> Menu => [.._menu];
    public void AddMenuItemToMenu(string name, double price, string description,
        Beverage.BeverageType beverageT, bool isAlcohol,
        List<Ingredient>? ingredients = null, Promotion? promotion = null, bool isAvailable = true)
    {
        new Beverage(this, name, price, description, beverageT, isAlcohol);
    }
    public void AddMenuItemToMenu(string name, double price, string description,
        Food.FoodType foodT,
        List<Ingredient>? ingredients,
        List<Food.DietaryPreferencesType>? dietaryPreference = null, Promotion? promotion = null, bool isAvailable = true)
    {
        new Food(this, name, price, description, foodT, ingredients, dietaryPreference, promotion, isAvailable);
    }
    public void AddMenuItemToMenu(string name, double price, string description, List<Food>? foods = null,
        List<Beverage>? beverages = null, List<DayOfWeek>? days = null, TimeSpan? startTime = null,
        TimeSpan? endTime = null, bool isAvailable = true) 
    {
        new SetOfMenuItem(this, name, price, description, foods, beverages, days, startTime, endTime, isAvailable);
    }
    public void AddMenuItemToMenu(MenuItem menuItem)
    {
        if(menuItem == null) throw new ArgumentNullException($"Menu item is null in AddMenuItemToMenu() Restaurant");
        if(menuItem.Restaurant != this) throw new AggregateException($"Menu item you are trying to add belong to other Restaurant: {menuItem.Restaurant.Name}");
        if(!_menu.Contains(menuItem)) _menu.Add(menuItem);
    }
    public void RemoveMenuItemFromMenu(MenuItem menuItem)
    {
        if(menuItem == null) throw new ArgumentException($"Menu item is null in RemoveMenuItemFromMenu() Restaurant");
        if (menuItem.Restaurant != this) throw new AggregateException($"you are trying to remove menu item with doesn't belong to this restaurant {menuItem.Restaurant.Name}.");
        if (_menu.Contains(menuItem))
        {
            _menu.Remove(menuItem);
            menuItem.RemoveMenuItem();
        }
    }
    
    
    // association with tables
    private List<Table> _tables = [];
    // association getter
    [JsonIgnore]
    public List<Table> Tables => [.._tables];
    // associaiton methods
    public void AddTable(int capacity, string? alias = null, string? description = null)
    {
        var table = new Table(this, capacity, description, alias);
    }
    public void AddTable(Table table)
    {
        if(table == null) throw new ArgumentNullException($"Table is null in the AddTable method");
        if(table.Restaurant != this) throw new ArgumentException($"Table can belong only to one Restaurant which is: {table.Restaurant.Name}");
        if(!_tables.Contains(table)) _tables.Add(table);     
    }
    public void RemoveTable(Table table) // if we remove the table we remove the whole object of table as table can't exit without the restaurant and table actually
    {
        if (table == null) throw new ArgumentNullException($"Table is null in the RemoveTable method restaurant.cs");
        if (table.Restaurant != this) throw new ArgumentException($"Table doesn't belong to this restaurant");
        if (_tables.Contains(table))
        {
            _tables.Remove(table);
            table.DeleteTable();
        }
    }
    
    
    //association with OnlineOrder 
    private List<OnlineOrder> _onlineOrders = [];
    //association getter
    public List<OnlineOrder> OnlineOrders => _onlineOrders;
    //association methods
    public void AddOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder == null) throw new ArgumentNullException($"onlineOrder is null in the AddOnlineOrder()");
        if(onlineOrder.Restaurant != this) throw new ArgumentException($"online order can belong only to one Restaurant which is: {onlineOrder.Restaurant.Name}");
        if (!_onlineOrders.Contains(onlineOrder))
        {
            _onlineOrders.Add(onlineOrder);
            onlineOrder.AddRestaurant(this);
        }
    }
    public void RemoveOnlineOrder(OnlineOrder onlineOrder)
    {
        if(onlineOrder == null) throw new ArgumentException($"online order can't be null in REmoveONlineOrder()");
        // if(onlineOrder.Restaurant != this) throw new ArgumentException($"online order doesn't belong to this restaurant");
        if (_onlineOrders.Contains(onlineOrder))
        {
            _onlineOrders.Remove(onlineOrder);
            onlineOrder.RemoveRestaurant();
        }
    }
    
    
    
    // multi-value attribute methods
    public void UpdateOpenHours(List<OpenHour> newOpenHours)
    {
        ValidateWorkHours(newOpenHours);
        ValidateWorkHoursDays(newOpenHours);
        _openHours = newOpenHours;
    }

    // validation
    private static void ValidateStringMandatory(string value, string text)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{text} cannot be null or empty");
    }

    private static void ValidateWorkHours(List<OpenHour> workHours)
    {
        if (workHours == null) throw new ArgumentException("work hours is null");
    }

    private static void ValidateWorkHoursDays(List<OpenHour> workHours)
    {
        if (workHours.Count != 7) throw new ArgumentException("Work hours cannot be less than 7");
        if (workHours.Select(wh => wh.Day).Distinct().Count() != 7)
            throw new ArgumentException("not all days specified.");
    }

    private static void ValidateLocation(Address value)
    {
        if (value == null) throw new ArgumentNullException(" address cannot be null");
    }

    // get, delete, add, update.
    public static List<Restaurant> GetRestaurants()
    {
        return [.._restaurants];
    }

    private static void AddRestaurant(Restaurant restaurant)
    {
        _restaurants.Add(restaurant);
    }

    public static void RemoveRestaurant(Restaurant restaurant)
    {
        // start stransaction
        //  check if all tables have any active orders or reservations
        foreach (var table in restaurant.Tables)
        {
            table.DeleteTable();
        }

        //chekc if there are any more online orders for this restaurant 
        foreach (var onlineOrder in restaurant.OnlineOrders)
        {
            onlineOrder.RemoveOrder();
        }

        //remove every menuItem in this restaurant 
        foreach (var menuItem in restaurant.Menu)
        {
            menuItem.RemoveMenuItem();
        }
        
        _restaurants.Remove(restaurant);
    }
    public void UpdateName(string newName)
    {
        Name = newName;
    }

    // methods
    public bool IsRestaurantOpen(DayOfWeek dayOfWeek, TimeSpan currentTime)
    {
        var openHoursDay = GetOpenHour(dayOfWeek);
        return openHoursDay.IsOpen && openHoursDay.OpenTime <= currentTime && openHoursDay.CloseTime >= currentTime;
    }
    public void UpdateOpenHour(DayOfWeek dayOfWeek, TimeSpan? openTime, TimeSpan? closeTime)
    {
        var day = GetOpenHour(dayOfWeek);
        day.UpdateTime(openTime, closeTime);
        Console.WriteLine($"time succesfully updated: {(day.IsOpen ? $"from {openTime} to {closeTime} on {dayOfWeek}" : $"closed on {dayOfWeek}")}");
    }
    public OpenHour GetOpenHour(DayOfWeek dayOfWeek)
    {
        var openHour = OpenHours.FirstOrDefault(openHour => openHour.Day == dayOfWeek);
        if ( openHour == null ) throw new KeyNotFoundException($"No open hours found for day {dayOfWeek}");
        return openHour;
    }
    public void ListStandByOrdersWIthTableOccupied()
    {
        var tablesWithOrders = Tables.Where(table => table.Orders.Any(order => order.Role == Order.OrderRole.StandBy) && table.IsOccupied).ToList();
        Console.WriteLine($"        There are {tablesWithOrders.Count()} Table which has StandBy orders and table is occupied (so the manager can send order to kitchen):");
        foreach (var table in tablesWithOrders)
        {
            var tableOrders = table.Orders.ToList();
            Console.WriteLine($"            Table id: {table.Id}. Belongs to {table.Restaurant.Name}. Table is occupied: {table.IsOccupied}. Has {tableOrders.Count} orders:");
            foreach (var order in tableOrders)
            {
                if (order is TableOrder tableOrder)
                {
                    Console.WriteLine($"                Table order id: {tableOrder.Id}. Belong to restaurant: {tableOrder.Table.Restaurant.Name}. Belong to table: {tableOrder.Table.Id}. Number of People: {tableOrder.NumberOfPeople}. Registered client: {tableOrder.RegisteredClient?.Id}, {tableOrder.RegisteredClient?.Name}, {tableOrder.RegisteredClient?.PhoneNumber}. Status of the TableOrder is: {tableOrder.Role}. Has the following items: {tableOrder.MenuItems.Count}: ");
                    foreach (var orderList in tableOrder.MenuItems)
                    {
                        Console.WriteLine($"                    [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity}. Price: {orderList.MenuItem.Price}. Discount: {orderList.MenuItem.Promotion?.DiscountPercent} ]");
                    }
                    Console.WriteLine($"                    Summary: Order price is: {tableOrder.OrderPrice}. Service Price is: {tableOrder.ServicePrice}. Discount: {tableOrder.DiscountAmount}. Total Price is: {tableOrder.TotalPrice}. "); 
                }
                if (order is OnlineOrder onlineOrder && onlineOrder.IsGuestsArrived)
                {
                    Console.WriteLine($"                Online order id: {onlineOrder.Id}. Belong to restaurant: {onlineOrder.Table.Restaurant.Name}. Belong to table: {onlineOrder.Table.Id}. Number of People: {onlineOrder.NumberOfPeople}. Registered client: {onlineOrder.RegisteredClient?.Id}, {onlineOrder.RegisteredClient?.Name}, {onlineOrder.RegisteredClient?.PhoneNumber}. NonRegisteredClietn: {onlineOrder.NonRegisteredClient?.Name}, {onlineOrder.NonRegisteredClient?.Name}. Status OnlineOrder is: {onlineOrder.Role}. Guests Arrived:: {onlineOrder.IsGuestsArrived}. Has the following items: {onlineOrder.MenuItems.Count}: ");
                    foreach (var orderList in onlineOrder.MenuItems)
                    {
                        Console.WriteLine($"                    [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity}. Price: {orderList.MenuItem.Price}. Discount: {orderList.MenuItem.Promotion?.DiscountPercent} ]");
                    }
                    Console.WriteLine($"                    Summary: Order price is: {onlineOrder.OrderPrice}. Service Price is: {onlineOrder.ServicePrice}. Discount: {onlineOrder.DiscountAmount}. Total Price is: {onlineOrder.TotalPrice}. "); 
                }
            }
        }
    }
    public void ListReservations()
    {
        Console.WriteLine($"        There are {OnlineOrders.Count} Online Orders:");
        foreach (var onlineOrder in OnlineOrders)
        {
            Console.WriteLine($"            Online order id: {onlineOrder.Id}, belong to table: {onlineOrder.Table.Id}. Number of People: {onlineOrder.NumberOfPeople}, Date and Time: {onlineOrder.DateAndTime}, Duration: {onlineOrder.Duration}, Description: {onlineOrder.Description}, Guests Arrived: {onlineOrder.IsGuestsArrived}. Registered client: {onlineOrder.RegisteredClient?.Id} {onlineOrder.RegisteredClient?.Name} {onlineOrder.RegisteredClient?.PhoneNumber}. Non registered: {onlineOrder.NonRegisteredClient?.Name}, {onlineOrder.NonRegisteredClient?.PhoneNumber}. Status of the TableOrder is: {onlineOrder.Role}. Has the following menu item: {onlineOrder.MenuItems.Count}:");
            foreach (var orderList in onlineOrder.MenuItems)
            {
                Console.WriteLine($"                [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity}. Price: {orderList.MenuItem.Price}. Discount: {orderList.MenuItem.Promotion?.DiscountPercent} ]");
            }
            Console.WriteLine($"                Summary: Order price is: {onlineOrder.OrderPrice}. Service Price is: {onlineOrder.ServicePrice}. Discount: {onlineOrder.DiscountAmount}. Total Price is: {onlineOrder.TotalPrice}");
        }
    }
    public void ListMenuForTableOrder()
    {
        Console.WriteLine($"        There are {Menu.Count} menu items in the restaurant:");
        var groupedMenuItems = Menu
            .GroupBy(menuItem => menuItem.GetType())
            .ToDictionary(group => group.Key, group => group.ToList());
        foreach (var group in groupedMenuItems)
        {
            // Console.WriteLine($"\n================================ Menu Type: {group.Key.Name} ================================================================    \n");
            Console.WriteLine($"            {group.Value.Count}/{Menu.Count} are {group.Key.Name}: ");
            foreach (var menuItem in group.Value)
            {
                Console.Write($"                        id: {menuItem.Id}, Name: {menuItem.Name}, Price: {menuItem.Price}, Description: {menuItem.Description}, IsAvailable: {menuItem.IsAvailable}. ");
                switch (menuItem)
                {
                    // =============================================  Load Food. MenuItem has Ingredients.
                    case Food food:
                    {
                        Console.Write($"foodType: {food.FoodT}, DietaryPreference: ");
                        if (food.DietaryPreferences.Count > 0)
                        {
                            Console.Write("[");
                            foreach (var dietaryPreference in food.DietaryPreferences)
                                Console.Write($"{dietaryPreference}, ");
                            Console.Write("]");
                        }
                        else
                        {
                            Console.Write("No dietary preferences");
                        }

                        Console.WriteLine();
                        break;
                    }
                    // =============================================  Load Beverage. MenuItem has Ingredients.
                    case Beverage beverage:
                    {
                        Console.WriteLine($" BeverageType: {beverage.BeverageT}");
                        break;
                    }
                    // =============================================  Load SetOfMenuItem. MenuItem has Ingredients.
                    case SetOfMenuItem setOfMenuItems:
                    {
                        Console.WriteLine();
                        Console.WriteLine($"                                 There are {setOfMenuItems.Foods.Count} foods: ");
                        if (setOfMenuItems.Foods.Count > 0)
                        {
                            foreach (var food in setOfMenuItems.Foods)
                            {
                                
                                Console.Write($"                                        [food name: {food.Name}");
                                Console.Write(
                                    $". Promotion for this food: {(food.Promotion == null ? "No Promotion" : food.Promotion.Name)}");
                                Console.Write(
                                    $". Ingredients in the food: {(food.Ingredients.Count == 0 ? "No ingredients" : " [")}");
                                if (food.Ingredients.Count > 0)
                                {
                                    foreach (var ingredient in food.Ingredients)
                                    {
                                        Console.Write($"{ingredient.Name}, ");
                                    }

                                    Console.Write("] ");
                                }

                                Console.WriteLine("]");
                            }
                        }

                        Console.WriteLine($"                                 There are {setOfMenuItems.Beverages.Count} beverages: ");
                        if (setOfMenuItems.Beverages.Count > 0)
                        {
                            foreach (var beverage in setOfMenuItems.Beverages)
                            {
                                Console.Write($"                                        [Beverage name: {beverage.Name}");
                                Console.Write(
                                    $". Promotion for this beverage: {(beverage.Promotion == null ? "No Promotion" : beverage.Promotion.Name)}");
                                Console.Write(
                                    $". Ingredients in the beverage: {(beverage.Ingredients.Count == 0 ? "No ingredients" : "There are ingredients: [")}");
                                if (beverage.Ingredients.Count > 0)
                                {
                                    foreach (var ingredient in beverage.Ingredients)
                                    {
                                        Console.Write($"{ingredient.Name}, ");
                                    }

                                    Console.Write("] ");
                                }

                                Console.WriteLine("]");
                            }
                        }

                        Console.Write($"                                 Is available on:\n");
                        Console.Write("                                        [");
                        foreach (var day in setOfMenuItems.Days) Console.Write($"{day}, ");
                        Console.Write($"] from {setOfMenuItems.StartTime} to {setOfMenuItems.EndTime}");
                        Console.WriteLine();
                        break;
                    }
                }

                Console.WriteLine(menuItem.Promotion == null
                    ? "                                 No promotion"
                    : $"                                 Promotion: [ name: {menuItem.Promotion.Name}. And {menuItem.Promotion}]");
                Console.Write(menuItem.Ingredients.Count == 0
                    ? "                                 No ingredients"
                    : "                                 There are ingredients: [");
                if (menuItem.Ingredients.Count > 0)
                {
                    foreach (var ingredient in menuItem.Ingredients)
                        Console.Write($"{ingredient.Name}, ");
                    Console.Write("]");   
                }
                Console.WriteLine("");
            }
        }
    }
    public void ListMenuForOnlineOrder()
    {
        // the same as in the ListMenuForTableOrder but with no business lunch.
    }
    public void ListAllFinalizedOrders()
    {
        var allOrders = new List<Order>();
        allOrders.AddRange(TableOrder.GetTableOrders());
        allOrders.AddRange(OnlineOrder.GetOnlineOrders());
        var orders = allOrders.Where(order => order.Role == Order.OrderRole.Finalized && this.Tables.Any(table => table == order.Table )).ToList();
        
        Console.WriteLine($"        There are {orders.Count()} finalized orders");
        foreach (var order in orders)
            {
                if (order is TableOrder tableOrder)
                {
                    Console.WriteLine($"                Table order id: {tableOrder.Id}. Belong to restaurant: {tableOrder.Table.Restaurant.Name}. Belong to table: {tableOrder.Table.Id}. Number of people: {order.NumberOfPeople}. Time and Date: {order.StartTime} - {order.TimeEnd}. Registered client: {tableOrder.RegisteredClient?.Id}, {tableOrder.RegisteredClient?.Name}, {tableOrder.RegisteredClient?.PhoneNumber}. Status of the TableOrder is: {tableOrder.Role}. Has: {order.MenuItems.Count} menuItems in order: ");
                    foreach (var orderList in tableOrder.MenuItems)
                    {
                        Console.WriteLine($"                    [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity}. Price: {orderList.MenuItem.Price}. Discount: {orderList.MenuItem.Promotion?.DiscountPercent} ]");
                    }
                    Console.WriteLine($"                    Summary: Order price is: {tableOrder.OrderPrice}. Service Price is: {tableOrder.ServicePrice}. Discount: {tableOrder.DiscountAmount}. Bonuses used: {order.BonusesUsed}. Total Price is: {tableOrder.TotalPrice}. "); 
                }
                if (order is OnlineOrder onlineOrder)
                {
                    Console.WriteLine($"                Online order id: {onlineOrder.Id}. Belong to restaurant: {onlineOrder.Table.Restaurant.Name}. Belong to table: {onlineOrder.Table.Id}. Number of People: {onlineOrder.NumberOfPeople}. Time and Date: {order.StartTime} - {order.TimeEnd}. Registered client: {onlineOrder.RegisteredClient?.Id}, {onlineOrder.RegisteredClient?.Name}, {onlineOrder.RegisteredClient?.PhoneNumber}. NonRegisteredClient: {onlineOrder.NonRegisteredClient?.Name}, {onlineOrder.NonRegisteredClient?.Name}. Status OnlineOrder is: {onlineOrder.Role}. Guests Arrived:: {onlineOrder.IsGuestsArrived}. Has: {order.MenuItems.Count} menuItems in order: ");
                    foreach (var orderList in onlineOrder.MenuItems)
                    {
                        Console.WriteLine($"                    [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity}. Price: {orderList.MenuItem.Price}. Discount: {orderList.MenuItem.Promotion?.DiscountPercent} ]");
                    }
                    Console.WriteLine($"                    Summary: Order price is: {onlineOrder.OrderPrice}. Service Price is: {onlineOrder.ServicePrice}. Discount: {onlineOrder.DiscountAmount}. Bonuses used: {order.BonusesUsed}. Total Price is: {onlineOrder.TotalPrice}. "); 
                }
            }
    }
}

// are custom objects, Complex attributes
[Serializable]
public class OpenHour
{
    [JsonConverter(typeof(StringEnumConverter))]
    public DayOfWeek Day { get; private set; }

    public TimeSpan? OpenTime { get; private set; }
    public TimeSpan? CloseTime { get; private set; }
    public bool IsOpen { get; private set; }

    [JsonConstructor]
    public OpenHour(DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        Day = day;
        UpdateTime(openTime, closeTime);
    }

    public void UpdateTime(TimeSpan? newOpenTime, TimeSpan? newCloseTime)
    {
        if (newOpenTime.HasValue && newCloseTime.HasValue)
            ValidateTime(newOpenTime, newCloseTime);

        if (newOpenTime.HasValue ^ newCloseTime.HasValue)
            throw new ArgumentException($" both of them can be null or not null.");

        OpenTime = newOpenTime;
        CloseTime = newCloseTime;
        IsOpen = !(OpenTime == null && CloseTime == null);
    }
    private static void ValidateTime(TimeSpan? openTime, TimeSpan? closeTime)
    {
        if (openTime >= closeTime) throw new ArgumentException("Closing time must be later than opening time");
    }
    public override string ToString()
    {
        return $"{Day}: {(IsOpen ? (OpenTime + " : " + CloseTime)  : "closed" )}";
    }
}

[Serializable]
public class Address
{
    private string _street;
    private string _city;
    private string _streetNumber;

    public string Street
    {
        get => _street;
        private set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Street cannot be null or empty.");
            _street = value;
        }
    }
    public string City
    {
        get => _city;
        private set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("City cannot be null or empty.");
            _city = value;
        }
    }
    public string StreetNumber
    {
        get => _streetNumber;
        private set
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Street Number cannot be null or empty.");
            _streetNumber = value;
        }
    }

    [JsonConstructor]
    public Address(string street, string city, string streetNumber)
    {
        Street = street;
        City = city;
        StreetNumber = streetNumber;
    }

    public override string ToString()
    {
        return $"adress: {Street} {StreetNumber}, {City}";
    }

    public Address Clone()
    {
        return new Address(Street, City, StreetNumber);
    }
}
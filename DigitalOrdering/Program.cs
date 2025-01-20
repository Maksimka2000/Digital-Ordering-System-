using System.Globalization;
using DidgitalOrdering;
using DigitalOrdering;
using Newtonsoft.Json;

CreateObjects();
// SaveClassExtent();
// LoadClassExtent();
OutputAllObjectsCreated();


return;

void LoadClassExtent()
{
    SerializationDeserialization.LoadJSON(
        Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Data.json"));
}

void SaveClassExtent()
{
    SerializationDeserialization.SaveJSON(
        Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Data.json"));
}

void OutputAllObjectsCreated()
{
    Console.WriteLine("================================  Ingredients ================================================================    \n");
    foreach (Ingredient ingredient in Ingredient.GetIngredients())
    {
        Console.WriteLine($"Ingredietn id: {ingredient.Id}, Name: {ingredient.Name}");
    }
    Ingredient.GetIngredients().ForEach(ingredient => Console.WriteLine($"Ingredient id: {ingredient.Id}, Name: {ingredient.Name}"));
    Console.WriteLine("\n================================ Registered Clients ================================================================    \n");
    foreach (var client in RegisteredClient.GetRegisteredClients())
    {
        Console.WriteLine($"Client ID: {client.Id}, Name: {client.Name}, Surname: {client.Surname}, Email: {client.Email}, Phone Number: {client.PhoneNumber}");
        Console.WriteLine($"        Client has: {client.Bonus} Bonuses");
        Console.WriteLine($"             client was invited by the: {client.InvitedBy?.Id}, {client.InvitedBy?.Name}");
        foreach (var clientInvited in client.Invited)
        {
            Console.WriteLine($"             client invited: {clientInvited.Id}, {clientInvited.Name}");
        }
        client.ListReservations();
        client.ListOrderHistory();
    }
    Console.WriteLine("\n=========================================== Restaurants are: ================================================================    \n");
    foreach (var restaurant in Restaurant.GetRestaurants())
    {
        Console.WriteLine("\n\n");
        Console.WriteLine($"Restaurant name is: {restaurant.Name}, location is: {restaurant.Location.ToString()},  ");
        Console.WriteLine("         Open hours are:");
        foreach (var openHour in restaurant.OpenHours)
        {
            Console.WriteLine($"            [{openHour.ToString()}]");
        }
        Console.WriteLine($"        Tables are: {restaurant.Tables.Count} tables in the restaurant:");
        foreach (var table in restaurant.Tables)
        {
            Console.WriteLine($"            [Table ID: {table.Id}, belong to the restaurant: {table.Restaurant.Name} Capacity: {table.Capacity}, Alias: {table.Alias ?? "no alias"}, Description: {table.Description ?? "no description"}, IsLocked: {table.IsOccupied}, belong to: {table.Restaurant.Name} restauratn]");
        }
        // Console.WriteLine("\n=================================== Menu                        ================================================================    \n");
        restaurant.ListMenuForTableOrder();
        // Console.WriteLine("\n================================ Online Orders (which are in standBy but guest are not arrived ================================================================    \n");
        restaurant.ListReservations();
        // Console.WriteLine("\n================================ All Orders (StandBy and table occupied and in case of the onlineOrder check for the guest arrived) ================================================================    \n");
        restaurant.ListStandByOrdersWIthTableOccupied();
        // Console.WriteLine("\n================================ all Orders (Finalized) ================================================================    \n");
        restaurant.ListAllFinalizedOrders();
    }

    
    // ================================================ association Ingredients in MenuItems  
    Console.WriteLine("\n=================================== Ingredients in MenuItems  ================================================================    \n");
    foreach (var ingredient in Ingredient.GetIngredients())
    {
        Console.Write($"Ingredient id: {ingredient.Id}, Ingredient name {ingredient.Name}, how many times ingredient is in other menu items: {ingredient.IngredientInMenuItems.Count} [");
        foreach (var menuItem in ingredient.IngredientInMenuItems)
        {
            Console.Write($"[Menu item id: {menuItem.Id} name:{menuItem.Name} Restaurant: {menuItem.Restaurant.Name}], ");
        }
        Console.Write($"] \n");
    }
    //
}

void CreateObjects()
{
    //==================================================================================================================================================================
    //===================================================================== Create Ingredietn 
    var tomatoIngredient = new Ingredient("Tomato");
    var mozzarellaIngredient = new Ingredient("Mozzarella");
    var basilIngredient = new Ingredient("Basil");
    var oliveOilIngredient = new Ingredient("Olive Oil");
    var garlicIngredient = new Ingredient("Garlic");
    var saltIngredient = new Ingredient("Salt");
    var pepperIngredient = new Ingredient("Pepper");
    var chickenIngredient = new Ingredient("Chicken");
    var mushroomIngredient = new Ingredient("Mushrooms");
    var parmesanIngredient = new Ingredient("Parmesan");
    var pastaIngredient = new Ingredient("Pasta");
    var creamIngredient = new Ingredient("Cream");
    var spinachIngredient = new Ingredient("Spinach");
    var broccoliIngredient = new Ingredient("Broccoli");
    var zucchiniIngredient = new Ingredient("Zucchini");
    var shrimpIngredient = new Ingredient("Shrimp");
    var baconIngredient = new Ingredient("Bacon");
    var eggIngredient = new Ingredient("Egg");
    var redPepperFlakesIngredient = new Ingredient("Red Pepper Flakes");
    var onionsIngredient = new Ingredient("Onions");
    var ginIngredient = new Ingredient("Gin");
    var vermouthIngredient = new Ingredient("Vermouth");
    var campariIngredient = new Ingredient("Campari");
    // ==================================================== Registered client
    RegisteredClient client1 = new RegisteredClient("Max", "32jpjoi3j04#A", "s23454@pjatk.com", "546 545 544");
    RegisteredClient client2 = new RegisteredClient("Alexa", "32jpjD$i3j04#A", null, "344 434 344", "Arstv");
    RegisteredClient client3 = new RegisteredClient("Max", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr", client1);
    RegisteredClient client4 = new RegisteredClient("Maximilian", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr", client1);
    RegisteredClient client5 = new RegisteredClient("Alexandra", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr", client2);
    RegisteredClient client6 = new RegisteredClient("Maksym", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr", client2);
    RegisteredClient client7 = new RegisteredClient("Aliaksandra", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr", client2);
    //========================================================= promotion
    var promo1 = new Promotion(10.0, "new year", "it is purpose delete this in february");
    var promo2 = new Promotion(10.0, "He[ppy", "nothi", Promotion.PromotionType.Regular);
    var promo3 = new Promotion(70.0, "buy season");
    //======================================================= WorkHours
    List<OpenHour> workHours = new List<OpenHour>
    {
        new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Sunday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0))
    };
    //==================================================================================================================================================================

    // ========================================================= Create Restaurant1 =========================================================================================
    Restaurant restaurant1 = new Restaurant("Miscusi", new Address("Zlota 43", "Warszawa", "22"), workHours);
    // restaurant1.AddTable(4, "near wc", "do not sit here a guts how are allergic to oil");
    var restaurant1table1 = new Table(restaurant1, 2);
    var restaurant1table2 = new Table(restaurant1, 2, "hello worlds");
    var restaurant1table3 = new Table(restaurant1, 4, "window");
    var restaurant1table4 = new Table(restaurant1, 4, "near wc", "do not sit here a guts how are allergic to oil");
    // ======================================================== Create Menu
    // ======================================================== Create FOod for Restaurant1
    var restaurant1food1 = new Food(restaurant1, "Spaghetti Carbonara", 12.99, "Classic pasta with bacon and eggs",
        Food.FoodType.Pasta, new List<Ingredient> { pastaIngredient, baconIngredient, eggIngredient }, null, promo1);
    var restaurant1food2 = new Food(restaurant1, "Penne Alfredo", 14.99, "Creamy Alfredo pasta with Parmesan",
        Food.FoodType.Pasta, new List<Ingredient> { pastaIngredient, creamIngredient, parmesanIngredient },
        new List<Food.DietaryPreferencesType> { Food.DietaryPreferencesType.GlutenFree }, promo2);
    var restaurant1food3 = new Food(restaurant1, "Fettuccine Primavera", 13.99,
        "Pasta with fresh vegetables and olive oil", Food.FoodType.Pasta,
        new List<Ingredient> { pastaIngredient, broccoliIngredient, zucchiniIngredient, spinachIngredient },
        new List<Food.DietaryPreferencesType> { Food.DietaryPreferencesType.Vegan }, promo3);
    var restaurant1food4 = new Food(restaurant1, "Spaghetti Aglio e Olio", 10.99,
        "Pasta with garlic, olive oil, red pepper flakes", Food.FoodType.Pasta,
        new List<Ingredient> { pastaIngredient, garlicIngredient, redPepperFlakesIngredient },
        new List<Food.DietaryPreferencesType>
            { Food.DietaryPreferencesType.LactoseFree, Food.DietaryPreferencesType.GlutenFree }, promo2);
    var restaurant1food5 = new Food(restaurant1, "Linguine Shrimp Scampi", 16.99,
        "Linguine with shrimp in garlic butter", Food.FoodType.Pasta,
        new List<Ingredient> { pastaIngredient, shrimpIngredient, garlicIngredient, oliveOilIngredient });
    // ======================================================== Create Beverages for Restaurant1
    var restaurant1beverage1 = new Beverage(restaurant1, "Cappuccino", 3.99, "Classic Italian coffee",
        Beverage.BeverageType.Cafeteria, false, null, null);
    var restaurant1beverage2 = new Beverage(restaurant1, "Mojito", 7.99, "Refreshing cocktail with mint and lime",
        Beverage.BeverageType.Cocktails, true, null, null);
    var restaurant1beverage3 = new Beverage(restaurant1, "Iced Tea", 2.99, "Cold brewed tea with lemon",
        Beverage.BeverageType.Cafeteria, false, null, null);
    var restaurant1beverage4 = new Beverage(restaurant1, "Negroni", 8.99, "Classic Italian cocktail",
        Beverage.BeverageType.Cocktails, true,
        new List<Ingredient> { ginIngredient, vermouthIngredient, campariIngredient }, promo3);
    var restaurant1beverage5 = new Beverage(restaurant1, "Lemonade", 2.99, "Refreshing lemon juice and sugar",
        Beverage.BeverageType.Drinks, false, null, null);
    // ======================================================== Create SetOfMenuItems for Restaurant1
    var restaurant1businessLunch1 = new SetOfMenuItem(restaurant1, "Business Special", 19.99,
        "A combination of three foods and a drink",
        new List<Food> { restaurant1food1, restaurant1food2, restaurant1food3 },
        new List<Beverage> { restaurant1beverage2 });
    var restaurant1businessLunch2 = new SetOfMenuItem(restaurant1, "WeekEnd Promo Special", 19.99,
        "A combination of two food beyound your imagination and a drink",
        new List<Food> { restaurant1food4, restaurant1food5 },
        new List<Beverage> { restaurant1beverage2, restaurant1beverage3 });
    var restaurant1businessLunch3 = new SetOfMenuItem(restaurant1, "New special prom", 19.99,
        "WIthour beverages and in monday and friday", new List<Food> { restaurant1food1, restaurant1food5 }, null,
        new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday });
    var restaurant1businessLunch4 = new SetOfMenuItem(restaurant1, "New special prom", 19.99,
        "WIthour beverages and in monday and friday", new List<Food> { restaurant1food5 }, null,
        new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday }, new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0));
    // ================================================ TableOrder
    var restaurant1tableOrder1 = new TableOrder(restaurant1table1, 8, new Dictionary<MenuItem, int> { { restaurant1food2, 6 }, { restaurant1food1, 2 }, {restaurant1businessLunch1, 2}  }, client1);
    // restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food2);
    // restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food2);
    // restaurant1food2.AddToOrder(restaurant1tableOrder1);
    // restaurant1food2.AddToOrder(restaurant1tableOrder1);
    // restaurant1food1.AddToOrder(restaurant1tableOrder1);
    // restaurant1food1.AddToOrder(restaurant1tableOrder1);
    // restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food1);
    // restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food1);
    // restaurant1tableOrder1.AddMenuItemToOrder(restaurant1businessLunch1, 2);
    var restaurant1tableOrder2 = new TableOrder(restaurant1table2, 4, new Dictionary<MenuItem, int> { { restaurant1beverage1, 1 } });
    // restaurant1tableOrder2.AddMenuItemToOrder(restaurant1beverage1);
    var restaurant1tableOrder3 = new TableOrder(restaurant1table2, 4, new Dictionary<MenuItem, int> { { restaurant1food5, 1 } });
    // restaurant1tableOrder3.AddMenuItemToOrder(restaurant1food5);
    // =============================================== Online order
    var restaurant1onlineOrder1 = new OnlineOrder(restaurant1, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), "tes", new Dictionary<MenuItem, int> { { restaurant1food3, 2 }, {restaurant1beverage3, 2} },  client2);
    // restaurant1onlineOrder1.AddMenuItemToOrder(restaurant1food3);
    // restaurant1onlineOrder1.AddMenuItemToOrder(restaurant1food3);
    // restaurant1onlineOrder1.AddMenuItemToOrder(restaurant1beverage3, 2);
    var restaurant1onlineOrder2 = new OnlineOrder(restaurant1, 2, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, new Dictionary<MenuItem, int> { { restaurant1food1, 1 } }, null,  new NonRegisteredClient("Max", "434 345 345"));
    // restaurant1onlineOrder2.AddMenuItemToOrder(restaurant1food1);
    var restaurant1onlineOrder3 = new OnlineOrder(restaurant1, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, new Dictionary<MenuItem, int> { { restaurant1food5, 2 } }, client1);
    // restaurant1onlineOrder3.AddMenuItemToOrder(restaurant1food5, 2);
    var restaurant1onlineOrder4 = new OnlineOrder(restaurant1, 1, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, new Dictionary<MenuItem, int> { { restaurant1food5, 2 } }, null,  new NonRegisteredClient("Alexa", "434 345 345"));
    // restaurant1onlineOrder4.AddMenuItemToOrder(restaurant1beverage1, 5);
    // ==========================================================================================================================================================================================================

    // ========================================================= Create Restaurant2 =========================================================================================
    Restaurant restaurant2 = new Restaurant("Miscusi on river", new Address("Zlote 44", "Krakow", "44"), workHours);
    // restaurant2.AddTable(4);
    var restaurant2table1 = new Table(restaurant2, 8);
    var restaurant2table2 = new Table(restaurant2, 8, "ali");
    var restaurant2table3 = new Table(restaurant2, 10, "window", "descrip");
    var restaurant2table4 = new Table(restaurant2, 4);
    var restaurant2table5 = new Table(restaurant2, 2);
    var restaurant2table6 = new Table(restaurant2, 6);
    // ======================================================== Create FOod for Restaurant1
    var restaurant2food1 = new Food(restaurant2, "Spaghetti Carbonara", 12.99, "Classic pasta with bacon and eggs",
        Food.FoodType.Pasta, new List<Ingredient> { pastaIngredient, baconIngredient, eggIngredient }, null, promo1);
    var restaurant2food2 = new Food(restaurant2, "Penne Alfredo", 14.99, "Creamy Alfredo pasta with Parmesan",
        Food.FoodType.Pasta, new List<Ingredient> { pastaIngredient, creamIngredient, parmesanIngredient },
        new List<Food.DietaryPreferencesType> { Food.DietaryPreferencesType.GlutenFree }, promo2);
    // ======================================================== Create Beverages for Restaurant1
    var restaurant2beverage1 = new Beverage(restaurant2, "Cappuccino", 3.99, "Classic Italian coffee",
        Beverage.BeverageType.Cafeteria, false, null, null);
    var restaurant2beverage2 = new Beverage(restaurant2, "Mojito", 7.99, "Refreshing cocktail with mint and lime",
        Beverage.BeverageType.Cocktails, true, null, null);
    // ======================================================== Create SetOfMenuItems for Restaurant1
    var restaurant2businessLunch1 = new SetOfMenuItem(restaurant2, "Business Special", 19.99,
        "A combination of three foods and a drink", new List<Food> { restaurant2food1, restaurant2food2 },
        new List<Beverage> { restaurant2beverage2 });
    var restaurant2businessLunch2 = new SetOfMenuItem(restaurant2, "WeekEnd Promo Special", 19.99,
        "A combination of two food beyound your imagination and a drink", new List<Food> { restaurant2food1 },
        new List<Beverage> { restaurant2beverage1 });
    // ================================================ TableOrder
    var restaurant2tableOrder1 = new TableOrder(restaurant2table1, 8, new Dictionary<MenuItem, int> { { restaurant2food1, 2 }, { restaurant2food2, 2 } , {restaurant2businessLunch1, 1}}, client1);
    var restaurant2tableOrder2 = new TableOrder(restaurant2table2, 8, new Dictionary<MenuItem, int> { { restaurant2businessLunch1, 2 } });
    var restaurant2tableOrder3 = new TableOrder(restaurant2table3, 10, new Dictionary<MenuItem, int> { { restaurant2food2, 1 } }, client2);
    restaurant2tableOrder1.FinalizeOrder(10);
    // =============================================== Online order
    var restaurant2onlineOrder1 = new OnlineOrder(restaurant2, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), "hell", new Dictionary<MenuItem, int> { { restaurant2beverage2, 2 } }, client2);
    var restaurant2onlineOrder2 = new OnlineOrder(restaurant2, 2, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, new Dictionary<MenuItem, int> { { restaurant2food1, 1 } }, null, new NonRegisteredClient("Max", "434 345 345"));
    restaurant2onlineOrder2.MarkAsGuestsArrived();
    var restaurant2onlineOrder3 = new OnlineOrder(restaurant2, 5, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(1, 0, 0), null, new Dictionary<MenuItem, int> { { restaurant2food1, 1 }, { restaurant2beverage1, 2} }, client3);
    restaurant2onlineOrder3.MarkAsGuestsArrived();
    restaurant2onlineOrder3.FinalizeOrder(10);
    // ==========================================================================================================================================================================================================
    restaurant2tableOrder2.SendOrderToTheKitchen();
    restaurant2onlineOrder2.SendOrderToTheKitchen();
    restaurant2tableOrder1.PrintBill();
    restaurant2onlineOrder3.PrintBill();
    client1.GenerateInvitationLink();
}
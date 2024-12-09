using System.Globalization;
using DigitalOrdering;
using Newtonsoft.Json;

CreateObjects();
LoadClassExtent();
OutputAllObjectsCreated();
// SaveClassExtent();











return;


void LoadClassExtent()
{
    Ingredient.LoadIngredientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Ingredients.json"));
    Food.LoadFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));
    Beverage.LoadBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));
    SetOfMenuItem.LoadSetOfMenuItemsJson(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "SetOfMenuItem.json"));
    Restaurant.LoadRestaurantJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Restaurant.json"));
    Table.LoadTableJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Table.json"));
    RegisteredClient.LoadRegisteredClientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "RegisteredClient.json"));
    TableOrder.LoadTableOrderJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "TableOrder.json"));
    OnlineOrder.LoadOnlineOrderJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "OnlineOrder.json"));
}

void SaveClassExtent()
{
    Ingredient.SaveIngredientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Ingredients.json"));
    Food.SaveFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));
    Beverage.SaveBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));
    SetOfMenuItem.SaveSetOfMenuItemsJson(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "SetOfMenuItem.json"));
    Restaurant.SaveRestaurantJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Restaurant.json"));
    Table.SaveTableJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Table.json"));
    RegisteredClient.SaveRegisteredClientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "RegisteredClient.json"));
    TableOrder.SaveTableOrderJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "TableOrder.json"));
    OnlineOrder.SaveOnlineOrderJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "OnlineOrder.json"));
}

void OutputAllObjectsCreated()
{
    //---------------------------------------------------- Load Ingredietns
    Console.WriteLine("================================  Ingredients ==================");
    foreach (Ingredient ingredient in Ingredient.GetIngredients())
    {
        Console.WriteLine($"Ingredietn id: {ingredient.Id}, Name: {ingredient.Name}");
    }
    
    // ------------------------------------------------------ Load Food
    Console.WriteLine("================================  Food ==================");
    foreach (Food food in Food.GetFoods())
    {
        Console.WriteLine();
        Console.Write(
            $"food id: {food.Id}, Name: {food.Name}, Price: {food.Price}, Description: {food.Description}, foodType: {food.FoodT}, DietaryPreference: ");
        if (food.DietaryPreferences.Count > 0)
        {
            Console.Write("[");
            foreach (var dietaryPreference in food.DietaryPreferences)
                Console.Write($"{dietaryPreference}, ");
            Console.WriteLine("]");
        }
        else
        {
            Console.Write("No dietary preferences");
        }
        Console.WriteLine();
        
        Console.WriteLine(food.Promotion == null
            ? "                    No promotion"
            : $"                    Promotion [ name: {food.Promotion.Name}. And {food.Promotion}]");
        Console.Write(food.Ingredients.Count == 0
            ? "                    No ingredients"
            : "                    There are ingredients: [");
        if (food.Ingredients.Count > 0)
            foreach (var ingredient in food.Ingredients)
                Console.Write($"{ingredient.Name}, ");
            Console.Write("] ");
        Console.WriteLine("");
    }

    // ------------------------------------------------------ Load beverage
    Console.WriteLine("================================ Beverages ==================");
    foreach (Beverage beverage in Beverage.GetBeverages())
    {
        Console.WriteLine(
            $"beverage id: {beverage.Id}, Name: {beverage.Name}, Price: {beverage.Price}, Description: {beverage.Description}, BeverageType: {beverage.BeverageT}");
        Console.WriteLine(beverage.Promotion == null
            ? "                    No promotion"
            : $"                    Promotion [ name: {beverage.Promotion.Name}. And {beverage.Promotion}]");
        Console.Write(beverage.Ingredients.Count == 0
            ? "                    No ingredients"
            : "                    There are ingredients: [");
        if (beverage.Ingredients.Count > 0)
        {
            foreach (var ingredient in beverage.Ingredients)
            {
                Console.Write($"{ingredient.Name}, ");
            }
            Console.Write("] ");
        }
        Console.WriteLine();
    }

    // ------------------------------------------------------ SetOfMenuItems
    Console.WriteLine("================================  SetOfMenuItems ==================");
    foreach (SetOfMenuItem setOfMenuItems in SetOfMenuItem.GetSetOfMenuItems())
    {
        Console.WriteLine(
            $"SetOfMenuItems id: {setOfMenuItems.Id}, Name: {setOfMenuItems.Name}, Price: {setOfMenuItems.Price}, Description: {setOfMenuItems.Description}");
        
        Console.WriteLine($"                    There are {setOfMenuItems.Foods.Count} foods: ");
        if (setOfMenuItems.Foods.Count > 0)
        {
            foreach (var food in setOfMenuItems.Foods)
            {
                Console.Write("                           [");
                Console.Write($"food name: {food.Name}");
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
        
        Console.WriteLine($"                    There are {setOfMenuItems.Beverages.Count} beverages: ");
        if (setOfMenuItems.Beverages.Count > 0)
        {
            foreach (var beverage in setOfMenuItems.Beverages)
            {
                Console.Write("                           [");
                Console.Write($"Beverage name: {beverage.Name}");
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
        
        Console.Write($"                    Is available on: [");
        foreach (var day in setOfMenuItems.Days) Console.Write($"{day}, ");
        Console.Write($"] from {setOfMenuItems.StartTime} to {setOfMenuItems.EndTime}");
        Console.WriteLine();
    }

    // ========================================== Load Restaurant
    Console.WriteLine("=========================================== Restaurant ===========================");
    foreach (var restaurant in Restaurant.GetRestaurants())
    {
        Console.WriteLine($"Restaurant name is: {restaurant.Name}, location is: {restaurant.Location.ToString()},  ");
        
        Console.WriteLine("Open hours are: ");
        foreach (var openHour in restaurant.OpenHours)
        {
            // Console.WriteLine($" {openHour.Day}: {(openHour.IsOpen ? ($"{openHour.OpenTime} to {openHour.CloseTime}") : "closed")}");
            Console.WriteLine($" {openHour.ToString()}");
        }

        Console.WriteLine($"is restaurant opened in {new TimeSpan(8, 0, 0)} on {DayOfWeek.Monday}?: {restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(8, 0, 0))}");
        restaurant.UpdateOpenHour(DayOfWeek.Monday, new TimeSpan(7, 0, 0), new TimeSpan(18, 0, 0));
        Console.WriteLine($"is restaurant opened in {new TimeSpan(8, 0, 0)} on {DayOfWeek.Monday}?: {restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(8, 0, 0))}");
        restaurant.UpdateOpenHour(DayOfWeek.Monday, null, null);
        Console.WriteLine($"is restaurant opened in {new TimeSpan(8, 0, 0)} on {DayOfWeek.Monday}?: {restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(8, 0, 0))}");
    }

    // ========================================== Load Tables
    Console.WriteLine("================================ Tables ===========================");
    foreach (var table in Table.GetTables())
    {
        Console.WriteLine(
            $"Table ID: {table.Id}, Alias: {table.Alias}, Capacity: {table.Capacity}, IsLocked: {table.IsLocked}");
    }

    // ========================================== Load Registered Clients
    Console.WriteLine("================================ Registered Clients ===========================");
    foreach (var client in RegisteredClient.GetRegisteredClients())
    {
        Console.WriteLine(
            $"Client ID: {client.Id}, Name: {client.Name}, Surname: {client.Surname}, Email: {client.Email}, Phone Number: {client.PhoneNumber}, Bonus: {client.Bonus}");
    }

    // ========================================== Load Table Orders
    Console.WriteLine("================================ Table Orders ===========================");
    foreach (var order in TableOrder.GetTableOrders())
    {
        Console.WriteLine(
            $"Table Order: Number of People: {order.NumberOfPeople}, Start Time: {order.StartTime}, QR Code Scan Time: {order.QRCodeScanTime}");
    }

    // ========================================== Load Online Orders
    Console.WriteLine("================================ Online Orders ===========================");
    foreach (var order in OnlineOrder.GetOnlineOrders())
    {
        Console.WriteLine(
            $"Online Order: Number of People: {order.NumberOfPeople}, Date and Time: {order.DateAndTime}, Description: {order.Description}, Guests Arrived: {order.HaveGuestsArrived}");
    }
}

void CreateObjects()
{
//===================================================================== Create Ingredietn 
    var tomatoIngredient = new Ingredient("Tomato");
    Ingredient.AddIngredient(tomatoIngredient);

    var mozzarellaIngredient = new Ingredient("Mozzarella");
    Ingredient.AddIngredient(mozzarellaIngredient);

    var basilIngredient = new Ingredient("Basil");
    Ingredient.AddIngredient(basilIngredient);

    var oliveOilIngredient = new Ingredient("Olive Oil");
    Ingredient.AddIngredient(oliveOilIngredient);

    var garlicIngredient = new Ingredient("Garlic");
    Ingredient.AddIngredient(garlicIngredient);

    var saltIngredient = new Ingredient("Salt");
    Ingredient.AddIngredient(saltIngredient);

    var pepperIngredient = new Ingredient("Pepper");
    Ingredient.AddIngredient(pepperIngredient);

    var chickenIngredient = new Ingredient("Chicken");
    Ingredient.AddIngredient(chickenIngredient);

    var mushroomIngredient = new Ingredient("Mushrooms");
    Ingredient.AddIngredient(mushroomIngredient);

    var parmesanIngredient = new Ingredient("Parmesan");
    Ingredient.AddIngredient(parmesanIngredient);

    var pastaIngredient = new Ingredient("Pasta");
    Ingredient.AddIngredient(pastaIngredient);

    var creamIngredient = new Ingredient("Cream");
    Ingredient.AddIngredient(creamIngredient);

    var spinachIngredient = new Ingredient("Spinach");
    Ingredient.AddIngredient(spinachIngredient);

    var broccoliIngredient = new Ingredient("Broccoli");
    Ingredient.AddIngredient(broccoliIngredient);

    var zucchiniIngredient = new Ingredient("Zucchini");
    Ingredient.AddIngredient(zucchiniIngredient);

    var shrimpIngredient = new Ingredient("Shrimp");
    Ingredient.AddIngredient(shrimpIngredient);

    var baconIngredient = new Ingredient("Bacon");
    Ingredient.AddIngredient(baconIngredient);

    var eggIngredient = new Ingredient("Egg");
    Ingredient.AddIngredient(eggIngredient);

    var redPepperFlakesIngredient = new Ingredient("Red Pepper Flakes");
    Ingredient.AddIngredient(redPepperFlakesIngredient);

    var onionsIngredient = new Ingredient("Onions");
    Ingredient.AddIngredient(onionsIngredient);

    var ginIngredient = new Ingredient("Gin");
    Ingredient.AddIngredient(ginIngredient);

    var vermouthIngredient = new Ingredient("Vermouth");
    Ingredient.AddIngredient(vermouthIngredient);

    var campariIngredient = new Ingredient("Campari");
    Ingredient.AddIngredient(campariIngredient);

    Ingredient.SaveIngredientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Ingredients.json"));

    // ======================================================== Create FOod

    var promo1 = new Promotion(10.0, "new year", "it is purpose delete this in february");
    var promo2 = new Promotion(10.0, "He[ppy", "nothi", Promotion.PromotionType.Regular);
    var promo3 = new Promotion(70.0, "buy season");
    
    var food1 = new Food("Spaghetti Carbonara", 12.99, "Classic pasta with bacon and eggs", Food.FoodType.Pasta,
        new List<Ingredient> { pastaIngredient, baconIngredient, eggIngredient }, null, promo1);
    Food.AddFood(food1);

    var food2 = new Food("Penne Alfredo", 14.99, "Creamy Alfredo pasta with Parmesan", Food.FoodType.Pasta,
        new List<Ingredient> { pastaIngredient, creamIngredient, parmesanIngredient }, new List<Food.DietaryPreferencesType>{Food.DietaryPreferencesType.GlutenFree}, promo2);
    Food.AddFood(food2);

    var food3 = new Food("Fettuccine Primavera", 13.99, "Pasta with fresh vegetables and olive oil",
        Food.FoodType.Pasta, new List<Ingredient> { pastaIngredient, broccoliIngredient, zucchiniIngredient, spinachIngredient }, new List<Food.DietaryPreferencesType>{Food.DietaryPreferencesType.Vegan}, promo3);
    Food.AddFood(food3);

    var food4 = new Food("Spaghetti Aglio e Olio", 10.99, "Pasta with garlic, olive oil, red pepper flakes",
        Food.FoodType.Pasta, new List<Ingredient> { pastaIngredient, garlicIngredient, redPepperFlakesIngredient }, new List<Food.DietaryPreferencesType>{Food.DietaryPreferencesType.LactoseFree, Food.DietaryPreferencesType.GlutenFree}, promo2);
    Food.AddFood(food4);

    var food5 = new Food("Linguine Shrimp Scampi", 16.99, "Linguine with shrimp in garlic butter",
        Food.FoodType.Pasta, new List<Ingredient> { pastaIngredient, shrimpIngredient, garlicIngredient, oliveOilIngredient });
    Food.AddFood(food5);

    Food.SaveFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));

// ======================================================== Create Beverages
    var beverage1 = new Beverage("Cappuccino", 3.99, "Classic Italian coffee",
        Beverage.BeverageType.Cafeteria, false, null, null);
    Beverage.AddBeverage(beverage1);

    var beverage2 = new Beverage("Mojito", 7.99, "Refreshing cocktail with mint and lime",
        Beverage.BeverageType.Cocktails, true, null, null);
    Beverage.AddBeverage(beverage2);

    var beverage3 = new Beverage("Iced Tea", 2.99, "Cold brewed tea with lemon",
        Beverage.BeverageType.Cafeteria, false, null, null);
    Beverage.AddBeverage(beverage3);

    var beverage4 = new Beverage("Negroni", 8.99, "Classic Italian cocktail",
        Beverage.BeverageType.Cocktails, true, new List<Ingredient> { ginIngredient, vermouthIngredient, campariIngredient }, promo3);
    Beverage.AddBeverage(beverage4);

    var beverage5 = new Beverage("Lemonade", 2.99, "Refreshing lemon juice and sugar",
        Beverage.BeverageType.Drinks, false, null, null);
    Beverage.AddBeverage(beverage5);

    Beverage.SaveBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));

// ======================================================== Create Business Lunch
    var businessLunch1 = new SetOfMenuItem("Business Special", 19.99,
        "A combination of three foods and a drink", new List<Food> { food1, food2, food3 },
        new List<Beverage> { beverage2 });
    SetOfMenuItem.AddSetOfMenuItems(businessLunch1);
    
    var businessLunch2 = new SetOfMenuItem("WeekEnd Promo Special", 19.99,
        "A combination of two food beyound your imagination and a drink", new List<Food> { food4, food5 },
        new List<Beverage> { beverage2, beverage3 });
    SetOfMenuItem.AddSetOfMenuItems(businessLunch2);
    
    var businessLunch3 = new SetOfMenuItem("New special prom", 19.99,
        "WIthour beverages and in monday and friday", new List<Food> { food1, food5 }, null, new List<DayOfWeek>{DayOfWeek.Monday, DayOfWeek.Friday});
    SetOfMenuItem.AddSetOfMenuItems(businessLunch3);
    
    var businessLunch4 = new SetOfMenuItem("New special prom", 19.99,
        "WIthour beverages and in monday and friday", new List<Food> {food5 }, null, new List<DayOfWeek>{DayOfWeek.Monday, DayOfWeek.Friday}, new TimeSpan(10,0,0), new TimeSpan(15,0,0));
    SetOfMenuItem.AddSetOfMenuItems(businessLunch4);

    SetOfMenuItem.SaveSetOfMenuItemsJson(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "SetOfMenuItem.json"));
    
// ========================================================= Create Restaurant

    Address address = new Address("Zlota 43", "Warszawa", "22");
    List<OpenHour> workHours = new List<OpenHour>
    {
        new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0)),
        new OpenHour(DayOfWeek.Sunday)
    };
    Restaurant restaurant = new Restaurant("Miscusi", address, workHours);
    Restaurant.AddRestaurant(restaurant);
    
    Restaurant.SaveRestaurantJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Restaurant.json"));

    // -================================================= Create Table
    Table table1 = new Table(2);
    Table.AddTable(table1);
    Table table2 = new Table(6, "window");
    Table.AddTable(table2);
    Table table3 = new Table(4, "near wc");
    Table.AddTable(table3);
    Table table4 = new Table(8, "alies");
    Table.AddTable(table4);
    Table.SaveTableJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Table.json"));

    // ==================================================== Registered client
    RegisteredClient client1 = new RegisteredClient("Max", "32jpjoi3j04#A", "s23454@pjatk.com", "546 545 544");
    RegisteredClient client2 = new RegisteredClient("Alexa", "32jpjD$i3j04#A", null, "344 434 344", "Arstv");
    RegisteredClient client3 = new RegisteredClient("Max", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr");
    RegisteredClient.AddRegisteredClient(client1);
    RegisteredClient.AddRegisteredClient(client2);
    RegisteredClient.AddRegisteredClient(client3);
    RegisteredClient.SaveRegisteredClientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "RegisteredClient.json"));

    // ================================================ TableOrder
    TableOrder tableOrder1 = new TableOrder(2);
    TableOrder tableOrder2 = new TableOrder(6);
    TableOrder.AddTableOrder(tableOrder1);
    TableOrder.AddTableOrder(tableOrder2);
    TableOrder.SaveTableOrderJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "TableOrder.json"));

    // =============================================== ONline order
    OnlineOrder onlineOrder1 = new OnlineOrder(4, DateTime.Now.AddDays(333), "heljfoadsf");
    OnlineOrder onlineOrder2 = new OnlineOrder(4, DateTime.Now.AddYears(1));
    OnlineOrder.AddOnlineOrder(onlineOrder1);
    OnlineOrder.AddOnlineOrder(onlineOrder2);
    OnlineOrder.SaveOnlineOrderJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "OnlineOrder.json"));
}
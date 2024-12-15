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
    SerializationDeserialization.LoadJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Data.json"));
}
void SaveClassExtent()
{
    SerializationDeserialization.SaveJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Data.json"));
}
void OutputAllObjectsCreated()
{
    // ================================================  Output Ingredietns
    Console.WriteLine("================================  Ingredients ================================================================    \n");
    foreach (Ingredient ingredient in Ingredient.GetIngredients())
    {
        Console.WriteLine($"Ingredietn id: {ingredient.Id}, Name: {ingredient.Name}");
    }
    // ------------------------------------------------------ Load Food. MenuItem has Ingredients.
    Console.WriteLine("\n================================  Food ================================================================    \n");
    foreach (Food food in Food.GetFoods())
    {
        Console.Write(
            $"food id: {food.Id}, Name: {food.Name}, Price: {food.Price}, Description: {food.Description}, foodType: {food.FoodT}, DietaryPreference: ");
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
        
        Console.WriteLine(food.Promotion == null
            ? "                    No promotion"
            : $"                    Promotion: [ name: {food.Promotion.Name}. And {food.Promotion}]");
        Console.Write(food.Ingredients.Count == 0
            ? "                    No ingredients"
            : "                    There are ingredients: [");
        if (food.Ingredients.Count > 0)
            foreach (var ingredient in food.Ingredients)
                Console.Write($"{ingredient.Name}, ");
            Console.Write("] \n");
    }
    // ------------------------------------------------------ Load beverage. MenuItem has Ingredients.
    Console.WriteLine("\n================================ Beverages ================================================================    \n");
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
    // ================================================ association Ingredients in MenuItems  
    Console.WriteLine("\n=================================== Ingredients in MenuItems  ================================================================    \n");
    foreach (var ingredient in Ingredient.GetIngredients())
    {
        Console.Write($"Ingredient id: {ingredient.Id}, Ingredient name {ingredient.Name}, how many times ingredient is in other menu items: {ingredient.IngredientInMenuItems.Count} [");
        foreach (var menuItem in ingredient.IngredientInMenuItems)
        {
            Console.Write($"[Menu item id: {menuItem.Id} name:{menuItem.Name}], ");
        }
        Console.Write($"] \n");
    }
    // ------------------------------------------------------ SetOfMenuItems has MenuItems
    Console.WriteLine("\n================================  SetOfMenuItems ================================================================    \n");
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
        Console.Write($"                    Is available on:\n");
        Console.Write("                           [");
        foreach (var day in setOfMenuItems.Days) Console.Write($"{day}, ");
        Console.Write($"] from {setOfMenuItems.StartTime} to {setOfMenuItems.EndTime}");
        Console.WriteLine();
    }
    // ================================================ association MenuItem in SetOfMenuItems
    Console.WriteLine("\n=================================== Food and Beverage in SetOfMenuItems  ================================================================    \n");
    foreach (var food in Food.GetFoods())
    {
        Console.Write($"Food id: {food.Id}, name: {food.Name} is in {food.FoodInSetOfMenuItems.Count} SetOfMenuItems: [");
        foreach (var setOfMenuItem in food.FoodInSetOfMenuItems)
        {
            Console.Write($"[SetOfMenuItems id: {setOfMenuItem.Id}, name: {setOfMenuItem.Name}], ");
        }
        Console.Write($"] \n");
    }
    foreach (var beverage in Beverage.GetBeverages())
    {
        Console.Write($"Food id: {beverage.Id}, name: {beverage.Name} is in {beverage.BeverageInSetOfMenuItems.Count} SetOfMenuItems: [");
        foreach (var setOfMenuItem in beverage.BeverageInSetOfMenuItems)
        {
            Console.Write($"[SetOfMenuItems id: {setOfMenuItem.Id}, name: {setOfMenuItem.Name}], ");
        }
        Console.Write($"] \n");
    }
    // ========================================== Load Restaurant. Restraung has tables
    Console.WriteLine("\n=========================================== Restaurant ================================================================    \n");
    foreach (var restaurant in Restaurant.GetRestaurants())
    {
        Console.WriteLine($"Restaurant name is: {restaurant.Name}, location is: {restaurant.Location.ToString()},  ");
        
        Console.WriteLine("                    Open hours are:");
        foreach (var openHour in restaurant.OpenHours)
        {
            // Console.WriteLine($" {openHour.Day}: {(openHour.IsOpen ? ($"{openHour.OpenTime} to {openHour.CloseTime}") : "closed")}");
            Console.WriteLine($"                        [{openHour.ToString()}]");
        }

        Console.WriteLine("                    Tables are:");
        foreach (var table in restaurant.Tables)
        {
            Console.WriteLine($"                        [Table ID: {table.Id}, Capacity: {table.Capacity}, Alias: {table.Alias ?? "no alias"}, Description: {table.Description ?? "no description"}, IsLocked: {table.IsLocked}, belong to: {table.Restaurant.Name} restauratn]");
        }
        // Console.WriteLine($"is restaurant opened in {new TimeSpan(8, 0, 0)} on {DayOfWeek.Monday}?: {restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(8, 0, 0))}");
        // restaurant.UpdateOpenHour(DayOfWeek.Monday, new TimeSpan(7, 0, 0), new TimeSpan(18, 0, 0));
        // Console.WriteLine($"is restaurant opened in {new TimeSpan(8, 0, 0)} on {DayOfWeek.Monday}?: {restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(8, 0, 0))}");
        // restaurant.UpdateOpenHour(DayOfWeek.Monday, null, null);
        // Console.WriteLine($"is restaurant opened in {new TimeSpan(8, 0, 0)} on {DayOfWeek.Monday}?: {restaurant.IsRestaurantOpen(DayOfWeek.Monday, new TimeSpan(8, 0, 0))}");
    }
    // ============================================= association Table and Restaurant. Table in Restaurants
    Console.WriteLine("\n=================================== Tables are in  Restaurants  ================================================================    \n");
    foreach (var table in Table.GetTables())
    {
        Console.WriteLine($"Table id: {table.Id} belong to the restaurant: {table.Restaurant.Name}");
    }
    // ========================================== Load Registered Clients
    Console.WriteLine("\n================================ Registered Clients ================================================================    \n");
    foreach (var client in RegisteredClient.GetRegisteredClients())
    {
        Console.WriteLine($"Client ID: {client.Id}, Name: {client.Name}, Surname: {client.Surname}, Email: {client.Email}, Phone Number: {client.PhoneNumber}, Bonus: {client.Bonus}. Current user has {client.OnlineOrders.Count} OnlineOrders which are active reservations");
        foreach (var onlineOrder in client.OnlineOrders)
        {
            Console.WriteLine($"             [ Order ID (Key): {onlineOrder.Key}, Order Details (Value) which is onlineOrder Id: {onlineOrder.Value.Id} ]");
        }
    }
    // ========================================== Load Online Orders. Online order belong to restaurant, online order belong to table, online order has menuItems
    Console.WriteLine("\n================================ Online Orders ================================================================    \n");
    foreach (var onlineOrder in OnlineOrder.GetOnlineOrders())
    {
        Console.WriteLine($"Online order id: {onlineOrder.Id}, belong to restaurant: {onlineOrder.Restaurant.Name}, belong to table: {onlineOrder.Table.Id} Number of People: {onlineOrder.NumberOfPeople}, Date and Time: {onlineOrder.DateAndTime}, Duration: {onlineOrder.Duration}, Description: {onlineOrder.Description}, Guests Arrived: {onlineOrder.HaveGuestsArrived} has the following orders: {onlineOrder.MenuItems.Count}.  Registered client: {onlineOrder.RegisteredClient?.Id}, {onlineOrder.RegisteredClient?.Name}, {onlineOrder.RegisteredClient?.PhoneNumber}. Non registered: {onlineOrder.NonRegisteredClient?.ToString()}. Following order is:");
        foreach (var orderList in onlineOrder.MenuItems)
        {
            Console.WriteLine($"            [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity} ]");
        }
    }
    // ========================================== Load Table Orders. table order belong to restaurant, table order belong to table, table order has menuItems
    Console.WriteLine("\n================================ Table Orders ================================================================    \n");
    foreach (var tableOrder in TableOrder.GetTableOrders())
    {
        Console.WriteLine($"Online order id: {tableOrder.Id}, belong to restaurant: {tableOrder.Table.Restaurant.Name}, belong to table: {tableOrder.Table.Id} Number of People: {tableOrder.NumberOfPeople},  has the following orders: {tableOrder.MenuItems.Count}. Registered client: {tableOrder.RegisteredClient?.Id}, {tableOrder.RegisteredClient?.Name}, {tableOrder.RegisteredClient?.PhoneNumber}. Following order is: ");
        foreach (var orderList in tableOrder.MenuItems)
        {
            Console.WriteLine($"            [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity} ]");
        }
    }
    // ============================================= association Order and MenuItem. Table order has menu items, Online order has  menu items, MenuItem is in Orders
    Console.WriteLine("\n=================================== Order contains MenuItems  ================================================================    \n");
    foreach (var onlineOrder in OnlineOrder.GetOnlineOrders())
    {
        Console.WriteLine($"Online order id: {onlineOrder.Id} has the following orders: {onlineOrder.MenuItems.Count}: ");
        foreach (var orderList in onlineOrder.MenuItems)
        {
            Console.WriteLine($"            [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity} ]");
        }
    }
    foreach (var tableOrder in TableOrder.GetTableOrders())
    {
        Console.WriteLine($"Table order id: {tableOrder.Id} has the following orders: {tableOrder.MenuItems.Count}: ");
        foreach (var orderList in tableOrder.MenuItems)
        {
            Console.WriteLine($"            [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity} ]");
        }
    }
    foreach (var food in Food.GetFoods())
    {
        Console.WriteLine($"Food id: {food.Id}, name: {food.Name} is added tohe following orders {food.Orders.Count}");
        foreach (var orderList in food.Orders)
        {
            Console.WriteLine($" [ order id {orderList.Order.Id} and menuItem id: {orderList.MenuItem.Id} ]");
        }
    }
    foreach (var beverage in Beverage.GetBeverages())
    {
        Console.WriteLine($"Beverage id: {beverage.Id}, name: {beverage.Name} is added to the following orders {beverage.Orders.Count}");
        foreach (var orderList in beverage.Orders)
        {
            Console.WriteLine($" [ order id {orderList.Order.Id} and menuItem id: {orderList.MenuItem.Id} ]");
        }
    }
    foreach (var setOfMenuItem in SetOfMenuItem.GetSetOfMenuItems())
    {
        Console.WriteLine($"SetOfMenuItem id: {setOfMenuItem.Id}, name: {setOfMenuItem.Name} is added to the following orders {setOfMenuItem.Orders.Count}");
        foreach (var orderList in setOfMenuItem.Orders)
        {
            Console.WriteLine($" [ order id {orderList.Order.Id} and menuItem id: {orderList.MenuItem.Id} ]");
        }
    }
    // ============================================= association Restaurant and OnlineOrders. Restaurant has online Orders
    Console.WriteLine("\n=================================== Restaurant has online orders  ================================================================    \n");
    foreach (var restaurant in Restaurant.GetRestaurants())
    {
        Console.WriteLine($"Restaurant {restaurant.Name} has {restaurant.OnlineOrders.Count} Online Orders. They are:");
        foreach (var onlineOrder in restaurant.OnlineOrders)
            Console.WriteLine(
                $"                                 Online Order id: {onlineOrder.Id}, belong to restaurant {onlineOrder.Restaurant.Name}. NumberOfPeople: {onlineOrder.NumberOfPeople},  table id: {onlineOrder.Table.Id}. Time: {onlineOrder.DateAndTime}, duration: {onlineOrder.Duration} ");    
    }
    // ============================================= association Table OnlineOrders. Table has online orders
    Console.WriteLine("\n=================================== Tables has online orders  ================================================================    \n");
    foreach (var table in Table.GetTables())
    {
        Console.WriteLine($"Table is {table.Id}, table belong to restaurant: {table.Restaurant.Name}, has online orders: {table.OnlineOrders.Count}");
        foreach (var onlineOrder in table.OnlineOrders)
        {
            Console.WriteLine($"                     [onlineOrder id: {onlineOrder.Id}, time: {onlineOrder.DateAndTime}, duration: {onlineOrder.Duration} ]");
        }
    }
    // ============================================= association Table TableOrder. Table has Table Orders
    Console.WriteLine("\n=================================== Tables has table orders  ================================================================    \n");
    foreach (var table in Table.GetTables())
    {
        Console.WriteLine($"Table is {table.Id}, table belong to restaurant: {table.Restaurant.Name},  has table orders: {table.TableOrder.Count}");
        foreach (var tableOrder in table.TableOrder)
        {
            Console.WriteLine($"                     [table order  id: {tableOrder.Id}, number of people: {tableOrder.NumberOfPeople} ]");
        }
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
    //========================================================= promotion
    var promo1 = new Promotion(10.0, "new year", "it is purpose delete this in february");
    var promo2 = new Promotion(10.0, "He[ppy", "nothi", Promotion.PromotionType.Regular);
    var promo3 = new Promotion(70.0, "buy season");
    // ======================================================== Create FOod
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
    // ======================================================== Create SetOfMenuItems
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
    // ========================================================= Create Restaurant
    List<OpenHour> workHours = new List<OpenHour>
    {
        new OpenHour(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHour(DayOfWeek.Sunday, new TimeSpan(9, 0,0), new TimeSpan(17, 0, 0))
    };
    Restaurant restaurant1 = new Restaurant("Miscusi", new Address("Zlota 43", "Warszawa", "22"), workHours);
    restaurant1.AddTable(2 );
    restaurant1.AddTable(2, "hello worlds");
    restaurant1.AddTable(4, "window");
    restaurant1.AddTable(4, "near wc", "do not sit here a guts how are allergic to oil");
    
    Restaurant restaurant2 = new Restaurant("Miscusi on river", new Address("Zlote 44", "Krakow", "44"), workHours);
    restaurant2.AddTable(2);
    restaurant2.AddTable(4);
    // ==================================================== Registered client
    RegisteredClient client1 = new RegisteredClient("Max", "32jpjoi3j04#A", "s23454@pjatk.com", "546 545 544");
    RegisteredClient client2 = new RegisteredClient("Alexa", "32jpjD$i3j04#A", null, "344 434 344", "Arstv");
    RegisteredClient client3 = new RegisteredClient("Max", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr");
    RegisteredClient.AddRegisteredClient(client1);
    RegisteredClient.AddRegisteredClient(client2);
    RegisteredClient.AddRegisteredClient(client3);
    // ================================================ TableOrder
    var tableOrder1 = new TableOrder(Table.GetTables()[0], 8, client1);
    tableOrder1.AddMenuItem(food2);
    tableOrder1.AddMenuItem(food2);
    food2.AddToOrder(tableOrder1);
    food2.AddToOrder(tableOrder1);
    food1.AddToOrder(tableOrder1);
    food1.AddToOrder(tableOrder1);
    tableOrder1.AddMenuItem(food1);
    tableOrder1.AddMenuItem(food1);
    tableOrder1.AddMenuItem(food4, 10);
    var tableOrder2 = new TableOrder(Table.GetTables()[1], 4);
    tableOrder2.AddMenuItem(beverage1);
    var tableOrder3 = new TableOrder(Table.GetTables()[1], 4);
    tableOrder3.AddMenuItem(food5);
    // =============================================== Online order
    var onlineOrder1 = new OnlineOrder(restaurant1, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0), new TimeSpan(2, 0, 0), "heljfoadsf", client2);
    onlineOrder1.AddMenuItem(food3);
    onlineOrder1.AddMenuItem(food3);
    onlineOrder1.AddMenuItem(businessLunch1, 2);
    var onlineOrder2 = new OnlineOrder(restaurant1, 2, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0), new TimeSpan(2, 0, 0), null, null, new NonRegisteredClient("Max", "434 345 345"));
    onlineOrder2.AddMenuItem(food1);
    onlineOrder2.AddMenuItem(businessLunch3);
    var onlineOrder3 = new OnlineOrder(restaurant1, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0), new TimeSpan(2, 0, 0), null, client1 );
    onlineOrder3.AddMenuItem(food5,2);
    var onlineOrder4 = new OnlineOrder(restaurant1, 1, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0), new TimeSpan(2, 0, 0), null, null, new NonRegisteredClient("Alexa", "434 345 345"));
    onlineOrder4.AddMenuItem(beverage1,5);
    var onlineOrder5 = new OnlineOrder(restaurant2, 1, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0), new TimeSpan(2, 0, 0), null, client2);
    onlineOrder5.AddMenuItem(food1, 2);
}
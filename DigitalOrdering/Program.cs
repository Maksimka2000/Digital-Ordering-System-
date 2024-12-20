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
        Console.WriteLine($"Client ID: {client.Id}, Name: {client.Name}, Surname: {client.Surname}, Email: {client.Email}, Phone Number: {client.PhoneNumber}, Bonus: {client.Bonus}. Current user has {client.OnlineOrders.Count} OnlineOrders which are active reservations");
        foreach (var onlineOrder in client.OnlineOrders)
        {
            Console.WriteLine($"             [ Order ID (Key): {onlineOrder.Key}, Order Details (Value) which is onlineOrder Id: {onlineOrder.Value.Id} ]");
        }
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
            Console.WriteLine($"            [Table ID: {table.Id}, belong to the restaurant: {table.Restaurant.Name} Capacity: {table.Capacity}, Alias: {table.Alias ?? "no alias"}, Description: {table.Description ?? "no description"}, IsLocked: {table.IsLocked}, belong to: {table.Restaurant.Name} restauratn]");
        }
        // Console.WriteLine("\n=================================== Menu                        ================================================================    \n");
        Console.WriteLine($"        There are {restaurant.Menu.Count} menu items in the restaurant:");
        var groupedMenuItems = restaurant.Menu
            .GroupBy(menuItem => menuItem.GetType())
            .ToDictionary(group => group.Key, group => group.ToList());
        foreach (var group in groupedMenuItems)
        {
            // Console.WriteLine($"\n================================ Menu Type: {group.Key.Name} ================================================================    \n");
            Console.WriteLine($"            {group.Value.Count}/{restaurant.Menu.Count} are {group.Key.Name}: ");
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
        // Console.WriteLine("\n================================ Online Orders ================================================================    \n");
        Console.WriteLine($"        There are {restaurant.OnlineOrders.Count} Online Orders:");
        foreach (var onlineOrder in restaurant.OnlineOrders)
        {
            Console.WriteLine($"            Online order id: {onlineOrder.Id}, belong to table: {onlineOrder.Table.Id}. Number of People: {onlineOrder.NumberOfPeople}, Date and Time: {onlineOrder.DateAndTime}, Duration: {onlineOrder.Duration}, Description: {onlineOrder.Description}, Guests Arrived: {onlineOrder.HaveGuestsArrived}. Registered client: {onlineOrder.RegisteredClient?.Id} {onlineOrder.RegisteredClient?.Name} {onlineOrder.RegisteredClient?.PhoneNumber}. Non registered: {onlineOrder.NonRegisteredClient?.ToString()}. Has the following item: {onlineOrder.MenuItems.Count}:");
            foreach (var orderList in onlineOrder.MenuItems)
            {
                Console.WriteLine($"                [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity}. Price: {orderList.MenuItem.Price}. Discount: {orderList.MenuItem.Promotion?.DiscountPercent} ]");
            }
            Console.WriteLine($"                Summary: Order price is: {onlineOrder.OrderPrice}. Service Price is: {onlineOrder.ServicePrice}. Discount: {onlineOrder.DiscountAmount}. Total Price is: {onlineOrder.TotalPrice}");
        }
        // Console.WriteLine("\n================================ Table Orders ================================================================    \n");
        var tablesWithTableOrders = restaurant.Tables.Where(table => table.TableOrders.Count > 0).ToList();
        Console.WriteLine($"        There are {tablesWithTableOrders.Count()} tables which have orders:");
        foreach (var table in tablesWithTableOrders)
        {
            Console.WriteLine($"            Table id: {table.Id} belong to {table.Restaurant.Name} has {table.TableOrders.Count} orders:");
            foreach (var tableOrder in table.TableOrders)
            {
                Console.WriteLine($"                Table order id: {tableOrder.Id}, belong to restaurant: {tableOrder.Table.Restaurant.Name}, belong to table: {tableOrder.Table.Id}. Number of People: {tableOrder.NumberOfPeople}. Registered client: {tableOrder.RegisteredClient?.Id}, {tableOrder.RegisteredClient?.Name}, {tableOrder.RegisteredClient?.PhoneNumber}.  has the following items: {tableOrder.MenuItems.Count}: ");
                foreach (var orderList in tableOrder.MenuItems)
                {
                    Console.WriteLine($"                    [ Order Id: {orderList.Order.Id}. MenuItem Id: {orderList.MenuItem.Id}, MenuItem Name: {orderList.MenuItem.Name}. Quantity: {orderList.Quantity}. Price: {orderList.MenuItem.Price}. Discount: {orderList.MenuItem.Promotion?.DiscountPercent} ]");
                }
                Console.WriteLine($"                Summary: Order price is: {tableOrder.OrderPrice}. Service Price is: {tableOrder.ServicePrice}. Discount: {tableOrder.DiscountAmount}. Total Price is: {tableOrder.TotalPrice}");
            }
        }
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
    RegisteredClient client3 = new RegisteredClient("Max", "32Apjoi3jf4#A", "s488@gjsp.com", null, "Skr");
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
    var restaurant1tableOrder1 = new TableOrder(restaurant1table1, 8, client1);
    restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food2);
    restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food2);
    restaurant1food2.AddToOrder(restaurant1tableOrder1);
    restaurant1food2.AddToOrder(restaurant1tableOrder1);
    restaurant1food1.AddToOrder(restaurant1tableOrder1);
    restaurant1food1.AddToOrder(restaurant1tableOrder1);
    restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food1);
    restaurant1tableOrder1.AddMenuItemToOrder(restaurant1food1);
    restaurant1tableOrder1.AddMenuItemToOrder(restaurant1businessLunch1, 2);
    var restaurant1tableOrder2 = new TableOrder(restaurant1table2, 4);
    restaurant1tableOrder2.AddMenuItemToOrder(restaurant1beverage1);
    var restaurant1tableOrder3 = new TableOrder(restaurant1table2, 4);
    restaurant1tableOrder3.AddMenuItemToOrder(restaurant1food5);
    // =============================================== Online order
    var restaurant1onlineOrder1 = new OnlineOrder(restaurant1, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), "heljfoadsf", client2);
    restaurant1onlineOrder1.AddMenuItemToOrder(restaurant1food3);
    restaurant1onlineOrder1.AddMenuItemToOrder(restaurant1food3);
    restaurant1onlineOrder1.AddMenuItemToOrder(restaurant1beverage3, 2);
    var restaurant1onlineOrder2 = new OnlineOrder(restaurant1, 2, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, null, new NonRegisteredClient("Max", "434 345 345"));
    restaurant1onlineOrder2.AddMenuItemToOrder(restaurant1food1);
    var restaurant1onlineOrder3 = new OnlineOrder(restaurant1, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, client1);
    restaurant1onlineOrder3.AddMenuItemToOrder(restaurant1food5, 2);
    var restaurant1onlineOrder4 = new OnlineOrder(restaurant1, 1, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, null, new NonRegisteredClient("Alexa", "434 345 345"));
    restaurant1onlineOrder4.AddMenuItemToOrder(restaurant1beverage1, 5);
    // ==========================================================================================================================================================================================================

    // ========================================================= Create Restaurant2 =========================================================================================
    Restaurant restaurant2 = new Restaurant("Miscusi on river", new Address("Zlote 44", "Krakow", "44"), workHours);
    // restaurant2.AddTable(4);
    var restaurant2table1 = new Table(restaurant2, 2);
    var restaurant2table2 = new Table(restaurant2, 4, "ali");
    var restaurant2table3 = new Table(restaurant2, 6, "window", "descrip");
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
    var restaurant2tableOrder1 = new TableOrder(restaurant2table1, 8, client1);
    restaurant2tableOrder1.AddMenuItemToOrder(restaurant2food1, 10);
    var restaurant2tableOrder2 = new TableOrder(restaurant2table2, 4);
    restaurant2tableOrder2.AddMenuItemToOrder(restaurant2businessLunch1, 2);
    // =============================================== Online order
    var restaurant2onlineOrder1 = new OnlineOrder(restaurant2, 4, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), "heljfoadsf", client2);
    restaurant2onlineOrder1.AddMenuItemToOrder(restaurant2beverage2, 2);
    var restaurant2onlineOrder2 = new OnlineOrder(restaurant2, 2, DateTime.Now.AddDays(3).Date + new TimeSpan(15, 0, 0),
        new TimeSpan(2, 0, 0), null, null, new NonRegisteredClient("Max", "434 345 345"));
    restaurant2onlineOrder2.AddMenuItemToOrder(restaurant2food1, 1);
    // ==========================================================================================================================================================================================================
}
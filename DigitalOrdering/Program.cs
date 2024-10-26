using DigitalOrdering;
using Newtonsoft.Json;

// CreateObjects();
LoadClassExtent();
OutputAllObjectsCreated();
// SaveClassExtent();


Restaurant restaurant = Restaurant.GetRestaurants().FirstOrDefault(n => n.Name == "Miscusi");
Console.WriteLine(restaurant.IsRestaurantOpen(DayOfWeek.Friday, new TimeSpan(9, 0, 0)));



void LoadClassExtent()
{
    Promotion.LoadPromotionJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Promotions.json"));
    Ingredient.LoadIngredientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Ingredients.json"));
    Food.LoadFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));
    Beverage.LoadBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));
    BusinessLunch.LoadBusinessLunchJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "BusinessLunches.json"));
    Restaurant.LoadRestaurantJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Restaurant.json"));
}

void SaveClassExtent()
{
    Promotion.SavePromotionJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Promotions.json"));
    Ingredient.SaveIngredientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Ingredients.json"));
    Food.SaveFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));
    Beverage.SaveBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));
    BusinessLunch.SaveBusinessLunchJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "BusinessLunches.json"));
    Restaurant.SaveRestaurantJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "Restaurant.json"));
}

void OutputAllObjectsCreated()
{
// ------------------------------------------------- Load promotion 
    Console.WriteLine("================================  Promotion ==================");
    var promotions = Promotion.GetPromotions();
    foreach (var item in promotions)
    {
        Console.WriteLine($"Promotion id: {item.Id}, Name: {item.Name}, Description: {item.Description}");
    }

// //---------------------------------------------------- Load Ingredietns
    Console.WriteLine("================================  Ingredients ==================");
    foreach (Ingredient ingredient in Ingredient.GetIngredients())
    {
        Console.WriteLine($"Ingredietn id: {ingredient.Id}, Name: {ingredient.Name}");
    }

// // // ------------------------------------------------------ Load Food
    Console.WriteLine("================================  Food ==================");
    foreach (Food food in Food.GetFoods())
    {
        Console.WriteLine(
            $"food id: {food.Id}, Name: {food.Name}, Price: {food.Price}, Description: {food.Description}, HasChangableIngredients: {food.HasChangableIngredients}, DietaryPrference: {food.DietaryPreference}, foodType: {food.FoodT} ");
        Console.WriteLine(food.Promotion == null
            ? "                    No promotion"
            : $"                   Promotion [Name: {food.Promotion.Name}, Description: {food.Promotion.Description}]");
        Console.Write(food.Ingredients == null
            ? "                    No ingredients"
            : "                    There are ingredients: [");
        if (food.Ingredients != null)
        {
            foreach (var ingredient in food.Ingredients)
            {
                Console.Write($"{ingredient.Name}, ");
            }

            Console.Write("] ");
        }

        Console.WriteLine("");
    }

// // ------------------------------------------------------ Load beverage
    Console.WriteLine("================================ Beverages ==================");
    foreach (Beverage beverage in Beverage.GetBeverages())
    {
        Console.WriteLine(
            $"beverage id: {beverage.Id}, Name: {beverage.Name}, Price: {beverage.Price}, Description: {beverage.Description}, HasChangableIngredients: {beverage.HasChangableIngredients}, BeverageType: {beverage.BeverageT}");
        Console.WriteLine(beverage.Promotion == null
            ? "                    No promotion"
            : $"                    Promotion [ name: {beverage.Promotion.Name}, Description: {beverage.Promotion.Description}]");
        Console.Write(beverage.Ingredients == null
            ? "                    No ingredients"
            : "                    There are ingredients: [");
        if (beverage.Ingredients != null)
        {
            foreach (var ingredient in beverage.Ingredients)
            {
                Console.Write($"{ingredient.Name}, ");
            }

            Console.Write("] ");
        }

        Console.WriteLine();
    }

// // ------------------------------------------------------ Load BusinessLunches
    Console.WriteLine("================================  BusinessLunches ==================");
    foreach (BusinessLunch businessLunch in BusinessLunch.GetBusinessLunches())
    {
        Console.WriteLine(
            $"Business Lunch id: {businessLunch.Id}, Name: {businessLunch.Name}, Price: {businessLunch.Price}, Description: {businessLunch.Description}, HasChangableIngredients: {businessLunch.HasChangableIngredients},");
        Console.WriteLine(businessLunch.Promotion == null
            ? "                    No promotion"
            : $"                    Promotion name: {businessLunch.Promotion.Name}, Description: {businessLunch.Promotion.Description}");
        Console.WriteLine(businessLunch.Ingredients == null
            ? "                    No ingredients"
            : "                    There are ingredients:");

        Console.WriteLine($"                    There are {businessLunch.Foods.Count} foods: ");
        foreach (var food in businessLunch.Foods)
        {
            Console.Write("                           [");
            Console.Write($"food name: {food.Name}");
            Console.Write(
                $". Promotion for this food: {(food.Promotion == null ? "No Promotion" : food.Promotion.Name)}");
            Console.Write(
                $". Ingredients in the food: {(food.Ingredients == null ? "No ingredients" : "There are ingredients: [")}");
            if (food.Ingredients != null)
            {
                foreach (var ingredient in food.Ingredients)
                {
                    Console.Write($"{ingredient.Name}, ");
                }

                Console.Write("] ");
            }

            Console.WriteLine("]");
        }


        Console.WriteLine($"                    There are {businessLunch.Beverages.Count} beverages: ");
        foreach (var beverage in businessLunch.Beverages)
        {
            Console.Write("                           [");
            Console.Write($"Beverage name: {beverage.Name}");
            Console.Write(
                $". Promotion for this beverage: {(beverage.Promotion == null ? "No Promotion" : beverage.Promotion.Name)}");
            Console.Write(
                $". Ingredients in the beverage: {(beverage.Ingredients == null ? "No ingredients" : "There are ingredients: [")}");
            if (beverage.Ingredients != null)
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

    // ========================================== Load Restaurant
    Console.WriteLine("=========================================== Restaurant ===========================");
    foreach (var restaurant in Restaurant.GetRestaurants())
    {
        Console.Write(
            $"Restaurant name is: {restaurant.Name}, location is: {restaurant.Location.Street} {restaurant.Location.City},  ");
        Console.WriteLine("Open hours are: ");
        foreach (var openHour in restaurant.WorkHours)
        {
            Console.WriteLine($" {openHour.Day}: {openHour.OpenTime} to {openHour.CloseTime}");
        }
    }
}

void CreateObjects()
{
    /// ================================DO not delete just in case)
//
// ==================================================================== Creating promotion class.
    var promo1 = new Promotion(30, "new year", "discount for the new year");
    Promotion.AddPromotion(promo1);
    var promo2 = new Promotion(50, "welcome back", "discount for returning clients");
    Promotion.AddPromotion(promo2);
    var promo3 = new Promotion(10, "Wednesday discount", "discount for clients on Wednesdays");
    Promotion.AddPromotion(promo3);

    Promotion.SavePromotionJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Promotions.json"));

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
    var food1 = new Food("Spaghetti Carbonara", 12.99, "Classic pasta with bacon and eggs", true,
        new List<Ingredient> { pastaIngredient, baconIngredient, eggIngredient }, promo2,
        Food.DietaryPreferencesType.NoPreference, Food.FoodType.Pasta);
    Food.AddFood(food1);

    var food2 = new Food("Penne Alfredo", 14.99, "Creamy Alfredo pasta with Parmesan", true,
        new List<Ingredient> { pastaIngredient, creamIngredient, parmesanIngredient }, promo1,
        Food.DietaryPreferencesType.NoPreference, Food.FoodType.Pasta);
    Food.AddFood(food2);

    var food3 = new Food("Fettuccine Primavera", 13.99, "Pasta with fresh vegetables and olive oil", true,
        new List<Ingredient> { pastaIngredient, broccoliIngredient, zucchiniIngredient, spinachIngredient }, promo3,
        Food.DietaryPreferencesType.Vegan, Food.FoodType.Pasta);
    Food.AddFood(food3);

    var food4 = new Food("Spaghetti Aglio e Olio", 10.99, "Pasta with garlic, olive oil, red pepper flakes", true,
        new List<Ingredient> { pastaIngredient, garlicIngredient, redPepperFlakesIngredient }, null,
        Food.DietaryPreferencesType.Vegetarian, Food.FoodType.Pasta);
    Food.AddFood(food4);

    var food5 = new Food("Linguine Shrimp Scampi", 16.99, "Linguine with shrimp in garlic butter", true,
        new List<Ingredient> { pastaIngredient, shrimpIngredient, garlicIngredient, oliveOilIngredient }, promo1,
        Food.DietaryPreferencesType.NoPreference, Food.FoodType.Pasta);
    Food.AddFood(food5);

    Food.SaveFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));

// ======================================================== Create Beverages
    var beverage1 = new Beverage("Cappuccino", 3.99, "Classic Italian coffee", false, null, null, false,
        Beverage.BeverageType.Cafeteria);
    Beverage.AddBeverage(beverage1);

    var beverage2 = new Beverage("Mojito", 7.99, "Refreshing cocktail with mint and lime", false, null, promo1, true,
        Beverage.BeverageType.Cocktails);
    Beverage.AddBeverage(beverage2);

    var beverage3 = new Beverage("Iced Tea", 2.99, "Cold brewed tea with lemon", false, null, promo2, false,
        Beverage.BeverageType.Cafeteria);
    Beverage.AddBeverage(beverage3);

    var beverage4 = new Beverage("Negroni", 8.99, "Classic Italian cocktail", false,
        new List<Ingredient> { ginIngredient, vermouthIngredient, campariIngredient }, promo3, true,
        Beverage.BeverageType.Cocktails);
    Beverage.AddBeverage(beverage4);

    var beverage5 = new Beverage("Lemonade", 2.99, "Refreshing lemon juice and sugar", false, null, null, false,
        Beverage.BeverageType.Drinks);
    Beverage.AddBeverage(beverage5);

    Beverage.SaveBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));

// ======================================================== Create Business Lunch
    var businessLunch = new BusinessLunch("Business Special", 19.99,
        "A combination of three foods and a drink",
        true, new List<Food> { food1, food2, food3 }, new List<Beverage> { beverage2 });
    BusinessLunch.AddBusinessLunch(businessLunch);

    BusinessLunch.SaveBusinessLunchJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data",
        "BusinessLunches.json"));


    // ========================================================= Create Restaurant

    Address address = new Address("Zlota 43", "Warszawa");
    List<OpenHours> workHours = new List<OpenHours>
    {
        new OpenHours(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHours(DayOfWeek.Tuesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHours(DayOfWeek.Wednesday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHours(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHours(DayOfWeek.Friday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),
        new OpenHours(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0)),
        new OpenHours(DayOfWeek.Sunday, new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0))
    };
    Restaurant restaurant = new Restaurant("Miscusi", address, workHours);
    Restaurant.AddRestaurant(restaurant);
    Restaurant.SaveRestaurantJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Restaurant.json"));
}
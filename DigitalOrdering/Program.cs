using DigitalOrdering;
using Newtonsoft.Json;


Promotion.LoadPromotionJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Promotions.json"));
Ingredient.LoadIngredientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Ingredients.json"));
Food.LoadFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));
Beverage.LoadBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));
BusinessLunch.LoadBusinessLunchJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "BusinessLunches.json"));




















































//
// // ------------------------------------------------- Load promotion 
// Console.WriteLine("Promotion ==================");
// var promotions = Promotion.GetPromotions();
// foreach (var item in promotions)
// {
//     Console.WriteLine(item.Id);
//     Console.WriteLine(item.Name);
// }
// // //---------------------------------------------------- Load Ingredietns
// Console.WriteLine("Ingredients ==================");
// foreach (Ingredient ingredient in Ingredient.GetIngredients())
// {
//     Console.WriteLine(ingredient.Id);
//     Console.WriteLine(ingredient.Name);
// }
// // // // ------------------------------------------------------ Load Food
// Console.WriteLine("Food ==================");
// foreach (Food food in Food.GetFoods())
// {
//     // Console.WriteLine(food.Id);
//     Console.WriteLine(food.Name);
//     // Console.WriteLine(food.Price);
//     // Console.WriteLine(food.Description);
//     // Console.WriteLine(food.hasChangableIngredients);
//     // Console.WriteLine(food.Ingredients == null ? "null" : "notNull");
//     // if (food.Ingredients != null)
//     // {
//     //     foreach (var ingredient in food.Ingredients)
//     //     {
//     //         Console.WriteLine(ingredient.Name);
//     //     }
//     // }
//     // Console.WriteLine(food.Promotion == null ? "null" : food.Promotion.Name);
//     // Console.WriteLine(food.DietaryPreference);
//     // Console.WriteLine(food.FoodT);
// }
// // // ------------------------------------------------------ Load beverage
// Console.WriteLine("Beverages ==================");
// foreach (Beverage beverage in Beverage.GetBeverages())
// {
//     Console.WriteLine(beverage.Id);
//     Console.WriteLine(beverage.Name);
//     Console.WriteLine(beverage.Price);
//     Console.WriteLine(beverage.Description);
//     Console.WriteLine(beverage.hasChangableIngredients);
//     Console.WriteLine(beverage.Ingredients == null ? "null" : "notNull");
//     if (beverage.Ingredients != null)
//     {
//         foreach (var ingredient in beverage.Ingredients)
//         {
//             Console.WriteLine(ingredient.Name);
//         }
//     }
//     Console.WriteLine(beverage.Promotion == null ? "null" : beverage.Promotion.Name);
//     Console.WriteLine(beverage.BeverageT);
//     
// }
//
// // // ------------------------------------------------------ Load BusinessLunches
// Console.WriteLine("BusinessLunches ==================");
// foreach (BusinessLunch businessLunch in BusinessLunch.GetBusinessLunches())
// {
//     Console.WriteLine(businessLunch.Id);
//     Console.WriteLine(businessLunch.Name);
//     Console.WriteLine(businessLunch.Price);
//     Console.WriteLine(businessLunch.Description);
//     Console.WriteLine(businessLunch.hasChangableIngredients);
//     Console.WriteLine(businessLunch.Ingredients == null ? "null" : "notNull");
//     Console.WriteLine(businessLunch.Promotion == null ? "null" : "notNull");
//     foreach (var food in businessLunch.Foods)
//     {
//         Console.WriteLine(food.Name);
//         Console.WriteLine(food.Promotion.Name);
//         foreach (var ingredient in food.Ingredients)
//         {
//             Console.WriteLine(ingredient.Name);
//         }
//     }
//     foreach (var beverage in businessLunch.Beverages)
//     {
//         Console.WriteLine(beverage.Name);
//         Console.WriteLine(beverage.Promotion.Name);
//     }
//     
// }
//
//
// /// ================================DO not delete just in case)
// //
// // ==================================================================== Creating promotion class.
// Promotion promo1 = new Promotion(30,"new year", "discount for the new year");
// Promotion promo2 = new Promotion(50,"welcome back", "discount for clients ");
// Promotion promo3 = new Promotion(10,"Wednesday discount", "wednesday discount for clients");
// Promotion.SavePromotionJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Promotions.json"));
//
// //===================================================================== Create Ingredietn 
// List<Ingredient> ingredients = new List<Ingredient>
// {
//     new Ingredient("Tomato"),
//     new Ingredient("Mozzarella"),
//     new Ingredient("Basil"),
//     new Ingredient("Olive Oil"),
//     new Ingredient("Garlic"),
//     new Ingredient("Salt"),
//     new Ingredient("Pepper"),
//     new Ingredient("Chicken"),
//     new Ingredient("Mushrooms"),
//     new Ingredient("Parmesan"),
//     new Ingredient("Pasta"),
//     new Ingredient("Cream"),
//     new Ingredient("Spinach"),
//     new Ingredient("Broccoli"),
//     new Ingredient("Zucchini"),
//     new Ingredient("Shrimp"),
//     new Ingredient("Bacon"),
//     new Ingredient("Egg"),
//     new Ingredient("Red Pepper Flakes"),
//     new Ingredient("Onions"),
//     new Ingredient("Gin London Dry Bombay Sapphire "),
//     new Ingredient("Vermouth: Cinzano Bianco 15%"),
//     new Ingredient("Liqueur: Campari")
// };
// Ingredient.SaveIngredientJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Ingredients.json"));
//
//
// //========================================================= Create food
// Food food1 = new Food("Spaghetti Carbonara", 12.99, "Classic pasta dish with bacon and eggs", true, 
//     new List<Ingredient> { ingredients[10], ingredients[16], ingredients[17] }, promo2, 
//     Food.DietaryPreferencesType.NoPreference, Food.FoodType.Pasta);
//
// Food food2 = new Food("Penne Alfredo", 14.99, "Creamy Alfredo pasta with Parmesan cheese", true, 
//     new List<Ingredient> { ingredients[10], ingredients[11], ingredients[9] }, promo1, 
//     Food.DietaryPreferencesType.NoPreference, Food.FoodType.Pasta);
//
// Food food3 = new Food("Fettuccine Primavera", 13.99, "Pasta with fresh vegetables and olive oil", true, 
//     new List<Ingredient> { ingredients[10], ingredients[13], ingredients[14], ingredients[15] }, promo3, 
//     Food.DietaryPreferencesType.Vegan, Food.FoodType.Pasta);
//
// Food food4 = new Food("Spaghetti Aglio e Olio", 10.99, "Pasta with garlic, olive oil, and red pepper flakes", true, 
//     new List<Ingredient> { ingredients[10], ingredients[4], ingredients[12], ingredients[18] }, null, 
//     Food.DietaryPreferencesType.Vegetarian, Food.FoodType.Pasta);
//
// Food food5 = new Food("Linguine Shrimp Scampi", 16.99, "Linguine pasta with shrimp in a garlic butter sauce", true, 
//     new List<Ingredient> { ingredients[10], ingredients[15], ingredients[4], ingredients[3] }, promo1, 
//     Food.DietaryPreferencesType.NoPreference, Food.FoodType.Pasta);
//
// Food food6 = new Food("Olives", 2.99, "Best Italy olives ever", false, 
//     null, null, Food.DietaryPreferencesType.NoPreference, Food.FoodType.Snack);
//     
// Food.SaveFoodJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Foods.json"));
//
//
// // ======================================================== Create Beverage
//
// Beverage beverage1 = new Beverage("Cappuccino", 3.99, "Classic Italian coffee", false, 
//     null, null, false, Beverage.BeverageType.Cafeteria);
//
// Beverage beverage2 = new Beverage("Mojito", 7.99, "Refreshing cocktail with mint and lime", false, 
//     null, promo1, true, Beverage.BeverageType.Cafeteria);
//
// Beverage beverage3 = new Beverage("Iced Tea", 2.99, "Cold brewed tea with lemon", false, 
//     null , promo2, false, Beverage.BeverageType.Cafeteria);
//
// Beverage beverage4 = new Beverage("Negroni", 8.99, "n", false, 
//     new List<Ingredient> { ingredients[20], ingredients[21], ingredients[22] }, promo3, true, Beverage.BeverageType.Cocktails);
//
// Beverage beverage5 = new Beverage("Lemonade", 2.99, "Refreshing lemon juice and sugar", false, 
//     null, null, false, Beverage.BeverageType.Drinks);
// Beverage.SaveBeverageJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "Beverages.json"));
//
// // ================================================ business luch 
// BusinessLunch businessLunch = new BusinessLunch("Business Special", 19.99, "A combination of three foods and a drink", 
//     true, new Food[] { food1, food2, food3 }, new Beverage[] { beverage2 });
// BusinessLunch.SaveBusinessLunchJSON(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Data", "BusinessLunches.json"));
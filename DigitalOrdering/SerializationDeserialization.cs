using Newtonsoft.Json;

using DigitalOrdering;

namespace DidgitalOrdering;

public class SerializationDeserialization
{
    private class ProjectState
    {
        public List<Ingredient> Ingredients { get; set; } = [];
        public List<Food> Foods { get; set; } = [];
        public List<Beverage> Beverages { get; set; } = [];
        public List<SetOfMenuItem> SetOfMenuItems { get; set; } = [];
        public List<Restaurant> Restaurants { get; set; } = [];
        public List<Table> Tables { get; set; } = [];
        public List<RegisteredClient> RegisteredClients { get; set; } = [];
        public List<TableOrder> TableOrders { get; set; } = [];
        public List<OnlineOrder> OnlineOrders { get; set; } = [];
    }
    
    public static void SaveJSON(string path)
    {
        try
        {
            var projectState = new ProjectState
            {
                Ingredients = Ingredient.GetIngredients(),
                Foods = Food.GetFoods(),
                Beverages = Beverage.GetBeverages(),
                SetOfMenuItems = SetOfMenuItem.GetSetOfMenuItems(),
                Restaurants = Restaurant.GetRestaurants(),
                Tables = Table.GetTables(),
                RegisteredClients = RegisteredClient.GetRegisteredClients(),
                TableOrders = TableOrder.GetTableOrders(),
                OnlineOrders = OnlineOrder.GetOnlineOrders()
            };

            
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };

            string json = JsonConvert.SerializeObject(projectState, settings);
            File.WriteAllText(path, json);
            Console.WriteLine("\n======================================================================================================");
            Console.WriteLine($"=======================File saved successfully at {path}=============================================");
            Console.WriteLine("======================================================================================================\n");
            
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error saving file: {e.Message}");
        }
    }
    
    public static void LoadJSON(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    TypeNameHandling = TypeNameHandling.Auto
                };
                string json = File.ReadAllText(path);
                
                var projectState = JsonConvert.DeserializeObject<ProjectState>(json, settings);
                
                foreach (var ingredient in projectState.Ingredients)
                    Ingredient.AddIngredient(ingredient);
                
                foreach (var food in projectState.Foods)
                    Food.AddFood(food);
                
                foreach (var beverage in projectState.Beverages)
                    Beverage.AddBeverage(beverage);
                
                foreach (var setOfMenuItem in projectState.SetOfMenuItems)
                    SetOfMenuItem.AddSetOfMenuItems(setOfMenuItem);
                
                foreach (var restaurant in projectState.Restaurants)
                    Restaurant.AddRestaurant(restaurant);
                
                foreach (var table in projectState.Tables)
                    Table.AddTable(table);
                
                foreach (var registeredClient in projectState.RegisteredClients)
                    RegisteredClient.AddRegisteredClient(registeredClient);

                foreach (var tableOrder in projectState.TableOrders)
                    TableOrder.AddTableOrder(tableOrder);

                foreach (var onlineOrder in projectState.OnlineOrders)
                    OnlineOrder.AddOnlineOrder(onlineOrder);
                
                Console.WriteLine($"File loaded successfully at {path}");
            }
            else throw new ArgumentException($"Error loading  file: path: {path} doesn't exist ");
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Error loading  file: {e.Message}");
        }
    }

    
}
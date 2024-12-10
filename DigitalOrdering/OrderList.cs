using DigitalOrdering;
using Newtonsoft.Json;

namespace DidgitalOrdering;

[Serializable]
public class OrderList
{
    public int Quantity { get; private set; }

    public MenuItem MenuItem { get; private set; }
    public Order Order { get; private set; }

    public void AddMenuItemToOrderList(MenuItem menuItem)
    {
        if (menuItem == null) throw new ArgumentNullException("MenuItem cannot be null in AddMenuItemToOrderList()");
        MenuItem = menuItem;
        // menuItem.AddOrderList(this);
    }

    public void AddOrderToOrderList(Order order)
    {
        if (order == null) throw new ArgumentNullException("Order cannot be null in AddOrderToOrderList()");
        Order = order;
        // order.AddOrderList(this);
    }
    
    [JsonConstructor]
    public OrderList(MenuItem menuItem, Order order, int quantity = 1)
    {
          AddMenuItemToOrderList(menuItem);
          AddOrderToOrderList(order);
          Quantity = quantity;
    }
    
    public void AddQuantity()
    {
        ++Quantity;
    }
}
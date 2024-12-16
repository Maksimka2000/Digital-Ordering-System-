using DigitalOrdering;
using Newtonsoft.Json;

namespace DidgitalOrdering;


public class OrderList
{
    // class extend 
    private static List<OrderList> _orderLists = [];
    
    public int Quantity { get; private set; }
    public MenuItem MenuItem { get; private set; }
    public Order Order { get; private set; }

    private void AddMenuItemToOrderList(MenuItem menuItem)
    {
        if (menuItem == null) throw new ArgumentNullException("MenuItem cannot be null in AddMenuItemToOrderList()");
        MenuItem = menuItem;
        menuItem.AddOrderList(this);
    }
    private void AddOrderToOrderList(Order order)
    {
        if (order == null) throw new ArgumentNullException("Order cannot be null in AddOrderToOrderList()");
        Order = order;
        order.AddOrderList(this);
    }
    public OrderList(MenuItem menuItem, Order order, int quantity = 1)
    {
          if(quantity <= 0) throw new ArgumentException($"quantity must be greater than zero");
          Quantity = quantity;
          AddMenuItemToOrderList(menuItem);
          AddOrderToOrderList(order);
          _orderLists.Add(this);
    }

    public static List<OrderList> GetOrderLists()
    {
        return [.._orderLists];
    }
}
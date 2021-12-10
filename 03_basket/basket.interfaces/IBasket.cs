using Dapr.Actors;
using System.Collections.Generic;

namespace Basket.Interfaces;

public interface IBasket : IActor
{
    Task AddProductAsync(string productId, int quantity);
    Task RemoveProductAsync(string productId);
    Task ClearAsync();

    Task<BasketContent> GetBasket();

    Task<Order> CreateOrder();

}



public class BasketContent
{
    public DateTime LastUpdate { get; set; }
    public string Id { get; set; }
    public List<BasketLine> Items { get; set; }
}

public class BasketLine
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}



public class Order
{
    public string Id { get; set; }
    public string BasketId { get; set; }
    public IEnumerable<OrderLine>  Lines { get; set; }
    public float HT { get; set; }
    public float TVA { get; set; }
    public float TTC { get; set; }
}

public class OrderLine
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public float PuHT { get; set; }    
    public float TVA { get; set; }
    public float TTC { get; set; }
}

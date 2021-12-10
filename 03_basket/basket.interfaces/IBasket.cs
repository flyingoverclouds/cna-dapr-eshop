using Dapr.Actors;


namespace Basket.Interfaces;

public interface IBasket : IActor
{
    Task AddProductAsync(string productId, int quantity);
    Task RemoveProductAsync(string productId);
    Task ClearAsync();

    Task<BasketContent> GetBasket();
}



public class BasketContent
{
    public string Id { get; set; }
    public List<BasketLine> Items { get; set; }
}

public class BasketLine
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
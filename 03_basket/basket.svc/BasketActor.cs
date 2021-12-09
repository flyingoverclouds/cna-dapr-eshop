using Dapr.Actors;
using Dapr.Actors.Runtime;
using Basket.Interfaces;
using System;
using System.Threading.Tasks;

namespace basketsvc;

internal  class BasketActor : Actor, IBasket
{
    public BasketActor( ActorHost host) : base (host)
    {
        
    }
    protected override Task OnActivateAsync()
    {
        Console.WriteLine($"Activating actor id: {this.Id}");
        return Task.CompletedTask;
    }

    protected override Task OnDeactivateAsync()
    {
        Console.WriteLine($"Deactivating actor id: {this.Id}");
        return Task.CompletedTask;
    }
    public async Task AddProductAsync(string productId, int quantity)
    {
        var basket=await this.StateManager.GetStateAsync<BasketContent>("basketcontent");
        var item = basket.Items.Where( l=>l.ProductId==productId).FirstOrDefault();
        if (item!=null)
        {
            item.Quantity+=quantity;
        }
        await this.StateManager.SetStateAsync<BasketContent>(
                "basketcontent",  
                basket);      
    }
    public async Task RemoveProductAsync(string productId)
    {
        var basket=await this.StateManager.GetStateAsync<BasketContent>("basketcontent");
        var item = basket.Items.Where( l=>l.ProductId==productId).FirstOrDefault();
        basket.Items.Remove(item);
        await this.StateManager.SetStateAsync<BasketContent>(
                "basketcontent",  
                basket);      
    }

    public async Task<BasketContent> GetBasket()
    {
        return await this.StateManager.GetStateAsync<BasketContent>("basketcontent");
    }
}

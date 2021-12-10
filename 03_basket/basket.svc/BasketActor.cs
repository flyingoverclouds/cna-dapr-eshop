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
    // protected override Task OnActivateAsync()
    // {
    //     Console.WriteLine($"#[{this.Id}] Activating");
    //     return Task.CompletedTask;
    // }

    // protected override Task OnDeactivateAsync()
    // {
    //     Console.WriteLine($"#[{this.Id}] Deactivating");
    //     return Task.CompletedTask;
    // }
    public async Task AddProductAsync(string productId, int quantity)
    {
        if (string.IsNullOrEmpty(productId) || quantity<1)
            return;
        try {
            Console.WriteLine($"#[{this.Id}] AddProductAsync");
            BasketContent basket;
            var baskettry=await this.StateManager.TryGetStateAsync<BasketContent>("basketcontent");
            if (baskettry.HasValue)
                basket=baskettry.Value;
            else
                basket=new BasketContent() { 
                    Id=this.Id.ToString() , 
                    Items=new List<BasketLine>()  // shoudl be initialized in contructor
                };

            var item = basket.Items.Where( l=>l.ProductId==productId).FirstOrDefault();
            if (item!=null)
            {
                item.Quantity+=quantity;
            }
            else 
            {
                item=new BasketLine() { ProductId=productId, Quantity=quantity };
                basket.Items.Add(item);
            }
            Console.WriteLine($"#[{this.Id}] ProductId={item.ProductId}  Quantity={item.Quantity}");
            await this.StateManager.SetStateAsync<BasketContent>(
                    "basketcontent",  
                    basket);      
        }
        catch(Exception ex)
        {
            Console.WriteLine($"#[{this.Id}] ERROR : \n {ex}");
        }
    }
    public async Task RemoveProductAsync(string productId)
    {
        Console.WriteLine($"#[{this.Id}] RemoveProductAsync()");
        var basket=await this.StateManager.GetStateAsync<BasketContent>("basketcontent");
        var item = basket.Items.Where( l=>l.ProductId==productId).FirstOrDefault();
        basket.Items.Remove(item);
        await this.StateManager.SetStateAsync<BasketContent>(
                "basketcontent",  
                basket);      
    }

    public async Task ClearAsync()
    {
        Console.WriteLine($"#[{this.Id}] ClearAsync()");
        await this.StateManager.TryRemoveStateAsync("basketcontent");
    }

    public async Task<BasketContent> GetBasket()
    {
        Console.WriteLine($"#[{this.Id}] GetBasket()");
        var bc = await this.StateManager.TryGetStateAsync<BasketContent>("basketcontent");
        if (bc.HasValue)
            return bc.Value;
        else
        {
             var b=new BasketContent() { 
                    Id=this.Id.ToString() , 
                    Items=new List<BasketLine>()  // shoudl be initialized in contructor
                };
            return b;
        }
    }
}

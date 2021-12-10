using Dapr.Actors;
using Dapr.Actors.Runtime;
using Basket.Interfaces;
using System;
using System.Threading.Tasks;

namespace basketsvc;

internal  class BasketActor : Actor, IBasket
{
    private readonly Dapr.Client.DaprClient _daprClient;
    public BasketActor( ActorHost host , Dapr.Client.DaprClient daprclient ) : base (host)
    {
        this._daprClient=daprclient;
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
            basket.LastUpdate=DateTime.Now;
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
        basket.LastUpdate=DateTime.Now;
        await this.StateManager.SetStateAsync<BasketContent>(
                "basketcontent",  
                basket);      
    }

    public async Task ClearAsync()
    {
        Console.WriteLine($"#[{this.Id}] ClearAsync()");
        await this.StateManager.TryRemoveStateAsync("basketcontent");
    }

    public async Task<Order> CreateOrder()
    {
        Console.WriteLine($"#[{this.Id}] CreateOrder() ");

        if (_daprClient==null)
        {
            Console.WriteLine("DAPR CLIENT IS NULL !!!");
        } 
        else
        {
            Console.WriteLine("daprclient is ok");
        }

        var bc = await this.StateManager.TryGetStateAsync<BasketContent>("basketcontent");
        if (!bc.HasValue)
        {
            Console.WriteLine($"#[{this.Id}] NO BASKET -> NO ORDER !");
            return null;
        }
        var b = bc.Value;

        var neworder = new Order() { 
            Id=Guid.NewGuid().ToString(),
            BasketId=b.Id,
            Lines=new List<OrderLine>() 
        };
        foreach(var bl in b.Items)
        {
            
            var line = new OrderLine(){
                ProductId = bl.ProductId,
                Quantity = bl.Quantity,
                PuHT = 1.0F,
            };
            line.TVA = (line.Quantity * line.PuHT)*.196F;
            line.TTC = (line.Quantity * line.PuHT) + line.TVA;
            neworder.Lines.Append(line);
        }

        // TODO PUSH Order in Queue

        await this.StateManager.TryRemoveStateAsync("basketcontent");

        return neworder;
    }
    public async Task<BasketContent> GetBasket()
    {
        Console.WriteLine($"#[{this.Id}] GetBasket() ");
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

using Dapr.Actors;
using Dapr.Actors.Runtime;
using Basket.Interfaces;
using System;
using System.Threading.Tasks;
using basket.svc.Models;

namespace basketsvc;

internal  class BasketActor : Actor, IBasket
{
    private readonly Dapr.Client.DaprClient _daprClient;
    private IConfiguration _configuration;
    public BasketActor( ActorHost host , 
                        IConfiguration configuration, 
                        Dapr.Client.DaprClient daprclient 
                    ) : base (host)
    {
        this._daprClient=daprclient;
        this._configuration=configuration;
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


    public async Task<Order> CreateOrder()
    {
        Console.WriteLine($"#[{this.Id}] CreateOrder() ");

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
            Lines=new List<OrderLine>() ,
            HT=0,
            TVA=0,
            TTC=0
        };
        foreach(var bl in b.Items)
        {
            var itm=await _daprClient.InvokeMethodAsync<CatalogItem?>(
                HttpMethod.Get,
                "catalogapi",
                "catalog/item/"+bl.ProductId
            );

            var line = new OrderLine(){
                ProductId = bl.ProductId,
                Description = itm.ProductName,
                Quantity = bl.Quantity,
                PuHT = 1.0F,
            };
            line.TVA = (line.Quantity * line.PuHT)*.196F;
            line.TTC = (line.Quantity * line.PuHT) + line.TVA;
            neworder.HT+=line.Quantity* line.PuHT;
            neworder.TVA+=line.TVA;
            neworder.TTC+=line.TTC;
            neworder.Lines.Add(line);
        }
        
        try {
            await _daprClient.PublishEventAsync("orderpubsub","neworder",neworder);
            Console.WriteLine($"#[{this.Id}] {neworder.Id} : order emitted.");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"#[{this.Id}] ERROR publishEvent : {ex}");
        }
        await this.StateManager.TryRemoveStateAsync("basketcontent");
        return neworder;
    }

}

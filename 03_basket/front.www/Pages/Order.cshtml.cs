using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using front.www.Models;
using Dapr.Client;
using Dapr.Actors;
using Dapr.Actors.Client;
using Basket.Interfaces;

namespace front.www.Pages;

public class OrderModel : PageModel
{
    private readonly ILogger<ProductModel> _logger;
    private readonly IConfiguration _configuration;

    private readonly DaprClient _daprClient;
    
    public OrderModel(ILogger<ProductModel> logger, DaprClient daprClient ,IConfiguration configuration)
    {
        _logger = logger;
        _configuration=configuration;
        _daprClient=daprClient;
    }

    public async Task  OnGet()
    {
        // var basketId=new ActorId("1"); // TODO : replace a cookie set on 1st call id needed
        // var proxy = ActorProxy.Create<IBasket>(basketId,"BasketActor");
        // await proxy.AddProductAsync(Request.Query["id"],1);
        // var basket = await proxy.GetBasket();

        // // Creating a viewmodel for the HTML partg
        // var basketContent = new List<Tuple<CatalogItem,int>>();

        // foreach(var bi in basket.Items)
        // {
        //     //_logger.LogInformation($"Basket: ProductId={bi.ProductId} Qty={bi.Quantity} ");
        //     if (!string.IsNullOrEmpty(bi.ProductId))
        //     {
        //         var itm=await _daprClient.InvokeMethodAsync<CatalogItem?>(
        //             HttpMethod.Get,
        //             "catalogapi",
        //             $"catalog/item/{bi.ProductId}");
        //             basketContent.Add(new Tuple<CatalogItem, int>(itm,bi.Quantity));
        //     }
        //     else
        //     _logger.LogWarning($"#[{basketId}] invalid basket : No product ID on a line !!");
        // }
        // ViewData["basket"]=basketContent;

    }
}

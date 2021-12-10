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

public class BasketAddModel : PageModel
{
    private readonly ILogger<ProductModel> _logger;
    private readonly IConfiguration _configuration;

    private readonly DaprClient _daprClient;
    
    public BasketAddModel(ILogger<ProductModel> logger, DaprClient daprClient ,IConfiguration configuration)
    {
        _logger = logger;
        _configuration=configuration;
        _daprClient=daprClient;
    }

    public async Task  OnGet()
    {
        var itm=await _daprClient.InvokeMethodAsync<CatalogItem?>(
            HttpMethod.Get,
            "catalogapi",
            "catalog/item/"+Request.Query["id"]
        );
        ViewData["Item"]= itm;

        var basketId=new ActorId("2"); // TODO : replace a cookie set on 1st call id needed
        var proxy = ActorProxy.Create<IBasket>(basketId,"BasketActor");
        await proxy.AddProductAsync(Request.Query["id"],1);
    }
}

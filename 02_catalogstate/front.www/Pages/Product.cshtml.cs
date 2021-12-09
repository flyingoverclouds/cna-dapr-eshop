using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using front.www.Models;
using Dapr.Client;

namespace front.www.Pages;

public class ProductModel : PageModel
{
    private readonly ILogger<ProductModel> _logger;
    private readonly IConfiguration _configuration;

    private readonly DaprClient _daprClient;
    
    public ProductModel(ILogger<ProductModel> logger, DaprClient daprClient ,IConfiguration configuration)
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
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using front.www.Models;
using Dapr.Client;

namespace front.www.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    private readonly DaprClient _daprClient;
    
    public IndexModel(ILogger<IndexModel> logger, DaprClient daprClient ,IConfiguration configuration)
    {
        _logger = logger;
        _configuration=configuration;
        _daprClient=daprClient;
    }

    public async Task  OnGet()
    {
        var itms=await _daprClient.InvokeMethodAsync<CatalogItem[]?>(
            HttpMethod.Get,
            _configuration["CatalogSvcName"],
            "catalog/items"
        );
        ViewData["Items"]= itms;
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using admin.www.Models;
using Dapr.Client;
using System.Text;

namespace admin.www.Pages;

public class ResetCatalogModel : PageModel
{
    private readonly ILogger<ResetCatalogModel> _logger;
    private readonly DaprClient _daprClient;
    private readonly IConfiguration _configuration;

    public ResetCatalogModel(ILogger<ResetCatalogModel> logger, IConfiguration configuration, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient=daprClient;
        _configuration=configuration;
    }

    public async Task OnGet()
    {
        _logger.LogDebug("ResetCatalogModel.OnGet");

        try{
            await _daprClient.InvokeMethodAsync(
                HttpMethod.Get,
                "catalogapi",
                "catalog/reset"
            );
            ViewData["log"]="Reset done";
        }
        catch(Exception ex)
        {
            _logger.LogError($"EXCEPTION:  {ex}");
            ViewData["log"]="Exception on call";
        }
            
    }

    
}


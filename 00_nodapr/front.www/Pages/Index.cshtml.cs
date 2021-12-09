using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using front.www.Models;

namespace front.www.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;
    
    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration=configuration;

    }

    public async Task  OnGet()
    {
        var itms=await GetItemsAsync(_configuration["CatalogApiUrl"]);
        ViewData["Items"]= itms;
    }

    private async Task<CatalogItem[]?> GetItemsAsync(string url)
    {
        HttpClient _httpClient=new HttpClient();
        string items = null;
        try
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                items = await response.Content.ReadAsStringAsync();
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,"Error while calling catalog api");
        }

        if (items != null)
        {
            var ci = JsonSerializer.Deserialize<CatalogItem[]>(
                items , 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true}
            );
            return ci;
        }
        return null;
    }

}

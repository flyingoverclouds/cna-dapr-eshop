using Microsoft.AspNetCore.Mvc;
using catalog.api.Models;
using Dapr.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace catalog.api.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly IConfiguration _configuration;
    private readonly DaprClient _daprClient;

    public CatalogController(ILogger<CatalogController> logger,IConfiguration config, DaprClient daprclient)
    {
        _logger = logger;
        _configuration=config;
        _daprClient=daprclient;
    }


    [Route("items")]
    public async Task<IEnumerable<CatalogItem>> GetItems(int count=5)
    {
        try{
            string DAPR_HTTP_PORT= System.Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
            string query = "{ 'query': { 'pagination' : { 'limit': 6} } }".Replace("'","\"");
            
            HttpClient api = new HttpClient();
            var response = await api.PostAsync(
                $"http://localhost:{DAPR_HTTP_PORT}/v1.0-alpha1/state/catalogstore/query",
                new StringContent(query)
                );
            var result = await response.Content.ReadAsStringAsync();
            
            JsonDocument jdoc = JsonDocument.Parse(result);
            var results = jdoc.RootElement.GetProperty("results").EnumerateArray();
            
            var items = new List<CatalogItem>();
            while(results.MoveNext())
            {
                var prodJson = results.Current.GetProperty("data").ToString();
                var ci = JsonSerializer.Deserialize<CatalogItem>(prodJson , new JsonSerializerOptions { PropertyNameCaseInsensitive = true} );
                items.Add(ci);
            }
            return items;
        }
        catch(Exception ex)
        {
            _logger.LogError("EXCEPTION",ex);
            return new List<CatalogItem>();
        }
    }

    [Route("item/{id}")]
    public async Task<CatalogItem> GetItem(string  id)
    {
        var item = await _daprClient.GetStateAsync<CatalogItem>(_configuration["catalogStateStoreName"],id);
        return item; // should return 404 if item==null ...
    }


    [Route("reset")]
    public async Task ResetCatalog()
    {
        var catalogStoreName=_configuration["catalogStateStoreName"];
        var consistency = new StateOptions { Consistency=ConsistencyMode.Eventual };

        StringBuilder sb= new StringBuilder();
        try{
            await _daprClient.DeleteStateAsync(catalogStoreName,"1",consistency);
            await _daprClient.DeleteStateAsync(catalogStoreName,"2",consistency);
            await _daprClient.DeleteStateAsync(catalogStoreName,"3",consistency);
            await _daprClient.DeleteStateAsync(catalogStoreName,"4",consistency);
          
            sb.AppendLine("4 item(s) deleted <br/><br/>");    

            var newDatas=GetCatalogInitDatas();
            foreach(var i in newDatas)
            {
                //i.ProductName+=" DAPR";
                sb.AppendLine($"inserting item {i.Id} / {i.ProductName}<br/>");
                await _daprClient.SaveStateAsync(catalogStoreName,i.Id,i,consistency);
            }
            sb.AppendLine($"{newDatas.Length} item(s) inserted<br/>");    
        }
        catch(Exception ex)
        {
            sb.AppendLine($"<hr/>ERROR: {ex}");
        }
        _logger.LogInformation(sb.ToString());
    }



    CatalogItem[] GetCatalogInitDatas()
    {
        return new CatalogItem[4] {
            new CatalogItem() {
                Id="1",
                OwnerId="999",
                ProductId="1",
                ProductAllergyInfo="None",
                PictureName="Tomate",
                ProductName= "Tomate",
                ProductPictureUrl="https://daprdemores.blob.core.windows.net/images/tomates.jpg?sv=2020-08-04&st=2021-12-08T19%3A52%3A56Z&se=2023-12-09T19%3A52%3A00Z&sr=b&sp=r&sig=Oui%2FK%2BW4B8rB2NaX489kDBtrGBTC3YuGwtJfSEtqhdc%3D"
            },
            new CatalogItem() {
                Id="2",
                OwnerId="999",
                ProductId="2",
                ProductAllergyInfo="None",
                PictureName="Aubergine",
                ProductName= "Aubergine",
                ProductPictureUrl="https://daprdemores.blob.core.windows.net/images/pain.jpg?sv=2020-08-04&st=2021-12-08T19%3A53%3A35Z&se=2023-12-09T19%3A53%3A00Z&sr=b&sp=r&sig=wK%2Brf2oivheFzW0fWElgAIpJV1g%2F04RvCBVuovURMZ0%3D"
            },new CatalogItem() {
                Id="3",
                OwnerId="999",
                ProductId="3",
                ProductAllergyInfo="None",
                PictureName="Aubergine",
                ProductName= "Aubergine",
                ProductPictureUrl="https://daprdemores.blob.core.windows.net/images/aubergine.jpg?sv=2020-08-04&st=2021-12-08T19%3A48%3A57Z&se=2023-12-09T19%3A48%3A00Z&sr=b&sp=r&sig=Chjd8V7ccVi2SiTYEVqybfnkbFKinNO9yYw0cb0JedI%3D"
            },new CatalogItem() {
                Id="4",
                OwnerId="999",
                ProductId="4",
                ProductAllergyInfo="None",
                PictureName="Cola",
                ProductName= "Cola",
                ProductPictureUrl="https://daprdemores.blob.core.windows.net/images/coca.jpg?sv=2020-08-04&st=2021-12-08T19%3A54%3A20Z&se=2023-12-09T19%3A54%3A00Z&sr=b&sp=r&sig=u6dAhIsaqANxYdic%2FG13Omn%2FsbpFKM55kXLXN4bmnlI%3D"
            }
        };
    }

}
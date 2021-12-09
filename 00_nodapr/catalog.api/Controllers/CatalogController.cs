using Microsoft.AspNetCore.Mvc;
using catalog.api.Models;

namespace catalog.api.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }

    //[HttpGet(Name = "GetTime")]
    [Route("time")]
    public string GetTime()
    {
        return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
    }


    //[HttpGet(Name = "GetItems")]
    [Route("items")]
    public IEnumerable<CatalogItem> GetItems(int count=5)
    {
        return GetCatalog_Fake();
    }

    [Route("item/{id}")]
    public CatalogItem GetItem(string  id)
    {
        var item=GetCatalog_Fake().Where( (i) => i.Id==id).FirstOrDefault();
        return item; // should return 404 if item==null ...
    }





    CatalogItem[] GetCatalog_Fake()
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
                PictureName="Pain",
                ProductName= "Pain",
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
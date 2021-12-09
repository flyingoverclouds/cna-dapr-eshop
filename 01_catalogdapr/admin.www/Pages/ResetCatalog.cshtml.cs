using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using admin.www.Models;

namespace admin.www.Pages;

public class ResetCatalogModel : PageModel
{
    private readonly ILogger<ResetCatalogModel> _logger;

    public ResetCatalogModel(ILogger<ResetCatalogModel> logger)
    {
        _logger = logger;
    }

    public string ResetCatalog()
    {
        var newDatas=GetCatalogNewDatas();
        
        return $"{newDatas.Length} item(s)";
    }

    public void OnGet()
    {
    }

    
    CatalogItem[] GetCatalogNewDatas()
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


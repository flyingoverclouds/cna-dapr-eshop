using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.www.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly IConfiguration _configuration;
    
    public PrivacyModel(ILogger<PrivacyModel> logger, IConfiguration configuration)
    {
        this._logger = logger;
        this._configuration=configuration;
    }

    public void OnGet()
    {
        ViewData["CatalogApiUrl"]=_configuration["CatalogApiUrl"];
        ViewData["CatalogSvcName"]=_configuration["CatalogSvcName"];
    }
}


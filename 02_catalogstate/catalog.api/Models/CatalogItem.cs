namespace catalog.api.Models;

public class CatalogItem
{
    public string Id { get; set; }
    public string OwnerId { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductPictureUrl { get; set; }
    public string ProductAllergyInfo { get; set; }
    public string PictureName { get; set; }
}
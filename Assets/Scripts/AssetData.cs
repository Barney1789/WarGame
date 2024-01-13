using System;
using System.Collections.Generic;

[Serializable]
public class AssetData
{
    public string Id { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string PreviewImageUrl { get; set; }
    public string BackgroundImageUrl { get; set; }


    public AssetData(string itemId, string itemDescription, int itemPrice, string previewImageUrl, string backgroundImageUrl)
    {
        Id = itemId;
        Description = itemDescription;
        Price = itemPrice;
        PreviewImageUrl = previewImageUrl;
        BackgroundImageUrl = backgroundImageUrl;
    }
}



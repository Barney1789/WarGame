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


    public AssetData(string id, string description, int price, string previewImageUrl, string backgroundImageUrl)
    {
        Id = id;
        Description = description;
        Price = price;
        PreviewImageUrl = previewImageUrl;
        BackgroundImageUrl = backgroundImageUrl;
    }
}



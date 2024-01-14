using System;
using System.Collections.Generic;

[Serializable]
public class AssetData
{
    public string ItemId { get; set; }
    public string ItemDescription { get; set; }
    public int ItemPrice { get; set; }
    public string PreviewImageUrl { get; set; }
    public string BackgroundImageUrl { get; set; }


    public AssetData(string itemId, string itemDescription, int itemPrice, string previewImageUrl, string backgroundImageUrl)
    {
        ItemId = itemId;
        ItemDescription = itemDescription;
        ItemPrice = itemPrice;
        PreviewImageUrl = previewImageUrl;
        BackgroundImageUrl = backgroundImageUrl;
    }
}



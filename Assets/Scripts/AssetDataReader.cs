using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine; // Make sure to include this for Debug.Log

public static class AssetDataReader
{
    public static List<AssetData> ReadAssetsFromXml(string xmlText, out string baseUrl)
    {
        List<AssetData> assets = new List<AssetData>();
        baseUrl = string.Empty;

        Debug.Log("Starting XML parsing.");

        XDocument doc = XDocument.Parse(xmlText); // Use Parse instead of Load if you have XML as string

        // Extracting baseUrl
        var baseUrlElement = doc.Root.Element("baseUrl");
        if (baseUrlElement != null)
        {
            baseUrl = baseUrlElement.Value;
            Debug.Log("Base URL found: " + baseUrl);
        }
        else
        {
            Debug.LogWarning("Base URL element not found in XML.");
        }

        // Parsing each item in the XML
        foreach (XElement itemElement in doc.Root.Elements("Item"))
        {
            Debug.Log("Parsing Item element.");

            string itemId = itemElement.Element("ItemId")?.Value;
            string itemDescription = itemElement.Element("ItemDescription")?.Value;
            string itemPriceStr = itemElement.Element("ItemPrice")?.Value;
            string previewImageUrl = itemElement.Element("PreviewImageUrl")?.Value;
            string backgroundImageUrl = itemElement.Element("BackgroundImageUrl")?.Value;

            if (itemId == null || itemDescription == null || itemPriceStr == null || previewImageUrl == null || backgroundImageUrl == null)
            {
                Debug.LogWarning("One or more Item element properties are missing.");
                continue;
            }

            int itemPrice;
            if (!int.TryParse(itemPriceStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out itemPrice))
            {
                Debug.LogWarning("Invalid item price: " + itemPriceStr);
                continue;
            }

            Debug.Log($"Parsed Item: Id={itemId}, Description={itemDescription}, Price={itemPrice}, PreviewImage={previewImageUrl}, BackgroundImage={backgroundImageUrl}");

            // Construct AssetData from parsed XML data and add to the list
            AssetData asset = new AssetData(
                itemId,
                itemDescription,
                itemPrice,
                previewImageUrl,
                backgroundImageUrl
            );

            assets.Add(asset);
        }

        Debug.Log("Finished parsing XML. Total assets parsed: " + assets.Count);
        return assets;
    }
}

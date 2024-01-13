using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

public static class AssetDataReader
{
    public static List<AssetData> ReadAssetsFromXml(string xmlText, out string baseUrl)
    {
        List<AssetData> assets = new List<AssetData>();
        baseUrl = string.Empty;

        XDocument doc = XDocument.Parse(xmlText); // Use Parse instead of Load if you have XML as string

        // Assuming the baseUrl is at the root level of your XML, similar to previous structure.
        // If not, you'll need to adjust the location where baseUrl is set.
        baseUrl = doc.Root.Element("baseUrl")?.Value ?? string.Empty;

        foreach (XElement itemElement in doc.Root.Elements("Item"))
        {
            string itemId = itemElement.Element("ItemId").Value;
            string itemDescription = itemElement.Element("ItemDescription").Value;
            int itemPrice = int.Parse(itemElement.Element("ItemPrice").Value, CultureInfo.InvariantCulture);
            string previewImageUrl = itemElement.Element("PreviewImageUrl").Value;
            string backgroundImageUrl = itemElement.Element("BackgroundImageUrl").Value;

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

        return assets;
    }
}

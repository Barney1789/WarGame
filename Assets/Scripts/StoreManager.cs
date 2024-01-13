using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{
    private List<StoreItem> storeItems;

    private void Start()
    {
        LoadAndParseXML();
        DisplayStoreItems();
    }

    private void LoadAndParseXML()
    {
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("manifest");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlTextAsset.text);

        storeItems = new List<StoreItem>();

        foreach (XmlNode itemNode in xmlDoc.SelectNodes("//Item"))
        {
            StoreItem item = new StoreItem
            {
                ItemId = itemNode.SelectSingleNode("ItemId").InnerText,
                ItemDescription = itemNode.SelectSingleNode("ItemDescription").InnerText,
                ItemPrice = int.Parse(itemNode.SelectSingleNode("ItemPrice").InnerText),
                PreviewImageUrl = itemNode.SelectSingleNode("PreviewImageUrl").InnerText,
                BackgroundImageUrl = itemNode.SelectSingleNode("BackgroundImageUrl").InnerText
            };

            storeItems.Add(item);
        }
    }

    private void DisplayStoreItems()
    {
        // Here, you would instantiate UI elements based on the data in storeItems
        // For example, for each item in storeItems, create a new UI panel with its details
    }
}

public class StoreItem
{
    public string ItemId { get; set; }
    public string ItemDescription { get; set; }
    public int ItemPrice { get; set; }
    public string PreviewImageUrl { get; set; }
    public string BackgroundImageUrl { get; set; }
}

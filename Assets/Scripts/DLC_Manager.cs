using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DLC_Manager : MonoBehaviour
{
    private FirebaseStorage _instance;
    [SerializeField] private Transform saleItemsContainer;
    [SerializeField] private GameObject saleItemPrefab;

    private bool IsPurchased(string itemId) {return PlayerPrefs.GetInt(itemId, 0) == 1;}

    void Start()
    {
        // Initialize Firebase
        _instance = FirebaseStorage.DefaultInstance;

        // Download the manifest
        DownloadManifestAsync("gs://war-card-game-789.appspot.com/manifest.xml");
    }

    private void DownloadManifestAsync(string manifestUrl)
    {
        StorageReference manifestRef = _instance.GetReferenceFromUrl(manifestUrl);
        string localFile = Path.Combine(Application.persistentDataPath, "manifest.xml");
        
        manifestRef.GetFileAsync(localFile).ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Manifest download finished.");
                ReadManifest(localFile);
            }
            else
            {
                Debug.LogError("Manifest download failed: " + task.Exception);
            }
        });
    }

    private void ReadManifest(string localFilePath)
    {
        string xmlText = File.ReadAllText(localFilePath);
        List<AssetData> assets = AssetDataReader.ReadAssetsFromXml(xmlText, out string imageBaseUrl);
        DisplayAssets(assets, imageBaseUrl);
    }
        private void DisplayAssets(List<AssetData> assets, string baseUrl) {
        Debug.Log("DisplayAssets called with assets count: " + assets.Count);

        foreach (AssetData asset in assets) {
            Debug.Log($"Creating item with ID: {asset.ItemId}");

            GameObject saleItem = Instantiate(saleItemPrefab, saleItemsContainer);
            if (saleItem == null) {
                Debug.LogError("Instantiated saleItem is null!");
                continue;
            }

            TMP_Text nameText = saleItem.transform.Find("Text").GetComponent<TMP_Text>();
            if (nameText == null) {
                Debug.LogError("NameText component not found!");
            } else {
                nameText.text = asset.ItemDescription;
                Debug.Log($"Assigned name: {asset.ItemDescription}");
            }

            TMP_Text priceText = saleItem.transform.Find("PriceText").GetComponent<TMP_Text>();
            if (priceText == null) {
                Debug.LogError("PriceText component not found!");
            } else {
                priceText.text = asset.ItemPrice.ToString();
                Debug.Log($"Assigned price: {asset.ItemPrice}");
            }

            RawImage rawImageComponent = saleItem.transform.Find("ItemImage").GetComponent<RawImage>();
            if (rawImageComponent == null) {
                Debug.LogError("RawImage component not found!");
            } else {
                StorageReference imageRef = _instance.GetReferenceFromUrl(asset.PreviewImageUrl);
                DownloadImageAsync(imageRef, rawImageComponent);
            }
            // Adding a listener to the sale item's purchase button
            Button purchaseButton = saleItem.transform.Find("Button").GetComponent<Button>();
            purchaseButton.onClick.AddListener(() => PurchaseDLC(asset.ItemPrice, asset, purchaseButton));
                // Disable the button if the item is already purchased
                if (IsPurchased(asset.ItemId)) {
                    purchaseButton.interactable = true;
                    purchaseButton.GetComponentInChildren<TMP_Text>().text = "Purchased";
                }
        }
    }


    private void DownloadImageAsync(StorageReference reference, RawImage imageComponent)
    {
        const long maxAllowedSize =6 * 1024 * 1024; // 6MB
        reference.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task => {
        if (task.IsFaulted || task.IsCanceled) {
        Debug.LogException(task.Exception);
        // Uh-oh, an error occurred!
        }
        else {
        byte[] fileContents = task.Result;
        // Create a new Texture2D object and load the image data
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileContents);
        // Assign the loaded texture to the image component
        imageComponent.texture = texture;
        Debug.Log("Finished downloading image in memory!");
        }
    });
    }

    private void PurchaseDLC(int price, AssetData asset, Button purchaseButton) {
        // Check if the item is already purchased
        if (IsPurchased(asset.ItemId)) {
            Debug.Log($"{asset.ItemDescription} is already owned.");
            // Additional code to use the already owned DLC
        } else {
            // Proceed with purchase if the item is not owned yet
            if (WalletManager.Instance.SpendCoins(price)) {
                // Grant access to the DLC
                Debug.Log($"Purchased {asset.ItemDescription}");
                PlayerPrefs.SetInt(asset.ItemId, 1); // Mark the item as purchased
                purchaseButton.GetComponentInChildren<TMP_Text>().text = "Owned"; // Update button text to "Owned"
                // Additional code to enable the DLC in the game
            } else {
                Debug.Log("Not enough coins to purchase this DLC.");
            }
        }
    }


    private void DownloadBackgroundImageAsync(string imageUrl) {
    StorageReference imageRef = _instance.GetReferenceFromUrl(imageUrl);
    DownloadImageAsync(imageRef, null); // Assuming you modify the existing DownloadImageAsync method
    Debug.Log("Download Background Done.");
    }

    private void OnOwnedButtonClicked(string itemId, string localFileName) {
    // Save the local file name of the purchased item
    PlayerPrefs.SetString("selectedDLCImage", localFileName);
    PlayerPrefs.Save();
    // Assuming you call this method when the Owned button is clicked
    }   



}

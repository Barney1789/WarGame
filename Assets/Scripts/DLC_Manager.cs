using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class DLC_Manager : MonoBehaviour
{
    private FirebaseStorage _instance;
    [SerializeField] private Transform saleItemsContainer;
    [SerializeField] private GameObject saleItemPrefab;

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

    private void DisplayAssets(List<AssetData> assets, string baseUrl)
    {
        foreach (AssetData asset in assets)
        {
            GameObject saleItem = Instantiate(saleItemPrefab, saleItemsContainer);
            saleItem.transform.Find("Text").GetComponent<TMP_Text>().text = asset.Description;
            saleItem.transform.Find("PriceText").GetComponent<TMP_Text>().text = asset.Price.ToString();
            StorageReference imageRef = _instance.GetReferenceFromUrl(asset.PreviewImageUrl);
            RawImage rawImageComponent = saleItem.transform.Find("ItemImage").GetComponent<RawImage>();
            DownloadImageAsync(imageRef, rawImageComponent);
        }
    }

    private void DownloadImageAsync(StorageReference reference, RawImage imageComponent)
    {
        const long maxAllowedSize =2 * 1024 * 1024; // 2MB
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
}

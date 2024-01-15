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
using System.Linq;

public class DisplayDLCImage : MonoBehaviour
{
    [SerializeField] private RawImage displayImage;

    void Start() {
        string lastImageFileName = PlayerPrefs.GetString("LastDisplayedImage", "");

        if (!string.IsNullOrEmpty(lastImageFileName)) {
            string filePath = Path.Combine(Application.persistentDataPath, lastImageFileName);
            if (File.Exists(filePath)) {
                LoadImageFromFile(filePath);
            } else {
                Debug.LogError("Saved image file not found: " + filePath);
            }
        }
    }



    void OnEnable() {
        DLC_Manager.OnDLCItemPurchased += DisplayImageForItem;
    }

    void OnDisable() {
        DLC_Manager.OnDLCItemPurchased -= DisplayImageForItem;
    }

    private void DisplayImageForItem(string itemId) {
        string fileName = itemId + "background.png"; // Assuming this is how your file names are formed
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath)) {
            LoadImageFromFile(filePath);
            PlayerPrefs.SetString("LastDisplayedImage", fileName); //display2
            PlayerPrefs.Save(); //display2
        } else {
            Debug.LogError("Image file not found: " + filePath);
        }
    }
    private void DisplayMostRecentDownloadedImage()
    {
        string mostRecentFile = GetMostRecentFile(Application.persistentDataPath, "*background.png");
        if (!string.IsNullOrEmpty(mostRecentFile))
        {
            LoadImageFromFile(mostRecentFile);
        }
        else
        {
            Debug.LogError("No downloaded image found");
        }
    }

    private string GetMostRecentFile(string directory, string pattern)
    {
        var files = new DirectoryInfo(directory).GetFiles(pattern);
        return files.OrderByDescending(f => f.CreationTime).FirstOrDefault()?.FullName;
    }

    private void LoadImageFromFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        displayImage.texture = texture;
    }
}


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

    void Start()
    {
        DisplayMostRecentDownloadedImage();
    }

    void OnEnable() {
        DLC_Manager.OnImageDownloaded += DisplayMostRecentDownloadedImage;
    }

    void OnDisable() {
        DLC_Manager.OnImageDownloaded -= DisplayMostRecentDownloadedImage;
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


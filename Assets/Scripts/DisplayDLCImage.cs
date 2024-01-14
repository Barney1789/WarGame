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

public class DisplayDLCImage : MonoBehaviour {
    public RawImage displayImage;

    void Start() {
        // Get the path to the image file
        string localFileName = PlayerPrefs.GetString("selectedDLCImage", "");
        if (!string.IsNullOrEmpty(localFileName)) {
            string filePath = Path.Combine(Application.persistentDataPath, localFileName);
            if (File.Exists(filePath)) {
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData)) {
                    displayImage.texture = texture;
                }
            } else {
                Debug.LogError("Saved image file not found.");
            }
        }
    }
}

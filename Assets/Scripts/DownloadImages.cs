using Firebase.Storage;
using UnityEngine;

public class DownloadImages : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference storageRef;

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://war-card-game-789.appspot.com");

        DownloadImage("Background1.jpg");
        DownloadImage("Background2.jpg");
        DownloadImage("Background3.jpg");
    }

    void DownloadImage(string fileName)
    {
        StorageReference imageRef = storageRef.Child(fileName);
        imageRef.GetBytesAsync(long.MaxValue).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                // Handle the error...
            }
            else
            {
                byte[] fileContents = task.Result;
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileContents);
                // Apply this texture to your UI element or wherever needed
            }
        });
    }
}

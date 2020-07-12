using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseClass : MonoBehaviour
{
    public MenuController menuController;

    //https://firebase.google.com/docs/storage/unity/start
    public void createRef()
    {
        Debug.Log("upload file to Firebase ----------");
        // Get a reference to the storage service, using the default Firebase App
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

        // Create a storage reference from our storage service
        Firebase.Storage.StorageReference storage_ref =
          storage.GetReferenceFromUrl("gs://novel-interaction-paradigms.appspot.com");

        // File located on disk
        string local_file = Application.persistentDataPath + "/" + menuController.PlayerName + ".xml" ;

        // Create a reference to the file you want to upload
        Firebase.Storage.StorageReference rivers_ref = storage_ref.Child(menuController.PlayerName + ".xml");

        StartCoroutine(CheckInternetConnection(isConnected =>
        {
            if (isConnected)
            {
                Debug.Log("Internet Available!");

                Stream stream = new FileStream(local_file, FileMode.Open);

                rivers_ref.PutStreamAsync(stream).ContinueWith((Task<StorageMetadata> task) =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        Debug.Log(task.Exception.ToString());
                        // Uh-oh, an error occurred!
                    }
                    stream.Close();
                });
            }
            else
            {
                Debug.Log("Internet Not Available");
            }
        }));
    }


    //https://stackoverflow.com/questions/34138707/check-for-internet-connectivity-from-unity
    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log("Error");
            action(false);
        }
        else
        {
            Debug.Log("Success");
            action(true);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FetchDungeonData : MonoBehaviour
{

    public void GetDungeonData()
    {
        Debug.Log("Fetching GAN dungeon data..."); 
        StartCoroutine(FetchData("http://127.0.0.1:8080/gan_dungeon"));
    }

    public void GetPixelCNNDungeonData()
    {
        Debug.Log("Fetching pixelcnn dungeon data..."); 
        StartCoroutine(FetchData("http://127.0.0.1:8080/dungeon_pixelcnn"));
    }

    IEnumerator FetchData(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log(json); // Print the received JSON data
                DungeonGenerator dungeonGenerator = FindObjectOfType<DungeonGenerator>();
                if (dungeonGenerator != null)
                {
                    dungeonGenerator.dungeonDataUrl = url; // Pass the URL to the DungeonGenerator
                }
                else
                {
                    Debug.LogError("DungeonGenerator not found in the scene.");
                }
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FetchDungeonData : MonoBehaviour
{
    public void GetDungeonData() // Change IEnumerator to public void
    {
        Debug.Log("Fetching dungeon data..."); 
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/gan_dungeon"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log(json); // Print the received JSON data
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}

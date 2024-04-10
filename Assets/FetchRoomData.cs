using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FetchRoomData : MonoBehaviour
{
    public void GetRoomData() // Change IEnumerator to public void
    {
        Debug.Log("Fetching room data..."); 
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:8080/vae_room"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                // Debug.Log(json); // Print the received JSON data
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}

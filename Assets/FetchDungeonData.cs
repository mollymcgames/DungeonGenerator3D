using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FetchDungeonData : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetDungeonData());
    }

    IEnumerator GetDungeonData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/get_dungeon_data"))
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

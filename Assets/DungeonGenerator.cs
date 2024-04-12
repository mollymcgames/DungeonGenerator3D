using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System; // Import IEnumerator

public class DungeonGenerator : MonoBehaviour
{
    // URL for Flask API to fetch dungeon layout data
    public string dungeonDataUrl;

    public string roomDataUrl; //URL for VAE room data

    public TextAsset roomData; // Assign the JSON room data file in the Unity editor
    public GameObject emptyRoomPrefab; // Prefab for an empty room
    public GameObject roomPrefab; // Prefab for a room

    // Prefabs for room objects
    public GameObject benchPrefab;
    public GameObject tablePrefab;
    public GameObject collectableItemPrefab;
    public GameObject bookshelfPrefab;
    public GameObject torchPrefab;
    public GameObject doorPrefab;
    public GameObject cratePrefab;

    void Start()
    {
        Debug.Log("Building selected dungeon: " + MainManager.Instance.selectedModelDungeon);
        Debug.Log("Building selected room: " + MainManager.Instance.selectedModelRoom);
        // Fetch JSON data for the dungeon layout from Flask API
        StartCoroutine(FetchDungeonData()); // Start coroutine to fetch dungeon data
        StartCoroutine(FetchRoomData()); // Start coroutine to fetch VAE room data
    }

    public IEnumerator FetchDungeonData() // Specify IEnumerator without type argument
    {
        string fullUrl = dungeonDataUrl;
        Debug.Log("Fetching selected dungeon: " + MainManager.Instance.selectedModelDungeon);
        switch (MainManager.Instance.selectedModelDungeon)
        {
            case "gandungeon":
                fullUrl += "/gan_dungeon";
                break;
            case "pixelcnndungeon":
                fullUrl += "/dungeon_pixelcnn";
                break;
            default:
                fullUrl += "/gan_dungeon";
                break;
        }

        Debug.Log("Fetching dungeon data from URL: " + fullUrl); // Add this line to check the URL

        using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUrl))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Parse the JSON data
                string jsonDungeon = webRequest.downloadHandler.text;
                List<List<int>> dungeonList = DeserializeJson(jsonDungeon);

                // Load JSON data for the room layout
                string jsonRoom = roomData.text;
                Debug.Log("room:");
                Debug.Log("jsonRoom:");
                // string jsonRoom = webRequest.downloadHandler.text;
                List<List<int>> roomList = DeserializeJson(jsonRoom);

                // Generate dungeon map based on the data
                GenerateMap(dungeonList, roomList);
            }
        }
    }

    IEnumerator FetchRoomData() // Specify IEnumerator without type argument
    {
        string fullUrl = roomDataUrl;
        Debug.Log("Fetching selected room: " + MainManager.Instance.selectedModelRoom);
       
        switch (MainManager.Instance.selectedModelRoom)
        {
            case "ganroom":
                fullUrl += "/room_gan";
                break;
            case "vaeroom":
                fullUrl += "/vae_room";
                break;
            default:
                fullUrl += "/vae_room";
                break;
        }

        Debug.Log("Fetching room data from URL: " + fullUrl); // Add this line to check the URL

        using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUrl))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Parse the JSON data
                string jsonRoom = webRequest.downloadHandler.text;
                List<List<int>> roomList = DeserializeJson(jsonRoom);

                Debug.Log("room:");
                Debug.Log("jsonRoom:");
                // Load JSON data for the room layout
                string jsonDungeon = webRequest.downloadHandler.text;
                List<List<int>> dungeonList = DeserializeJson(jsonDungeon);

                // Generate dungeon map based on the data
                GenerateMap(dungeonList, roomList);
            }
        }
    }

    List<List<int>> DeserializeJson(string json)
    {
        List<List<int>> result = new List<List<int>>();
        json = json.Replace("\n", ""); // Remove newline characters
        json = json.Replace("\r", ""); // Remove carriage return characters
        json = json.Trim(); // Remove leading and trailing whitespace

        string[] rows = json.Split('['); // Split JSON by '[' to separate rows
        foreach (string row in rows)
        {
            if (!string.IsNullOrEmpty(row))
            {
                List<int> rowData = new List<int>();
                string[] values = row.Replace("]", "").Split(','); // Split row by ',' to separate values
                foreach (string value in values)
                {
                    int intValue;
                    if (int.TryParse(value, out intValue))
                    {
                        rowData.Add(intValue);
                    }
                }
                result.Add(rowData);
            }
        }
        return result;
    }


    void GenerateMap(List<List<int>> dungeonList, List<List<int>> roomList)
    {
        // Define tile size and spacing
        float tileSize = 20.0f; // Adjust as needed
        float spacing = 0.1f; // Adjust as needed

        // Loop through dungeon layout
        for (int y = 0; y < dungeonList.Count; y++)
        {
            for (int x = 0; x < dungeonList[y].Count; x++)
            {
                // Calculate position based on grid coordinates with spacing
                Vector3 position = new Vector3(x * (tileSize + spacing), 0, y * (tileSize + spacing));

                // Check if room exists in the dungeon layout
                if (dungeonList[y][x] == 1)
                {
                    // If room exists, generate room based on room layout
                    GenerateRoom(position, roomList);

                    // Instantiate room prefab at the position
                    Instantiate(roomPrefab, position, Quaternion.identity); //change this logic later as the room will generate the tiles itself
                }
                else
                {
                    // Otherwise, generate empty room
                    Instantiate(emptyRoomPrefab, position, Quaternion.identity);
                }
            }
        }
    }

    void GenerateRoom(Vector3 position, List<List<int>> roomList)
    {
        // Define tile size and spacing for room
        float tileSize = 1.0f; // Adjust as needed
        float spacing = 0.1f; // Adjust as needed

        // Calculate the offset based on the size of the room
        float offsetX = (roomList[0].Count * (tileSize + spacing)) / 2.0f;
        float offsetY = (roomList.Count * (tileSize + spacing)) / 2.0f;

        // Loop through room layout
        for (int y = 0; y < roomList.Count; y++)
        {
            for (int x = 0; x < roomList[y].Count; x++)
            {
                // Calculate position based on grid coordinates with spacing and apply offset
                Vector3 roomPosition = position + new Vector3((x * (tileSize + spacing)) - offsetX, 0, (y * (tileSize + spacing)) - offsetY);

                // Check if room object exists in the room layout
                int roomObjectIndex = roomList[y][x];
                if (roomObjectIndex > 1)
                {
                    // If room object exists, instantiate corresponding prefab
                    GameObject roomObjectPrefab = GetRoomObjectPrefab(roomObjectIndex);
                    if (roomObjectPrefab != null)
                    {
                        InstantiateCorrectlyOriented(roomObjectPrefab, x, y, roomList, roomPosition, roomObjectIndex == 7);                        
                    }
                }
            }
        }
    }

    void InstantiateCorrectlyOriented(GameObject roomObjectPrefab, int x, int y, List<List<int>> roomList, Vector3 roomPosition, bool doRotation)
    {
        int wallValue = 0;

        if (doRotation) // Check if it's the door prefab (index 7 based on your code)
        {
            // Get surrounding tiles information (assuming walls have a specific value in roomList)
            int leftValue = 0; // Adjust based on your wall value in roomList

            if (x > 0) // Check if there's a tile to the left
            {
                leftValue = roomList[y][x - 1];
            }
            //Debug.Log("Left value:" + leftValue);

            // Similar checks for right, up, and down (adjust as needed)
            int rightValue = 0;
            int upValue = 0;
            int downValue = 0;

            // Determine rotation based on surrounding walls
            if (leftValue == wallValue) // Wall to the left, rotate 90 degrees on Y-axis
            {
                //Debug.Log("Rotate left");
                Instantiate(roomObjectPrefab, roomPosition, Quaternion.Euler(0, 90f, 0));
            }
            else if (rightValue == wallValue) // Wall to the right, rotate -90 degrees on Y-axis
            {
                //Debug.Log("Rotate right");
                Instantiate(roomObjectPrefab, roomPosition, Quaternion.Euler(0, -90f, 0));
            }
            else if (upValue == wallValue) // Wall above, rotate 180 degrees
            {
                //Debug.Log("Rotate up");
                Instantiate(roomObjectPrefab, roomPosition, Quaternion.Euler(0, 180f, 0));
            }
            else if (downValue == wallValue) // Wall below, no rotation needed (assuming default)
            {
                //Debug.Log("Rotate down");
                Instantiate(roomObjectPrefab, roomPosition, Quaternion.identity);
            }
            else // No surrounding walls, default rotation
            {
                Instantiate(roomObjectPrefab, roomPosition, Quaternion.identity);
            }
        }
        else
        {
            // No rotation needed, just instantiate!
            Instantiate(roomObjectPrefab, roomPosition, Quaternion.identity);
        }
    }

    GameObject GetRoomObjectPrefab(int index)
    {
        // Define a mapping between room object index and prefab
        Dictionary<int, GameObject> prefabMap = new Dictionary<int, GameObject>
        {
            { 2, benchPrefab },
            { 3, tablePrefab },
            { 4, collectableItemPrefab },
            { 5, bookshelfPrefab },
            { 6, torchPrefab },
            { 7, doorPrefab },
            { 8, cratePrefab }
        };

        // Return corresponding prefab for the given index
        if (prefabMap.ContainsKey(index))
        {
            return prefabMap[index];
        }
        else
        {
            return null; // Return null if no prefab found for the index
        }
    }
}

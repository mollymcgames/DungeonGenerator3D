using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public TextAsset dungeonData; // Assign your JSON file in the Unity editor
    public GameObject emptyRoomPrefab; // Prefab for an empty room
    public GameObject roomPrefab; // Prefab for a room

    void Start()
    {
        // Load JSON data
        string json = dungeonData.text;
        List<List<int>> dungeonList = DeserializeJson(json);

        // Generate dungeon map based on the data
        GenerateMap(dungeonList);
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


    void GenerateMap(List<List<int>> dungeonList)
    {
        // Define tile size and spacing
        float tileSize = 20.0f; // Adjust as needed
        float spacing = 0.1f; // Adjust as needed

        // Instantiate tiles based on dungeon data
        for (int y = 0; y < dungeonList.Count; y++)
        {
            for (int x = 0; x < dungeonList[y].Count; x++)
            {
                // Calculate position based on grid coordinates with spacing
                Vector3 position = new Vector3(x * (tileSize + spacing), 0, y * (tileSize + spacing));

                GameObject tilePrefab = dungeonList[y][x] == 1 ? roomPrefab : emptyRoomPrefab;
                Instantiate(tilePrefab, position, Quaternion.identity);
            }
        }
    }

}

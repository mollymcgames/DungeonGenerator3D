using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Debug.Log("MainMenu script started!");
    }

    public void StartGame()
    {
        Debug.Log("StartGame() called");
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame() called");
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        Debug.Log("BackToMainMenu() called");
        SceneManager.LoadScene("MainMenuStart");
    }

    public void SelectDungeon(string dungeonType)
    {
        MainManager.Instance.selectedModelDungeon = dungeonType;
        Debug.Log("Selected dungeon: " + MainManager.Instance.selectedModelDungeon);
    }

    public void SelectRoom(string roomType)
    {
        MainManager.Instance.selectedModelRoom = roomType;
        Debug.Log("Selected room: " + MainManager.Instance.selectedModelRoom);
    }

}

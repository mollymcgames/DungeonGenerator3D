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

    //Go back to main menu
    public void BackToMainMenu()
    {
        Debug.Log("BackToMainMenu() called");
        SceneManager.LoadScene("MainMenuStart");
    }

    public void SelectDungeon(string dungeonType)
    {
        // add code here to handle when a color is selected
        MainManager.Instance.selectedModelDungeon = dungeonType;
        Debug.Log("Selected dungeon: " + MainManager.Instance.selectedModelDungeon);
    }

    public void SelectRoom(string roomType)
    {
        // add code here to handle when a color is selected
        MainManager.Instance.selectedModelRoom = roomType;
        Debug.Log("Selected room: " + MainManager.Instance.selectedModelRoom);
    }

}

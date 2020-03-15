using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackAndExit_ExitScene : MonoBehaviour
{
    public static GameObject ExitPanel;

    void Start()
    {
        ExitPanel = GameObject.Find("ExitCanvas");
    }

    public void backButton()
    {
        Debug.Log("Back Button pressed!");
        ExitPanel.SetActive(false);
    }

    public void QuitGame()
    {
        // save any game data here
    #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}

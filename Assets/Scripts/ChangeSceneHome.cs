using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneHome : MonoBehaviour
{

    public static GameObject ExitPanel, InstructionPanel;

    void Start()
    {
        ExitPanel = GameObject.Find("ExitCanvas"); ExitPanel.SetActive(false);
        InstructionPanel = GameObject.Find("InstructionCanvas"); InstructionPanel.SetActive(false);
    }

    public void InstructionButton()
    {

        if (!InstructionPanel.activeSelf)
        {
            InstructionPanel.SetActive(true);
            if (ExitPanel.activeSelf)
            {
                ExitPanel.SetActive(false);
            }
        }
        else {
            InstructionPanel.SetActive(false);
        }

    }

    public void exitButton()
    {
        Debug.Log("Exit Button pressed!");
        ExitPanel.SetActive(true);
        InstructionPanel.SetActive(false);
    }

}

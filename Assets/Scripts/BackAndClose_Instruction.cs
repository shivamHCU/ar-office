using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackAndClose_Instruction : MonoBehaviour
{
    public static GameObject InstructionPanel;

    void Start()
    {
        InstructionPanel = GameObject.Find("InstructionCanvas");
    }

    public void backButton()
    {
        Debug.Log("Back Button pressed!");
        InstructionPanel.SetActive(false);
    }
}


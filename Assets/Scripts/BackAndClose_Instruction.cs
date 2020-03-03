using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackAndClose_Instruction : MonoBehaviour
{
    public void backButton()
    {
        Debug.Log("Back Button pressed!");
        SceneManager.LoadScene("CloudRecoScene");
    }
}


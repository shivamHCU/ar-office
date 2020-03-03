using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneHome : MonoBehaviour
{
    public void InstructionButton()
    {
        Debug.Log("Instruction Button pressed!");
        SceneManager.LoadScene("InstructionScene");
    }

    public void exitButton()
    {
        Debug.Log("Exit Button pressed!");
        SceneManager.LoadScene("ExitScene");
    }

}

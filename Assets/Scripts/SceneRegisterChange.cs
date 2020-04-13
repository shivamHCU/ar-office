using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneRegisterChange : MonoBehaviour
{
    
    public void LoadLoginScene()
    {
        SceneManager.LoadScene("LoginMenu");
    }

    public void LoadSignInScene()
    {
        SceneManager.LoadScene("RegisterMenu");
    }

    public void LoadCloudRecoScene()
    {
        SceneManager.LoadScene("CloudRecoScene");
    }


    public void LoadAdminScene()
    {
        SceneManager.LoadScene("AdminPanelLogin");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("CloudRecoScene");
    }

    
}

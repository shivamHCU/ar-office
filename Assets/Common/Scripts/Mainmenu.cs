﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    public Text playerDisplay;

    private void Start()
    {
        if (DBManager.LoggedIn)
        {
            playerDisplay.text = "Player: " + DBManager.username;
        }
    }

    public void GoToRegister()
    {
        SceneManager.LoadScene("RegisterMenu");
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("LoginMenu");
    }
}
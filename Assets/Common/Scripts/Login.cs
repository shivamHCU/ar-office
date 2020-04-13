using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;

    public void CalltheLogin()
    {
        StartCoroutine(UseLogin());

    }

    IEnumerator UseLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        UnityWebRequest www = UnityWebRequest.Post("https://shivamgangwar.000webhostapp.com/sqlconnect/login.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }

        else if (www.downloadHandler.text == "0")
        {
            Debug.Log("loggedin");
            DBManager.username = nameField.text;
            UnityEngine.SceneManagement.SceneManager.LoadScene("AdminPanelVWS");
            
        }
        else
        {
            Debug.Log(www.downloadHandler.text + "Incorrect username or password");
        }

        Debug.Log(www.downloadHandler.text);
    }




    public void VerifyInput()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}

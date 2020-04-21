using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;
    public Text Errmsgfield;
    public Button submitButton;

    private static string username;

    public void CalltheLogin()
    {
        StartCoroutine(UseLogin());
    }

    public IEnumerator UseLogin()
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
            Errmsgfield.text = www.downloadHandler.text;
        }

        else if (www.downloadHandler.text == "0")
        {
            Debug.Log("loggedin");
            Errmsgfield.text = www.downloadHandler.text;
            DBManager.username = nameField.text;
            username = nameField.text;
            UnityEngine.SceneManagement.SceneManager.LoadScene("AdminPanelVWS");

        }
        else
        {
            Debug.Log(www.downloadHandler.text + "Incorrect username or password");
            Errmsgfield.text = www.downloadHandler.text + "Incorrect username or password";
        }

        Debug.Log(www.downloadHandler.text);
        Errmsgfield.text = www.downloadHandler.text;
    }




    public void VerifyInput()
    {
        //submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }

    public static string getUsername() {
        return username;
    }
}

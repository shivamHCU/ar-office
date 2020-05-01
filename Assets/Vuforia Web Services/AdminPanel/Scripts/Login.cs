using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public InputField nameField;    //username
    public InputField passwordField; //password
    public Text Errmsgfield; //error message
    public Button submitButton;  //login button

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

        UnityWebRequest www = UnityWebRequest.Post("https://shivamgangwar.000webhostapp.com/sqlconnect/login.php", form);   //connect to database with a form containing username and pwd
        yield return www.SendWebRequest();  //Wait for return of data from the server
         
        if (www.isNetworkError || www.isHttpError) 
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            Errmsgfield.text = www.downloadHandler.text;   //get error message
        }

        else if (www.downloadHandler.text == "0")   //if php echos a "0" signifying all was correct
        {
            Debug.Log("loggedin");
            Errmsgfield.text = www.downloadHandler.text; //download error message
            DBManager.username = nameField.text;  
            username = nameField.text;  //set username for display
            UnityEngine.SceneManagement.SceneManager.LoadScene("AdminPanelVWS");

        }
        else
        {
            Debug.Log(www.downloadHandler.text + "Incorrect username or password");
            Errmsgfield.text = www.downloadHandler.text + "Incorrect username or password";
        }

        Debug.Log(www.downloadHandler.text);
        Errmsgfield.text = www.downloadHandler.text;  //show error message
    }




    public void VerifyInput()
    {
        //submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }

    public static string getUsername() {
        return Login.username;
    }
    public static void setUsername(string name)
    {
        Login.username = name;
    }
}

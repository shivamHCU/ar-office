using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Registration : MonoBehaviour
{
    public InputField UserNameField;  //username
    public InputField nameField;  //name
    public InputField passwordField; //password
    public InputField cnfpwdField; //confirm password
    public Text Errmsgfield; //error message
    public Button submitButton;  //register

    public void CalltheRegister()
    {
        StartCoroutine(Register());

    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("Username", UserNameField.text);
        form.AddField("password", passwordField.text);
        if (passwordField.text == cnfpwdField.text)
        {

            UnityWebRequest www = UnityWebRequest.Post("https://shivamgangwar.000webhostapp.com/sqlconnect/register.php", form);  //connect to database with a form containing username and pwd and name
            yield return www.SendWebRequest();  //Wait for return of data from the server
            
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
                Errmsgfield.text = www.downloadHandler.text; //catch network error  message
            }
            else if (www.downloadHandler.text == "0")
            {
                Errmsgfield.text = www.downloadHandler.text; //catch what was echoed
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginMenu");
                Debug.Log("Form upload complete!");
            }
            else
            {
                Errmsgfield.text = www.downloadHandler.text.Substring(0,www.downloadHandler.text.Length-1); //catch error if no other problem occured
                Debug.Log(www.downloadHandler.text);
            }
        }
        else
        {
            Debug.Log("Passwords don't match");
            Errmsgfield.text = "Passwords don't match";
        }
    }

    public void VerifyInput()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}

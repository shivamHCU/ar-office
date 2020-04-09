using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Registration : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;

    public void CalltheRegister()
    {
        StartCoroutine(Register());

    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        UnityWebRequest www = UnityWebRequest.Post("https://shivamgangwar.000webhostapp.com/sqlconnect/register.php", form);
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError)
        { 
            Debug.Log(www.error);
        }
        else
            {
            UnityEngine.SceneManagement.SceneManager.LoadScene("AdminPanelLogin");
            Debug.Log("Form upload complete!");
            }
        
    }
   
    

    public void VerifyInput()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}

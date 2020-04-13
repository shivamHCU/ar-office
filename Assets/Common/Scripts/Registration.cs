using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Registration : MonoBehaviour
{
    public InputField UserNameField;
    public InputField nameField;
    public InputField passwordField;
    public InputField cnfpwdField;

    public Button submitButton;

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

            UnityWebRequest www = UnityWebRequest.Post("https://shivamgangwar.000webhostapp.com/sqlconnect/register.php", form);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else if (www.downloadHandler.text=="0")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginMenu");
                Debug.Log("Form upload complete!");
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
        else
        {
            Debug.Log("Failed");
        }
    }
   
    

    public void VerifyInput()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}

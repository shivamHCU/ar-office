using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.Collections;

public class WebFileUplaoder : MonoBehaviour
{
    public string screenShotURL = "http://www.my-server.com/cgi-bin/screenshot.pl";

    public Image image;
    public VideoPlayer videoPlayer;
    public InputField TargetMetaField;

    public void UploadImage() {
        StartCoroutine("UploadPNG");
    }

    IEnumerator UploadPNG()
    {
        // Create a texture the size of the screen, RGB24 format

        Texture2D tex = image.sprite.texture;

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        // Create a Web Form
        Debug.Log(System.DateTime.Today.ToString());

        WWWForm form = new WWWForm();
        
        form.AddField("name", System.DateTime.Today.ToString());
        form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");

        Debug.Log(form.data.ToString());

        yield return true;

        /*
        // Upload to a cgi script
        using (var w = UnityWebRequest.Post(screenShotURL, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
            }
            else
            {
                print("Finished Uploading Screenshot");
            }
        }
        */
    }
}

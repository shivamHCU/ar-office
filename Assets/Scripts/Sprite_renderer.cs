using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sprite_renderer : MonoBehaviour
{
    public SpriteRenderer imageToDisplay;

    public string imageUrl;

    public IEnumerator loadSpriteImageFromUrl(string URL)
    {
        Debug.Log("< color = red > Running the coroutine with url = </ color > "+ URL);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {

            Texture2D texture = new Texture2D(1, 1);
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            float x = (texture.height / 635.0f) * 100.0f;

            Sprite sprite = Sprite.Create(texture,
                new Rect(-0.0016f, -0.0013f, texture.width, texture.height), new Vector2(0.5f, 0.5f), x);

            imageToDisplay.sprite = sprite;
        }


        /*
        WWW www = new WWW(URL);
        while (!www.isDone)
        {
            Debug.Log("Download image on progress" + www.progress);
            yield return null;
        }

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Download failed");
        }
        else
        {
            Debug.Log("Download succes");
            Texture2D texture = new Texture2D(1, 1);
            www.LoadImageIntoTexture(texture);

            float x = (texture.height/635.0f) * 100.0f;

            Sprite sprite = Sprite.Create(texture,
                new Rect(-0.0016f, -0.0013f, texture.width, texture.height), new Vector2(0.5f,0.5f ), x);


            imageToDisplay.sprite = sprite;
        }
        */
    }
}
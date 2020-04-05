using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_renderer : MonoBehaviour
{
    public SpriteRenderer imageToDisplay;
    string url = "https://media-private.canva.com/ddoPw/MAD4ZOddoPw/1/s.jpg?response-expires=Fri%2C%2003%20Apr%202020%2014%3A55%3A08%20GMT&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Date=20200403T121740Z&X-Amz-SignedHeaders=host&X-Amz-Expires=9447&X-Amz-Credential=AKIAJWF6QO3UH4PAAJ6Q%2F20200403%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Signature=8f61eca09c8d52259cfa139ba9af9332b3a23bc4e453899beeb33ae76d048673";

    void Start()
    {
        StartCoroutine(loadSpriteImageFromUrl(url));
    }

    IEnumerator loadSpriteImageFromUrl(string URL)
    {

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

            Sprite sprite = Sprite.Create(texture,
                new Rect(-0.0016f, -0.0013f, texture.width, texture.height), new Vector2(0.5f,0.5f ));


            imageToDisplay.sprite = sprite;
        }
    }
}
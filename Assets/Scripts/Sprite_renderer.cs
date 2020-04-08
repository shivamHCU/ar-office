using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_renderer : MonoBehaviour
{
    public SpriteRenderer imageToDisplay;

    public string imageUrl;

    /*
    void Start()
    {
        StartCoroutine(funtioncall());
    }
    

    IEnumerator funtioncall() {
        yield return new WaitForSeconds(2F);
        Debug.Log("< color = red > About to start the coroutine. </ color > ");
        StartCoroutine(loadSpriteImageFromUrl(imageUrl));
    }
    */


    public IEnumerator loadSpriteImageFromUrl(string URL)
    {
        Debug.Log("< color = red > Running the coroutine with url = </ color > "+ URL);
        
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
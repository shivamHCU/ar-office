using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sprite_renderer : MonoBehaviour
{
    public SpriteRenderer imageToDisplay;
    public GameObject parent;
    public string imageUrl;
    float x;

    private void Update()
    {
        if (imageUrl.Equals("")) {
            imageToDisplay.sprite = null;
        }
    }


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

            //float x = (texture.height / 635.0f) * 100.0f;
            //1.5827 |              
            //1 1.5827/6.350 = 0.0002492 
            //1800   nop = 1204

            //1 / 0.083 = ;

            //1/ 0.1582 = 632.1;

            Debug.Log(texture.height);
            Debug.Log(texture.width);
            x = 1.0122f * texture.width - 10.647f;
            Debug.Log(x);


            //1200 - 1204;
            //635 - 632.1;


            //float yScale = -0.5f;

            //parent.GetComponent<Transform>().localScale += new Vector3(0f,yScale, 0f);


            Sprite sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2(0.5f, 0.5f),x);

            imageToDisplay.sprite = sprite;

            //=1800*0.0002492f
        }
    }
}
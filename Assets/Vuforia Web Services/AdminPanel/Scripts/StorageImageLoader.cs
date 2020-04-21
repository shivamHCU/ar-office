using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class StorageImageLoader : MonoBehaviour
{

    public Transform ContentImageHolder;
    public GameObject ImageButton;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("FileLoaderStart()");
        string path=null;
        #if UNITY_ANDROID
            path = Application.persistentDataPath + "/";
        #endif
        #if UNITY_EDITOR
            path = "C://MyTemp/";
        #endif

        DirectoryInfo levelDirectoryPath = new DirectoryInfo(path);
        FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*");
//        fileInfo.Union(levelDirectoryPath.GetFiles("*.jpeg"));
//        fileInfo.Union(levelDirectoryPath.GetFiles("*.png"));

        foreach (FileInfo file in fileInfo)
        {
            Debug.Log(file.Name);
            GameObject btnObject = Instantiate(ImageButton);
            StartCoroutine(loadSpriteImageFromUrl(btnObject.GetComponentInChildren<Image>(), file.Name));
            btnObject.transform.SetParent(ContentImageHolder);
        }

    }


    public IEnumerator loadSpriteImageFromUrl(Image ImgHolder,string name)
    {

        //make the url first
        string URL=null;
        #if UNITY_ANDROID
                URL = "file://" + Application.persistentDataPath + "/" + name;
        #endif
        #if UNITY_EDITOR
                URL = "file:///C:/MyTemp/" + name;
        #endif
        
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Download Succesfull!");
            Texture2D texture = new Texture2D(1, 1);
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            ImgHolder.sprite = sprite;
        }
    }




}

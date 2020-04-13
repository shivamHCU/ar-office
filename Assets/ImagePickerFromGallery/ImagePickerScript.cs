using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//NOTE: we're using LukeWaffel.AndroidGallery, without this it won't work
using LukeWaffel.AndroidGallery;

public class DemoScript : MonoBehaviour {

	[Header ("Refrences")]
	public Image frame;

    public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//This function is called by the Button
	public void OpenGalleryButton(){

        text.text += "OpenGalleryButton() called!";

        if (frame.sprite)
        {
            text.text += "not having any sprite but still showing this msg. somethingis wrong!";
        }
        if (!frame.sprite)
        {
            text.text += "not having any sprite before loading! let hope for the best after load!";
        }

        //NOTE: we're using LukeWaffel.AndroidGallery (As seen at the top of this script), without this it won't work

        //This line of code opens the Android image picker, the parameter is a callback function the AndroidGallery script will call when the image has finished loading
        
        try
        {
            AndroidGallery.Instance.OpenGallery(ImageLoaded);
        }
        catch (AndroidJavaException e)
        {
            text.text += e.Message;
        }

    }

	//This is the callback function we created
	public void ImageLoaded(){

		//You can put anything in the callback function. You can either just grab the image, or tell your other scripts the custom image is available

		text.text += "The image has succesfully loaded!";
        try
        {
            frame.sprite = AndroidGallery.Instance.GetSprite();
            if (frame.sprite) {
                text.text += "Sprite is set successfully!";
            }

            frame.material.mainTexture = AndroidGallery.Instance.GetTexture();

            AndroidGallery.Instance.ResetOutput();
        }
        catch (UnityException e) {
            text.text += e.Message;
        }

	}

	//This function exits the app
	public void Exit(){
		Application.Quit ();
	}
}

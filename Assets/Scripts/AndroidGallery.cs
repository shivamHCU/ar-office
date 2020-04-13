using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LukeWaffel.AndroidGallery{

	public class AndroidGallery : Singleton<AndroidGallery>{

		public delegate void OnImageLoadedCallback();
		private OnImageLoadedCallback callback;

		private Sprite loadedSprite;
		private Texture loadedTexture;

		private WWW imageWWW;

		// Use this for initialization
		void Start () {

			gameObject.name = "AndroidGallery";

		}
		
		// Update is called once per frame
		void Update () {

			//Here we check if we're currently loading an image, and wether it's done
			if (imageWWW != null && imageWWW.isDone)
				//If we are, and we're done, we call the OnImageLoaded function
				OnImageLoaded ();
		}

		//This function opens the gallery
		public void OpenGallery(OnImageLoadedCallback Callback){

			//First we check if we're on Android
			if(Application.platform == RuntimePlatform.Android){

				//If we are, we set our callback variable
				callback = Callback;

				//We create the required JavaClass and JavaObject
				AndroidJavaClass javaClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				AndroidJavaObject javaObject = new AndroidJavaObject ("com.lukewaffel.androidgallery.UnityBinder");

				//We open the image picker
				javaObject.CallStatic ("OpenGallery", javaClass.GetStatic<AndroidJavaObject> ("currentActivity"));
			

			}else{
				//If we're not on Android, we log an error
				Debug.LogError("AndroidGallery can only be used in Android. Because of the dependancy of the Android OS it can NOT be tested in the editor.");
			}


		}

		//This function can be used to reset the loaded files
		public void ResetOutput(){

			//We set both the loaded texture and Sprite to null
			loadedTexture = null;
			loadedSprite = null;

		}

		//This function can be used to retrieve the loaded texture
		public Texture GetTexture(){

			//We simply return the loaded texture
			return loadedTexture;

		}

		//This function can be used to retrieve the loaded Sprite
		public Sprite GetSprite(){

			//We simple return the loaded Sprite
			return loadedSprite;

		}

		//This function is called when the selected image is done loaded
		public void OnImageLoaded(){

			//We first set our loaded texture variable to the texture of the loaded WWW
			loadedTexture = imageWWW.texture;

			//We then set our loaded Sprite variable to a Sprite we create using the loaded WWW
			loadedSprite = Sprite.Create (imageWWW.texture, new Rect (0, 0, imageWWW.texture.width, imageWWW.texture.height), new Vector2 (0, 0));

			//We reset the WWW (Without this you can only load one image)
			imageWWW = null;

			//We call our callback and reset it
			callback ();
			callback = null;

		}

		//This function is called by the Android plugin when the user selects and image
		public void OnImageSelect(string path){

			//We create a new WWW to load the selected file
			imageWWW = new WWW ("file://" + path);
			
		}

	}
}

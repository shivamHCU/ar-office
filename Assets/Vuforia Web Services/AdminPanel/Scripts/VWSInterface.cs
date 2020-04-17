using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.Video;

public class VWSInterface : MonoBehaviour 
{
	public Transform TargetListContent;
	public GameObject TargetListPrefab;
	[Space]
	public Transform LogPanelContent;
	public GameObject LogMessagePrefab;
	public Scrollbar LogPanelScroll;
	[Space]
	public Text DatabaseTitle;
	public InputField DatabaseAccessField;
	public InputField DatabaseSecretField;
	[Space]
	public InputField TargetIDField;
	public InputField TargetNameField;
	public InputField TargetWidthField;
	public Toggle TargetFlagToggle;
	public InputField TargetMetaField;
	public Image TargetImage;
    [Space]
    public Image AugmentationImage;
    public VideoPlayer AugmentationVideo;
    

    void Start()
    {
        //LoadDatabaseCredentials ();
        ConnectToDatabase();

    }

    //void LoadDatabaseCredentials ()
    //{
    //	if (string.IsNullOrEmpty(VWS.Instance.accessKey) && PlayerPrefs.HasKey("accessKey"))
    //	{
    //           VWS.Instance.accessKey = PlayerPrefs.GetString("accessKey");
    //           DatabaseAccessField.text = VWS.Instance.accessKey;
    //       }

    //	if (string.IsNullOrEmpty(VWS.Instance.secretKey) && PlayerPrefs.HasKey("secretKey"))
    //	{
    //           VWS.Instance.secretKey = PlayerPrefs.GetString("secretKey");
    //           DatabaseSecretField.text = VWS.Instance.secretKey;
    //       }

    //	ConnectToDatabase ();
    //}

    public void ConnectToDatabase()
    {
        //VWS.Instance.accessKey = DatabaseAccessField.text;
        //VWS.Instance.secretKey = DatabaseSecretField.text;

        //PlayerPrefs.SetString("accessKey", DatabaseAccessField.text);
        //PlayerPrefs.SetString("secretKey", DatabaseSecretField.text);
        //PlayerPrefs.Save();

        LogMessage("Requesting database summary...");
        VWS.Instance.RetrieveDatabaseSummary(response =>
        {
            if (response.result_code == "Success")
            { 

                string log = "Name: " + response.name + "\n";
                log += "Active images: " + response.active_images + "\n";
                log += "Failed images: " + response.failed_images + "\n";
                log += "Inactive images: " + response.inactive_images;

                LogMessage(log);

                LoadTargetList();
            }
            else
            {
                LogMessage(response.result_code);
            }
        }
        );
    }

    public void ConnectToDatabaseRuntime()
    {
        VWS.Instance.accessKey = DatabaseAccessField.text;
        VWS.Instance.secretKey = DatabaseSecretField.text;

        //PlayerPrefs.SetString("accessKey", DatabaseAccessField.text);
        //PlayerPrefs.SetString("secretKey", DatabaseSecretField.text);
        //PlayerPrefs.Save();

        LogMessage("Requesting database summary...");
        VWS.Instance.RetrieveDatabaseSummary(response =>
        {
            if (response.result_code == "Success")
            {
                DatabaseTitle.text = response.name;

                string log = "Name: " + response.name + "\n";
                log += "Active images: " + response.active_images + "\n";
                log += "Failed images: " + response.failed_images + "\n";
                log += "Inactive images: " + response.inactive_images;

                LogMessage(log);

                LoadTargetList();
            }
            else
            {
                LogMessage(response.result_code);
            }
        }
        );
    }

    public void LoadTargetList ()
	{
		LogMessage("Requesting target list...");
		VWS.Instance.RetrieveTargetList( response =>
			{
				if (response.result_code == "Success")
				{
					RebuildTargetList(response.results);
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}


    void RebuildTargetList(string[] targets)
    {
        foreach (Transform tr in TargetListContent)
        {
            Destroy(tr.gameObject);
        }

        foreach (var targetID in targets)
        {
            GameObject btnObject = Instantiate(TargetListPrefab, TargetListContent);
            btnObject.transform.localScale = Vector3.one;

            btnObject.GetComponentInChildren<Text>().text = targetID;
            SetTargetNameInMainlist(btnObject.GetComponentInChildren<Text>(), targetID);

            string target = targetID;
            btnObject.GetComponent<Button>().onClick.AddListener(() => UpdateTargetIDField(target));
        }
    }



    public void SetTargetNameInMainlist(Text listItem, string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            LogMessage("Please specify target id");
            return;
        }

        VWS.Instance.RetrieveTarget(id, response =>
        {
            if (response.result_code == "Success")
            {
                listItem.text = response.target_record.name;
            }
            else
            {
                LogMessage(response.result_code);
            }
        }
        );

    }



    public void RetrieveTarget()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Requesting target record...");
		VWS.Instance.RetrieveTarget(TargetIDField.text, response =>
			{
				if (response.result_code == "Success")
				{
					TargetNameField.text = response.target_record.name;
					TargetWidthField.text = response.target_record.width.ToString();
					TargetFlagToggle.isOn= response.target_record.active_flag;

					string log = "Name: " + response.target_record.name + "\n";
					log += "Width: " + response.target_record.width + "\n";
					log += "Active: " + response.target_record.active_flag + "\n";
					log += "Rating: " + response.target_record.tracking_rating;

					LogMessage(log);
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void RetrieveTargetSummary()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Requesting target summary...");
		VWS.Instance.RetrieveTargetSummary(TargetIDField.text, response =>
			{
				if (response.result_code == "Success")
				{
					string log = "DB Name: " + response.database_name + "\n";
					log += "Name: " + response.target_name + "\n";
					log += "Date: " + response.upload_date + "\n";
					log += "Status: " + response.status + "\n";
					log += "Total Recos: " + response.total_recos + "\n";
					log += "Current Month: " + response.current_month_recos + "\n";
					log += "Previous Month: " + response.previous_month_recos;

					LogMessage(log);
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}
		
	public void RetrieveTargetDuplicates()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Requesting target duplicates...");
		VWS.Instance.RetrieveTargetDuplicates(TargetIDField.text, response =>
			{
				if (response.result_code == "Success")
				{
					RebuildTargetList(response.similar_targets);
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void AddTarget ()
	{
		LogMessage("Creating new target...");
		VWS.Instance.AddTarget(TargetNameField.text,
			float.Parse(TargetWidthField.text),
			TargetImage.sprite.texture,
			TargetFlagToggle.isOn,
			TargetMetaField.text,
			response =>
			{
				if (response.result_code == "Success" || response.result_code == "TargetCreated")
				{
					TargetIDField.text = response.target_id;
					string log = "New Target ID: " + response.target_id;
					LogMessage(log);
					LoadTargetList();
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void DeleteTarget ()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Deleting target...");
		VWS.Instance.DeleteTarget(TargetIDField.text,
			response =>
			{
				if (response.result_code == "Success")
				{
					LogMessage("Target deleted");
					LoadTargetList();
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void UpdateTarget ()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Updating target data...");
		VWS.Instance.UpdateTarget(TargetIDField.text,
			TargetNameField.text,
			float.Parse(TargetWidthField.text),
			TargetImage.sprite.texture,
			TargetFlagToggle.isOn,
			TargetMetaField.text,
			response =>
			{
				if (response.result_code == "Success")
				{
					LogMessage("Target updated");
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void UpdateTargetName ()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Updating target name...");
		VWS.Instance.UpdateTargetName(TargetIDField.text,
			TargetNameField.text,
			response =>
			{
				if (response.result_code == "Success")
				{
					LogMessage("Target name updated");
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void UpdateTargetWidth ()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Updating target width...");
		VWS.Instance.UpdateTargetWidth(TargetIDField.text,
			float.Parse(TargetWidthField.text),
			response =>
			{
				if (response.result_code == "Success")
				{
					LogMessage("Target width updated");
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void UpdateTargetFlag ()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Updating target flag...");
		VWS.Instance.UpdateTargetFlag(TargetIDField.text,
			TargetFlagToggle.isOn,
			response =>
			{
				if (response.result_code == "Success")
				{
					LogMessage("Target flag updated");
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void UpdateTargetMeta ()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Updating target metadata...");
		VWS.Instance.UpdateTargetMetadata(TargetIDField.text,
			TargetMetaField.text,
			response =>
			{
				if (response.result_code == "Success")
				{
					LogMessage("Target metadata updated");
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}

	public void UpdateTargetImage ()
	{
		if(string.IsNullOrEmpty(TargetIDField.text))
		{
			LogMessage("Please specify target id");
			return;
		}

		LogMessage("Updating target image...");
		VWS.Instance.UpdateTargetImage(TargetIDField.text,
			TargetImage.sprite.texture,
			response =>
			{
				if (response.result_code == "Success")
				{
					LogMessage("Target image updated");
				}
				else 
				{
					LogMessage(response.result_code);
				}
			}
		);
	}


    public void UpdateAugmentation()
    {
        if (string.IsNullOrEmpty(TargetIDField.text))
        {
            LogMessage("Please specify target id");
            return;
        }

        LogMessage("Updating Augmentation on Web Server...");
        /*
        VWS.Instance.UpdateTargetImage(TargetIDField.text,
            TargetImage.sprite.texture,
            response =>
            {
                if (response.result_code == "Success")
                {
                    LogMessage("Target image updated");
                }
                else
                {
                    LogMessage(response.result_code);
                }
            }
        );
        */
    }


    public void ClearLog()
	{
		foreach (Transform tr in LogPanelContent)
		{
			Destroy(tr.gameObject);
		}
	}

	public void PickImage()
	{
		TargetImage.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
	}

    public void OpenGalleryButton(Image TargetImg)
    {
        if (AugmentationVideo.isPlaying)
        {
            AugmentationVideo.Pause();
        }
        Texture2D texture = new Texture2D(1, 1);
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            LogMessage("Path :- " + path);
            if (path != null)
            {
                texture = NativeGallery.LoadImageAtPath(path);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
                TargetImg.sprite = sprite;
            }
        });

        Destroy(texture);

    }

    public void PickVideo()
    {
        if (AugmentationImage.sprite != null) {
            AugmentationImage.sprite = null;
        }

        //AugmentationVideo.url = "file://" + "C:/SenkathirSelvan.mp4";

        if (TargetFlagToggle.isOn) {
            NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
            {
                if (path != null)
                {
                    AugmentationVideo.url = "file://" + path;
                    LogMessage("AugmentationVideo.url =" + "file://" + path);
                    //Handheld.PlayFullScreenMovie("file://" + path);
                }
            }, "Select a video");

            LogMessage("Permission result: " + permission);
        }
        else
        {
            AugmentationVideo.url = TargetIDField.text;
        }
    }




    void UpdateTargetIDField (string targetID)
	{
		TargetIDField.text = targetID;
		RetrieveTarget();
	}

	void LogMessage(string message)
	{
		GameObject btnObject = Instantiate(LogMessagePrefab);
		btnObject.GetComponentInChildren<Text>().text = message;
		btnObject.transform.SetParent(LogPanelContent);
		btnObject.transform.localScale = Vector3.one;
		LogPanelScroll.value = 0f;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;

public class VWSInterface : MonoBehaviour 
{
	public Transform TargetListContent;
	public GameObject TargetListPrefab;
	[Space]
	public Transform LogPanelContent;
	public GameObject LogMessagePrefab;
	public Scrollbar LogPanelScroll;
	[Space]
    public InputField TargetIDField;
    public InputField TargetNameField;
	public InputField TargetWidthField;
	public Toggle TargetFlagToggle;
    [Space]
    public InputField AugLinkField;
    public InputField NoOfQuestions;
    public InputField Domain;
    public InputField TargetMetaField;
    [Space]
    public Image TargetImage;
    [Space]
    public Image AugmentationImage;
    public VideoPlayer AugmentationVideo;
    public Text AugmentationQuizDetails;
    [Space]
    public GameObject ImagePicker;
    [Space]
    public InputField Question;
    public InputField Questiondomain;
    public InputField Option1;
    public InputField Option2;
    public InputField Option3;
    public InputField Option4;
    public InputField CorrectOption;
    public Text QuizAddMsg;

    string username; 

    void Start()
    {
        username = Login.getUsername() + "-";
        ConnectToDatabase();
    }

    public void ConnectToDatabase()
    {
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
            
            VWS.Instance.RetrieveTarget(targetID, response =>
            {
                if (response.result_code == "Success")
                {
                    string name = response.target_record.name;
                    string[] splitedName = name.Split('-');
                    if (splitedName[0].Equals(username.Substring(0, username.Length - 1)))
                    {
                        GameObject btnObject = Instantiate(TargetListPrefab, TargetListContent);
                        btnObject.transform.localScale = Vector3.one;
                        btnObject.GetComponentInChildren<Text>().text = name.Substring(username.Length);
                        string target = targetID;
                        btnObject.GetComponent<Button>().onClick.AddListener(() => UpdateTargetIDField(target));
                    }
                }
                else
                {
                    LogMessage(response.result_code);
                }
            });   
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

    public string GenerateMetaData() {
        int type = ToggleControl.SelectedAug();
        string metadata = "" + type;
        if (type == 1 || type == 2)
        {
            if (!AugLinkField.text.Equals(""))
            {
                if (type == 1)
                {
                    if (!AugLinkField.text.EndsWith(".jpg") && !AugLinkField.text.EndsWith(".png") && !AugLinkField.text.EndsWith(".jpeg"))
                    {
                        LogMessage("Please provide a downloadable link of image of type (jpg/jpeg/png)");
                        return null;
                    }
                }
                else
                {
                    if (!AugLinkField.text.EndsWith(".mp4") && !AugLinkField.text.EndsWith(".mkv"))
                    {
                        LogMessage("Please provide a downloadable link of video of type (.mp4/.mkv)");
                        return null;
                    }
                }
                metadata += " " + AugLinkField.text;
                Debug.Log(metadata);
            }
            else
            {

                string errormsg = "";
                if (type == 1)
                {
                    errormsg += "Please specify link of Image";
                }
                else
                {
                    errormsg += "Please specify link of Video";
                }
                LogMessage(errormsg);
                return null;
            }
        }
        else
        {
            if (NoOfQuestions.text.Equals(""))
            {
                LogMessage("Please Enter the No of Questions!");
                return null;
            }
            else
            {
                metadata += " " + NoOfQuestions.text;
            }

            if (Domain.text.Equals(""))
            {
                LogMessage("Please Enter the Domain!");
                return null;
            }
            else
            {
                metadata += " " + username + Domain.text;
            }
        }

        return metadata;
    }

    public void SetMetaData()
    {
        TargetMetaField.text = GenerateMetaData();
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
					TargetNameField.text = response.target_record.name.Substring(username.Length);
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
        string metadata = GenerateMetaData();
        LogMessage("Creating new target...");

        if (metadata == null)
        {
            LogMessage("Not Created! Make sure your entered the meta data for target!");
            return;
        }
        TargetMetaField.text = metadata;

        VWS.Instance.AddTarget(username+TargetNameField.text,
			float.Parse(TargetWidthField.text),
			TargetImage.sprite.texture,
			TargetFlagToggle.isOn,
			metadata,
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

        string metadata = GenerateMetaData();
        
        if (metadata == null)
        {
            LogMessage("Not Updated! Make sure your entered the meta data for target Correctly!");
            return;
        }

        TargetMetaField.text = metadata;

        VWS.Instance.UpdateTarget(TargetIDField.text,
			username+TargetNameField.text,
			float.Parse(TargetWidthField.text),
			TargetImage.sprite.texture,
			TargetFlagToggle.isOn,
			metadata,
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
			username+TargetNameField.text,
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

        SetMetaData();
        string metadata = GenerateMetaData();
        
        if (metadata == null)
        {
            LogMessage("Metadata not Updated! Make sure your entered the metadata correctly!");
            return;
        }

        TargetMetaField.text = metadata;

        VWS.Instance.UpdateTargetMetadata(TargetIDField.text,
			metadata,
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

        SceneManager.LoadScene("CloudRecoScene");
	}

	public void PickImage()
	{
		AugmentationImage.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
	}

    public void OpenGalleryButton(Image TargetImg)
    {
        #if UNITY_EDITOR
            if (!ImagePicker.activeSelf)
            {
                ImagePicker.SetActive(true);
            }
        #endif
        #if UNITY_ANDROID
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
        #endif
    }


    public void AugmentationPreview()
    {
        int AugCode = ToggleControl.SelectedAug();
        if (AugCode == 1)
        {
            ImagePreview();
        }
        else if (AugCode == 2) {
            VideoPreview();
        }
        else{
            QuizDBPreview();
        }
        
    }


    public void ImagePreview()
    {
        if (AugmentationVideo.isPlaying)
        {
            AugmentationVideo.Pause();
        }
        if (AugLinkField.text != "") {
            Debug.Log("Calling the coroutine with" + AugLinkField.text + "Link!");
            StartCoroutine(loadSpriteImageFromUrl(AugLinkField.text));
        }
        else
        {
            LogMessage("Please specify the link!");
        }
    }

    IEnumerator loadSpriteImageFromUrl(string URL)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            LogMessage(www.error);
        }
        else
        {
            Debug.Log("Connecting Done!");
            Texture2D texture = new Texture2D(1, 1);
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Debug.Log("Textture Downloaded!");
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
            AugmentationImage.sprite = sprite;

        }
    }


    public void VideoPreview()
    {
        if (AugmentationImage.sprite != null) {
            AugmentationImage.sprite = null;
        }
        AugmentationVideo.url = AugLinkField.text;
        AugmentationVideo.Play();
    }


    public void QuizDBPreview() {


        if (AugmentationVideo.isPlaying)
        {
            AugmentationVideo.Pause();
        }

        if (Domain.text != null) {
            StartCoroutine(QuizSummary());
        }
    }

    IEnumerator QuizSummary()
    {
        WWWForm form = new WWWForm();
        form.AddField("domain", username + Domain.text);
        UnityWebRequest www = UnityWebRequest.Post("http://shivamgangwar.000webhostapp.com/quiz/domainSummary.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            LogMessage(www.downloadHandler.text);
        }
        else
        {
            if (!Domain.text.Equals("ALL"))
            {
                if (www.downloadHandler.text.Equals("0"))
                {
                    AugmentationQuizDetails.text = "No such Domain Found!\nSearch with a input as \"ALL\" in InputField !!";
                }
                else
                {
                    string summary = "Domain Name : " + Domain.text + "\n";
                    summary += "Total No of question for this domain present :" + www.downloadHandler.text;
                    AugmentationQuizDetails.text = summary;
                }
            }
            else
            {
                string response = www.downloadHandler.text;
                string[] domainList = response.Split(';');
                Debug.Log("\n 0 -" + response);
                string printableDomain = "";
                foreach (string s in domainList) {
                    if (!s.Equals(""))
                    {
                        printableDomain += s.Substring(username.Length);
                        printableDomain += "\n";
                    }
                }
                AugmentationQuizDetails.text = printableDomain;
            }
        }
    }


    void ShowQuizAddMsg(string s, Color col)
    {
        QuizAddMsg.text = s;
        QuizAddMsg.color = col;
        StartCoroutine(RemoveAfterSeconds(3, QuizAddMsg));
    }

    IEnumerator RemoveAfterSeconds(int seconds, Text obj)
    {
        yield return new WaitForSeconds(seconds);
        obj.text = "";
        QuizAddMsg.color = Color.black;
    }

    public void AddNewQuestion()
    {
        StartCoroutine(addQuiz());
    }

    IEnumerator addQuiz()
    {
        WWWForm form = new WWWForm();
        form.AddField("question", Question.text);
        form.AddField("domain", username+Questiondomain.text);
        form.AddField("option1", Option1.text);
        form.AddField("option2", Option2.text);
        form.AddField("option3", Option3.text);
        form.AddField("option4", Option4.text);
        form.AddField("correctOption", CorrectOption.text);
        
        if (Question.text.Length>0 && Questiondomain.text.Length > 0 && Option1.text.Length > 0 && Option2.text.Length > 0 && Option3.text.Length > 0 && Option4.text.Length > 0 && CorrectOption.text.Length > 0)
        {
            UnityWebRequest www = UnityWebRequest.Post("https://shivamgangwar.000webhostapp.com/quiz/addQues.php", form);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            { 
                ShowQuizAddMsg("[Server Error] Unable to add Question!", Color.red);
            }
            else if (www.downloadHandler.text.Equals("0"))
            {
                ShowQuizAddMsg("Question added sucessfully!", Color.green);
                Question.text = "";
                Option1.text = "";
                Option2.text = "";
                Option3.text = "";
                Option4.text = "";
                CorrectOption.text = "";
            }
            else if (www.downloadHandler.text.Equals("1"))
            {
                ShowQuizAddMsg("[Repeated Question] Question already Exist!", Color.red);
            }
            else
            {
                ShowQuizAddMsg(www.downloadHandler.text + "[Insert Query Failed] Unable to add Question!", Color.red);
            }
        }
        else
        {
            ShowQuizAddMsg("Please fill all the fields!", Color.red);
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

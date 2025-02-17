﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.Video;
using System;

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
    public Dropdown DomainDropDown;
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
    public Dropdown AddDomainDropDown;
    public InputField Questiondomain;
    public InputField Option1;
    public InputField Option2;
    public InputField Option3;
    public InputField Option4;
    public InputField CorrectOption;
    public Text QuizAddMsg;
    [Space]
    public Transform QuestionListContent;
    public GameObject QuestionListPrefeb;
    public Text SummaryDomainName;
    public Text SummaryNoOfQuestion;
    public Text SummaryQuestionPreview;
    public Text SummaryQuestionId;
    public GameObject QuesSummaryPanel;
    
    string username;
    
    void Start()
    {
        username = Login.getUsername() + "-";
        ConnectToDatabase();
        StartCoroutine(LoadDomainListFromDB());
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


    /*
     * #### Future Implimentation ####
     * Functions to Deal With all DB Question (@QuizDomainSummaryPanel)
     * 1. Add New Question (Done)
     * 2. Load Question List
     * 3. Update Question
     * 4. Delete Question
     * 5. Delete Domain
     * 6. Clear Summary data
     */

    public class QuestionClass      //Create Question Class that will make it easier to contain all questions has variables :- Questions, no of options, options , correct option number out of 4 maximum choices, Domain of the question.

    {
        public int id;
        public string ques;
        public int noOfOpt;
        public string[] options;
        public int correctOptionNo;
        public string domain;

        public QuestionClass(int id, string ques, int noOfOpt, string[] opt, int corrOptNo, string domain)
        {
            this.id = id;
            this.ques = ques;
            this.noOfOpt = noOfOpt;
            options = opt;
            correctOptionNo = corrOptNo;
            this.domain = domain;
        }
    }

    public void LoadQuestionList()
    {
        StartCoroutine(LoadQuestionFromDatabase());
    }

    IEnumerator LoadQuestionFromDatabase()
    {
        string link = "https://shivamgangwar.000webhostapp.com/quiz/fetchAll.php"; //link to quiz database and get all the data there
        string quesdomain = DomainDropDown.options[DomainDropDown.value].text;
        // Create a form object for domain and noOfQuestion to the server
        WWWForm form = new WWWForm();
        form.AddField("domain", username+quesdomain);
        // Create a download object
        var download = UnityWebRequest.Post(link, form);
        // Wait until the download is done
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            LogMessage("Error downloading: " + download.error);
        }
        else
        {
            string downloadString = download.downloadHandler.text;
            string[] dstr = downloadString.Split(';');
            int totalNoOfQuestions = Int32.Parse(dstr[0]);
            QuestionClass[] Questions = new QuestionClass[totalNoOfQuestions]; // create an array of objects of question class  according to the total no of questions
            for (int i = 1; i <= totalNoOfQuestions; i++)  //populate the array through the loop
            {
                string x = dstr[i];
                string[] quesset = x.Split(',');
                int id = Int32.Parse(quesset[0]);
                int nop = Int32.Parse(quesset[2]);
                int corop = Int32.Parse(quesset[7]);
                Questions[i-1] = new QuestionClass(id, quesset[1], nop, new string[4] { quesset[3], quesset[4], quesset[5], quesset[6] }, corop, quesset[8]);  //Generate all Questions and populate the list one by one
            }
            RebuildQuestionList(Questions);
        }
    }

    public void DeleteQuestion()
    {
        StartCoroutine(DeleteQuestionFromDatabase());
    }


    IEnumerator DeleteQuestionFromDatabase()
    {
        string link = "https://shivamgangwar.000webhostapp.com/quiz/DeleteWithId.php";
        // Create a form object for domain and noOfQuestion to the server
        WWWForm form = new WWWForm();
        string s = SummaryQuestionId.text;
        if (s.Equals(""))
        {
            SummaryQuestionPreview.text = "Please Select a question!";
            SummaryQuestionPreview.color = Color.red;
            StartCoroutine(RemoveAfterSeconds(3, SummaryQuestionPreview));

        }
        else
        {
            form.AddField("quesid", Int32.Parse(SummaryQuestionId.text));
            // Create a download object
            var download = UnityWebRequest.Post(link, form);
            // Wait until the download is done
            yield return download.SendWebRequest();

            if (download.isNetworkError || download.isHttpError)
            {
                LogMessage("Error downloading: " + download.error);
            }
            else
            {
                LoadQuestionList();
                QuizDBPreview();
                SummaryQuestionPreview.text = "";
                SummaryQuestionId.text = "";
            }
        }
    }


    public void DeleteDomain()
    {
        StartCoroutine(DeleteDomainFromDatabase());
    }


    IEnumerator DeleteDomainFromDatabase()
    {
        string link = "https://shivamgangwar.000webhostapp.com/quiz/DeleteWithDomainName.php";
        // Create a form object for domain and noOfQuestion to the server
        WWWForm form = new WWWForm();
        
        form.AddField("domain", username+SummaryDomainName.text);
        // Create a download object
        var download = UnityWebRequest.Post(link, form);
        // Wait until the download is done
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            LogMessage("Error downloading: " + download.error);
        }
        else
        {
            StartCoroutine(LoadDomainListFromDB());
            AddDomainDropDown.value = 0;
            DomainDropDown.value = 0;
            SummaryDomainName.text = "--";
            SummaryNoOfQuestion.text = "--";
            SummaryQuestionPreview.text = "";
            SummaryQuestionId.text = "";
            foreach (Transform tr in TargetListContent)
            {
                Destroy(tr.gameObject);
            }
            QuesSummaryPanel.SetActive(false);
        }

    }


    void RebuildQuestionList(QuestionClass[] questions)
    {
        foreach (Transform tr in QuestionListContent)
        {
            Destroy(tr.gameObject);
        }

        foreach (QuestionClass targetQues in questions)
        {
            GameObject btnObject = Instantiate(QuestionListPrefeb, QuestionListContent);
            btnObject.transform.localScale = Vector3.one;
            btnObject.GetComponentInChildren<Text>().text = targetQues.ques;
            btnObject.GetComponent<Button>().onClick.AddListener(() => UpdateQuesPreviewField(targetQues));
        }
    }
    void UpdateQuesPreviewField(QuestionClass ques)
    {
        SummaryQuestionPreview.text = ques.ques;
        SummaryQuestionId.text = ques.id.ToString();
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
                    if (splitedName[0].Equals(username.Substring(0, username.Length - 1)))   //If the Login Username matches the username written in the name field of the target  then show them else skip
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
        if (type == 1 || type == 2)     //Type 1 corresponds to Images and Type 2 corresponds to Videos while Type 3 Corresponds to Quiz
        {
            if (!AugLinkField.text.Equals(""))
            {
                if (type == 1)
                {
                    if (!AugLinkField.text.EndsWith(".jpg") && !AugLinkField.text.EndsWith(".png") && !AugLinkField.text.EndsWith(".jpeg"))    //check if the image ends with .jpeg or .png
                    {
                        LogMessage("Please provide a downloadable link of image of type (jpg/jpeg/png)");
                        return null;
                    }
                }
                else
                {
                    if (!AugLinkField.text.EndsWith(".mp4") && !AugLinkField.text.EndsWith(".mkv"))     // check if tvideo ends with .mkv or .mp4
                    {
                        LogMessage("Please provide a downloadable link of video of type (.mp4/.mkv)");
                        return null;
                    }
                }
                metadata += " " + AugLinkField.text;  //SetMetaData Metadata accordingly
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
            string SelectedDomain = DomainDropDown.options[DomainDropDown.value].text;
            if (SelectedDomain.Equals("Select Domain"))
            {
                LogMessage("Please Select the Domain!");
                return null;
            }
            else
            {
                metadata += " " + username + SelectedDomain;
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
				if (response.result_code == "Success")  //If successful get various details and display in the panel
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

        VWS.Instance.AddTarget(username+TargetNameField.text,  //Add target using the given funtion 
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
            LogMessage("Not Updated! Make sure your entered the metadata for target Correctly!");
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

    //The following functions modify a single value of the whole admin panel like the target's name, width, image or metadata
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

    public void ClearLog()
	{
		foreach (Transform tr in LogPanelContent)
		{
			Destroy(tr.gameObject);
		}
	}
    
    public void PickImage()  //image picker this will initialize when we press the top right square.
    {
		AugmentationImage.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
	}
    //make a duplicate texture so that it may be accessed from a code and will be easily resized or reformated
    Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear); //make a temporary texture of the source height and width

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
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
                if (path != null)
                {
                    texture = NativeGallery.LoadImageAtPath(path);
                    Texture2D ReadableTexture = DuplicateTexture(texture);
                    Sprite sprite = Sprite.Create(ReadableTexture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100); // create a sprite with the data of the downloaded image 
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

        string SelectedDomain = DomainDropDown.options[DomainDropDown.value].text;

        if (SelectedDomain.Equals("Select Domain")) {
            LogMessage("Select or Add a new Domain.");
        }
        else {
            StartCoroutine(QuizSummary(SelectedDomain));
        }
    }

    IEnumerator QuizSummary(string str)
    {
        WWWForm form = new WWWForm();
        form.AddField("domain", username + str);
        UnityWebRequest www = UnityWebRequest.Post("https://shivamgangwar.000webhostapp.com/quiz/domainSummary.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            LogMessage(www.downloadHandler.text);
        }
        else
        {
            if (www.downloadHandler.text.Equals("0"))
            {
                LogMessage("Network/Server error ");
            }
            else
            {
                SummaryDomainName.text = str;
                SummaryNoOfQuestion.text = www.downloadHandler.text;
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
        obj.color = Color.black;
    }

    public void AddNewQuestion()
    {
        StartCoroutine(AddQuizQues());
    }

    public void CheckDomainDropDownData() {
        string DDvalue = AddDomainDropDown.options[AddDomainDropDown.value].text;
        if(DDvalue.Equals("Add New Domain"))
        {
            Questiondomain.gameObject.SetActive(true);
        }
        else
        {
            if (Questiondomain.gameObject.activeSelf)
            {
                Questiondomain.gameObject.SetActive(false);
            }
        }

    }

    IEnumerator LoadDomainListFromDB()
    {
        string url = "https://shivamgangwar.000webhostapp.com/quiz/getDomainList.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            LogMessage(www.downloadHandler.text);
        }
        else
        {
            string downloadedText = www.downloadHandler.text;
            if (!downloadedText.Equals("0")) {
                string data = www.downloadHandler.text;
                string itemsDataString = data.Substring(1);
                string[] wwwItems = itemsDataString.Split(';'); //the values in mysql db is picked in a single string and should have some marker so as to split
                AddDomainDropDown.options.Clear();
                DomainDropDown.options.Clear();
                DomainDropDown.options.Add(new Dropdown.OptionData("Select Domain"));
                foreach (string str in wwwItems)
                { 
                    if (!str.Equals("") && !str.Equals("0"))
                    {
                        string value = str.Substring(username.Length);
                        AddDomainDropDown.options.Add(new Dropdown.OptionData(value));
                        DomainDropDown.options.Add(new Dropdown.OptionData(value));
                    }
                }
                AddDomainDropDown.options.Add(new Dropdown.OptionData("Add New Domain"));
                if(AddDomainDropDown.options.Count > 1)
                {
                    Questiondomain.gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator AddQuizQues()
    {
        string domain = AddDomainDropDown.options[AddDomainDropDown.value].text;
        if (Questiondomain.gameObject.activeSelf)
        {
            domain = Questiondomain.text;
        }

        WWWForm form = new WWWForm();
        form.AddField("question", Question.text);
        form.AddField("domain", username+domain);
        form.AddField("option1", Option1.text);
        form.AddField("option2", Option2.text);
        form.AddField("option3", Option3.text);
        form.AddField("option4", Option4.text);
        form.AddField("correctOption", CorrectOption.text);
        
        if (Question.text.Length>0 && domain.Length > 0 && Option1.text.Length > 0 && Option2.text.Length > 0 && Option3.text.Length > 0 && Option4.text.Length > 0 && CorrectOption.text.Length > 0)
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
                if (Questiondomain.gameObject.activeSelf)
                {
                    StartCoroutine(LoadDomainListFromDB());
                }
                AddDomainDropDown.value = 0;
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

    public void openURL()
    {
        Application.OpenURL("https://shivamgangwar.000webhostapp.com/uploadmedia/upload.php");
    }

    void LogMessage(string message)
	{
		GameObject btnObject = Instantiate(LogMessagePrefab);
		btnObject.GetComponentInChildren<Text>().text = message;
		btnObject.transform.SetParent(LogPanelContent);
		btnObject.transform.localScale = Vector3.one;
		LogPanelScroll.value = 0f;
	}

    public void Logout()
    {
        username = "";
        Login.setUsername("");
        SceneManager.LoadScene("CloudRecoScene");
    }
}

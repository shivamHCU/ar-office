using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

public class UIQuestion : MonoBehaviour
{

    public int totalNoOfQuestions;
    public String domain;
    int score;
    public GameObject optionPanel;

    public class Question
    {
        public string ques;
        public int noOfOpt;
        public string[] options;
        public int correctOptionNo;
        public string domain;

        public Question(string ques, int noOfOpt, string[] opt, int corrOptNo, string domain)
        {
            this.ques = ques;
            this.noOfOpt = noOfOpt;
            options = opt;
            correctOptionNo = corrOptNo;
            this.domain = domain;
        }
    }

    private Button[] optionsBtn;
    private static Text[] optionsText;
    private static TextMeshPro questionText;
    private Question[] quesSet;
    private int questionNo;
    private string[] dstr;
    private string[] quesset;
    private string x;

    // Start is called before the first frame update
    private void Start()
    {
        score = 0;
        optionsBtn = GameObject.Find("QuestSet").GetComponentsInChildren<Button>();
        questionText = GameObject.Find("QuestSet").GetComponentInChildren<TextMeshPro>();
    }


    public IEnumerator LoadQuestionFromDatabase() {
        string link = "https://shivamgangwar.000webhostapp.com/quiz/";
        // Create a form object for domain and noOfQuestion to the server
        WWWForm form = new WWWForm();
        form.AddField("domain", domain);
        form.AddField("noOfQues", totalNoOfQuestions);

        // Create a download object
        var download = UnityWebRequest.Post(link, form);
        // Wait until the download is done
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            Debug.Log(download.downloadHandler.text);
            string downloadString = download.downloadHandler.text;

            dstr = downloadString.Split(';');
            quesSet = new Question[totalNoOfQuestions];
            questionNo = 0;
            for (int i = 0; i < totalNoOfQuestions; i++)
            {
                x = dstr[i];
                quesset = x.Split(',');
                int nop = Convert.ToInt32(quesset[2][0] - '0');
                int corop = Convert.ToInt32(quesset[7][0] - '0');
                quesSet[i] = new Question(quesset[1], nop, new string[4] { quesset[3], quesset[4], quesset[5], quesset[6] }, corop, quesset[8]);
            }

           
            /*
            optionsBtn = GameObject.Find("QuestSet").GetComponentsInChildren<Button>();
            questionText = GameObject.Find("QuestSet").GetComponentInChildren<TextMeshPro>();
            */        

            for (int i = 0; i < quesSet[questionNo].noOfOpt; i++)
            {
                optionsBtn[i].GetComponentInChildren<Text>().text = quesSet[questionNo].options[i];
            }

            if (quesSet[questionNo].noOfOpt == 2)
            {
                optionsBtn[2].GetComponentInChildren<Text>().text = "";
                optionsBtn[3].GetComponentInChildren<Text>().text = "";
            }
            
            //loading the first question
            questionText.text = quesSet[questionNo].ques;
        }
    }

    public void validateAndUpdateScore(Button button)
    {
        StartCoroutine(waitAndLoad(button));
    }


    private IEnumerator waitAndLoad(Button button)
    {
        
        string btnName = button.name;
        char lastchar = btnName[btnName.Length - 1];
        int btnNo = lastchar - '0';

        if (btnNo == quesSet[questionNo].correctOptionNo)
        {
            button.image.color = Color.green;
            score++;
            //theColor.normalColor = Color.green;//new Color(15, 205, 85);
            //button.colors = theColor;
        }
        else
        {
            button.image.color = Color.red;
            //theColor.normalColor = Color.red; //new Color(251, 50, 46);
            //button.colors = theColor;
        }

        yield return new WaitForSeconds(0.5F);

        
        if (quesSet[questionNo].noOfOpt == 2)
        {
            if (btnNo == 0 || btnNo == 1) {
                questionNo += 1;
            }
        }
        else
        {
            questionNo += 1;
        }

        if (questionNo < totalNoOfQuestions)
        {
            questionText.text = quesSet[questionNo].ques;

            for (int i = 0; i < quesSet[questionNo].noOfOpt; i++)
            {
                optionsBtn[i].GetComponentInChildren<Text>().text = quesSet[questionNo].options[i];
            }

            if (quesSet[questionNo].noOfOpt == 2)
            {
                optionsBtn[2].GetComponentInChildren<Text>().text = "";
                optionsBtn[3].GetComponentInChildren<Text>().text = "";
            }

            button.image.color = Color.gray;
        }
        else
        {
            questionText.text = "Your score is : " + score + "/" + totalNoOfQuestions;
            for (int i = 0; i < 4; i++)
            {
                optionsBtn[i].GetComponentInChildren<Text>().text = "";
                
            }
            button.image.color = Color.gray;
            optionPanel.SetActive(false);
        }
    }
}

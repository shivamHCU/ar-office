using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuestion : MonoBehaviour
{

    public class Question
    {
        public string ques;
        public int noOfOpt;
        public string[] options;
        public int correctOptionNo;


        public Question(string ques, int noOfOpt, string[] opt, int corrOptNo)
        {
            this.ques = ques;
            this.noOfOpt = noOfOpt;
            /*
              for (int i = 0; i < this.noOfOpt; i++) {
                options[i] = opt[i];
              }
            */
            options = opt;
            correctOptionNo = corrOptNo;
        }
    }

    private Button[] optionsBtn;
    private static Text[] optionsText;
    private static TextMeshPro questionText;
    private Question[] quesSet;
    private int questionNo;


    // Start is called before the first frame update
    void Start()
    {
        quesSet = new Question[5];
        questionNo = 0;

        /*Questions static objects for now later will be replaced with input from DB)*/
        quesSet[0] = new Question("When does the ArrayIndexOutOfBoundsException occur?", 4, new string[4] { "Compile-time", "Run-time", "Not an exception", "Not an error" }, 2);
        quesSet[1] = new Question("Which of the following concepts make extensive use of arrays?", 4, new string[4] { "Spatial locality", "Caching", "Process Scheduling", "Binary trees" }, 1);
        quesSet[2] = new Question("The process of accessing data stored in a serial access memory is similar to manipulating data on a __?", 4, new string[4] { "Array","heap","Tree", "Stack" }, 4);
        quesSet[3] = new Question("Efficiency of finding the next record in B+ tree is __?", 4, new string[4] { "O(log n)", "O(n log n)", "O(1)", "O(n)" }, 3);
        quesSet[4] = new Question("A single channel is shared by multiple signals by __?", 4, new string[4] { "digital modulation", "modulation", "analog modulation", "multiplexing" }, 4);

        optionsBtn = GameObject.Find("UI_Question").GetComponentsInChildren<Button>();
        questionText = GameObject.Find("UI_Question").GetComponentInChildren<TextMeshPro>();

        for (int i = 0; i < 4; i++)
        {
            optionsBtn[i].GetComponentInChildren<Text>().text = quesSet[questionNo].options[i];
        }

        //loading the first question
        questionText.text = quesSet[questionNo].ques;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //updateing the question in each frame
        

    }

    public void validateAndUpdateScore(Button button) {

        
        

        StartCoroutine(waitAndLoad(button));
    }


    private IEnumerator waitAndLoad(Button button) {

        string btnName = button.name;
        char lastchar = btnName[btnName.Length - 1];
        int btnNo = lastchar - '0';

        

        if (btnNo == quesSet[questionNo].correctOptionNo)
        {
            button.image.color = Color.green;
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

        if (questionNo < 4)
        {
            questionNo++;
        }

        questionText.text = quesSet[questionNo].ques;
        for (int i = 0; i < 4; i++)
        {
            optionsBtn[i].GetComponentInChildren<Text>().text = quesSet[questionNo].options[i];
        }
        
        button.image.color = Color.gray;
    }
}

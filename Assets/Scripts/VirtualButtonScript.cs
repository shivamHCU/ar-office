using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using TMPro;

public class VirtualButtonScript : MonoBehaviour, IVirtualButtonEventHandler
{

    //public Text msgBox;

    //private float delay = 5.0f;
    //private float startTime = 0.0f;
    //private bool increaseTime = false;

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

    private GameObject defaultOption1Indicator;
    private GameObject defaultOption2Indicator;
    private GameObject rightOption1Indicator;
    private GameObject rightOption2Indicator;
    private GameObject wrongOption1Indicator;
    private GameObject wrongOption2Indicator;

    private GameObject[] nextButtonGroup;
    private VirtualButtonBehaviour[] vrb;

    private Question[] quesSet;
    public TextMeshPro quesHolder;
    public TextMeshPro ansHolder1;
    public TextMeshPro ansHolder2;
    public TextMeshPro NextButtonText;

    private int questionNo;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
       
        // Find the models based on the names in the Hierarchy
        defaultOption1Indicator = GameObject.Find("DefaultOption1Indicator").gameObject;
        defaultOption2Indicator = GameObject.Find("DefaultOption2Indicator").gameObject;

        rightOption1Indicator = GameObject.Find("RightOption1Indicator").gameObject;
        rightOption2Indicator = GameObject.Find("RightOption2Indicator").gameObject;

        wrongOption1Indicator = GameObject.Find("WrongOption1Indicator").gameObject;
        wrongOption2Indicator = GameObject.Find("WrongOption2Indicator").gameObject;

        nextButtonGroup = GameObject.FindGameObjectsWithTag("SubmitOrNext");

        rightOption1Indicator.SetActive(false);
        rightOption2Indicator.SetActive(false);
        wrongOption1Indicator.SetActive(false);
        wrongOption2Indicator.SetActive(false);
        defaultOption1Indicator.SetActive(true);
        defaultOption2Indicator.SetActive(true);

        foreach (GameObject model in nextButtonGroup)
        {
            model.SetActive(false);
        }

        questionNo = 0;
        score = 0;
        quesSet = new Question[5];

        /*Questions static objects for now later will be replaced with input from DB)*/
        quesSet[0] = new Question("When does the ArrayIndexOutOfBoundsException occur?", 2, new string[2]{ "Compile-time", "Run-time" }, 2);
        quesSet[1] = new Question("Which of the following concepts make extensive use of arrays?", 2, new string[2]{ "Spatial locality", "Caching" }, 1);
        quesSet[2] = new Question("The process of accessing data stored in a serial access memory is similar to manipulating data on a __?", 2, new string[2] { "Array", "Stack" }, 2);
        quesSet[3] = new Question("Efficiency of finding the next record in B+ tree is __?", 2, new string[2] { "O(log n)", "O(1)" }, 2);
        quesSet[4] = new Question("A single channel is shared by multiple signals by __?", 2, new string[2] { "multiplexing", "modulation" }, 1);

        vrb = GetComponentsInChildren<VirtualButtonBehaviour>();

        for (int i = 0; i < vrb.Length; i++) {
            vrb[i].RegisterEventHandler(this);   
        }

        /*
        TextMeshPro[] textMeshPro = GetComponentsInChildren<TextMeshPro>();

        for (int i = 0; i < textMeshPro.Length; i++)
        {
            Debug.Log(textMeshPro[i].name);
        }
        */

        //NextButtonText = GameObject.Find("SubmitText").GetComponent<TextMeshPro>();
        NextButtonText.text = "NEXT";

        //quesHolder = GameObject.Find("QuesText").GetComponent<TextMeshPro>();

        quesHolder.text = quesSet[questionNo].ques;

        //ansHolder1  = GameObject.Find("Option1").GetComponent<TextMeshPro>();
        ansHolder1.text = quesSet[questionNo].options[0];


        //ansHolder2 = GameObject.Find("Option2").GetComponent<TextMeshPro>();
        ansHolder2.text = quesSet[questionNo].options[1];

    }

    void Update() {
        /*
        if (increaseTime)
        {
            startTime += Time.deltaTime;
            if (startTime > delay)
            {
                msgBox.text = "";               
                increaseTime = false;
                startTime = 0.0f;
            }
           
        }
        */

        /*Updating Question on Panel*/
        if (questionNo < 5)
        {
            if (questionNo == 4)
            {
                NextButtonText.text = "SUBMIT";
            }

            quesHolder.text = quesSet[questionNo].ques;
            ansHolder1.text = quesSet[questionNo].options[0];
            ansHolder2.text = quesSet[questionNo].options[1];
        }

    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {

        if (vb.VirtualButtonName == "Opt1VB")
        {
            Debug.Log("Opt1VB Button is Pressed!");
            if (quesSet[questionNo].correctOptionNo == 1)
            {
                rightOption1Indicator.SetActive(true);
            }
            else
            {
                wrongOption1Indicator.SetActive(true);
            }
            rightOption2Indicator.SetActive(false);
            wrongOption2Indicator.SetActive(false);
            defaultOption1Indicator.SetActive(false);
            defaultOption2Indicator.SetActive(true);

            foreach (GameObject model in nextButtonGroup)
            {
                model.SetActive(true);
            }
            for (int i = 0; i < vrb.Length; i++)
            {
                if(vrb[i].gameObject.name == "Opt2VB")
                {
                    vrb[i].gameObject.SetActive(false);
                }
            }
        }
        else if (vb.VirtualButtonName == "Opt2VB")
        {
            Debug.Log("Opt2VB Button is Pressed!");

            if (quesSet[questionNo].correctOptionNo == 2)
            {
                rightOption2Indicator.SetActive(true);
            }
            else
            {
                wrongOption2Indicator.SetActive(true);
            }

            rightOption1Indicator.SetActive(false);
            wrongOption1Indicator.SetActive(false);
            defaultOption2Indicator.SetActive(false);
            defaultOption1Indicator.SetActive(true);
            
            foreach (GameObject model in nextButtonGroup)
            {
                model.SetActive(true);
            }

            for (int i = 0; i < vrb.Length; i++)
            {
                if (vrb[i].gameObject.name == "Opt1VB")
                {
                    vrb[i].gameObject.SetActive(false);
                }
            }
        }
        else if (vb.VirtualButtonName == "SubmitOrNextBtn")
        {
            if (questionNo < 4 && (!defaultOption1Indicator.activeSelf || !defaultOption2Indicator.activeSelf))
            {
                questionNo++;
                Debug.Log("Next Button is Pressed!");

                rightOption1Indicator.SetActive(false);
                wrongOption1Indicator.SetActive(false);
                defaultOption1Indicator.SetActive(true);
                rightOption2Indicator.SetActive(false);
                wrongOption2Indicator.SetActive(false);
                defaultOption2Indicator.SetActive(true);
                foreach (GameObject model in nextButtonGroup)
                {
                    model.SetActive(false);
                }

            }
            else {

                rightOption1Indicator.SetActive(false);
                wrongOption1Indicator.SetActive(false);
                defaultOption1Indicator.SetActive(false);
                rightOption2Indicator.SetActive(false);
                wrongOption2Indicator.SetActive(false);
                defaultOption2Indicator.SetActive(false);
                foreach (GameObject model in nextButtonGroup)
                {
                    model.SetActive(false);
                }
            }

            for (int i = 0; i < vrb.Length; i++)
            {
                if (!vrb[i].gameObject.activeSelf)
                {
                    vrb[i].gameObject.SetActive(true);
                }
            }

        }
        else
        {
            throw new UnityException(vb.VirtualButtonName + "Virtual Button not supported!");
        }
        
        /*
        if (vb.VirtualButtonName == "YesBtn")
        {
            msgBox.text = "Your Response is 'YES' and recorded !";
            increaseTime = true;
        }
        else if (vb.VirtualButtonName == "NoBtn")
        {
            msgBox.text = "Your Response is 'NO' and recorded !";
            increaseTime = true;
        }
        else
        {
            throw new UnityException(vb.VirtualButtonName + "Virtual Button not supported!");
        }
        */
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        //nothing to do for now
    }
}




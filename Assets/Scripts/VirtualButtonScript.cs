using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class VirtualButtonScript : MonoBehaviour, IVirtualButtonEventHandler
{

    //public Text msgBox;

    //private float delay = 5.0f;
    //private float startTime = 0.0f;
    //private bool increaseTime = false;

    private GameObject model_1;
    private GameObject model_2;
    private GameObject model_3;
    private GameObject model_4;
    private GameObject[] model_5;

    // Start is called before the first frame update
    void Start()
    {
        VirtualButtonBehaviour[] vrb = GetComponentsInChildren<VirtualButtonBehaviour>();

        for (int i = 0; i < vrb.Length; i++) {
            vrb[i].RegisterEventHandler(this);   
        }

        // Find the models based on the names in the Hierarchy
        model_1 = GameObject.Find("YesOptionIndicator").gameObject;
        model_2 = GameObject.Find("NoOptionIndicator").gameObject;
        model_3 = GameObject.Find("DefaultYesButtonShader").gameObject;
        model_4 = GameObject.Find("DefaultNoButtonShader").gameObject;
        model_5 = GameObject.FindGameObjectsWithTag("SubmitOrNext");

        model_1.SetActive(false);
        model_2.SetActive(false);
        model_3.SetActive(true);
        model_4.SetActive(true);
        foreach (GameObject model in model_5)
        {
            model.SetActive(false);
        }

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
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {

        if (vb.VirtualButtonName == "YesBtn")
        {
            model_1.SetActive(true);
            model_2.SetActive(false);
            model_3.SetActive(false);
            model_4.SetActive(true);
            foreach (GameObject model in model_5)
            {
                model.SetActive(true);
            }

        }
        else if (vb.VirtualButtonName == "NoBtn")
        {
            model_1.SetActive(false);
            model_2.SetActive(true);
            model_3.SetActive(true);
            model_4.SetActive(false);
            foreach (GameObject model in model_5)
            {
                model.SetActive(true);
            }
        }
        else if (vb.VirtualButtonName == "SubmitBtn")
        {
            model_1.SetActive(false);
            model_2.SetActive(false);
            model_3.SetActive(true);
            model_4.SetActive(true);
            foreach (GameObject model in model_5)
            {
                model.SetActive(false);
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

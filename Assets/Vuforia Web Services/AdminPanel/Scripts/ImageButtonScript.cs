using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageButtonScript : MonoBehaviour
{

    Image img;
    GameObject imagePickerPanel;
    // Start is called before the first frame update
    void Start()
    {
        Button button = this.GetComponent<Button>();
        img = GameObject.Find("ImagePanelVWS").GetComponent<Image>();
        imagePickerPanel = GameObject.Find("ImagePickerPanel").gameObject;
        button.onClick.AddListener(Foo);
 
    }
 
    public void Foo()
    {
        img.sprite = this.GetComponent<Image>().sprite;
        imagePickerPanel.SetActive(false);
    }
}

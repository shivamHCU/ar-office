using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleControl : MonoBehaviour
{

    bool isImage;

    public Image imageBtnBG;
    public Image VideoBtnBG;
    public GameObject AugImage;
    public GameObject AugVideo;
    public GameObject PlayButton;
    public GameObject PauseButton;


    // Start is called before the first frame update
    void Start()
    {
        isImage = true;
    }

    private void Update()
    {
        if (isImage)
        {
            imageBtnBG.color = Color.green;
            VideoBtnBG.color = Color.white;
            if (AugVideo.activeSelf)
            {
                AugVideo.SetActive(false);
            }
            if (!AugImage.activeSelf)
            {
                AugImage.SetActive(true);
            }
            if (PlayButton.activeSelf)
            {
                PlayButton.SetActive(false);
            }
            if (PauseButton.activeSelf)
            {
                PauseButton.SetActive(false);
            }

        }
        else {
            imageBtnBG.color = Color.white;
            VideoBtnBG.color = Color.green;
            if (!AugVideo.activeSelf)
            {
                AugVideo.SetActive(true);
            }
            if (AugImage.activeSelf)
            {
                AugImage.SetActive(false);
            }
            if (!PlayButton.activeSelf)
            {
                PlayButton.SetActive(true);
            }
            if (!PauseButton.activeSelf)
            {
                PauseButton.SetActive(true);
            }

        }
    }

    public void controlFunction() {
        isImage = !isImage;
    }

}

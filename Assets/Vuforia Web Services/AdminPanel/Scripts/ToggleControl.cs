using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleControl : MonoBehaviour
{

    static bool isImage;
    static bool isVideo;
    static bool isQuiz;

    public Image imageBtnBG;
    public Image VideoBtnBG;
    public Image QuizBtnBG;
    public GameObject AugImage;   //Augmented Image
    public GameObject AugVideo;  //Augmented Video
    public GameObject AugQuiz;  //Quiz
    public GameObject PlayButton;
    public GameObject PauseButton;
    public GameObject AddQuesButton;
    public GameObject QuesMetaInput;  //Input Question Meta data
    public GameObject MetaLinkInput; // Input link Metadat for Image or Video

    // Start is called before the first frame update
    void Start()
    {
        isImage = true;
        isVideo = false;
        isQuiz = false;
    }

    private void Update()
    {
        if (isImage)
        {
            imageBtnBG.color = Color.green;  //Set Image button background colour to green which signifies it being selected
            VideoBtnBG.color = Color.white;
            QuizBtnBG.color = Color.white;
            if (AugVideo.activeSelf)
            {
                AugVideo.SetActive(false);
            }
            if (!AugImage.activeSelf)
            {
                AugImage.SetActive(true);
            }
            if (AugQuiz.activeSelf)
            {
                AugQuiz.SetActive(false);
            }
            if (PlayButton.activeSelf)
            {
                PlayButton.SetActive(false);
            }
            if (PauseButton.activeSelf)
            {
                PauseButton.SetActive(false);
            }
            if (AddQuesButton.activeSelf)
            {
                AddQuesButton.SetActive(false);
            }
            if (!MetaLinkInput.activeSelf)
            {
                MetaLinkInput.SetActive(true);
            }
            if (QuesMetaInput.activeSelf)
            {
                QuesMetaInput.SetActive(false);
            }
            
        }
        else if (isVideo)
        {
            imageBtnBG.color = Color.white;
            VideoBtnBG.color = Color.green;//Set Video button background colour to green which signifies it being selected
            QuizBtnBG.color = Color.white;
            if (!AugVideo.activeSelf)
            {
                AugVideo.SetActive(true);
            }
            if (AugImage.activeSelf)
            {
                AugImage.SetActive(false);
            }
            if (AugQuiz.activeSelf)
            {
                AugQuiz.SetActive(false);
            }
            if (!PlayButton.activeSelf)
            {
                PlayButton.SetActive(true);
            }
            if (!PauseButton.activeSelf)
            {
                PauseButton.SetActive(true);
            }
            if (AddQuesButton.activeSelf)
            {
                AddQuesButton.SetActive(false);
            }
            if (!MetaLinkInput.activeSelf)
            {
                MetaLinkInput.SetActive(true);
            }
            if (QuesMetaInput.activeSelf)
            {
                QuesMetaInput.SetActive(false);
            }
            
        }
        else if (isQuiz) {
            imageBtnBG.color = Color.white;
            VideoBtnBG.color = Color.white;
            QuizBtnBG.color = Color.green; //Set Quiz button background colour to green which signifies it being selected
            if (AugVideo.activeSelf)
            {
                AugVideo.SetActive(false);
            }
            if (AugImage.activeSelf)
            {
                AugImage.SetActive(false);
            }
            if (!AugQuiz.activeSelf)
            {
                AugQuiz.SetActive(true);
            }
            if (PlayButton.activeSelf)
            {
                PlayButton.SetActive(false);
            }
            if (PauseButton.activeSelf)
            {
                PauseButton.SetActive(false);
            }
            if (!AddQuesButton.activeSelf)
            {
                AddQuesButton.SetActive(true);
            }
            if (MetaLinkInput.activeSelf)
            {
                MetaLinkInput.SetActive(false);
            }
            if (!QuesMetaInput.activeSelf)
            {
                QuesMetaInput.SetActive(true);
            }
        }
    }

    public void ImageTogglecontrol() {
        isImage = true;
        isVideo = false;
        isQuiz = false;
    }

    public void VideoTogglecontrol()
    {
        isImage = false;
        isVideo = true;
        isQuiz = false;
    }

    public void QuizTogglecontrol()
    {
        isImage = false;
        isVideo = false;
        isQuiz = true;
    }

    public static int SelectedAug() {
        if (isImage) return 1;
        else if (isVideo) return 2;
        else return 3;
    }

}

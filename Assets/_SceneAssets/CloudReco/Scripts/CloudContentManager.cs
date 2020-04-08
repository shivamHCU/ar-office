/*===============================================================================
Copyright (c) 2017-2018 PTC Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CloudContentManager : MonoBehaviour
{

    #region PRIVATE_MEMBER_VARIABLES

    [SerializeField] Transform CloudTarget = null;
    [SerializeField] UnityEngine.UI.Text cloudTargetInfo = null;

    [System.Serializable]
    public class AugmentationObject
    {
        public int type;
        public GameObject augmentation;
    }

    public AugmentationObject[] AugmentationObjects;

    string metadata;
    
    
    /* Types of Augumentation Types that can be encoded into metadata
     * 1. Image 
     * 2. Video
     * 3. QuesSet
     */
    int type;
    string resultUrl;
    int noOfQues;
    string quizDomain;
    string typeAsString;

    readonly string[] starRatings = { "☆☆☆☆☆", "★☆☆☆☆", "★★☆☆☆", "★★★☆☆", "★★★★☆", "★★★★★" };

    Dictionary<int, GameObject> Augmentations;
    Transform contentManagerParent;
    Transform currentAugmentation;

    #endregion // PRIVATE_MEMBER_VARIABLES

    VideoPlayer videoHolder;
    Sprite_renderer ImageHolder;
    UIQuestion QuestionsHolder;

    #region UNITY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        type = 0;
        resultUrl = null;
        noOfQues = 0;
        quizDomain = null;

        videoHolder = GameObject.Find("VideoHolder").GetComponent<VideoPlayer>();
        ImageHolder = GameObject.Find("imageHolder").GetComponent<Sprite_renderer>();
        QuestionsHolder = GameObject.Find("QuestSet").GetComponent<UIQuestion>();

        Augmentations = new Dictionary<int, GameObject>();

        for (int a = 0; a < AugmentationObjects.Length; ++a)
        {
            Augmentations.Add(AugmentationObjects[a].type,
                              AugmentationObjects[a].augmentation);
        }
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void ShowTargetInfo(bool showInfo)
    {
        /*Canvas canvas = cloudTargetInfo.GetComponentInParent<Canvas>();

        canvas.enabled = showInfo;*/
    }

    public void HandleTargetFinderResult(Vuforia.TargetFinder.CloudRecoSearchResult targetSearchResult)
    {
        Debug.Log("<color=blue>HandleTargetFinderResult(): " + targetSearchResult.TargetName + "</color>");

        metadata = targetSearchResult.MetaData;
        metadata = metadata.Trim();

        Debug.Log("<color=red> meta data set to " + metadata + ".</color>");

        if (metadata != null)
        {
            string[] splitStrings = metadata.Split(' ');

            type = Convert.ToInt32(splitStrings[0]);

            if (type == 3)
            {
                noOfQues = Convert.ToInt32(splitStrings[1]);
                quizDomain = splitStrings[2];
                Debug.Log("<color=red> FROM METADATA : type{" + type + "}, noOfQues{" + noOfQues + "}, quizDomain{ " + quizDomain + "}.</color>");

                //Setting parameter to the QuizHolder

                QuestionsHolder.totalNoOfQuestions = noOfQues;
                QuestionsHolder.domain = quizDomain.Trim();
                QuestionsHolder.StartCoroutine(QuestionsHolder.LoadQuestionFromDatabase());
            }
            else
            {
                resultUrl = splitStrings[1];
                Debug.Log("<color=red> FROM METADATA : type{" + type + "}, link{" + resultUrl + "}.</color>");
                //Setting parameter to the QuizHolder
                if (type == 1)
                {
                    ImageHolder.imageUrl = resultUrl.Trim();

                    Debug.Log("<color=red> Calling SpriteRenderer from could manager script  </color>");
                    ImageHolder.StartCoroutine(ImageHolder.loadSpriteImageFromUrl(ImageHolder.imageUrl));
                }
                else
                {
                    videoHolder.url = resultUrl.Trim();
                }
            }
        }

        cloudTargetInfo.text =
            "Name: " + targetSearchResult.TargetName +
            "\nRating: " + starRatings[targetSearchResult.TrackingRating] +
            "\nMetaData: " + ((targetSearchResult.MetaData.Length > 0) ? targetSearchResult.MetaData : "No") +
            "\nTarget Id: " + targetSearchResult.UniqueTargetId;

        

        GameObject augmentation = GetValuefromDictionary(Augmentations, type);

        if (augmentation != null)
        {
            if (augmentation.transform.parent != CloudTarget.transform)
            {
                Renderer[] augmentationRenderers;

                if (currentAugmentation != null && currentAugmentation.parent == CloudTarget)
                {
                    currentAugmentation.SetParent(contentManagerParent);
                    currentAugmentation.transform.localPosition = Vector3.zero;
                    currentAugmentation.transform.localScale = Vector3.one;

                    augmentationRenderers = currentAugmentation.GetComponentsInChildren<Renderer>();
                    foreach (var objrenderer in augmentationRenderers)
                    {
                        objrenderer.gameObject.layer = LayerMask.NameToLayer("UI");
                        objrenderer.enabled = true;
                    }
                }

                // store reference to content manager's parent object of the augmentation
                contentManagerParent = augmentation.transform.parent;
                // store reference to the current augmentation
                currentAugmentation = augmentation.transform;

                // set new target augmentation parent to cloud target
                augmentation.transform.SetParent(CloudTarget);
                augmentation.transform.localPosition = Vector3.zero;
                augmentation.transform.localScale = Vector3.one;

                augmentationRenderers = augmentation.GetComponentsInChildren<Renderer>();
                foreach (var objrenderer in augmentationRenderers)
                {
                    objrenderer.gameObject.layer = LayerMask.NameToLayer("Default");
                    objrenderer.enabled = true;
                }

            }
        }
    }

    #endregion // PUBLIC_METHODS


    #region // PRIVATE_METHODS

    GameObject GetValuefromDictionary(Dictionary<int, GameObject> dictionary, int key)
    {
        Debug.Log("<color=blue>GetValuefromDictionary() called.</color>");
        if (dictionary == null)
            Debug.Log("dictionary is null");

        if (dictionary.ContainsKey(key))
        {
            Debug.Log("key: " + key);
            GameObject value;
            dictionary.TryGetValue(key, out value);
            Debug.Log("value: " + value.name);
            return value;
        }

        return null;
        //return "Key not found.";
    }

    #endregion // PRIVATE_METHODS
}

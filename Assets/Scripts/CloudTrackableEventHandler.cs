/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.
 
Copyright (c) 2010-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

public class CloudTrackableEventHandler : DefaultTrackableEventHandler
{

    public UnityEngine.Video.VideoPlayer VideoPlayer;

    #region PRIVATE_MEMBERS
    CloudRecoBehaviour m_CloudRecoBehaviour;
    CloudContentManager m_CloudContentManager;
    bool isVideoPlayer;

    TargetFinder.CloudRecoSearchResult xyz;
    //bool isImage;
    //Sprite_renderer ImageHolder;

    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    protected override void Start()
    {
        base.Start();
        isVideoPlayer = false;
        //isImage = false;

        m_CloudRecoBehaviour = FindObjectOfType<CloudRecoBehaviour>();
        m_CloudContentManager = FindObjectOfType<CloudContentManager>();
        //ImageHolder = GameObject.Find("imageHolder").GetComponent<Sprite_renderer>();
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region BUTTON_METHODS
    public void OnReset()
    {
        Debug.Log("<color=blue>OnReset()</color>");

        OnTrackingLost();
        TrackerManager.Instance.GetTracker<ObjectTracker>().GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);       
    }
    #endregion BUTTON_METHODS


    #region PUBLIC_METHODS
    /// <summary>
    /// Method called from the CloudRecoEventHandler
    /// when a new target is created
    /// </summary>
    public void TargetCreated(TargetFinder.CloudRecoSearchResult targetSearchResult)
    {
        string metadata = targetSearchResult.MetaData;
        if (metadata != null)
        {
            string[] splitStrings = metadata.Split(' ');
            if (splitStrings[0].Equals("2"))
            {
                isVideoPlayer = true;
            }
            /*
            else if (splitStrings[0].Equals("1"))
            {
                isImage = true;
            }
            */
        }
        m_CloudContentManager.HandleTargetFinderResult(targetSearchResult);
    }
    #endregion // PUBLIC_METHODS


    #region PROTECTED_METHODS
    
    protected override void OnTrackingFound()
    {
        Debug.Log("<color=blue>OnTrackingFound()</color>");

        base.OnTrackingFound();

        if (isVideoPlayer  && VideoPlayer != null)
        { 
            VideoPlayer.Play();
        }

        if (m_CloudRecoBehaviour)
        {
            m_CloudRecoBehaviour.CloudRecoEnabled = false;
        }

        if (m_CloudContentManager)
        {
            m_CloudContentManager.ShowTargetInfo(true);
        }
    }

    protected override void OnTrackingLost()
    {
        Debug.Log("<color=blue>OnTrackingLost()</color>");

        base.OnTrackingLost();

        if (isVideoPlayer && VideoPlayer != null)
        {
            VideoPlayer.Stop();
            isVideoPlayer = false;
        }

        /*
        if (isImage)
        {
            ImageHolder.imageUrl = "";
            isImage = false;
        }
        */

        if (m_CloudRecoBehaviour)
        {
            // Changing CloudRecoBehaviour.CloudRecoEnabled to true will call TargetFinder.StartRecognition()
            // and also call all registered ICloudRecoEventHandler.OnStateChanged() with true.
            m_CloudRecoBehaviour.CloudRecoEnabled = true;
        }

        if (m_CloudContentManager)
        {
            m_CloudContentManager.ShowTargetInfo(false);
        }
    }

    #endregion // PROTECTED_METHODS
}

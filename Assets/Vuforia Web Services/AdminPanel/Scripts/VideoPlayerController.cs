using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;



public class VideoPlayerController : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;


    private void Start()
    {
        LoadAndPlay();
    }
    void Update()
    {
        if (videoPlayer.url != null) {
            LoadAndPlay();
        }
    }
    void LoadAndPlay() {
        StartCoroutine("PlayVideo");
    }

    IEnumerator PlayVideo() { // play video after waiting for several seconds then once the player is ready replace the texture with that of the video player
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared) {
            yield return waitForSeconds;
            break;
        }
        rawImage.texture = videoPlayer.texture;
    }
}

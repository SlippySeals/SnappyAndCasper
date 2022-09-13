using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class VideoPlayerExample : MonoBehaviour
{
    public VideoClip clipToPlay; // reference to the clip we want to play.
    public VideoPlayer videoPlayer; // reference to the videplayer

    public float timeTillPlay = 5; // the amount of time we wait to start playing the video.
    private float currentTime = 0; // the time till video starts playing.

    public RawImage rawImage; // the raw image that will be on our canvas.
    public RenderTexture renderTexture; // render texture that our video will be playing on

    public PlayerInput playerInput; // new input system reference.

    private void OnEnable()
    {
        // subscribe to the new input system
        playerInput.onActionTriggered += PlayerInput_onActionTriggered;
    }

    private void OnDisable()
    {
        // unsubscribe to the new input system.
        playerInput.onActionTriggered -= PlayerInput_onActionTriggered;
    }

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Prepare();
        // set the current time + 5 seconds, and set the video clip to the one we want to play.
        currentTime = Time.time + timeTillPlay;
        videoPlayer.clip = clipToPlay;
        rawImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if enough time has elapsed and we are not currently playing the video; then play
        if (Time.time >= currentTime && !videoPlayer.isPlaying)
        {
            rawImage.enabled = true;
            videoPlayer.Play();
            rawImage.texture = renderTexture;
        }
    }

    /// <summary>
    // This will be called when ever input is detected from the new input system, if there is we should stop the video.
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerInput_onActionTriggered(InputAction.CallbackContext obj)
    {
        if(videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            rawImage.enabled = false;
        }
        currentTime = Time.time + timeTillPlay;
    }
}

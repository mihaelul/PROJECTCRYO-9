using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class video_manager : MonoBehaviour
{
    public GameObject videoScreen; // RawImage GameObject
    public VideoPlayer videoPlayer;
    public Button playButton;

    void Start()
    {
        // La început, ascundem ecranul video și butonul Play
        videoScreen.SetActive(false);
        playButton.gameObject.SetActive(true);

        // Ne asigurăm că nu pornește singur
        videoPlayer.playOnAwake = false;
    }

    public void ShowVideoUI()
    {
        videoScreen.SetActive(true);
        playButton.gameObject.SetActive(true);
    }

    public void PlayVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    public void SeekVideoUI()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
        videoScreen.SetActive(false);
    }
}

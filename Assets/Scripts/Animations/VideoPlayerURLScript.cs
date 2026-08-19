using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerURLScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "final_cutscene.webm");

        videoPlayer.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GM_VideoPlayer : MonoBehaviour, IEventSystemHandler
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    [SerializeField]
    private Scrollbar _frameChecker;
    [SerializeField]
    private Image _frameAmount;

    private double _fps;

    private Queue<float> RecordStart;
    private Queue<float> RecordEnd;



    // Start is called before the first frame update
    void Start()
    {
        _fps = 0f;
        _videoPlayer.Stop();
        Debug.Log(_videoPlayer.frameCount);
        RecordStart = new Queue<float>();
        RecordEnd = new Queue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        SlideBarChecker();
  
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _videoPlayer.Play();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _videoPlayer.Pause();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _videoPlayer.time = 3.0f;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            RecordStart.Enqueue((float)_videoPlayer.time);
            Debug.Log(RecordStart.Dequeue());
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            RecordEnd.Enqueue((float)_fps);
            Debug.Log(RecordEnd.Dequeue());
        }        
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _videoPlayer.time -= 3.0f;
        }
    }


    float SlideBarChecker()
    {
        var a = _videoPlayer.clip.length;
        var b = _videoPlayer.time;
        _fps = b/a;
        _frameChecker.value = (float)_fps;
        _frameAmount.fillAmount = (float)_fps;
        return 0;
    }
}

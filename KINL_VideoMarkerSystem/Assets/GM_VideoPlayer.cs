using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class GM_VideoPlayer : MonoBehaviour
{
    public static GM_VideoPlayer instance;

    [SerializeField]
    public VideoPlayer _videoPlayer;
    public VideoClip[] _videoClips;
    public Dropdown _currentPlayVideo;

    [SerializeField]
    public Scrollbar _frameChecker;
    [SerializeField]
    public Image _frameAmount;
    [SerializeField]
    public InputField inputField;
    public Button inputCheckButton;

    public Text savePos;

    public GameObject _recordEndPanel;
    public Button _recordRestarted;

    private double _fps;

    public Queue<string> RecordQueue;

    [SerializeField]
    private bool bRecord = false;
    private bool bReset = false;
    private bool bStart = false;
    private bool bRessonCheck = true;

    public GameObject recordStart;
    public GameObject recordEnd;

    public Text timer;
    public List<GameObject> recordObject;

    string v;
    string videoname;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _fps = 0f;
        _videoPlayer.Play();
        _videoPlayer.Stop();
        Debug.Log(_videoPlayer.frameCount);
        RecordQueue = new Queue<string>();
        CurrentVideo_DropDownMaker();
        videoname = _videoPlayer.clip.name;
        _recordRestarted.onClick.AddListener(Record_ButtonClicked);
        _recordEndPanel.SetActive(false);
        bRecord = false;
        bReset = false;
    }
    #region DropBox Changed
    void CurrentVideo_DropDownMaker()
    {
        _currentPlayVideo.options.Clear();
        for (int i = 0; i < _videoClips.Length; i++)//1부터 10까지
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = $"운동명 :{_videoClips[i].name}";
            _currentPlayVideo.options.Add(option);
        }
    }
    public void CurrentVideo_DropDownChanged()
    {
        _videoPlayer.Stop();
        _videoPlayer.Play();
        _videoPlayer.clip = _videoClips[_currentPlayVideo.value];
        videoname = _videoPlayer.clip.name;
        DestroyCheckPoint();
    }
    void DestroyCheckPoint()
    {
        bRecord = false;
        v = null;
        GM_DataRecord.instance.bSaved = false;
        for (int i = 0; i < GameObject.Find("Canvas/This").transform.childCount; i++)
        {
            Destroy(GameObject.Find("Canvas/This").transform.GetChild(i).gameObject);
        }
    }
    #endregion

    void Record_ButtonClicked()
    {
        _recordEndPanel.SetActive(false);
    }
    void ResetRecord()
    {
        if (bReset)
        {
            videoname = _videoPlayer.clip.name;
            bReset = false;
            bRecord = false;
            v = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        SlideBarChecker();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!bStart && bRessonCheck)
            {
                _videoPlayer.Play();
                bStart = true;
            }
            else if (bStart && bRessonCheck)
            {
                _videoPlayer.Pause();
                bStart = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)&&bRessonCheck)
        {
            bReset = true;
            _currentPlayVideo.enabled = true;
            GM_DataRecord.instance.Enequeue_Data();
            _videoPlayer.Play();
            _videoPlayer.Stop();
            _recordEndPanel.SetActive(true);
            DestroyCheckPoint();
            ResetRecord();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (bRessonCheck == true)
            {
                if (bRecord == false)
                {
                    var a = Instantiate(recordStart, _frameChecker.handleRect.transform.position, Quaternion.identity, GameObject.Find("Canvas/This").transform);
                    a.transform.GetChild(0).GetComponent<Text>().text = _videoPlayer.time.ToString("N3");
                    v += videoname + "," + (float)_videoPlayer.time + ",";
                    //RecordQueue.Enqueue(_videoPlayer.name+","+"recordStart" + "," + (float)_videoPlayer.time + ",");
                    bRecord = true;
                    Debug.Log("CCCC");
                }
                else if (bRecord == true)
                {
                    var a = Instantiate(recordEnd, _frameChecker.handleRect.transform.position, Quaternion.identity, GameObject.Find("Canvas/This").transform);
                    a.transform.GetChild(0).GetComponent<Text>().text = _videoPlayer.time.ToString("N3");
                    _videoPlayer.Pause();
                    //inputField = a.transform.GetChild(1).GetComponent<InputField>();
                    //inputCheckButton = a.transform.GetChild(2).GetComponent<Button>();
                    inputField.gameObject.SetActive(true);
                    inputCheckButton.gameObject.SetActive(true);
                    inputCheckButton.onClick.AddListener(DataSaved);
                    v += (float)_videoPlayer.time;
                    bRessonCheck = false;
                    _currentPlayVideo.enabled=false;
                    Debug.Log("SSSS");
                    //RecordQueue.Enqueue(v);
                    //v = null;
                    //bRecord = false;
                }
            }
            ////Debug.Log(_videoPlayer.clip.name + "," + (float)_videoPlayer.time + ";");
            ////   RecordQueue.Enqueue(_videoPlayer.clip.name + "," + (float)_videoPlayer.time + ";");
            //if (bRecord && bReset == false)
            //{
            //    var a = Instantiate(recordStart, _frameChecker.handleRect.transform.position, Quaternion.identity, GameObject.Find("Canvas/This").transform);
            //    a.transform.GetChild(0).GetComponent<Text>().text = _videoPlayer.time.ToString("N3");
            //    v += videoname + "," + (float)_videoPlayer.time + ",";
            //    //RecordQueue.Enqueue(_videoPlayer.name+","+"recordStart" + "," + (float)_videoPlayer.time + ",");
            //    bRecord = false;
            //}
            //else if (!bRecord && bReset == false)
            //{
            //    var a = Instantiate(recordEnd, _frameChecker.handleRect.transform.position, Quaternion.identity, GameObject.Find("Canvas/This").transform);
            //    a.transform.GetChild(0).GetComponent<Text>().text = _videoPlayer.time.ToString("N3");
            //    if (bReset == false)
            //    {
            //        v += (float)_videoPlayer.time + ";";
            //        RecordQueue.Enqueue(v);
            //    }
            //}
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _videoPlayer.time -= 3.0f;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _videoPlayer.time += 3.0f;
        }
    }
    public void DataSaved()
    {
        v +="," +inputField.text + ";";
        RecordQueue.Enqueue(v);
        v = null;
        bRecord = false;
        inputField.text = null;
        inputField.gameObject.SetActive(false);
        inputCheckButton.gameObject.SetActive(false);
        inputCheckButton.onClick.RemoveAllListeners();
        //inputField = null;
        //inputCheckButton = null;
        _videoPlayer.Play();
        bRessonCheck = true;
        _currentPlayVideo.enabled = true;
    }
    void SlideBarChecker()
    {
        var a = _videoPlayer.clip.length;
        var b = _videoPlayer.time;
        _fps = b / a;
        _frameChecker.value = (float)_fps;
        _frameAmount.fillAmount = (float)_fps;
        timer.text = _videoPlayer.time.ToString("N2");
    }

    public void OnPointPress(PointerEventData eventData)
    {
        Debug.Log(eventData.pressPosition);
        if (eventData.selectedObject == _frameChecker)
        {
            Debug.Log("_frameChecker");
            return;
        }
        else
        {
            Debug.Log("event Start");
        }
    }

}

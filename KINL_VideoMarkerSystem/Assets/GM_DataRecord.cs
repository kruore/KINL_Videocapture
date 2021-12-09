using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

using System.Text;
using UnityEngine.SceneManagement;
using TMPro;

public class GM_DataRecord : MonoBehaviour
{

    public static GM_DataRecord instance = null;

    private string rootpath = string.Empty;
    private string folder_Path = string.Empty;

    private string folderName = "KINLAB_VideoDataLog";
    private string fileName = string.Empty;

    private string str_DataCategory = string.Empty;



    [HideInInspector]
    public Queue<string> Queue_EX_DATA;
    [HideInInspector]

    private bool isCategoryPrinted = false;


    [SerializeField]
    private TextMeshProUGUI text_SavedFiles;

    private int count_SavedFiles = 0;

    public bool bSaved = false;

    [SerializeField]
    private TextMeshProUGUI text_SavedPercent;

    [SerializeField]
    private TextMeshProUGUI text_EventLog;
    //-------------------------------

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //MakeFolder();

        //SetFileName();
        //string curFile = Application.persistentDataPath + "/K_PT_SprotsData/" + fileName + ".txt";
        //if (File.Exists(curFile))
        //    Debug.LogError("File.Exists(curFile)");
    }

    private void Start()
    {
        Queue_EX_DATA = new Queue<string>();
        MakeFolder();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {

    }
    private void MakeFolder()
    {
        rootpath = Directory.GetCurrentDirectory();

        folder_Path = System.IO.Path.Combine(rootpath, folderName);

        Directory.CreateDirectory(folder_Path);
    }

    private void SetFileName()
    {
        fileName = GM_VideoPlayer.instance._videoPlayer.clip.name +" , " + DateTime.Now.ToString("yyyyMMddHHmmss");
     
    }

    public void Enequeue_Data()
    {
        var counter = GM_VideoPlayer.instance.RecordQueue.Count;
        Debug.Log(counter + "CCCC");
        for (int i = 0; i < counter; i++)
        {
            Queue_EX_DATA.Enqueue(GM_VideoPlayer.instance.RecordQueue.Dequeue());
        }
        Debug.Log(Queue_EX_DATA.Count + "CCCC");
        if (GM_VideoPlayer.instance.RecordQueue.Count == 0)
        {
            Save_SteamingData_Batch();
        }

    }

    public void Save_SteamingData_Batch()
    {
        WriteSteamingData_Batch(ref Queue_EX_DATA);
    }

    public bool WriteSteamingData_Batch(ref Queue<string> _Queue_ex)
    {
        if (bSaved)
        {
            return false;
        }

        bSaved = true;

        bool tempb = false;

        try
        {
            SetFileName();

            string m_str_DataCategory = string.Empty;

            int totalCountoftheQueue = _Queue_ex.Count;

            string catestr = string.Empty;

            Debug.Log("Saving Data Starts. Queue Count : " + totalCountoftheQueue);
            Debug.Log(Application.persistentDataPath + "/" + fileName + ".txt");
            GM_VideoPlayer.instance.savePos.text = (Application.persistentDataPath + "/" + fileName + ".txt").ToString();
            //using (StreamWriter streamWriter = File.AppendText(Application.persistentDataPath + "/" + fileName + ".txt"))
            using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/" + fileName + ".txt"))
            {
                while (_Queue_ex.Count != 0)
                {
                    for (int i = 0; i < totalCountoftheQueue; i++)
                    {
                        string stringData = _Queue_ex.Dequeue();
                        
                        //if (stringData.Length > 0)
                        //{
                        if (!isCategoryPrinted)
                        {
                            str_DataCategory = "VideoName,"
                                + "Check_StartTime,"
                                + "Check_EndTime,"
                                + "Check_DataLog";
                            isCategoryPrinted = true;
                            streamWriter.WriteLine(str_DataCategory);
                        }
                        //}
                        streamWriter.WriteLine(stringData);

                    }
                }
            }
            Debug.Log("Save Completed");

            tempb = true;

            //  StartCoroutine(CheckSavingDataCompleted());
        }
        catch (Exception e)
        {
            Debug.Log("WriteSteamingData_BatchProcessing ERROR : " + e);
            //TCPTestClient.instance.Send_Message_Index("\n" + "WriteSteamingData_BatchProcessing ERROR : " + e, 0);
        }

        return tempb;
    }
}
//private IEnumerator CheckSavingDataCompleted()
//{

//    if (curren_EX_Type == Experiment_Type.CNES_04)
//    {
//        _KINLAB_IMU.IMU_Management.instance.Disconnect_IMU_Sensors();

//    }


//    yield return new WaitForSeconds(3.0f);

//    TempDF.instance.Func01();
//    TCPTestClient.instance.Send_Message_Index("\n" + DateTime.Now.ToString("yyyyMMddHHmmss.fff") + " : " + "All the data are saved.)", 0);

//    yield break;
//}


//    public void Rec(HealthInfoData _structData)
//    {
//        if (bSaved)
//            return;

//        StringBuilder sb = new StringBuilder();

//        sb.Append(',');
//        sb.Append(_structData.date.ToString()).Append(',');

//        sb.AppendFormat("{0:F4}", _structData.acc_data.x).Append(',');
//        sb.AppendFormat("{0:F4}", _structData.acc_data.y).Append(',');
//        sb.AppendFormat("{0:F4}", _structData.acc_data.z).Append(',');

//        sb.AppendFormat("{0:F4}", _structData.gyro_data.x).Append(',');
//        sb.AppendFormat("{0:F4}", _structData.gyro_data.y).Append(',');
//        sb.AppendFormat("{0:F4}", _structData.gyro_data.z).Append(',');

//        sb.AppendFormat("{0:F4}", _structData.hrm_data);

//        if (sb.Length > 0 && sb[sb.Length - 1] == ',')
//        {
//            sb.Remove(sb.Length - 1, 1);
//        }

//        Enequeue_Data(sb.ToString());
//    }
//}
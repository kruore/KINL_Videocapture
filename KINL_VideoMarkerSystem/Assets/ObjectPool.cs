using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

    }

    void Initalize(int initCount)
    {
        for(int i =0; i< initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }
    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).gameObject;
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    //public static GameObject GetObject()
    //{

    //}
}

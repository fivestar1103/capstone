using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private int objectNum;
    public int ObjectNum { get { return objectNum; } }
    public GameObject poolObject;
    public List<GameObject> poolObjects = new List<GameObject>();

    public void SetManager()
    {
        poolObjects = new List<GameObject>();
        for(int i = 0; i < objectNum; i++)
        {
            GameObject gameObject = Instantiate(poolObject);
            gameObject.SetActive(false);
            poolObjects.Add(gameObject);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach(GameObject gameObject in poolObjects) 
        {
            if(!gameObject.activeInHierarchy)
            {
                return gameObject;
            }
        }

        GameObject poolObj = Instantiate(poolObject);
        poolObj.SetActive(false);
        poolObjects.Add(poolObj);
        return poolObj;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    public List<GameObject> pooledObjects;
    public GameObject objectsToPool;
    public int amountToPool;

    private void Awake()
    {
        SharedInstance = this;

		pooledObjects = new List<GameObject>();

		for (int j = 0; j < amountToPool; j++)
		{
			GameObject obj = Instantiate(objectsToPool);
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
	}
    // Use this for initialization
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }
        return null;
    }

    public void AddToPool()
    {
        GameObject obj = Instantiate(objectsToPool);
        obj.SetActive(false);
        pooledObjects.Add(obj);
    }
}

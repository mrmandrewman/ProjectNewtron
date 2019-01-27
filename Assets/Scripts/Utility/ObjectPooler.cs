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

		pooledObjects = new List<GameObject>();

		for (int j = 0; j < amountToPool; j++)
		{
			GameObject obj = Instantiate(objectsToPool);
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
	}

	private void Start()
	{
		SharedInstance = this;
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

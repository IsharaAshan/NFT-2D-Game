using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolObject
{
    public string objectTag;
    public GameObject objectPrefab;
    public int initialPoolSize;
}

public class Pooler : MonoBehaviour
{
    // Singleton instance
    private static Pooler instance;
    public static Pooler Instance
    {
        get
        {
            if (instance == null)
            {
                // If the instance is null, find the Pooler object in the scene
                instance = FindObjectOfType<Pooler>();

                // If it's still null, create a new GameObject with the Pooler component
                if (instance == null)
                {
                    GameObject obj = new GameObject("Pooler");
                    instance = obj.AddComponent<Pooler>();
                }
            }
            return instance;
        }
    }

    public List<PoolObject> poolObjects;
    private Dictionary<string, List<GameObject>> objectPools;

    private void Awake()
    {
        // Singleton pattern: make sure there is only one instance of the Pooler
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        objectPools = new Dictionary<string, List<GameObject>>();

        // Create object pools for each pool object
        foreach (PoolObject poolObject in poolObjects)
        {
            List<GameObject> objectPool = new List<GameObject>();

            // Create a parent object to hold the pooled objects
            GameObject parentObject = new GameObject(poolObject.objectTag + " Pool");
            parentObject.transform.SetParent(transform); // Set the parent to the Pooler transform

            for (int i = 0; i < poolObject.initialPoolSize; i++)
            {
                GameObject obj = Instantiate(poolObject.objectPrefab, parentObject.transform);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            objectPools.Add(poolObject.objectTag, objectPool);
        }
    }

    public GameObject GetPooledObject(string objectTag)
    {
        if (objectPools.ContainsKey(objectTag))
        {
            List<GameObject> objectPool = objectPools[objectTag];

            // Find the first inactive object in the pool and return it
            for (int i = 0; i < objectPool.Count; i++)
            {
                if (!objectPool[i].activeInHierarchy)
                {
                    return objectPool[i];
                }
            }

            // If no inactive objects are found, create a new one and add it to the pool
            GameObject prefab = GetObjectPrefab(objectTag);
            if (prefab != null)
            {
                GameObject newObj = Instantiate(prefab);
                newObj.SetActive(false);
                objectPool.Add(newObj);
                return newObj;
            }
        }

        Debug.LogWarning("Object with tag '" + objectTag + "' does not exist in the pooler!");
        return null;
    }

    private GameObject GetObjectPrefab(string objectTag)
    {
        foreach (PoolObject poolObject in poolObjects)
        {
            if (poolObject.objectTag == objectTag)
            {
                return poolObject.objectPrefab;
            }
        }

        Debug.LogWarning("Object with tag '" + objectTag + "' does not have a prefab assigned in the pooler!");
        return null;
    }

    public void GetVfx(string vfxName, Vector2 vfxTransform)
    {
        GameObject desVfx = Pooler.Instance.GetPooledObject(vfxName);
        desVfx.transform.position = vfxTransform;
        desVfx.SetActive(true);
    }

}

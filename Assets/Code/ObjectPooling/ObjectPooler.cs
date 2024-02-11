using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public SurfaceTagMapping surfaceTagMapping;

    public GameObject SpawnEffect(SurfaceType surfaceType, Vector3 position, Quaternion rotation)
    {
        string tag = surfaceTagMapping.GetTagForSurfaceType(surfaceType);
        if (string.IsNullOrEmpty(tag))
        {
            Debug.LogWarning($"No particle effect mapped for surface type {surfaceType}.");
            return null;
        }

        return SpawnFromPool(tag, position, rotation);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist. ");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        switch (tag)
        {
            case "SmallExplosion":
                //Do code
                break;
        }
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}

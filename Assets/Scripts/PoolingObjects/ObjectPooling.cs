using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling : MonoBehaviour
{
    // Class representing a pool of objects
    [System.Serializable]
    public class Pool
    {
        public string tag; // Unique tag to identify the pool
        public GameObject prefab; // Prefab to instantiate for the pool
        public int poolSize; // Number of objects to create in the pool
        public Transform parentTransform; // Parent transform for organizing the pooled objects
    }

    #region Singleton
    // Singleton pattern to ensure only one instance of ObjectPooling exists
    public static ObjectPooling Instance;

    private void Awake()
    {
        Instance = this; // Set the static instance to this instance
    }
    #endregion

    public List<Pool> pools; // List of pools

    public Dictionary<string, Queue<GameObject>> poolDictionary; // Dictionary to hold the object pools

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); // Initialize the dictionary

        // Loop through each pool in the pools list
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); // Create a new queue for the pool

            // Instantiate objects for the pool
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab); // Instantiate the prefab
                obj.SetActive(false); // Set the object to inactive
                obj.transform.SetParent(pool.parentTransform); // Set the parent transform
                objectPool.Enqueue(obj); // Enqueue the object into the pool
            }

            poolDictionary.Add(pool.tag, objectPool); // Add the pool to the dictionary
        }
    }

    // Method to spawn an object from the pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // Check if the pool with the specified tag exists
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag: " + tag + " doesn't exist.");
            return null;
        }

        // Dequeue an object from the pool
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true); // Activate the object
        objectToSpawn.transform.position = new Vector3(position.x, position.y, position.z); // Set position
        objectToSpawn.transform.rotation = rotation; // Set rotation

        // Check if the object has the IPooledObject interface and call OnObjectSpawn if it does
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        // Enqueue the object back into the pool
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn; // Return the spawned object
    }
}

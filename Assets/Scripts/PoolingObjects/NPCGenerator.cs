using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{
    public NPCScript npcPrefab; // Prefab of the NPC to be spawned
    public Transform[] spawnPoints; // Array of potential spawn points
    public float timeBetweenNPCs = 2f; // Time delay between spawning NPCs
    public bool canSpawnNPCs = true; // Flag to control if NPCs can be spawned

    private ObjectPooling objectPooling; // Reference to the ObjectPooling instance

    // Start is called before the first frame update
    void Start()
    {
        objectPooling = ObjectPooling.Instance; // Get the instance of ObjectPooling
    }

    // FixedUpdate is called at a fixed interval
    private void FixedUpdate()
    {
        if (canSpawnNPCs)
        {
            // Start the coroutine to spawn NPCs if allowed
            StartCoroutine(SpawnNPC());
        }
    }

    IEnumerator SpawnNPC()
    {
        canSpawnNPCs = false; // Prevent spawning more NPCs until the coroutine completes
        yield return new WaitForSeconds(timeBetweenNPCs); // Wait for the specified time

        // Select a random spawn point from the array
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn an NPC from the object pool and get the NPCScript component
        NPCScript npc = objectPooling.SpawnFromPool("Target", spawnPoint.position, spawnPoint.localRotation).GetComponent<NPCScript>();

        // Call the OnObjectSpawn method on the spawned NPC
        npc.OnObjectSpawn();

        canSpawnNPCs = true; // Allow spawning more NPCs
    }
}

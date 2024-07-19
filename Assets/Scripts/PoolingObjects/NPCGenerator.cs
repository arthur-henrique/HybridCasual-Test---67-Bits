using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{
    public NPCScript npcPrefab;
    public Transform[] spawnPoints;
    public float timeBetweenNPCs = 2f;
    public bool canSpawnNPCs = true;

    ObjectPooling objectPooling;
    // Start is called before the first frame update
    void Start()
    {
        objectPooling = ObjectPooling.Instance;
    }

    private void FixedUpdate()
    {
        if (canSpawnNPCs)
        {
            StartCoroutine(SpawnNPC());
        }
    }

    IEnumerator SpawnNPC()
    {
        canSpawnNPCs = false;
        yield return new WaitForSeconds(timeBetweenNPCs);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        NPCScript npc = objectPooling.SpawnFromPool("Target", spawnPoint.position, spawnPoint.localRotation).GetComponent<NPCScript>();
        npc.OnObjectSpawn();
        canSpawnNPCs = true;
    }

}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TargetStacker : MonoBehaviour
{
    public Transform stackParent; // The parent object for the stack, usually the player
    public GameObject targetPrefab; // Prefab of the target model to instantiate
    public float stackHeightOffset = 1f; // Height offset for stacking targets
    public Quaternion originalRotation = new Quaternion(-0.470148504f, 0.520530581f, 0.477739811f, 0.52893573f); // Original rotation of the prefab

    private List<Transform> stackedTargets = new List<Transform>();
    ObjectPooling objectPooling;
    // Start is called before the first frame update
    void Start()
    {
        objectPooling = ObjectPooling.Instance;
    }
    public void AddTargetToStack()
    {
        if(stackedTargets.Count < GameManager.instance.playerStackSize)
        {
            // Instantiate the target prefab
            StackedNPCScript npc = objectPooling.SpawnFromPool("Stacked", stackParent.position, originalRotation).GetComponent<StackedNPCScript>();

            GameObject newTarget = npc.gameObject;

            // Calculate the new Y position for the stacked target
            Vector3 newPosition = stackParent.position;
            newPosition.y += stackHeightOffset * stackedTargets.Count;

            // Set the target's position and parent
            newTarget.transform.localPosition = new Vector3(1.9f, newPosition.y, -1.12f); // Specified position relative to stackParent
            newTarget.transform.localRotation = originalRotation; // Set the original rotation

            //Add the target to the list of stacked targets
            stackedTargets.Add(npc.transform);
            npc.OnObjectSpawn();
        }

    }

    public void EmptyStack()
    {
        foreach (Transform item in stackedTargets)
        {
            item.gameObject.SetActive(false);
            GameManager.instance.playerCurrency++;
        }
        stackedTargets.Clear();
        GameManager.instance.UpdateCoinText();
    }
}

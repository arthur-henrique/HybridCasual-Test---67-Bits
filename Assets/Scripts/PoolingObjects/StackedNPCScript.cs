using UnityEngine;

public class StackedNPCScript : MonoBehaviour, IPooledObject
{
    public void OnObjectSpawn()
    {
        // Other scripts can be called here
        print("Got ME");
    }

}

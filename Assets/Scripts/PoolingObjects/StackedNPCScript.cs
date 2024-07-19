using UnityEngine;

public class StackedNPCScript : MonoBehaviour, IPooledObject
{
    public void OnObjectSpawn()
    {
        print("Got ME");
    }

    
}

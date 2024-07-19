using System.Collections;
using System.Threading;
using UnityEngine;
public class FrameRateManager : MonoBehaviour
{
    [Header("Frame Settings")]
    public int TargetFrameRate = 60;
    float currentFrameTime;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;

        // To use on more complicated situations

        //DontDestroyOnLoad(gameObject);
        //currentFrameTime = Time.realtimeSinceStartup;
        //StartCoroutine("WaitForNextFrame");
    }
    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
}
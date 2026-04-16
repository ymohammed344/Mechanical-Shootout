using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagSceneTrigger : MonoBehaviour
{
    [Header("Platform Settings")]
    public string platformTag = "FlagPlatform";
    public float captureRange = 5f;

    private bool hasTriggered = false;
    private Transform flagPlatform;
    private CTFObjectiveManager objectiveManager;

    void Start()
    {
        GameObject platform = GameObject.FindGameObjectWithTag(platformTag);
        if (platform != null)
        {
            flagPlatform = platform.transform;
        }
        else
        {
            Debug.LogWarning($"[FlagSceneTrigger] No GameObject with tag '{platformTag}' found in scene!");
        }

        objectiveManager = CTFObjectiveManager.GetInstance();
        if (objectiveManager == null)
        {
            Debug.LogWarning("[FlagSceneTrigger] No CTFObjectiveManager found in scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag(platformTag))
        {
            CheckCapture();
        }
    }

    public void OnFlagDropped()
    {
        if (hasTriggered || flagPlatform == null) return;

        float distance = Vector3.Distance(transform.position, flagPlatform.position);
        
        if (distance <= captureRange)
        {
            Debug.Log($"[FlagSceneTrigger] Flag dropped within capture range ({distance:F2}m)!");
            CheckCapture();
        }
        else
        {
            Debug.Log($"[FlagSceneTrigger] Flag dropped too far from platform ({distance:F2}m / {captureRange}m)");
        }
    }

    private void CheckCapture()
    {
        if (objectiveManager != null)
        {
            hasTriggered = true;
            objectiveManager.OnFlagDelivered();
        }
        else
        {
            Debug.LogError("[FlagSceneTrigger] Cannot check capture - no ObjectiveManager!");
        }
    }
}

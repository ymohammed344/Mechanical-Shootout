using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public static ShieldManager Instance { get; private set; }
    
    private float shieldEndTime;
    private bool isShieldActive = false;

    void Awake()
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

    void Start()
    {
        CheckAndApplyShield();
    }

    void Update()
    {
        if (isShieldActive && Time.time >= shieldEndTime)
        {
            DeactivateShield();
        }
    }

    void CheckAndApplyShield()
    {
        if (PlayerPrefs.GetInt("ShieldActive", 0) == 1)
        {
            float shieldDuration = 15f;
            shieldEndTime = Time.time + shieldDuration;
            isShieldActive = true;
            Debug.Log($"Shield Activated! Duration: {shieldDuration}s");
        }
    }


    void DeactivateShield()
    {
        isShieldActive = false;

        PlayerPrefs.SetInt("ShieldActive", 0);
        PlayerPrefs.Save();

        Debug.Log("Shield Deactivated!");
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    public float GetTimeLeft()
    {
        if (!isShieldActive) return 0f;
        return Mathf.Max(0f, shieldEndTime - Time.time);
    }
}

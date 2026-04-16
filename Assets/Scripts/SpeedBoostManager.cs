using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class SpeedBoostManager : MonoBehaviour
{
    private Movement movementComponent;
    private float originalRunSpeed;
    private float originalWalkSpeed;
    private float boostEndTime;
    private bool isBoostActive = false;

    void Start()
    {
        movementComponent = GetComponent<Movement>();
        if (movementComponent == null)
        {
            Debug.LogError("SpeedBoostManager: Movement component not found!");
            return;
        }

        originalRunSpeed = GetFieldValue<float>(movementComponent, "speedRunning");
        originalWalkSpeed = GetFieldValue<float>(movementComponent, "speedWalking");

        CheckAndApplySpeedBoost();
    }

    void Update()
    {
        if (isBoostActive && Time.time >= boostEndTime)
        {
            DeactivateSpeedBoost();
        }
    }

    void CheckAndApplySpeedBoost()
    {
        if (PlayerPrefs.GetInt("SpeedBoostActive", 0) == 1)
        {
            float boostDuration = PlayerPrefs.GetFloat("BoostDuration", 30f);
            float boostMultiplier = PlayerPrefs.GetFloat("BoostMultiplier", 2f);

            boostEndTime = Time.time + boostDuration;
            isBoostActive = true;

            float boostedRunSpeed = originalRunSpeed * boostMultiplier;
            float boostedWalkSpeed = originalWalkSpeed * boostMultiplier;

            SetFieldValue(movementComponent, "speedRunning", boostedRunSpeed);
            SetFieldValue(movementComponent, "speedWalking", boostedWalkSpeed);

            Debug.Log($"Speed Boost Activated! Run: {boostedRunSpeed}, Walk: {boostedWalkSpeed}, Duration: {boostDuration}s");
        }
    }

    void DeactivateSpeedBoost()
    {
        isBoostActive = false;

        SetFieldValue(movementComponent, "speedRunning", originalRunSpeed);
        SetFieldValue(movementComponent, "speedWalking", originalWalkSpeed);

        PlayerPrefs.SetInt("SpeedBoostActive", 0);
        PlayerPrefs.Save();

        Debug.Log("Speed Boost Deactivated!");
    }

    private T GetFieldValue<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.Public);
        
        if (field != null)
            return (T)field.GetValue(obj);
        
        return default(T);
    }

    private void SetFieldValue(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.Public);
        
        if (field != null)
            field.SetValue(obj, value);
    }

    public bool IsBoostActive()
    {
        return isBoostActive;
    }

    public float GetRemainingTime()
    {
        if (!isBoostActive) return 0f;
        return Mathf.Max(0f, boostEndTime - Time.time);
    }
}

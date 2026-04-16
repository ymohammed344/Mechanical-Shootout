using UnityEngine;
using UnityEngine.UI;
using InfimaGames.LowPolyShooterPack;

public class StaminaManager : MonoBehaviour
{
    [Header("Stamina Settings")]
    [Tooltip("Maximum stamina the player can have")]
    public float maxStamina = 100f;

    [Tooltip("How fast stamina drains while sprinting (per second)")]
    public float staminaDrainRate = 20f;

    [Tooltip("How fast stamina regenerates when not sprinting (per second)")]
    public float staminaRegenRate = 15f;

    [Tooltip("Delay before stamina starts regenerating after sprinting stops")]
    public float regenDelay = 1f;

    [Tooltip("Percentage of stamina required to sprint again (0.3 = 30%)")]
    [Range(0f, 1f)]
    public float sprintRecoveryThreshold = 0.3f;

    [Header("UI References")]
    [Tooltip("Reference to the stamina UI slider")]
    public Slider staminaSlider;

    private float currentStamina;
    private float timeSinceLastSprint;
    private CharacterBehaviour character;
    private bool canSprint = true;
    private Movement movementComponent;
    private float originalRunSpeed;
    private float originalWalkSpeed;

    private void Start()
    {
        currentStamina = maxStamina;

        character = GetComponent<CharacterBehaviour>();
        if (character == null)
        {
            Debug.LogError("StaminaManager: CharacterBehaviour component not found on player!");
        }

        movementComponent = GetComponent<Movement>();
        if (movementComponent != null)
        {
            originalRunSpeed = GetFieldValue<float>(movementComponent, "speedRunning");
            originalWalkSpeed = GetFieldValue<float>(movementComponent, "speedWalking");
        }
        else
        {
            Debug.LogWarning("StaminaManager: Movement component not found!");
        }

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
        else
        {
            Debug.LogWarning("StaminaManager: Stamina Slider not assigned!");
        }
    }

    private void Update()
    {
        if (character == null) return;

        bool isRunning = character.IsRunning();

        if (isRunning && canSprint)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
            timeSinceLastSprint = 0f;

            if (currentStamina <= 0f)
            {
                canSprint = false;
                ForceStopSprinting();
            }
        }
        else
        {
            timeSinceLastSprint += Time.deltaTime;

            if (timeSinceLastSprint >= regenDelay)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);

                if (currentStamina >= maxStamina * sprintRecoveryThreshold)
                {
                    canSprint = true;
                    RestoreRunSpeed();
                }
            }
        }

        if (!canSprint && isRunning)
        {
            ForceStopSprinting();
        }

        UpdateUI();
    }

    private void ForceStopSprinting()
    {
        if (movementComponent != null)
        {
            SetFieldValue(movementComponent, "speedRunning", originalWalkSpeed);
        }
    }

    private void RestoreRunSpeed()
    {
        if (movementComponent != null)
        {
            SetFieldValue(movementComponent, "speedRunning", originalRunSpeed);
        }
    }

    private void UpdateUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    public bool CanSprint()
    {
        return canSprint && currentStamina > 0f;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public float GetStaminaPercent()
    {
        return currentStamina / maxStamina;
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
}

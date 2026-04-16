using UnityEngine;
using UnityEngine.UI;
using InfimaGames.LowPolyShooterPack;

/// <summary>
/// Connects a UI Slider to the CameraLook sensitivity at runtime.
/// </summary>
public class SensitivitySlider : MonoBehaviour
{
    [Tooltip("The slider that controls mouse sensitivity.")]
    [SerializeField] private Slider sensitivitySlider;

    private CameraLook cameraLook;

    private void Awake()
    {
        cameraLook = FindObjectOfType<CameraLook>();

        if (cameraLook == null)
        {
            Debug.LogError("[SensitivitySlider] No CameraLook component found in the scene.");
            return;
        }

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        // Initialise slider to match the current sensitivity.
        sensitivitySlider.value = cameraLook.Sensitivity;
    }

    private void OnDestroy()
    {
        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
    }

    /// <summary>
    /// Called by the slider's onValueChanged event.
    /// </summary>
    private void OnSensitivityChanged(float value)
    {
        cameraLook.Sensitivity = value;
    }
}

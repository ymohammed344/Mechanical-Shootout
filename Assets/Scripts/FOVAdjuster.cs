using UnityEngine;
using UnityEngine.UI;

public class FOVAdjuster : MonoBehaviour
{
    public Camera targetCamera;
    public Slider fovSlider;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (fovSlider != null)
        {
            fovSlider.minValue = 30f;
            fovSlider.maxValue = 90f;
            fovSlider.value = targetCamera.fieldOfView;

            fovSlider.onValueChanged.AddListener(delegate { UpdateFOV(); });
        }
    }

    public void UpdateFOV()
    {
        targetCamera.fieldOfView = fovSlider.value;
    }
}


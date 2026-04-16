using UnityEngine;
using UnityEngine.UI;

public class ToggleImageWithKey : MonoBehaviour
{
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (image != null)
            {
                image.enabled = !image.enabled;
            }
        }
    }
}

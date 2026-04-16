using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[DefaultExecutionOrder(-1000)]
public class FixEventSystemInputModule : MonoBehaviour
{
    void Awake()
    {
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        
        if (eventSystem == null)
        {
            Debug.LogWarning("No EventSystem found in scene. Creating one...");
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystem = eventSystemObj.AddComponent<EventSystem>();
        }

        StandaloneInputModule oldModule = eventSystem.GetComponent<StandaloneInputModule>();
        if (oldModule != null)
        {
            Debug.Log("Replacing StandaloneInputModule with InputSystemUIInputModule");
            Destroy(oldModule);
        }

        InputSystemUIInputModule newModule = eventSystem.GetComponent<InputSystemUIInputModule>();
        if (newModule == null)
        {
            eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            Debug.Log("Added InputSystemUIInputModule to EventSystem");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

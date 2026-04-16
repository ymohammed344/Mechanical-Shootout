using UnityEngine;

public class TabMenuController : MonoBehaviour
{
    public GameObject tabMenuCanvas;

    private bool isPaused = false;

    void Start()
    {
        tabMenuCanvas.SetActive(false);
        LockCursor(true);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        tabMenuCanvas.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; 
            LockCursor(false);  
            DisableInput();     
        }
        else
        {
            Time.timeScale = 1f; 
            LockCursor(true);    
            EnableInput();      
        }
    }

    void LockCursor(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }

    void DisableInput()
    {
      
        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
        {
            if (script.GetType().Name.Contains("Shooter"))
                script.enabled = false;
        }
    }

    void EnableInput()
    {
        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
        {
            if (script.GetType().Name.Contains("Shooter"))
                script.enabled = true;
        }
    }
}

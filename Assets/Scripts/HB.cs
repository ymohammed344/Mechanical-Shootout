using UnityEngine;
using UnityEngine.SceneManagement;

public class HB : MonoBehaviour
{
    public void LoadHomeScene()
    {
    
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    
        SceneManager.LoadScene(0);
    }
}

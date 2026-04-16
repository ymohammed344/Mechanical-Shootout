using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        SceneManager.LoadScene(6); 
        SceneManager.LoadScene(6); 
    }

    public void LoadCTFScene()
    {
        SceneManager.LoadScene(7); 
    }

    public void LoadBackScene()
    {
        SceneManager.LoadScene(0); 
    }

    public void LoadForwardScene()
    {
        SceneManager.LoadScene(2); 
    }

}

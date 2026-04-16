using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton3 : MonoBehaviour
{
    private const string LastSceneKey = "LastSceneIndex";

    void Start()
    {
      
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt(LastSceneKey, currentSceneIndex);
        PlayerPrefs.Save();
    }

    public void LoadHomeScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

       
        if (currentSceneIndex >= 0 && currentSceneIndex <= 5)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
         
            int lastSceneIndex = PlayerPrefs.GetInt(LastSceneKey, 0);
            SceneManager.LoadScene(lastSceneIndex);
        }
    }

    public void Loadmap()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        if (currentSceneIndex >= 1 && currentSceneIndex >= 5)
        {
            SceneManager.LoadScene(1);
        }
        else
        {

            int lastSceneIndex = PlayerPrefs.GetInt(LastSceneKey, 1);
            SceneManager.LoadScene(lastSceneIndex);
        }
    }
}

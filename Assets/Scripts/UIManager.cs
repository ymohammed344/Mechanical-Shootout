using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public void OnHomeButtonPressed()
    {
        SceneManager.LoadScene(0);
    }


    public void OnStoreButtonPressed()
    {
        SceneManager.LoadScene(3);
    }

 
    public void OnSettingsButtonPressed()
    {
        SceneManager.LoadScene(4);
    }


    public void OnInformationButtonPressed()
    {
        SceneManager.LoadScene(5);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class BBButtonLoader : MonoBehaviour
{
    public void OnBBButtonPressed()
    {
        SceneManager.LoadScene(1);
    }
}


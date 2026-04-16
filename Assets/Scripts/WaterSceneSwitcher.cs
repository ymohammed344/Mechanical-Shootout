using UnityEngine;
using UnityEngine.SceneManagement; 

public class WaterSceneSwitcher : MonoBehaviour
{
    public string playerTag = "Player"; 
    public string enemyTag = "Enemy"; 
    public int playerSceneToLoad = 16; 
    public int enemySceneToLoad = 16; 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(playerTag))
        {
            SceneManager.LoadScene(playerSceneToLoad);
        }
        
        else if (other.CompareTag(enemyTag))
        {
            SceneManager.LoadScene(enemySceneToLoad);
        }
    }
}
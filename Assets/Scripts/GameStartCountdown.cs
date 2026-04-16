using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartCountdown : MonoBehaviour
{
    public GameObject countdownTextObject; 
    private TextMeshProUGUI countdownText;

    void Start()
    {
       
        countdownText = countdownTextObject.GetComponent<TextMeshProUGUI>();


        FreezeGameplay(true);


        countdownTextObject.SetActive(true);


        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        int count = 5;
        while (count > 0)
        {
            countdownText.text = $"Game starts in {count}...";
            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        countdownTextObject.SetActive(false);
        FreezeGameplay(false); 
    }

    void FreezeGameplay(bool freeze)
    {
     
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            if (script == this) continue;
            if (script.gameObject.CompareTag("UI")) continue; 
            script.enabled = !freeze;
        }

    
        Rigidbody[] bodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            if (freeze)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
            }
        }
    }
}

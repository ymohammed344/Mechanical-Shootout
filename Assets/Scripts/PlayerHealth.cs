using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;
    public GameObject deathTextUI;

    [Header("UI")]
    public Text shieldTimerText;
    public Text hitText;

    [Header("Settings")]
    public float hitTextDuration = 3f;

    private bool isDead = false;
    private Coroutine hitTextCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (deathTextUI != null) deathTextUI.SetActive(false);
        if (shieldTimerText != null) shieldTimerText.gameObject.SetActive(false);
        if (hitText != null) hitText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ShieldManager.Instance != null && ShieldManager.Instance.IsShieldActive())
        {
            currentHealth = maxHealth;

            if (healthSlider != null)
                healthSlider.value = maxHealth;

            if (shieldTimerText != null)
            {
                shieldTimerText.gameObject.SetActive(true);
                shieldTimerText.text = "Shield: " + Mathf.Ceil(ShieldManager.Instance.GetTimeLeft()) + "s";
            }
        }
        else
        {
            if (shieldTimerText != null && shieldTimerText.gameObject.activeSelf)
                shieldTimerText.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead || (ShieldManager.Instance != null && ShieldManager.Instance.IsShieldActive()))
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth > 0f)
        {
            if (hitTextCoroutine != null)
                StopCoroutine(hitTextCoroutine);

            hitTextCoroutine = StartCoroutine(ShowHitText());
        }

        if (currentHealth <= 0f)
            Die();
    }

    private IEnumerator ShowHitText()
    {
        float healthPercent = currentHealth / maxHealth;

        if (hitText != null)
        {
            hitText.gameObject.SetActive(true);

            if (healthPercent <= 0.3f)
                hitText.text = "Low Health!";
            else
                hitText.text = "";
        }

        yield return new WaitForSeconds(hitTextDuration);

        if (hitText != null)
            hitText.gameObject.SetActive(false);
    }

    void Die()
    {
        isDead = true;

        FreezePlayerCompletely();

        if (deathTextUI != null)
        {
            deathTextUI.SetActive(true);
            Text deathText = deathTextUI.GetComponent<Text>();
            if (deathText != null)
                deathText.text = "You have died! Try again...";
        }

        StartCoroutine(RestartScene());
    }

    void FreezePlayerCompletely()
    {
   
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;


        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }


        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            animator.speed = 0f;    
            animator.enabled = false;
        }

     
        MonoBehaviour[] allScripts = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            if (script != this && script.enabled)
            {
                script.StopAllCoroutines();
                script.enabled = false;
            }
        }
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(13);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShieldPickup"))
        {
            Destroy(other.gameObject);
            Debug.Log("Shield pickup collected.");
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}

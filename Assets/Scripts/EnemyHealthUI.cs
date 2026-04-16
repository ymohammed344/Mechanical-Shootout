using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;
    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SceneLoader");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<SceneLoader>();
            }
            return _instance;
        }
    }

    public void LoadSceneAfterDelay(int sceneIndex, float delay)
    {
        StartCoroutine(LoadAfterDelay(sceneIndex, delay));
    }

    private IEnumerator LoadAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}

public class EnemyHealthUI : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Elements")]
    public Image foregroundImage;
    public Text timerText;

    [Header("Behavior Settings")]
    public float fillSpeed = 2f;
    public float moveSpeed = 2f;

    [Header("Scene Management")]
    public int nextSceneIndex = 12;

    private float targetFillAmount;
    private bool isDead = false;
    private bool deathHandled = false;

    private static bool timerStarted = false;
    private static float timer = 600f;

    private bool followPlayer = false;
    private Transform player;
    private EnemyFollowAndShoot enemyAI;

    void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int lastSceneIndex = PlayerPrefs.GetInt("LastScene", -1);

     
        if (currentSceneIndex != lastSceneIndex)
        {
            timerStarted = false;
            timer = 600f;
            PlayerPrefs.SetInt("LastScene", currentSceneIndex);
            PlayerPrefs.Save();
        }

        currentHealth = maxHealth;
        targetFillAmount = 1f;
        if (foregroundImage != null) foregroundImage.fillAmount = 1f;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyAI = GetComponent<EnemyFollowAndShoot>();

        if (!timerStarted)
        {
            timerStarted = true;
            StartCoroutine(TimerCountdown());
        }
    }

    void Update()
    {
        if (foregroundImage != null)
        {
            foregroundImage.fillAmount = Mathf.Lerp(foregroundImage.fillAmount, targetFillAmount, Time.deltaTime * fillSpeed);

            if (isDead && !deathHandled && Mathf.Abs(foregroundImage.fillAmount - targetFillAmount) < 0.01f)
            {
                deathHandled = true;
                StartCoroutine(HandleDeathSequence());
            }
        }

        if (timerStarted && timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }

        if (followPlayer && player != null && !isDead)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    public void TakeDamage(float damagePercent)
    {
        if (isDead) return;

        float damage = maxHealth * damagePercent;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        targetFillAmount = currentHealth / maxHealth;

        followPlayer = true;

        if (currentHealth <= 0f)
        {
            isDead = true;
        }
    }

    public void TakeHalfDamage()
    {
        TakeDamage(0.5f);
    }

    IEnumerator HandleDeathSequence()
    {

        if (enemyAI != null)
            enemyAI.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = Vector3.zero;

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Stop();
            audio.mute = true;
        }


        yield return new WaitForSeconds(0.5f);


        Destroy(gameObject);


        EnemyEliminated.ShowEnemyEliminated();
    }

    IEnumerator TimerCountdown()
    {
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Debug.Log("Time up! Not all enemies were defeated.");
        SceneLoader.Instance.LoadSceneAfterDelay(nextSceneIndex, 0f);
    }
}

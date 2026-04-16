using UnityEngine;
using TMPro;

public class PlayerGrenadeThrower : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform throwPoint;
    public float throwForce = 15f;

    private GrenadeManager grenadeManager;

    [Header("UI")]
    public TextMeshProUGUI grenadeCountText;

    void Start()
    {
        grenadeManager = GetComponent<GrenadeManager>();
        if (grenadeManager == null)
        {
            Debug.LogError("GrenadeManager component not found on player!");
        }
    }

    void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (grenadeManager != null && grenadeManager.HasGrenades())
            {
                ThrowGrenade();
                grenadeManager.UseGrenade();
            }
            else
            {
                Debug.Log("No grenades available!");
            }
        }
    }

    void ThrowGrenade()
    {
        if (grenadePrefab == null || throwPoint == null)
        {
            Debug.LogError("Grenade prefab or throw point not assigned!");
            return;
        }

        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.VelocityChange);
        }

        Debug.Log("GRENADE THROWN");
    }

    void UpdateUI()
    {
        if (grenadeCountText == null || grenadeManager == null)
            return;

        int count = grenadeManager.GetGrenadeCount();
        int max = grenadeManager.GetMaxGrenades();

        if (count > 0)
        {
            grenadeCountText.gameObject.SetActive(true);
            grenadeCountText.text = $"💣 Grenades: {count}/{max} [Press K]";
        }
        else
        {
            grenadeCountText.gameObject.SetActive(false);
        }
    }
}

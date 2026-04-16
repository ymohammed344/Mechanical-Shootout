using UnityEngine;
using TMPro;

public class FlagPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    public float holdDistance = 2f;
    public float holdHeight = 1.5f;

    [Header("UI Settings")]
    public TextMeshProUGUI pickupPromptText;
    public string pickupMessage = "Press Y to pick up flag";
    public string dropMessage = "Press Y to drop flag";
    public string lockedMessage = "Eliminate all enemies first!";

    private bool isHeld = false;
    private bool isUnlocked = false;

    private Transform player;
    private Renderer flagRenderer;
    private Collider flagCollider;

    private FlagSceneTrigger sceneTrigger;
    private CTFObjectiveManager objectiveManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        sceneTrigger = GetComponent<FlagSceneTrigger>();
        objectiveManager = CTFObjectiveManager.GetInstance();

        flagRenderer = GetComponent<Renderer>();
        flagCollider = GetComponent<Collider>();

   
        SetFlagActive(false);

        if (pickupPromptText != null)
            pickupPromptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

     
        if (!isUnlocked && objectiveManager != null && objectiveManager.AreAllEnemiesDead())
        {
            UnlockFlag();
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool inRange = distance <= pickupRange;

        UpdateUI(inRange);

        if (Input.GetKeyDown(KeyCode.Y))
        {

            if (!isUnlocked)
            {
                ShowLockedMessage();
                return;
            }

            if (!isHeld && inRange)
            {
                PickUpFlag();
            }
            else if (isHeld)
            {
                DropFlag();
            }
        }

        if (isHeld)
        {
            Vector3 holdPosition = player.position + player.forward * holdDistance + Vector3.up * holdHeight;
            transform.position = holdPosition;
        }
    }

    void UnlockFlag()
    {
        isUnlocked = true;
        SetFlagActive(true);
        Debug.Log("Flag unlocked! All enemies eliminated.");
    }

    void SetFlagActive(bool state)
    {
        if (flagRenderer != null)
            flagRenderer.enabled = state;

        if (flagCollider != null)
            flagCollider.enabled = state;
    }

    void UpdateUI(bool inRange)
    {
        if (pickupPromptText == null) return;

        if (!isUnlocked)
        {
            if (inRange)
            {
                pickupPromptText.gameObject.SetActive(true);
                pickupPromptText.text = lockedMessage;
            }
            else
            {
                pickupPromptText.gameObject.SetActive(false);
            }
            return;
        }

        if (isHeld)
        {
            pickupPromptText.gameObject.SetActive(true);
            pickupPromptText.text = dropMessage;
        }
        else if (inRange)
        {
            pickupPromptText.gameObject.SetActive(true);
            pickupPromptText.text = pickupMessage;
        }
        else
        {
            pickupPromptText.gameObject.SetActive(false);
        }
    }

    void ShowLockedMessage()
    {
        if (pickupPromptText == null) return;

        pickupPromptText.gameObject.SetActive(true);
        pickupPromptText.text = lockedMessage;
    }

    void PickUpFlag()
    {
        isHeld = true;
        Debug.Log("Flag picked up!");

        if (objectiveManager != null)
        {
            objectiveManager.OnFlagPickedUp();
        }
    }

    void DropFlag()
    {
        isHeld = false;

        Vector3 dropOrigin = player.position + player.forward * 1.5f + Vector3.up * 2f;

        if (Physics.Raycast(dropOrigin, Vector3.down, out RaycastHit hitInfo, 5f))
        {
            float flagHeight = GetComponent<Renderer>().bounds.size.y;
            transform.position = hitInfo.point + Vector3.up * (flagHeight / 2f + 0.01f);
        }
        else
        {
            transform.position = player.position + player.forward * 1.5f + Vector3.up * 1f;
        }

        Debug.Log("Flag dropped!");

        if (sceneTrigger != null)
        {
            sceneTrigger.OnFlagDropped();
        }
    }
}
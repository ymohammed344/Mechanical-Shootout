using UnityEngine;

public class EnemyShooterHitscan : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    
    [Header("Movement")]
    public float followRange = 15f;
    public float stopDistance = 5f;
    public float speed = 3f;
    public float wanderChangeInterval = 3f;
    public float wanderRadius = 5f;
    
    [Header("Combat")]
    public Transform shootPoint;
    public float shootCooldown = 1.5f;
    public float damageAmount = 10f;
    public float maxShootDistance = 50f;
    
    [Header("Accuracy")]
    [Tooltip("0 = perfect accuracy, 5 = moderate spread, 10 = poor accuracy")]
    [Range(0f, 15f)]
    public float accuracySpread = 3f;
    [Tooltip("Height offset to aim at (1.5 = chest, 1.8 = head)")]
    public float aimHeightOffset = 1.5f;
    [Tooltip("Enable predictive aiming for moving targets")]
    public bool usePredictiveAiming = true;
    
    [Header("Line of Sight")]
    [Tooltip("Layers that block line of sight (walls, obstacles)")]
    public LayerMask lineOfSightBlockers;
    [Tooltip("Show debug visualization in Scene view")]
    public bool showLineOfSightDebug = true;
    
    [Header("Effects")]
    public GameObject muzzleFlashPrefab;
    public float muzzleFlashDuration = 0.1f;
    public LineRenderer bulletTracer;
    public float tracerDuration = 0.1f;
    public GameObject hitEffectPrefab;
    
    [Header("Obstacles")]
    public float obstacleDetectionDistance = 1f;
    public LayerMask obstacleLayer;
    public LayerMask shootingRaycastMask;
    
    [HideInInspector]
    public bool isFrozen = false;

    private AudioSource audioSource;
    private float lastShootTime;
    private Vector3 wanderTarget;
    private float wanderTimer;
    private Vector3 lastPlayerPosition;
    private Vector3 playerVelocity;
    private int velocityFrameCount = 0;
    private const int VELOCITY_TRACKING_FRAMES = 5;
    private bool hasLineOfSight = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastShootTime = -shootCooldown;
        SetNewWanderTarget();
        
        if (player != null)
            lastPlayerPosition = player.position;
            
        if (shootingRaycastMask == 0)
            shootingRaycastMask = ~0;

        if (lineOfSightBlockers == 0)
        {
            lineOfSightBlockers = LayerMask.GetMask("Wall", "Invisible Wall", "Default");
            Debug.LogWarning($"[{gameObject.name}] Line of Sight Blockers not set. Using default: Wall, Invisible Wall, Default");
        }
    }

    void Update()
    {
        if (isFrozen || player == null) return;

        if (usePredictiveAiming)
            TrackPlayerVelocity();

        hasLineOfSight = CheckLineOfSight();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= followRange)
        {
            FollowAndShoot(distanceToPlayer);
        }
        else
        {
            Wander();
        }
    }

    bool CheckLineOfSight()
    {
        if (player == null || shootPoint == null) return false;

        Vector3 targetPoint = player.position + Vector3.up * aimHeightOffset;
        Vector3 directionToPlayer = targetPoint - shootPoint.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, directionToPlayer.normalized, out hit, distanceToPlayer, lineOfSightBlockers))
        {
            if (showLineOfSightDebug)
            {
                Debug.DrawRay(shootPoint.position, directionToPlayer.normalized * hit.distance, Color.red);
            }
            return false;
        }

        if (showLineOfSightDebug)
        {
            Debug.DrawRay(shootPoint.position, directionToPlayer.normalized * distanceToPlayer, Color.green);
        }

        return true;
    }

    void TrackPlayerVelocity()
    {
        velocityFrameCount++;
        
        if (velocityFrameCount >= VELOCITY_TRACKING_FRAMES)
        {
            playerVelocity = (player.position - lastPlayerPosition) / (Time.deltaTime * VELOCITY_TRACKING_FRAMES);
            lastPlayerPosition = player.position;
            velocityFrameCount = 0;
        }
    }

    void FollowAndShoot(float distance)
    {
        Vector3 lookTarget = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookTarget);
        transform.Rotate(0f, 180f, 0f);

        if (distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.Move(direction * speed * Time.deltaTime);
            }
            else
            {
                transform.position += direction * speed * Time.deltaTime;
            }
        }

        if (hasLineOfSight)
        {
            if (audioSource != null && !audioSource.isPlaying)
                audioSource.Play();

            float currentCooldown = (gameObject.name == "Enemy4") ? 5f : shootCooldown;

            if (Time.time - lastShootTime >= currentCooldown)
            {
                ShootRaycast();
                lastShootTime = Time.time;
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderChangeInterval)
        {
            SetNewWanderTarget();
            wanderTimer = 0f;
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        Vector3 moveDirection = direction * (speed * 0.5f) * Time.deltaTime;

        if (!Physics.Raycast(transform.position, direction, obstacleDetectionDistance, obstacleLayer))
        {
            transform.LookAt(new Vector3(wanderTarget.x, transform.position.y, wanderTarget.z));
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.Move(moveDirection);
            }
            else
            {
                transform.position += moveDirection;
            }
        }
        else
        {
            SetNewWanderTarget();
        }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }

    void SetNewWanderTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);
    }

    void ShootRaycast()
    {
        if (shootPoint == null || player == null) return;

        Vector3 targetPosition = CalculateAimPoint();
        Vector3 aimDirection = (targetPosition - shootPoint.position).normalized;
        aimDirection = ApplyAccuracySpread(aimDirection);

        bool hitSomething = Physics.Raycast(shootPoint.position, aimDirection, out RaycastHit hit, maxShootDistance, shootingRaycastMask);

        if (hitSomething)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (ShieldManager.Instance != null && ShieldManager.Instance.IsShieldActive())
                {
                    Debug.Log("Enemy shot blocked by shield!");
                }
                else
                {
                    PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damageAmount);
                        Debug.Log($"Enemy hit player for {damageAmount} damage!");
                    }
                }
            }

            ShowHitEffect(hit.point, hit.normal);
            ShowBulletTracer(shootPoint.position, hit.point);
        }
        else
        {
            Vector3 endPoint = shootPoint.position + aimDirection * maxShootDistance;
            ShowBulletTracer(shootPoint.position, endPoint);
        }

        ShowMuzzleFlash();
    }

    Vector3 CalculateAimPoint()
    {
        Vector3 baseAimPoint = player.position + Vector3.up * aimHeightOffset;

        if (usePredictiveAiming && playerVelocity.magnitude > 0.1f)
        {
            float distanceToPlayer = Vector3.Distance(shootPoint.position, baseAimPoint);
            float timeToReach = distanceToPlayer / 100f;
            baseAimPoint += playerVelocity * timeToReach;
        }

        return baseAimPoint;
    }

    Vector3 ApplyAccuracySpread(Vector3 direction)
    {
        if (accuracySpread <= 0) return direction;

        float spreadX = Random.Range(-accuracySpread, accuracySpread);
        float spreadY = Random.Range(-accuracySpread, accuracySpread);

        Quaternion spread = Quaternion.Euler(spreadX, spreadY, 0);
        return spread * direction;
    }

    void ShowMuzzleFlash()
    {
        if (muzzleFlashPrefab != null && shootPoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
            Destroy(flash, muzzleFlashDuration);
        }
    }

    void ShowBulletTracer(Vector3 start, Vector3 end)
    {
        if (bulletTracer != null)
        {
            LineRenderer tracer = Instantiate(bulletTracer);
            tracer.SetPosition(0, start);
            tracer.SetPosition(1, end);
            Destroy(tracer.gameObject, tracerDuration);
        }
    }

    void ShowHitEffect(Vector3 position, Vector3 normal)
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.LookRotation(normal));
            Destroy(effect, 2f);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (shootPoint != null && player != null)
        {
            Vector3 targetPos = player.position + Vector3.up * aimHeightOffset;
            
            Gizmos.color = hasLineOfSight ? Color.green : Color.red;
            Gizmos.DrawLine(shootPoint.position, targetPos);
            Gizmos.DrawWireSphere(targetPos, 0.2f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}

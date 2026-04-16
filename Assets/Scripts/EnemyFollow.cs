using UnityEngine;

public class EnemyFollowAndShoot : MonoBehaviour
{
    public Transform player;
    public float followRange = 10f;
    public float stopDistance = 2f;
    public float speed = 3f;

    public GameObject bulletPrefab;
    public GameObject alternateProjectilePrefab;
    public Transform shootPoint;
    public float shootCooldown = 1.5f;
    public GameObject muzzleFlashPrefab;

    [Header("Accuracy Settings")]
    [Tooltip("Height offset to aim at (1.5 = chest height, 1.8 = head height)")]
    public float aimHeightOffset = 1.5f;
    [Tooltip("Random spread in degrees (0 = perfect accuracy, 5 = slight spread)")]
    public float accuracySpread = 2f;
    
    [Header("Prediction Settings")]
    [Tooltip("Enable predictive aiming to lead moving targets")]
    public bool usePredictiveAiming = true;
    [Tooltip("Bullet speed - must match actual projectile speed")]
    public float bulletSpeed = 10f;
    [Tooltip("How many frames to track player velocity over")]
    public int velocityTrackingFrames = 5;

    private AudioSource audioSource;
    private float lastShootTime;

    private Vector3 wanderTarget;
    private float wanderTimer;
    public float wanderChangeInterval = 3f;
    public float wanderRadius = 5f;

    public float obstacleDetectionDistance = 1f;
    public LayerMask obstacleLayer;

    [HideInInspector]
    public bool isFrozen = false;

    private Vector3 lastPlayerPosition;
    private Vector3 playerVelocity;
    private int frameCount = 0;

    [Header("Map Boundaries")]
[Tooltip("Enable boundary checking to keep enemies on the map")]
public bool useBoundaries = true;
public Vector3 mapCenter = Vector3.zero;
public Vector3 mapSize = new Vector3(50f, 100f, 50f); 

private void LateUpdate()
{
    if (useBoundaries)
    {
        ClampToBoundaries();
    }
}

void ClampToBoundaries()
{
    Vector3 pos = transform.position;
    
    pos.x = Mathf.Clamp(pos.x, mapCenter.x - mapSize.x / 2, mapCenter.x + mapSize.x / 2);
    pos.z = Mathf.Clamp(pos.z, mapCenter.z - mapSize.z / 2, mapCenter.z + mapSize.z / 2);
    
   
    
    transform.position = pos;
}


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastShootTime = -shootCooldown;
        SetNewWanderTarget();
        
        if (player != null)
            lastPlayerPosition = player.position;
    }

    void Update()
    {
        if (isFrozen) return;

        if (player == null) return;

        if (usePredictiveAiming)
            TrackPlayerVelocity();

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

    void TrackPlayerVelocity()
    {
        frameCount++;
        
        if (frameCount >= velocityTrackingFrames)
        {
            playerVelocity = (player.position - lastPlayerPosition) / (Time.deltaTime * velocityTrackingFrames);
            lastPlayerPosition = player.position;
            frameCount = 0;
        }
    }

    void FollowAndShoot(float distance)
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        transform.Rotate(0f, 180f, 0f);

        if (distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

        if (!audioSource.isPlaying)
            audioSource.Play();

        float currentCooldown = (gameObject.name == "Enemy4") ? 5f : shootCooldown;

        if (Time.time - lastShootTime >= currentCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
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
            transform.position += moveDirection;
        }
        else
        {
            SetNewWanderTarget();
        }

        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    void SetNewWanderTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);
    }

    void Shoot()
    {
        if (shootPoint == null || player == null) return;

        GameObject projectileToShoot = gameObject.name == "Enemy4" ? alternateProjectilePrefab : bulletPrefab;

        if (projectileToShoot == null) return;

        GameObject projectile = Instantiate(projectileToShoot, shootPoint.position, Quaternion.identity);
        
        Vector3 targetPosition;
        
        if (usePredictiveAiming)
        {
            targetPosition = CalculatePredictedPosition();
        }
        else
        {
            targetPosition = player.position + Vector3.up * aimHeightOffset;
        }
        
        Vector3 direction = (targetPosition - shootPoint.position).normalized;
        
        direction = ApplySpread(direction, accuracySpread);
        
        projectile.transform.forward = direction;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
            Destroy(flash, 0.1f);
        }
    }

    Vector3 CalculatePredictedPosition()
    {
        Vector3 currentPlayerPos = player.position + Vector3.up * aimHeightOffset;
        
        float distanceToPlayer = Vector3.Distance(shootPoint.position, currentPlayerPos);
        float timeToReachPlayer = distanceToPlayer / bulletSpeed;
        
        Vector3 predictedPosition = currentPlayerPos + (playerVelocity * timeToReachPlayer);
        
        return predictedPosition;
    }

    Vector3 ApplySpread(Vector3 direction, float spreadAngle)
    {
        if (spreadAngle <= 0) return direction;

        float randomX = Random.Range(-spreadAngle, spreadAngle);
        float randomY = Random.Range(-spreadAngle, spreadAngle);

        Quaternion spread = Quaternion.Euler(randomX, randomY, 0);
        return spread * direction;
    }
}

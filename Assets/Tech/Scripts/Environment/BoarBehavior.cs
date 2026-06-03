using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BoarBehavior : MonoBehaviour
{
    private enum BoarState
    {
        Wandering,
        Grazing,
        Threatening,
        Chasing,
        Windup,
        Charging,
        Recharge,
        Stunned,
        Dead
    }

    [Header("Wandering")]
    [SerializeField] private float wanderRadius = 30f;
    [SerializeField] private float wanderTimer = 10f;
    [SerializeField] private float wanderSpeed = 1f;

    [Header("Chasing")]
    [SerializeField] private float chaseStartDistance = 15f;
    [SerializeField] private float chaseSpeed = 2.5f;
    [SerializeField] private float chaseSpeedIncrease = 0.5f;
    [SerializeField] private float giveUpTime = 4f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 25f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask chargeHitMask;
    [SerializeField] private LayerMask visionObstacleMask;

    [Header("Threat Call")]
    [SerializeField] private float threatCallTime = 1f;

    [Header("Charge")]
    [SerializeField] private float chargeStartDistance = 10f;
    [SerializeField] private float chargeWindupTime = 1f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargePastPlayerDistance = 5f;
    [SerializeField] private float chargeHitRadius = 0.5f;
    [SerializeField] private float afterChargePause = 1f;

    [Header("Health")]
    [SerializeField] private float boarHealth = 3;
    [SerializeField] private float stunTime = 2f;
    [SerializeField] private float deathDelay = 2f;
    private bool isDead;

    [Header("Damage")]
    [SerializeField] private int playerDamage = 25;
    [SerializeField] private float knockbackDistance = 10f;
    [SerializeField] private float knockbackDuration = 2f;
    [SerializeField] private float knockbackUpHeight = 2f;

    [Header("Jungle Area")]
    [SerializeField] private Collider[] jungleAreas;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private BoarSpawner boarSpawner;
    [SerializeField] private BoarSound boarSound;

    [Header("Drops")]
    [SerializeField] private GameObject meatPickupPrefab;

    [SerializeField] private float navMeshCheckDistance = 2f;

    private BoarState currentState;

    private float wanderCounter;
    private float lostSightCounter;

    private bool hasSpottedPlayer = false;
    private Vector3 lastSeenPlayerPosition;

    private Vector3 chargeDirection;
    private Vector3 chargeTarget;

    [Header("Axe Hit Settings")]
    [SerializeField] private bool canBeHitByAxe = true;
    [SerializeField] private float dmgDelay = 1f;
    
    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        if (boarSound == null)
        {
            boarSound = GetComponentInChildren<BoarSound>();
        }

        agent.autoBraking = false;

        wanderCounter = wanderTimer;

        ChangeState(BoarState.Wandering);
    }

    private void Update()
    {
        switch (currentState)
        {
            case BoarState.Wandering:
                UpdateWandering();
                break;

            case BoarState.Grazing:
                UpdateGrazing();
                break;

            case BoarState.Chasing:
                UpdateChasing();
                break;

            case BoarState.Charging:
                UpdateCharging();
                break;
        }

       // UpdateAnimator();
    }

    private void UpdateWandering()
    {
        if (CanSeePlayer() && !hasSpottedPlayer)
        {
            hasSpottedPlayer = true;
            StartCoroutine(ThreatCallThenChase());
            return;
        }

        wanderCounter += Time.deltaTime;

        if (wanderCounter >= wanderTimer)
        {
            SetNewWanderDestination();
            return;
        }

        if (HasReachedDestination())
        {
            ChangeState(BoarState.Grazing);
            boarSound.PlayBoarClip("Idle");
        }
    }

   private void UpdateGrazing()
    {
        if (CanSeePlayer())
        {
            StartCoroutine(ThreatCallThenChase());
            return;
        }

        wanderCounter += Time.deltaTime;

        if (wanderCounter >= wanderTimer)
        {
            ChangeState(BoarState.Wandering);
            SetNewWanderDestination();
        }
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        return true;
    }

    private void SetNewWanderDestination()
    {
        Vector3 newPos = GenerateRandomPoint(transform.position, wanderRadius);
        agent.SetDestination(newPos);
        wanderCounter = 0f;
    }

    private void UpdateChasing()
    {
        if (!IsPlayerInsideJungle())
        {
            ChangeState(BoarState.Wandering);
            return;
        }

        if (CanSeePlayer())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            lostSightCounter = 0f;
            lastSeenPlayerPosition = player.position;

            agent.SetDestination(player.position);

            if (distanceToPlayer <= chargeStartDistance)
            {
                StartCoroutine(WindupThenCharge());
                return;
            }
        }
        else
        {
            lostSightCounter += Time.deltaTime;

            agent.SetDestination(lastSeenPlayerPosition);

            if (lostSightCounter >= giveUpTime)
            {
                ChangeState(BoarState.Wandering);
            }
        }
    }

    private IEnumerator ThreatCallThenChase()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;
        Quaternion targetRotation;

        if (lookDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);
        }

        yield return new WaitForSeconds(0.5f);

        boarSound.PlayBoarClip("Alert");   
        ChangeState(BoarState.Threatening);

        yield return new WaitForSeconds(threatCallTime);

        if (currentState != BoarState.Threatening || isDead)
            yield break;

        if (IsPlayerInsideJungle())
        {
            ChangeState(BoarState.Chasing);
        }
        else
        {
            ChangeState(BoarState.Wandering);
        }
    }

    private IEnumerator WindupThenCharge()
    {
        ChangeState(BoarState.Windup);

        boarSound.PlayBoarClip("Charge");
        float timer = 0f;

        while (timer < chargeWindupTime)
        {
            timer += Time.deltaTime;

            Vector3 lockedPlayerPosition = player.position;
            lockedPlayerPosition.y = transform.position.y;

            chargeDirection = (lockedPlayerPosition - transform.position).normalized;
            chargeTarget = lockedPlayerPosition + chargeDirection * chargePastPlayerDistance;

            if (chargeDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(chargeDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,Time.deltaTime * 8f);
            }

            yield return null;
        }

        if (currentState != BoarState.Windup || isDead)
            yield break;

        ChangeState(BoarState.Charging);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
    }

    private void UpdateCharging()
    {
        float moveDistance = chargeSpeed * Time.deltaTime;

       if (Physics.SphereCast(transform.position + Vector3.up * 0.5f, chargeHitRadius, chargeDirection, out RaycastHit hit, moveDistance, chargeHitMask, QueryTriggerInteraction.Collide))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("BananaPeel"))
            {
                KillBoarByBananaPeel();
                return;
            }

            if (hitObject.CompareTag("Obstacle"))
            {
                HitObstacle();
                return;
            }

            if (hitObject.CompareTag("Player"))
            {
                HitPlayer();
                StartCoroutine(PauseThenChaseAgain());
                return;
            }
        }

        Vector3 nextPosition = transform.position + chargeDirection * moveDistance;

        if (NavMesh.Raycast(transform.position, nextPosition, out NavMeshHit navHit, NavMesh.AllAreas))
        {
            transform.position = navHit.position;
            StartCoroutine(PauseThenChaseAgain());
            return;
        }

        transform.position = nextPosition;

        if (Vector3.Distance(transform.position, chargeTarget) <= 0.5f)
        {
            StartCoroutine(PauseThenChaseAgain());
            return;
        }
    }

    private IEnumerator PauseThenChaseAgain()
    {
        if (!EnableAgentAgain())
            yield break;

        ChangeState(BoarState.Recharge); //Perhaps a different enum

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(afterChargePause);

        agent.isStopped = false;

        if (IsPlayerInsideJungle())
        {
            ChangeState(BoarState.Chasing);
        }
        else
        {
            ChangeState(BoarState.Wandering);
        }
    }

    private void HitObstacle()
    {
        boarSound.PlayBoarClip("HitObstacle");
        anim.SetTrigger("BoarDamaged");
        boarHealth -= 1;

        if (boarHealth <= 0)
        {
            KillBoar();
            return;
        }

        chaseSpeed += chaseSpeedIncrease;

        StartCoroutine(StunBoar());
    }

    private IEnumerator StunBoar()
    {
        if (!EnableAgentAgain())
            yield break;

        ChangeState(BoarState.Stunned);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(stunTime);

        if (!agent.enabled)
            yield break;

        agent.isStopped = false;

        if (IsPlayerInsideJungle())
        {
            ChangeState(BoarState.Chasing);
        }
        else
        {
            ChangeState(BoarState.Wandering);
        }
    }

    private void HitPlayer()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.RemoveHealth(playerDamage);
        }

        if (playerController != null)
        {
            PlayerController playerMovement = playerController.GetComponent<PlayerController>();

            if (playerMovement != null)
            {
                playerMovement.playerVelocity.y = 0f;
            }

            StartCoroutine(KnockbackPlayer(playerMovement));
        }
    }

    private IEnumerator KnockbackPlayer(PlayerController playerMovement)
    {
        playerMovement.canMove = false;
        float timer = 0f;

        Vector3 horizontalDirection = chargeDirection;
        horizontalDirection.y = 0f;
        horizontalDirection.Normalize();

        Vector3 totalHorizontalMovement = horizontalDirection * knockbackDistance;

        float previousHeight = 0f;

        while (timer < knockbackDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / knockbackDuration;

            Vector3 horizontalMove = totalHorizontalMovement * Time.deltaTime / knockbackDuration;

            float currentHeight = Mathf.Sin(progress * Mathf.PI) * knockbackUpHeight;
            float verticalMove = currentHeight - previousHeight;
            previousHeight = currentHeight;

            playerController.Move(horizontalMove + Vector3.up * verticalMove);

            yield return null;
        }
        playerMovement.canMove = true;
    }

    public void HitByAxe()
    {
        if (canBeHitByAxe)
        {
            canBeHitByAxe = false;
            
            boarHealth -= 0.25f;
            anim.SetTrigger("BoarDamaged");

            if (boarHealth <= 0)
            {
                KillBoar();
                return;
            }

            boarSound.PlayBoarClip("HitObstacle");
            StartCoroutine(HitCooldown());
        }
    }

    private IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(dmgDelay);
        canBeHitByAxe = true;
    }

    private void KillBoarByBananaPeel()
    {
        if (isDead)
            return;

        isDead = true;

        EnableAgentAgain();

        ChangeState(BoarState.Dead);

        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (anim != null)
            //anim.SetTrigger("BoarFlip");

        StartCoroutine(DestroyBoarAfterDelay());
    }

    private void KillBoar()
    {
        if (isDead)
            return;

        isDead = true;

        if (EnableAgentAgain())
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        ChangeState(BoarState.Dead);

        StartCoroutine(DestroyBoarAfterDelay());
    }

    private IEnumerator DestroyBoarAfterDelay()
    {
        boarSound.PlayBoarClip("Death");
        yield return new WaitForSeconds(deathDelay);

        if (meatPickupPrefab != null)
        {
            Instantiate(meatPickupPrefab, transform.position, Quaternion.identity);
            boarSound.PlayBoarClip("MeatDrop");
        }

        if (boarSpawner != null)
        {
            boarSpawner.SpawnBoar();
        }

        Destroy(gameObject);
    }

    private void ChangeState(BoarState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        UpdateAnimator();

        if (currentState == BoarState.Wandering)
        {
            if (EnableAgentAgain())
            {
                agent.speed = wanderSpeed;
                agent.isStopped = false;
            }

            hasSpottedPlayer = false;
            lostSightCounter = 0f;
            wanderCounter = wanderTimer;
        }

        else if (currentState == BoarState.Grazing)
        {
            if (EnableAgentAgain())
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        else if (currentState == BoarState.Threatening)
        {
            if (EnableAgentAgain())
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        else if (currentState == BoarState.Chasing)
        {
            if (EnableAgentAgain())
            {
                agent.speed = chaseSpeed;
                agent.isStopped = false;
            }

            lostSightCounter = 0f;

            if (player != null)
                lastSeenPlayerPosition = player.position;
        }

        else if (currentState == BoarState.Windup)
        {
            if (EnableAgentAgain())
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        else if (currentState == BoarState.Charging)
        {
            if (agent.enabled)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        else if (currentState == BoarState.Recharge)
        {
            if (EnableAgentAgain())
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        else if (currentState == BoarState.Stunned)
        {
            if (EnableAgentAgain())
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }

        else if (currentState == BoarState.Dead)
        {
            if (EnableAgentAgain())
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        if (!IsPlayerInsideJungle())
            return false;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance)
            return false;

        if (distanceToPlayer <= chaseStartDistance)
            return true;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer > viewAngle / 2f)
            return false;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.8f;
        Vector3 playerTarget = player.position + Vector3.up * 0.8f;
        Vector3 rayDirection = (playerTarget - rayOrigin).normalized;

        LayerMask combinedMask = playerMask | visionObstacleMask;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, viewDistance, combinedMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPlayerInsideJungle()
    {
        if (player == null)
        {
            return false;
        }

        if (jungleAreas == null || jungleAreas.Length == 0)
        {
            return true;
        }

        foreach (Collider area in jungleAreas)
        {
            if (area == null)
            {
                continue;
            }
            
            if (area.bounds.Contains(player.position))
            {
                return true;
            }
        }

        return false;
    }

    private Vector3 GenerateRandomPoint(Vector3 origin, float dist)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * dist;
            randomDirection += origin;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, dist, NavMesh.AllAreas))
            {
                if (IsPointInsideJungle(navHit.position))
                {
                    return navHit.position;
                }
            }
        }

        return origin;
    }

    private bool IsPointInsideJungle(Vector3 point)
    {
        if (jungleAreas == null || jungleAreas.Length == 0)
        {
            return true;
        }
            

        foreach (Collider area in jungleAreas)
        {
            if (area == null) 
            {
                continue;
            }

            if (area.bounds.Contains(point))
            {
            return true;
            }
        }

        return false;
    }

    private bool EnableAgentAgain()
    {
        if (agent.enabled)
            return true;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit navHit, navMeshCheckDistance, NavMesh.AllAreas))
        {
            transform.position = navHit.position;
            agent.enabled = true;
            return true;
        }

        Debug.LogWarning("Could not place boar back on NavMesh.");
        return false;
    }

    private void UpdateAnimator()
    {
        if (anim == null)
            return;

        int animState = currentState switch {
            BoarState.Wandering => 0,
            BoarState.Grazing => 1,
            BoarState.Threatening => 2,
            BoarState.Chasing => 3,
            BoarState.Windup => 4,
            BoarState.Charging => 5,
            BoarState.Recharge => 6,
            BoarState.Stunned => 7,
            BoarState.Dead => 8,
            _ => 0
        };

        anim.SetInteger("BoarAnimState", animState);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftDirection = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.8f, leftDirection * viewDistance);
        Gizmos.DrawRay(transform.position + Vector3.up * 0.8f, rightDirection * viewDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chargeStartDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseStartDistance);
    }
}
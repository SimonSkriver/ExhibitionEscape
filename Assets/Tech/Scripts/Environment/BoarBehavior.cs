using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BoarBehavior : MonoBehaviour
{
    private enum BoarState
    {
        Wandering,
        Grazing,
        Chasing,
        Windup,
        Charging,
        Stunned,
        Dead
    }

    [Header("Wandering")]
    [SerializeField] private float wanderRadius = 20f;
    [SerializeField] private float wanderTimer = 10f;
    [SerializeField] private float wanderSpeed = 1f;

    [Header("Chasing")]
    [SerializeField] private float chaseSpeed = 2.5f;
    [SerializeField] private float chaseSpeedIncrease = 0.5f;
    [SerializeField] private float giveUpTime = 2f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 20f;
    [SerializeField] private float viewAngle = 50f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask visionObstacleMask;

    [Header("Charge")]
    [SerializeField] private float chargeStartDistance = 10f;
    [SerializeField] private float chargeWindupTime = 1.5f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargePastPlayerDistance = 5f;
    [SerializeField] private float chargeHitRadius = 0.5f;
    [SerializeField] private float afterChargePause = 1f;

    [Header("Health")]
    [SerializeField] private int boarHealth = 3;
    [SerializeField] private float stunTime = 2f;
    [SerializeField] private float deathDelay = 2f;

    [Header("Damage")]
    [SerializeField] private int playerDamage = 25;
    [SerializeField] private float knockbackDistance = 5f;
    [SerializeField] private float knockbackDuration = 1f;

    [Header("Jungle Area")]
    [SerializeField] private Collider[] jungleAreas;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private BoarSpawner boarSpawner;

    [Header("Drops")]
    [SerializeField] private GameObject meatPickupPrefab;

    [SerializeField] private float navMeshCheckDistance = 2f;

    private BoarState currentState;

    private float wanderCounter;
    private float lostSightCounter;

    private Vector3 lastSeenPlayerPosition;

    private Vector3 chargeDirection;
    private Vector3 chargeTarget;
    

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();

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
        if (CanSeePlayer())
        {
            ChangeState(BoarState.Chasing);
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
        }
    }

    private void UpdateGrazing()
    {
        if (CanSeePlayer())
        {
            ChangeState(BoarState.Chasing);
            return;
        }

        wanderCounter += Time.deltaTime;

        if (wanderCounter >= wanderTimer)
        {
            agent.isStopped = false;
            SetNewWanderDestination();
            ChangeState(BoarState.Wandering);
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
            lostSightCounter = 0f;
            lastSeenPlayerPosition = player.position;

            agent.SetDestination(player.position);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

    private IEnumerator WindupThenCharge()
    {
        ChangeState(BoarState.Windup);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        Vector3 lockedPlayerPosition = player.position;
        lockedPlayerPosition.y = transform.position.y;

        chargeDirection = (lockedPlayerPosition - transform.position).normalized;
        chargeTarget = lockedPlayerPosition + chargeDirection * chargePastPlayerDistance;

        float timer = 0f;

        while (timer < chargeWindupTime)
        {
            timer += Time.deltaTime;

            if (chargeDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(chargeDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
            }

            yield return null;
        }

        agent.enabled = false;

        ChangeState(BoarState.Charging);
    }

    private void UpdateCharging()
    {
        float moveDistance = chargeSpeed * Time.deltaTime;

        if (Physics.SphereCast(transform.position + Vector3.up * 0.5f, chargeHitRadius, chargeDirection, out RaycastHit hit, moveDistance))
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
        EnableAgentAgain();

        ChangeState(BoarState.Stunned); //Perhaps a different enum

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
        boarHealth--;

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
        EnableAgentAgain();

        ChangeState(BoarState.Stunned);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(stunTime);

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
            StartCoroutine(KnockbackPlayer());
        }
    }

    private IEnumerator KnockbackPlayer()
    {
        float timer = 0f;

        Vector3 knockbackDirection = chargeDirection.normalized;
        Vector3 knockbackMovement = knockbackDirection * knockbackDistance;

        while (timer < knockbackDuration)
        {
            timer += Time.deltaTime;

            playerController.Move(knockbackMovement * Time.deltaTime / knockbackDuration);

            yield return null;
        }
    }

    private void KillBoarByBananaPeel()
    {
        EnableAgentAgain();

        ChangeState(BoarState.Dead);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        if (anim != null)
            anim.SetTrigger("BoarFlip");

        StartCoroutine(DestroyBoarAfterDelay());
    }

    private void KillBoar()
    {
        EnableAgentAgain();

        ChangeState(BoarState.Dead);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        if (anim != null)
            anim.SetTrigger("Die");

        StartCoroutine(DestroyBoarAfterDelay());
    }

    private IEnumerator DestroyBoarAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);

        if (meatPickupPrefab != null)
        {
            Instantiate(meatPickupPrefab, transform.position, Quaternion.identity);
        }

        if (boarSpawner != null)
        {
            StartCoroutine(boarSpawner.SpawnBoar());
        }

        Destroy(gameObject);
    }

    private void ChangeState(BoarState newState)
    {
        currentState = newState;

        if (currentState == BoarState.Wandering)
        {
            EnableAgentAgain();

            agent.speed = wanderSpeed;
            agent.isStopped = false;

            lostSightCounter = 0f;
            wanderCounter = wanderTimer;
        }

        if (currentState == BoarState.Grazing)
        {
            EnableAgentAgain();

            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (currentState == BoarState.Chasing)
        {
            EnableAgentAgain();

            agent.speed = chaseSpeed;
            agent.isStopped = false;

            lostSightCounter = 0f;

            if (player != null)
                lastSeenPlayerPosition = player.position;
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

    private void EnableAgentAgain()
    {
        if (agent.enabled)
            return;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit navHit, navMeshCheckDistance, NavMesh.AllAreas))
        {
            transform.position = navHit.position;
            agent.enabled = true;
        }
    }

    private void UpdateAnimator()
    {
        if (anim == null)
            return;

        anim.SetBool("IsWalking", currentState == BoarState.Wandering);
        anim.SetBool("IsGrazing", currentState == BoarState.Grazing);
        anim.SetBool("IsChasing", currentState == BoarState.Chasing);
        anim.SetBool("IsPreparingCharge", currentState == BoarState.Windup);
        anim.SetBool("IsCharging", currentState == BoarState.Charging);
        anim.SetBool("IsStunned", currentState == BoarState.Stunned);
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
    }
}
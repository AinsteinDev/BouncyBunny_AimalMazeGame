using UnityEngine;
using UnityEngine.AI;

public class BushMonster : MonoBehaviour
{
    public Transform player;
    public float activationRange = 6f;
    public float attackRange = 1f;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 16f;
    public float idleWiggleSpeed = 2f;
    public float idleWiggleAmount = 0.05f;

    NavMeshAgent agent;

    Vector3 originalScale;

    bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalScale = transform.localScale;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (!chasing && dist <= activationRange)
        {
            chasing = true;
            agent.isStopped = false;
        }

        if (chasing)
            Chase();
        else
            IdleWiggle();
    }

    void Chase()
    {
        if (player == null) return;

        agent.SetDestination(player.position);

        // Shake effect when moving
        float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;

        transform.localScale = originalScale + new Vector3(shake, -shake, shake * 0.5f);

        // Reach player → trigger game over later if you want
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            // For now: reload level
            GameManager.Instance.Retry();
        }
    }

    void IdleWiggle()
    {
        // Slight breathing motion
        float wiggle = Mathf.Sin(Time.time * idleWiggleSpeed) * idleWiggleAmount;

        transform.localScale = originalScale + new Vector3(0, wiggle, 0);
    }

    public void StopMonster()
    {
        activationRange = 0;
        attackRange = 0;

        chasing = false;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }
}

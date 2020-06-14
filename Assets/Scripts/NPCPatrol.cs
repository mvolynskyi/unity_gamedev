using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    [SerializeField]
    bool patrolWaiting;

    [SerializeField]
    float totalWaitTime = 3.0f;
    
    [SerializeField]
    float patrolSwithPribability;
    
    [SerializeField]
    List<Waypoint> patrolPoints;

    NavMeshAgent navMeshAgent;
    int currentPatrolIndex;
    bool isTravelling;
    bool isWaiting;
    bool patrolForward;
    float waitTimer;

    private Animator modelAnimator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(navMeshAgent != null)
        {
            if(patrolPoints.Count > 1)
            {
                currentPatrolIndex = 0;
                SetDestination();
            }
            else
                Debug.Log("You need more Patrol Points!!!");    
        }
        else
            Debug.LogError("Oops navMeshAgent is NULL");

        modelAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         NPSPatrol();
    }

    void NPSPatrol()
    {
        if(navMeshAgent == null || !navMeshAgent.enabled)
            return;

        if(isTravelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            isTravelling = false;

            if(patrolWaiting)
            {
                isWaiting = true;
                waitTimer = 0.0f;
            }
            else
            {
                SetNextPatrolPoint();
                SetDestination();
            }
        }
        
        if(isWaiting)
        {
            waitTimer += Time.deltaTime;

            if(waitTimer >= totalWaitTime)
            {
                isWaiting = false;

                SetNextPatrolPoint();
                SetDestination();
            }
        }

        if(modelAnimator != null)
        {
            modelAnimator.SetInteger("Velocity", Mathf.CeilToInt(Mathf.Max(
                Mathf.Abs(navMeshAgent.velocity.x), Mathf.Abs(navMeshAgent.velocity.z))));
        }
            
    }   


    private void SetDestination()
    {
        Vector3 targetVec = patrolPoints[currentPatrolIndex].transform.position;
        navMeshAgent.SetDestination(targetVec);
        isTravelling = true;
    }

    private void SetNextPatrolPoint()
    {
        if(Random.Range(0.0f, 1.0f) <= patrolSwithPribability)
            patrolForward = !patrolForward;

        if(patrolForward)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        else if(--currentPatrolIndex < 0)
            currentPatrolIndex = patrolPoints.Count - 1;
    }
}

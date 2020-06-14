using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    [SerializeField]
    int health = 100;
    
    private Animator modelAnimator;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidBody;
    
    bool isDead;

    void Start()
    {
        modelAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();

        if(health > 0)
            isDead = false;
    }
        
    public int TakeDamage(int damage)
    {
        if(isDead)
            return -1;

        health -= damage;
        if(health <= 0)
        {
            Die();
            return 0;
        }
        return 1;
    }

    void Die()
    {
        if(modelAnimator != null)
            modelAnimator.enabled = false;

        if(navMeshAgent != null)
            navMeshAgent.enabled = false;

        isDead = true;
        Destroy(gameObject, 15.0f);
    }
}

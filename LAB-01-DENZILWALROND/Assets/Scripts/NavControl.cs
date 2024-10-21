using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavControl : MonoBehaviour
{
    [SerializeField] public Transform target;
    private NavMeshAgent agent;

    bool isWalking = true;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
        if (isWalking)
        {
            agent.destination = target.position;
        }
        else
        {
            transform.forward = Vector3.back;
            agent.destination = target.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Dragon")
        {
            isWalking = false;
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Dragon")
        {
            isWalking = true;
            animator.SetTrigger("Walk");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUPathController : MonoBehaviour
{
    [SerializeField]
    private PathManager pathManager;

    [SerializeField] Animator animator;
    bool isWalking;

    List<Waypoint> thePath;
    Waypoint target;

    public float MoveSpeed;
    public float RotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        isWalking = true;
        animator.SetBool("isWalking", isWalking);
        animator.SetTrigger("CheckWalking");

        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            target = thePath[0];
        }
    }

    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.Pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void MoveForward()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.Pos);
        if (distanceToTarget < stepSize)
        {
            return;
        }
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            rotateTowardsTarget();
            MoveForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isWalking = false;
            animator.SetBool("isWalking", isWalking);
        }
        else if (other.CompareTag("PathB"))
        {
            target = pathManager.GetNextTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isWalking = true;
            animator.SetBool("isWalking", isWalking);
        }
    }
}

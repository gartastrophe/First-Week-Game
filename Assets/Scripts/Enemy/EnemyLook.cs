using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    /*
    //Code from https://github.com/Comp3interactive/FieldOfView. All I did was add a transform for where I want the raycasts to start.
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    //public Transform raycastStart;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = false;
                }
                else
                {
                    canSeePlayer = true;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
    */

    public GameObject playerRef;
    public float chaseDistance = 10f;
    public Transform enemyEyes;
    public float fieldOfView = 45f;

    private void OnDrawGizmos()
    {
        //asked chatgpt to fix the gizmos because the one from class doesnt work :)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftDirection = Quaternion.Euler(0, fieldOfView * .5f, 0) * enemyEyes.forward;
        Vector3 rightDirection = Quaternion.Euler(0, -fieldOfView * .5f, 0) * enemyEyes.forward;

        Vector3 leftRayPoint = enemyEyes.position + (leftDirection * chaseDistance);
        Vector3 rightRayPoint = enemyEyes.position + (rightDirection * chaseDistance);

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }


    public bool IsPlayerInClearFov()
    {
        RaycastHit hit;

        Vector3 directionToPlayer = playerRef.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.DrawLine(enemyEyes.position, playerRef.transform.position, Color.red);
                    return true;
                }

                return false;
            }
            return false;
        }
        return false;
    }
}

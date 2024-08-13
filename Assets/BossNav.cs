using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossNav : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator anim;
    public float bossAnimSpeed = 6f;
    private GameObject player;

    LevelManager levelManager;

    public float attackDistance = 1f;
    float distanceToPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        anim.SetInteger("state", 1);
        anim.speed = bossAnimSpeed;
    }

    private void Update()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        agent.SetDestination(player.transform.position);

        if (distanceToPlayer <= attackDistance)
        {
            levelManager.LevelLost();
            gameObject.SetActive(false);
        }
    }
}

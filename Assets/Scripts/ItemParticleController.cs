using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemParticleController : MonoBehaviour
{
    public Transform player;
    public float proximityRange = 10f;

    ParticleSystem particles;
    float distanceToPlayer;
    
    void Start()
    {
        if(player == null) 
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        particles = GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer < proximityRange)
        {
            particles.Play();
        }
        else 
        {
            particles.Pause();
        }
    }
}

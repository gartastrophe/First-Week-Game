using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    public Transform enemy;
    public float nearProximity = 20f;
    public float immediateProximity = 8f;
    public AudioSource nearSFX;
    public AudioSource immediateSFX;

    float distanceToEnemy;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToEnemy = Vector3.Distance(transform.position, enemy.position);
        AudioControl();
    }

    void AudioControl()
    {
        if (distanceToEnemy < nearProximity) 
        {
            nearSFX.Play();
        }
        if (distanceToEnemy < immediateProximity)
        {
            immediateSFX.Play();
        }
        else
        {
            nearSFX.Pause();
            immediateSFX.Pause();
        }
    }
}

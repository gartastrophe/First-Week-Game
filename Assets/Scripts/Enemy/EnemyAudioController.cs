using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    public Transform enemy;
    public float nearProximity = 20f;
    public float immediateProximity = 8f;
    public AudioClip nearSFX;
    public AudioClip immediateSFX;
    public AudioSource audioSource;

    float distanceToEnemy;

    // Update is called once per frame
    void Update()
    {
        distanceToEnemy = Vector3.Distance(transform.position, enemy.position);
        AudioControl();
    }

    void AudioControl()
    {
        if (distanceToEnemy < immediateProximity)
        {
            audioSource.UnPause();
            if (audioSource.clip != immediateSFX)
            {
                audioSource.clip = immediateSFX;
                audioSource.Play();
            }
        }
        else if (distanceToEnemy < nearProximity)
        {
            audioSource.UnPause();
            if (audioSource.clip != nearSFX)
            {
                audioSource.clip = nearSFX;
                audioSource.Play();
            }
        }
        else if (distanceToEnemy > nearProximity)
        {
            //Debug.Log("audio PAUSED");
            audioSource.Pause();
        }
    }
}

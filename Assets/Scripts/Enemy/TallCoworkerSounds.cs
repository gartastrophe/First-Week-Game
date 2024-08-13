using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallCoworkerSounds : MonoBehaviour
{
    //https://www.youtube.com/watch?v=Bnm8mzxnwP8
    public AudioClip[] footstepSounds;
    public AudioSource audioSource;

    void PlayFootstepSFX()
    {
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}

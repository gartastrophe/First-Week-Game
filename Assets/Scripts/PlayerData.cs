using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static float timeElapsed = 0;

    void Update() {
        timeElapsed += Time.deltaTime;

        Debug.Log("Time:" + timeElapsed);
    }
}

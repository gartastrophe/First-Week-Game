using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{

    public GameObject playerData;
    public static float timeElapsed;

    void Start() {

    }

    void Update() {
        timeElapsed += Time.deltaTime;

        PlayerData.timeElapsed = timeElapsed;
    }
}

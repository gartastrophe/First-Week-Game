using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime : MonoBehaviour
{
    public void Pause() {
        Time.timeScale = 0f;
    }

    public void Unpause() {
        Time.timeScale = 1f;
    }
}

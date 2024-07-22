using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Image staminaBar;

    PlayerControls player;

    void Start()
    {
        player = GetComponentInParent<PlayerControls>();
        staminaBar.fillAmount = player.sprintMaxCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        //https://www.youtube.com/watch?v=ju1dfCpDoF8
        
        staminaBar.fillAmount = player.sprintCapacity / player.sprintMaxCapacity;
    }
}

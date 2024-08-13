using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Image staminaBar;
    public Image staminaBarBackground;

    PlayerControls player;

    public float startingAlpha = .7f;
    public float fadeSpeed = 2f;

    void Start()
    {
        Color staminaBarColor = staminaBar.color;
        Color staminaBarBackgroundColor = staminaBarBackground.color;
        player = GetComponentInParent<PlayerControls>();
        staminaBar.fillAmount = player.sprintMaxCapacity;
        staminaBarColor.a = 0;
        staminaBarBackgroundColor.a = 0;
        staminaBar.color = staminaBarColor;
        staminaBarBackground.color = staminaBarBackgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
        //https://www.youtube.com/watch?v=ju1dfCpDoF8
        staminaBar.fillAmount = player.sprintCapacity / player.sprintMaxCapacity;

        Color staminaBarColor = staminaBar.color;
        Color staminaBarBackgroundColor = staminaBarBackground.color;

        //asked chatgpt how i should fade the stamina bar
        if (player.sprintCapacity >= player.sprintMaxCapacity)
        {
            staminaBarColor.a = Mathf.Lerp(staminaBarColor.a, 0f, fadeSpeed * 3 * Time.deltaTime);
            staminaBarBackgroundColor.a = Mathf.Lerp(staminaBarBackgroundColor.a, 0f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            staminaBarColor.a = Mathf.Lerp(staminaBarColor.a, startingAlpha, fadeSpeed * Time.deltaTime);
            staminaBarBackgroundColor.a = Mathf.Lerp(staminaBarBackgroundColor.a, 1f, fadeSpeed * Time.deltaTime);
        }

        staminaBar.color = staminaBarColor;
        staminaBarBackground.color = staminaBarBackgroundColor;
    }
}

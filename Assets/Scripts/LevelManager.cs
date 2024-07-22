using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Text scoreText;
    public Text directionsText;
    public Text timerText;
    public Text gameOverText;
    public Text gameWonText;

    public float startTime = 300f;

    public PlayerControls playerControls;
    public MouseLook mouseLook;
    public JumpScareController jumpScareController;
    public AudioClip winAudioClip;

    private int score = 0;
    private int totalItems;
    private float timer;
    private bool directionsVisible = true;
    private bool gameOverTriggered = false;
    private bool gameWonTriggered = false;

    private AudioSource audioSource;

    void Start()
    {
        Debug.Log("Game Started");
        InitializeGame();
    }

    void Update()
    {
        if (gameOverTriggered || gameWonTriggered)
        {
            Debug.Log("Update skipped: Game over or won already triggered");
            return;
        }

        UpdateScore();
        CheckWinCondition();

        UpdateTimer();
        CheckLossCondition();

        HandleDirectionsToggle();
    }

    private void InitializeGame()
    {
        timer = startTime;
        score = 0; // Reset the score
        DeliveryHandler.deliveryCount = 0; // Reset the delivery count

        GameObject collectables = GameObject.Find("Collectables");
        if (collectables != null)
        {
            totalItems = collectables.transform.childCount;
            Debug.Log("Total items to collect: " + totalItems);
        }
        else
        {
            Debug.LogError("Collectables object not found!");
        }

        SetScoreText();
        HideGameEndTexts();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void UpdateScore()
    {
        score = DeliveryHandler.deliveryCount;
        Debug.Log("Current score: " + score);
        SetScoreText();
    }

    private void CheckWinCondition()
    {
        if (score >= totalItems && !gameWonTriggered && !gameOverTriggered)
        {
            Debug.Log("Win condition met. Setting gameWonTriggered to true.");
            gameWonTriggered = true;
            LevelBeat();
        }
        else
        {
            Debug.Log("Win condition not met. Score: " + score + ", Total Items: " + totalItems + ", gameWonTriggered: " + gameWonTriggered + ", gameOverTriggered: " + gameOverTriggered);
        }
    }

    private void UpdateTimer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            SetTimerText();
        }
    }

    private void CheckLossCondition()
    {
        if (timer <= 0 && !gameOverTriggered && !gameWonTriggered)
        {
            Debug.Log("Loss condition met");
            timer = 0;
            gameOverTriggered = true;
            LevelLost();
        }
    }

    private void HandleDirectionsToggle()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            directionsVisible = !directionsVisible;
            directionsText.gameObject.SetActive(directionsVisible);
        }
    }

    private void SetScoreText()
    {
        scoreText.text = "Items Delivered: " + score.ToString() + "/" + totalItems.ToString();
    }

    private void SetTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    private void HideGameEndTexts()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
        if (gameWonText != null)
        {
            gameWonText.gameObject.SetActive(false);
        }
    }

    private void LevelLost()
    {
        Debug.Log("LevelLost called");
        gameOverTriggered = true;

        DisablePlayerControls();
        HideUIElements();

        gameOverText.text = "You're Fired!";
        gameOverText.gameObject.SetActive(true);

        if (jumpScareController != null)
        {
            jumpScareController.TriggerJumpScare();
        }

        Invoke("ReloadCurrentLevel", 2);
    }

    private void LevelBeat()
    {
        Debug.Log("LevelBeat called");
        gameWonTriggered = true;

        DisablePlayerControls();
        HideUIElements();

        gameWonText.text = "You've completed your shift!";
        gameWonText.gameObject.SetActive(true);

        if (winAudioClip != null && audioSource != null)
        {
            audioSource.clip = winAudioClip;
            audioSource.Play();
        }

        Invoke("ProceedToNextLevel", 2);
    }

    private void DisablePlayerControls()
    {
        if (playerControls != null)
        {
            playerControls.enabled = false;
        }
        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }
    }

    private void HideUIElements()
    {
        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (directionsText != null) directionsText.gameObject.SetActive(false);
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (gameWonText != null) gameWonText.gameObject.SetActive(false);
    }

    private void ReloadCurrentLevel()
    {
        Debug.Log("Reloading current level");
        gameOverTriggered = false;
        gameWonTriggered = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ProceedToNextLevel()
    {
        Debug.Log("Proceeding to next level");
        // Add your logic for proceeding to the next level here
    }
}

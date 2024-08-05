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

    public float totalTime = 30; 

    public PlayerControls playerControls;
    public MouseLook mouseLook;
    public JumpScareController jumpScareController;
    public AudioClip winAudioClip;

    public string nextSceneName; 

    private int score = 0;
    private int totalItems;
    private float timer;
    private bool directionsVisible = true;
    public bool gameOverTriggered = false;
    private bool gameWonTriggered = false;

    private AudioSource audioSource;

    private int startHour = 9;
    private int endHour = 17; // 5 PM

    [HideInInspector] public bool timerDisabled;

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

        if (!timerDisabled)
        {
            UpdateTimer();
        }

        CheckLossCondition();

        HandleDirectionsToggle();
    }

    private void InitializeGame()
    {
        timer = totalTime; 
        score = 0; 
        DeliveryHandler.deliveryCount = 0; 

        GameObject Collectibles = GameObject.Find("Collectibles");
        if (Collectibles != null)
        {
            totalItems = Collectibles.transform.childCount;
            Debug.Log("Total items to collect: " + totalItems);
        }
        else
        {
            Debug.LogError("Collectibles object not found!");
        }

        SetScoreText();
        HideGameEndTexts();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void UpdateScore()
    {
        score = DeliveryHandler.deliveryCount;
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
        float elapsedTime = (totalTime - timer) / totalTime * (endHour - startHour);
        int currentHour = startHour + Mathf.FloorToInt(elapsedTime);

        string period = currentHour >= 12 ? "PM" : "AM";
        int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
        if (displayHour == 0) displayHour = 12;

        timerText.text = string.Format("{0:0} {1}", displayHour, period);
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

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Level3")
        {
            gameWonText.text = "You have completed your first week!";
            gameWonText.gameObject.SetActive(true);

            if (winAudioClip != null && audioSource != null)
            {
                audioSource.clip = winAudioClip;
                audioSource.Play();
            }

            Invoke("ProceedToNextLevel", 2);
        }
        else
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Invoke("LoadNextScene", 2);
            }
        }
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

    public void ReloadCurrentLevel()
    {
        Debug.Log("Reloading current level");
        gameOverTriggered = false;
        gameWonTriggered = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    private void ProceedToNextLevel()
    {
        Debug.Log("Proceeding to next level");
    }
}

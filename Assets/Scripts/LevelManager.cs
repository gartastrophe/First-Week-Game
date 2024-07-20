using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Text scoreText;
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score = DeliveryHandler.deliveryCount;
        setScoreText();
    }

    void setScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

}

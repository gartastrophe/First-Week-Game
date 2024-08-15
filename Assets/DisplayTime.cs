using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{
    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt.text = PlayerData.timeElapsed.ToString();
    }
}

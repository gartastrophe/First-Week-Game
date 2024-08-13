using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    //private bool lightEnabled = true;

    private Light flickerLight;
    private bool isFlickering = true;
    private float timeDelay;

    public float minTimeDelay = .1f;
    public float maxTimeDelay = 2f;

    //https://www.youtube.com/watch?v=DNMdu3kylec
    //I just incorporated some time delay variables to customize the flickering more

    private void Start()
    {
        flickerLight = gameObject.GetComponent<Light>();
    }

    void Update()
    {
        if (BlackOut.lightEnabled)
        {
            if (isFlickering == true)
            {
                StartCoroutine(FlickerLight());
            }
        } 
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator FlickerLight()
    {
        isFlickering = false;
        GetComponent<Light>().enabled = false;
        timeDelay = Random.Range(minTimeDelay, maxTimeDelay);
        yield return new WaitForSeconds(timeDelay);
        GetComponent<Light>().enabled = true;
        timeDelay = Random.Range(minTimeDelay, maxTimeDelay);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = true;
    }
}

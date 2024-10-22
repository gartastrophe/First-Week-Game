using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class InteractionHandler : MonoBehaviour
{
    public GameObject createdObj;
    public float interactRange = 5f;
    public float instXOff;
    public float instZOff;
    public AudioClip interactionSFX;

    Vector3 spawnPos;
    GameObject interactable;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //perform raycast to check if player is looking at object within interaction range
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange))
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.tag == "CanInteract")
                {
                    interactable = hit.transform.gameObject;

                    Debug.Log("Interacting");
                    //ProcessInteract();
                }
            }
        }
        
    }

    /*
    private void ProcessInteract()
    {
        spawnPos.x = interactable.transform.position.x + instXOff + Random.Range(0.0f, 0.1f);
        spawnPos.y = interactable.transform.position.y;
        spawnPos.z = interactable.transform.position.z + instZOff + Random.Range(0.0f, 0.1f);

        AudioSource.PlayClipAtPoint(interactionSFX, transform.position);
        Instantiate(createdObj, spawnPos, transform.rotation);
        createdObj.SetActive(true);
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Defs for Interactables
    public GameObject createdObj;
    public float instXOff;
    public float instZOff;
    public float numInteractionMax;
    public AudioClip interactionSFX;
    
    Vector3 spawnPos;
    float numInteractionCurrent;

    // Defs for Pickups
    public GameObject player;
    public Transform holdPos;
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private int LayerNumber; //layer index
    private GameObject pickupItem;
    private GameObject interactItem;


    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");

        if(player == null) 
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        numInteractionCurrent = 0;
    }

    
    void Update()
    {
        // Pickup action
        if (Input.GetKeyDown(KeyCode.E) && pickupItem != null)
        {
            if (heldObj == null) //if currently not holding anything
            {
                PickUpObject(pickupItem);
            }
            else
            {
                StopClipping(); //prevents object from clipping through walls
                DropObject();
            }
        }

        // Interact action
        else if (Input.GetKeyDown(KeyCode.Q) && interactItem != null)
        {
            // be sure to add empty game objects as children of collectible to increase score
            if (numInteractionCurrent <= numInteractionMax) //limit the amount of deliverables which can be created.
            {
                ProcessInteract(interactItem);
            }
            else 
            {
                Debug.Log("max interaction reached");
            }
            numInteractionCurrent++;
        }

        // Check if player is holding an object 
        if (heldObj != null)
        {
            MoveObject(); //keep object position at holdPos
        }
    }



    /*  TRIGGER METHODS:
        
        Check what is currently in the cone trigger. 
        Create values to check... 
            pickUpItem: if an item stays in the trigger and it is tagged with "canPickup"
            interactItem: if an item stays in the trigger and it is tagged with "CanInteract"
    */

    void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("canPickUp"))
        {
            pickupItem = collider.GameObject();
        }
        else if(collider.CompareTag("CanInteract"))
        {
            interactItem = collider.GameObject();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("canPickUp"))
        {
            pickupItem = null;
        }
        else if(collider.CompareTag("CanInteract"))
        {
            interactItem = null;
        }
    }

    // The methods PickupObject, DropObject, MoveObject, and StopClipping are copy/paste from JohnDevTutorials github
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }


    void ProcessInteract(GameObject interactableObj)
    {
        spawnPos.x = interactableObj.transform.position.x + instXOff + Random.Range(0.0f, 0.1f);
        spawnPos.y = interactableObj.transform.position.y;
        spawnPos.z = interactableObj.transform.position.z + instZOff + Random.Range(0.0f, 0.1f);

        AudioSource.PlayClipAtPoint(interactionSFX, transform.position);
        Instantiate(createdObj, spawnPos, transform.rotation);
        createdObj.SetActive(true);
    }
}

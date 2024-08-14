using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    CharacterController controller;
    Vector3 input, moveDirection;

    //[Header("PlayerSpeed")]
    public float speed = 3f;

    //[Header("PlayerJump")]
    public float jumpHeight = 3f;
    public float gravity = 30f;
    public float airControl = 10f;

    //[Header("PlayerSprint")]
    public float sprintSpeed = 1.5f;
    public float sprintMaxCapacity = 100f;
    public float sprintDrainSpeed = 30f;
    public float sprintRegenSpeed = 20f;

    public AudioClip jumpSFX;
    public AudioSource walkingSFX;
    public AudioSource runningSFX;

    [HideInInspector] public float sprintCapacity;
    private bool canRegenSprint = true;
    private bool canSprint = true;
    private bool sprintToggle;

    private Coroutine regenerateCoroutine;

    void Start()
    {
        sprintCapacity = sprintMaxCapacity;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerInput();
        MovePlayer();
        AudioControl();
    }

    void PlayerInput()
    {
        //Get player input here
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        sprintToggle = Input.GetKey(KeyCode.LeftShift);

        input = (transform.right * moveHorizontal + transform.forward * moveVertical);

        //Normalize the input vector so you don't go faster when holding two input directions. Used ChatGPT to help me figure that out.
        if (input.magnitude > 1)
        {
            input.Normalize();
        }

        //If you're holding sprint button and you're moving, sprint.
        if (sprintToggle && controller.velocity.magnitude > 0f)
        {
            SprintControl();
            canRegenSprint = false;
        }  
        //If not holding sprint button, start coroutine for 2 seconds so you can regenerate stamina again.
        else
        {
            if (regenerateCoroutine == null)
            {
                regenerateCoroutine = StartCoroutine(ResetStamina());
            }
        }

        if (canRegenSprint)
        {
            StaminaRegeneration();
        }



        //Scale input by speed
        input *= speed;
    }

    void MovePlayer()
    {
        JumpControl();

        //General movement behavior
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void JumpControl()
    {
        if (controller.isGrounded)
        {
            moveDirection = input;

            //Jump behavior
            if (Input.GetButton("Jump"))
            {
                //Velocity
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            //Controls movement in air
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }
    }
    
    void SprintControl()
    {
        //Asked ChatGPT for help restarting the coroutine properly.
        if (regenerateCoroutine != null)
        {
            StopCoroutine(regenerateCoroutine);
            regenerateCoroutine = null;
        }

        if (canSprint)
        {
            input *= sprintSpeed;
        }
        sprintCapacity -= Time.deltaTime * sprintDrainSpeed;
        if (sprintCapacity <= 0)
        {
            sprintCapacity = 0;
            canSprint = false;
        } 
        else
        {
            canSprint = true;
        }
    }

    IEnumerator ResetStamina()
    {
        yield return new WaitForSeconds(2f);
        canRegenSprint = true;
    }

    void StaminaRegeneration()
    {
        if (sprintCapacity < sprintMaxCapacity)
        {
            sprintCapacity += Time.deltaTime * sprintRegenSpeed;
            if (sprintCapacity  > sprintMaxCapacity)
            {
                sprintCapacity = sprintMaxCapacity;
            }
        }
    }

    void AudioControl()
    {
        if (controller.collisionFlags != CollisionFlags.Below)
        {
            // if sprint not toggled and controller grounded, play walking
            if (!sprintToggle) 
            {
                runningSFX.Pause();
                walkingSFX.Play();
            }
            // if sprint toggled and controller grounded, play running
            else if (sprintToggle)
            {
                walkingSFX.Pause();
                runningSFX.Play();
            }
            else
            {
                walkingSFX.Pause();
                runningSFX.Pause();
            }
        }
    }
}

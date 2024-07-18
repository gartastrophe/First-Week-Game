using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 input, moveDirection;

    public float jumpHeight = 3f;
    public float speed = 10f;
    public float gravity = 30f;
    public float airControl = 10f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerInput();
        MovePlayer();
    }

    void PlayerInput()
    {
        //Get player input here
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical);

        //Normalize the input vector so you don't go faster when holding two input directions. Used ChatGPT to help me figure that out.
        if (input.magnitude > 1)
        {
            input.Normalize();
        }

        //Scale input by speed
        input *= speed;
    }

    void MovePlayer()
    {
        if (controller.isGrounded)
        {
            moveDirection = input;

            //Jump behavior
            if (Input.GetButton("Jump"))
            {
                //Velocity
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
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

        //General movement behavior
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}

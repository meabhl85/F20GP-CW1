using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsPlayerMovement : MonoBehaviour
{
    //Movement variables
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;

    //Ground Check
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;


    Vector3 velocity; 

    public float jumpHeight = 3f;
    
    public Score score;

    public bool playerEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if (playerEnabled)
        {
            //Check for ground
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            //Get user input
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            //Jump check
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);


            //Check for falling of edge
            if (!isGrounded && transform.position.y < 50f)
            {
                //Update score
                int endScore = score.getScore();

                //Send score to game manager end screen
                FindObjectOfType<GameManager>().EndGame(endScore);

            }

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_Movement : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;

    public Transform player;


    public float runSpeed = 20f;

    float horizontalMove = 0f;

    public float stopDistance;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetFloat("Speed", horizontalMove);
    }

    // Update is called once per frame
    void Update()
    {
        // MOVE AND SHOOT LOGIC
        if (Vector2.Distance(player.position, transform.position) > (stopDistance - 4) && Vector2.Distance(player.position, transform.position) < (stopDistance + 4))
        {
            horizontalMove = 0;
            animator.SetFloat("Speed", 0f);
        }

        else if (Vector2.Distance(player.position, transform.position) > stopDistance + 4)
        {
            horizontalMove = -1 * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }
        else if (Vector2.Distance(player.position, transform.position) < stopDistance - 4)
        {
            horizontalMove = runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }
         
        // horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
       
    }

    void FixedUpdate()
    {
        // Move  character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false, false);
        
    }





}

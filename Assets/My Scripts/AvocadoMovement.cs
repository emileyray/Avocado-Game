using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvocadoMovement : MonoBehaviour{

    public AvocadoController controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    bool jump = false;
    bool onThorn = false;
    bool isClimbing = false;

    public float GetHorizontalDirection()
    {
        return horizontalMove/runSpeed;
    }

    public float GetVerticalDirection()
    {
        return verticalMove / (runSpeed/1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed/1.5f;

        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            if (onThorn)
            {
                jump = false;
                onThorn = false;
            }
            else
            {
                jump = true;
                animator.SetBool("isJumping", true);
            } 

            if (isClimbing)
            {
                animator.SetBool("isClimbing", true);
            }
        }

    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void SetOnThorn(bool newOnThorn)
    {
        onThorn = newOnThorn;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump, isClimbing);
        jump = false;
        onThorn = false;
        isClimbing = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            isClimbing = true;
        } 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region Movement Variables
    [SerializeField]
    [Tooltip("The maximum speed at which the player can move.")]
    private float maxSpeed;

    [SerializeField]
    [Tooltip("The rate at which the player accelerates in the direction they are moving.")]
    private float acceleration;

    private int facingDirection = 0;
    private bool moving = false;
    #endregion

    #region Components
    private Rigidbody2D playerRB;
    private Animator animator;
    #endregion

    #region Menu Variables
    private bool menuOpen; 
    #endregion

    #region Unity functions
    private void Start() {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        menuOpen = false;
    }

    private void Update()
    {
        CheckVelocity();
    }

    private void FixedUpdate()
    {
        if (!menuOpen)
            DoMovement();
    }
    #endregion

    #region Movement functions
    private void DoMovement() {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        int oldDirection = facingDirection;

        if (xInput > 0) {
            facingDirection = 3;
        } else if (xInput < 0) {
            facingDirection = 1;
        } else if (yInput > 0) {
            facingDirection = 2;
        } else if (yInput < 0) {
            facingDirection = 0;
        }

        bool oldMoving = moving;
        moving = xInput != 0 || yInput != 0;

        animator.SetBool("Moving", moving);
        animator.SetInteger("FacingDirection", facingDirection);
        animator.SetFloat("Speed", playerRB.velocity.magnitude / maxSpeed);
        if (oldDirection != facingDirection || oldMoving != moving) animator.SetTrigger("ChangeMode");

        Vector2 direction = new Vector2(xInput, yInput);
        direction.Normalize();

        playerRB.AddForce(direction * acceleration, ForceMode2D.Impulse);
    }

    private void CheckVelocity() {
        if (playerRB.velocity.magnitude > maxSpeed) {
            playerRB.velocity = playerRB.velocity.normalized * maxSpeed;
        }
    }
    #endregion

    #region Menu Functions
    public void SetMenuOpen(bool b)
    {
        menuOpen = b;
    }
    #endregion

}

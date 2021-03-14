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
    #endregion

    #region Components
    private Rigidbody2D playerRB;
    #endregion


    #region Unity functions
    private void Start() {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckVelocity();
    }

    private void FixedUpdate()
    {
        DoMovement();
    }
    #endregion


    #region Movement functions
    private void DoMovement() {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
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

    
}

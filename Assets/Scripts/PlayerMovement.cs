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

    #region Attack Variables
    [SerializeField]
    [Tooltip("The damage dealt by the currently equipped weapon.")]
    private int damage;
    [SerializeField]
    [Tooltip("The amount of time player must wait after attacking before attacking again.")]
    private float cooldown;
    private float attackTimer;
    [SerializeField]
    [Tooltip("The distance from the player that the currently equipped weapon can reach when attacking.")]
    private float reach;
    private bool isAttacking;
    #endregion


    #region Components
    private Rigidbody2D playerRB;
    #endregion


    #region Unity functions
    private void Start() {
        playerRB = GetComponent<Rigidbody2D>();
        attackTimer = 0;
        isAttacking = false;
    }

    private void Update() {
        CheckVelocity();
        UpdateCooldown();
    }

    private void FixedUpdate() {
        DoMovement();
        DoAttack();
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

    #region Attack functions
    private void DoAttack() {
        float attackInput = Input.GetAxis("Fire1");
        if (attackInput == 0 || isAttacking) {
            return;
        } else if (attackTimer > 0) { // Yes, this else if can be merged with the above if, I just have it to debug cooldowns for now.
            Debug.Log("On Cooldown!");
            return;
        } else {
            Debug.Log("Fire1");
            attackTimer = cooldown;
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine() {
        isAttacking = true;

        RaycastHit2D hit = Physics2D.Raycast(playerRB.position, new Vector2(0f, 1f), reach, LayerMask.GetMask("Enemy"));

        if (hit.transform != null) {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy")) {
                hit.transform.GetComponent<Enemy>().takeDamage(damage);
            }
        }
        isAttacking = false;

        yield return null;
    }

    private void UpdateCooldown() {
        if (attackTimer > 0 && !isAttacking) {
            attackTimer -= Time.deltaTime;
        }
    }
    #endregion
}

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
    private Vector2 mousePos;
    [SerializeField]
    [Tooltip("The camera. Used to track mouse position.")]
    private Camera cam;
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
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
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
        Vector2 direction = mousePos - playerRB.position;

        Vector2 cardinalDirection = getCardinal(direction);

        RaycastHit2D hit = Physics2D.Raycast(playerRB.position, cardinalDirection, reach, LayerMask.GetMask("Enemy"));
        Debug.DrawRay(playerRB.position, direction, Color.blue, 10.0f, false); // For debugging purposes
        Debug.DrawRay(playerRB.position, cardinalDirection, Color.red, 10.0f, false); // For debugging purposes

        if (hit.transform != null) {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy")) {
                hit.transform.GetComponent<Enemy>().takeDamage(damage);
            }
        }
        isAttacking = false;

        yield return null;
    }

    private Vector2 getCardinal(Vector2 dir) {
        float angle = Mathf.Atan2(dir.y, dir.x);
        float octant = Mathf.Round( 8 * angle / (2*Mathf.PI) + 8 ) % 8;
        if (octant == 0) {
            // East
            return new Vector2(1,0);
        } else if (octant == 1) {
            // Northeast
            return new Vector2(1, 1);
        } else if (octant == 2) {
            // North
            return new Vector2(0, 1);
        } else if (octant == 3) {
            // Northwest
            return new Vector2(-1, 1);
        } else if (octant == 4) {
            // West
            return new Vector2(-1, 0);
        } else if (octant == 5) {
            // Southwest
            return new Vector2(-1, -1);
        } else if (octant == 6) {
            // South
            return new Vector2(0, -1);
        } else if (octant == 7) {
            // Northeast
            return new Vector2(1, -1);
        }
        return Vector2.zero;
    }

    private void UpdateCooldown() {
        if (attackTimer > 0 && !isAttacking) {
            attackTimer -= Time.deltaTime;
        }
    }
    #endregion
}

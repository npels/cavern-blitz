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

    #region Mining Variables
    private bool isMining;
    public bool canMine;
    [SerializeField]
    [Tooltip("The amount of time player must wait after mining before mining again.")] // Right now it won't let you attack or mine again until the attack cooldown and mining cooldown is finished.
    private float miningCooldown;
    #endregion


    #region Components
    private Rigidbody2D playerRB;
    #endregion


    #region Unity functions
    private void Start() {
        playerRB = GetComponent<Rigidbody2D>();
        attackTimer = 0;
        isAttacking = false;
        isMining = false;
        miningCooldown = 1;
    }

    private void Update() {
        CheckVelocity();
        UpdateCooldown();
    }

    private void FixedUpdate() {
        DoMovement();
        DoAttack();
        DoMining();
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
        if (attackInput == 0 || isAttacking || isMining) {
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

    #region Mining Functions
    private void DoMining()
    {
        float miningInput = Input.GetAxis("Fire2");
        if (!canMine || miningInput == 0 || isMining || isAttacking)
        {
            return;
        }
        else
        {
            StartCoroutine(MiningRoutine());
        }
    }

    IEnumerator MiningRoutine()
    {
        isMining = true;
        Debug.Log("You are now Mining");
        yield return new WaitForSeconds(miningCooldown);
        Debug.Log("Finished mining");
        isMining = false;
        yield return null;
    }

    #endregion
}

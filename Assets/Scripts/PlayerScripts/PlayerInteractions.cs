using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    // Temporary variables, should be removed later
    public Image attackCooldown;
    public Image mineCooldown;
    private float mineTimer;

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

    #region Mining Variables
    private bool isMining;
    [SerializeField]
    [Tooltip("The amount of time player must wait after mining before mining again.")]
    private float miningCooldown;
    private float miningReach; // The distance from the player that the currently equipped pickaxe can reach
    private int pickaxeDamage; // The damage of the currently equipped pickaxe 
    #endregion

    #region Inventory Vars
    private bool menuOpen;
    #endregion

    #region Components
    private Rigidbody2D playerRB;
    #endregion


    #region Unity functions
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        attackTimer = 0;
        isAttacking = false;
        isMining = false;
        miningReach = 1;
        pickaxeDamage = 1;
        menuOpen = false;
        mineTimer = 0;
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void FixedUpdate()
    {
        DoAttack();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        DoMining();
    }
    #endregion

    #region Attack functions
    private void DoAttack()
    {
        float attackInput = Input.GetAxis("Fire1");
        if (attackInput == 0 || isAttacking || isMining || attackTimer > 0 || menuOpen)
        {
            return;
        }
        else
        {
            Debug.Log("Fire1");
            attackTimer = cooldown;
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        Vector2 direction = mousePos - playerRB.position;

        Vector2 cardinalDirection = getCardinal(direction);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + (cardinalDirection * 0.5f), new Vector2(0.3f, 0.3f), 0, cardinalDirection, reach, LayerMask.GetMask("Enemy"));
        Debug.DrawRay(playerRB.position, direction, Color.blue, 10.0f, false); // For debugging purposes
        Debug.DrawRay(playerRB.position, cardinalDirection, Color.red, 10.0f, false); // For debugging purposes
        foreach (RaycastHit2D hit in hits) {
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.GetComponent<Enemy>().takeDamage(damage);
                }
            }
        }
        
        isAttacking = false;

        yield return null;
    }

    private Vector2 getCardinal(Vector2 dir)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        //Debug.Log(angle);
        if (angle >= 45 && angle < 135)
        {
            // North
            return new Vector2(0, 1);
        }
        else if (angle >= 135 || angle < -135)
        {
            // West
            return new Vector2(-1, 0);
        }
        else if (angle >= -135 && angle < -45)
        {
            // South
            return new Vector2(0, -1);
        }
        else if (angle >= -45 || angle < 45)
        {
            // East
            return new Vector2(1, 0);
        }
        return Vector2.zero;
    }

    private void UpdateCooldown()
    {
        if (attackTimer > 0 && !isAttacking)
        {
            attackTimer -= Time.deltaTime;
            attackCooldown.rectTransform.sizeDelta = new Vector2(100, 100 * attackTimer / cooldown);
        }
        if (mineTimer > 0)
        {
            mineTimer -= Time.deltaTime;
            mineCooldown.rectTransform.sizeDelta = new Vector2(100, 100 * mineTimer / miningCooldown);
        }
    }
    #endregion

    #region Mining Functions
    private void DoMining()
    {
        float miningInput = Input.GetAxis("Fire2");
        if (miningInput == 0 || isMining || isAttacking || menuOpen)
        {
            return;
        }
        else if (mineTimer > 0)
        {
            Debug.Log("On Cooldown!");
            return;
        }
        else
        {
            mineTimer = miningCooldown;
            StartCoroutine(MiningRoutine());
        }
    }

    IEnumerator MiningRoutine()
    {
        isMining = true;

        Vector2 direction = mousePos - playerRB.position;

        Vector2 cardinalDirection = getCardinal(direction);

        RaycastHit2D hit = Physics2D.Raycast(playerRB.position, cardinalDirection, miningReach, LayerMask.GetMask("Environment"));
        Debug.DrawRay(playerRB.position, direction, Color.black, 10.0f, false); // For debugging purposes
        Debug.DrawRay(playerRB.position, cardinalDirection, Color.green, 10.0f, false); // For debugging purposes

        if (hit.transform != null)
        {
            Ore ore = hit.transform.GetComponent<Ore>();
            ore.TakeDamage(pickaxeDamage);
        }

        yield return new WaitForSeconds(miningCooldown);
        isMining = false;
        yield return null;
    }
    #endregion

    #region Inventory Functions
    public void SetMenuOpen(bool b)
    {
        menuOpen = b;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour {
    private Image attackCooldown;
    private Image mineCooldown;
    private TMPro.TextMeshProUGUI oreText;
  
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
    [SerializeField]
    [Tooltip("The width of the boxcast hitbox of the player's weapon.")]
    private float width;
    private bool isAttacking;
    private Vector2 mousePos;
    #endregion

    #region Health variables
    [SerializeField]
    [Tooltip("The maximum full health of the player.")]
    private float maxHealth;
    private float currentHealth;
    #endregion

    #region Mining Variables
    private bool isMining;
    [SerializeField]
    [Tooltip("The amount of time player must wait after mining before mining again.")]
    private float miningCooldown;
    [SerializeField]
    [Tooltip("The offset from which the pickaxe ray is cast when mining.")]
    private Vector2 miningOffset;
    private float miningReach; // The distance from the player that the currently equipped pickaxe can reach
    private int pickaxeDamage; // The damage of the currently equipped pickaxe 
    #endregion

    #region Inventory Vars
    private bool inventoryOpen;

    public Item attackItem;
    public Item mineItem;
    #endregion

    #region Components
    private Rigidbody2D playerRB;
    private PlayerMovement playerMovement;
    private Animator animator;
    #endregion

    #region Unity functions
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        attackTimer = 0;
        isAttacking = false;
        isMining = false;
        miningReach = 1;
        pickaxeDamage = 1;

        currentHealth = maxHealth;

        attackCooldown = GameManager.instance.uiManager.attackCooldown;
        mineCooldown = GameManager.instance.uiManager.mineCooldown;
        oreText = GameManager.instance.uiManager.oreText;
        
        inventoryOpen = false;
        mineTimer = 0;
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void FixedUpdate()
    {
        DoAttack();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DoMining();
    }
    #endregion

    #region Attack functions
    private void DoAttack()
    {
        float attackInput = Input.GetAxis("Fire1");
        if (attackInput == 0 || isAttacking || isMining || attackTimer > 0 || inventoryOpen)
        {
            return;
        }
        else
        {
            Debug.Log("Fire1");
            attackTimer = (cooldown - PlayerAttributes.attackSpeedBonus);
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        GetComponent<Animator>().SetTrigger("Swing");

        playerMovement.canMove = false;

        Vector2 direction = mousePos - playerRB.position;

        Vector2 cardinalDirection = getCardinal(direction);

        int facingDirection = 0;

        if (cardinalDirection.x > 0) {
            facingDirection = 3;
        } else if (cardinalDirection.x < 0) {
            facingDirection = 1;
        } else if (cardinalDirection.y > 0) {
            facingDirection = 2;
        } else if (cardinalDirection.y < 0) {
            facingDirection = 0;
        }

        animator.SetInteger("FacingDirection", facingDirection);
        animator.SetTrigger("ChangeMode");
        animator.SetTrigger("Swing");

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = attackItem.sprite;
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position, new Vector2(width, width), 0, cardinalDirection, reach, LayerMask.GetMask("Enemy"));

        // DEBUG ---------------------
        cardinalDirection.Normalize();
        cardinalDirection = cardinalDirection*(reach + width/2);
        Vector2 ortho = new Vector2(cardinalDirection.y, cardinalDirection.x);
        ortho.Normalize();
        ortho = ortho * width/2;
        Color col = Color.blue;
        Debug.DrawRay(playerRB.position + ortho, cardinalDirection, col, 10.0f, false);
        Debug.DrawRay(playerRB.position - ortho, cardinalDirection, col, 10.0f, false);
        Debug.DrawRay(playerRB.position + cardinalDirection, ortho, col, 10.0f, false);
        Debug.DrawRay(playerRB.position + cardinalDirection, -ortho, col, 10.0f, false);
        Debug.DrawRay(playerRB.position, ortho, col, 10.0f, false);
        Debug.DrawRay(playerRB.position, -ortho, col, 10.0f, false);
        // DEBUG ----------------------

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
            attackCooldown.rectTransform.sizeDelta = new Vector2(20, 20 * attackTimer / (cooldown - PlayerAttributes.attackSpeedBonus));
            if (attackTimer <= 0) playerMovement.canMove = true;
        }
        if (mineTimer > 0)
        {
            mineTimer -= Time.deltaTime;
            mineCooldown.rectTransform.sizeDelta = new Vector2(20, 20 * mineTimer / (miningCooldown - PlayerAttributes.miningSpeedBonus));
        }
    }
    #endregion

    #region Health Functions
    public void takeDamage(int dmg) {
        Debug.Log("Damage taken!");
        currentHealth -= dmg / (1 + PlayerAttributes.armorValue);
        if (currentHealth <= 0) {
            gameObject.SetActive(false);
            GameManager.instance.PlayerDie();
        } else {
            StartCoroutine(DamageFlash());
            GameManager.instance.uiManager.SetHealth(currentHealth / maxHealth);
        }
    }

    public IEnumerator DamageFlash() {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Mining Functions
    private void DoMining()
    {
        float miningInput = Input.GetAxis("Fire2");
        if (miningInput == 0 || isMining || isAttacking || inventoryOpen)
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
            mineTimer = (miningCooldown - PlayerAttributes.miningSpeedBonus);
            StartCoroutine(MiningRoutine());
        }
    }

    IEnumerator MiningRoutine()
    {
        isMining = true;

        Vector2 direction = mousePos - playerRB.position;

        Vector2 cardinalDirection = getCardinal(direction);

        int facingDirection = 0;

        if (cardinalDirection.x > 0) {
            facingDirection = 3;
        } else if (cardinalDirection.x < 0) {
            facingDirection = 1;
        } else if (cardinalDirection.y > 0) {
            facingDirection = 2;
        } else if (cardinalDirection.y < 0) {
            facingDirection = 0;
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mineItem.sprite;

        animator.SetInteger("FacingDirection", facingDirection);
        animator.SetTrigger("ChangeMode");
        animator.SetTrigger("Swing");

        RaycastHit2D hit = Physics2D.Raycast(playerRB.position + miningOffset, cardinalDirection, miningReach, LayerMask.GetMask("Environment"));
        Debug.DrawRay(playerRB.position + miningOffset, direction, Color.black, 10.0f, false); // For debugging purposes
        Debug.DrawRay(playerRB.position + miningOffset, cardinalDirection, Color.green, 10.0f, false); // For debugging purposes

        if (hit.transform != null)
        {
            Ore ore = hit.transform.GetComponent<Ore>();
            ore.TakeDamage(pickaxeDamage);
        }

        playerMovement.canMove = false;

        yield return new WaitForSeconds((miningCooldown - PlayerAttributes.miningSpeedBonus));
        isMining = false;
        playerMovement.canMove = true;
        yield return null;
    }
    #endregion

    #region Inventory Functions
    public void SetMenuOpen(bool b)
    {
        inventoryOpen = b;
    }
    #endregion
}

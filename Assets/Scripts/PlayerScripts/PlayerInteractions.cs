using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour {
    private Image leftCooldown;
    private Image rightCooldown;
    private TMPro.TextMeshProUGUI oreText;
  
    private float mineTimer;

    #region Attack Variables
    [SerializeField]
    [Tooltip("The damage dealt by the currently equipped weapon.")]
    private int damage;
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
    private bool invulnerable = false;
    #endregion

    #region Mining Variables
    private bool isMining;
    [SerializeField]
    [Tooltip("The offset from which the pickaxe ray is cast when mining.")]
    private Vector2 miningOffset;
    private float miningReach; // The distance from the player that the currently equipped pickaxe can reach
    #endregion

    #region Inventory Vars
    private bool inventoryOpen;
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

        currentHealth = maxHealth;

        leftCooldown = GameManager.instance.uiManager.leftCooldown;
        rightCooldown = GameManager.instance.uiManager.rightCooldown;
        
        inventoryOpen = false;
        mineTimer = 0;
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HandleClicks();
    }
    #endregion

    #region Attack functions
    private void HandleClicks()
    {
        float leftInput = Input.GetAxis("Fire1");
        if (PlayerAttributes.leftHand != null && leftInput > 0) {
            if (PlayerAttributes.leftHand.type == ToolItem.ToolType.WEAPON) {
                if (leftInput != 0 && !isAttacking && !isMining && attackTimer <= 0 && !inventoryOpen) {
                    attackTimer = (PlayerAttributes.attackSpeed - PlayerAttributes.attackSpeedBonus);
                    StartCoroutine(AttackRoutine(true));
                    return;
                }
            } else {
                if (leftInput != 0 && !isMining && !isAttacking && mineTimer <= 0 && !inventoryOpen) {
                    mineTimer = (PlayerAttributes.miningSpeed - PlayerAttributes.miningSpeedBonus);
                    StartCoroutine(MiningRoutine(true));
                    return;
                }
            }
        }

        float rightInput = Input.GetAxis("Fire2");
        if (PlayerAttributes.rightHand != null && rightInput > 0) {
            if (PlayerAttributes.rightHand.type == ToolItem.ToolType.WEAPON) {
                if (!isAttacking && !isMining && attackTimer <= 0 && !inventoryOpen) {
                    attackTimer = (PlayerAttributes.attackSpeed - PlayerAttributes.attackSpeedBonus);
                    StartCoroutine(AttackRoutine(false));
                    return;
                }
            } else {
                if (!isMining && !isAttacking && mineTimer <= 0 && !inventoryOpen) {
                    mineTimer = (PlayerAttributes.miningSpeed - PlayerAttributes.miningSpeedBonus);
                    StartCoroutine(MiningRoutine(false));
                    return;
                }
            }
        }
    }

    IEnumerator AttackRoutine(bool isLeft)
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

        playerMovement.facingDirection = facingDirection;
        animator.SetInteger("FacingDirection", facingDirection);
        animator.SetTrigger("ChangeMode");
        animator.SetFloat("SwingSpeed", 1f / (PlayerAttributes.attackSpeed - PlayerAttributes.attackSpeedBonus));
        animator.SetTrigger("Swing");

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = isLeft ? PlayerAttributes.leftHand.sprite : PlayerAttributes.rightHand.sprite;
        
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
                    hit.transform.GetComponent<Enemy>().takeDamage(isLeft ? PlayerAttributes.leftHand.attackDamage : PlayerAttributes.rightHand.attackDamage, transform.position);
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
        if (PlayerAttributes.leftHand != null) {
            GameManager.instance.uiManager.leftSprite.enabled = true;
            GameManager.instance.uiManager.leftSprite.sprite = PlayerAttributes.leftHand.sprite;
            GameManager.instance.uiManager.leftSprite.rectTransform.sizeDelta = PlayerAttributes.leftHand.spriteSize;
        } else {
            GameManager.instance.uiManager.leftSprite.enabled = false;
        }

        if (PlayerAttributes.rightHand != null) {
            GameManager.instance.uiManager.rightSprite.enabled = true;
            GameManager.instance.uiManager.rightSprite.sprite = PlayerAttributes.rightHand.sprite;
            GameManager.instance.uiManager.rightSprite.rectTransform.sizeDelta = PlayerAttributes.rightHand.spriteSize;
        } else {
            GameManager.instance.uiManager.rightSprite.enabled = false;
        }

        if (attackTimer > 0 && !isAttacking)
        {
            attackTimer -= Time.deltaTime;
            (PlayerAttributes.leftHand.type == ToolItem.ToolType.WEAPON ? leftCooldown : rightCooldown).rectTransform.sizeDelta = new Vector2(20, 20 * attackTimer / (PlayerAttributes.attackSpeed - PlayerAttributes.attackSpeedBonus));
            if (attackTimer <= 0) playerMovement.canMove = true;
        }
        if (mineTimer > 0)
        {
            mineTimer -= Time.deltaTime;
            (PlayerAttributes.leftHand.type == ToolItem.ToolType.PICKAXE ? leftCooldown : rightCooldown).rectTransform.sizeDelta = new Vector2(20, 20 * mineTimer / (PlayerAttributes.miningSpeed - PlayerAttributes.miningSpeedBonus));
        }
    }
    #endregion

    #region Health Functions
    public void takeDamage(float dmg) {
        if (invulnerable) return;
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

    public void HealPlayer(float amount) {
        currentHealth += amount;
        GameManager.instance.uiManager.SetHealth(currentHealth / maxHealth);
    }

    public IEnumerator DamageFlash() {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Protection(float duration) {
        StartCoroutine(ProtectionRoutine(duration));
    }

    public IEnumerator ProtectionRoutine(float duration) {
        invulnerable = true;
        GetComponent<SpriteRenderer>().color = Color.magenta;
        yield return new WaitForSeconds(duration);
        GetComponent<SpriteRenderer>().color = Color.white;
        invulnerable = false;
    }
    #endregion

    #region Mining Functions

    IEnumerator MiningRoutine(bool isLeft)
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

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = isLeft ? PlayerAttributes.leftHand.sprite : PlayerAttributes.rightHand.sprite;

        playerMovement.facingDirection = facingDirection;
        animator.SetInteger("FacingDirection", facingDirection);
        animator.SetTrigger("ChangeMode");
        animator.SetFloat("SwingSpeed", 1f / PlayerAttributes.miningSpeed - PlayerAttributes.miningSpeedBonus);
        animator.SetTrigger("Swing");

        RaycastHit2D hit = Physics2D.Raycast(playerRB.position + miningOffset, cardinalDirection, miningReach, LayerMask.GetMask("Environment"));
        Debug.DrawRay(playerRB.position + miningOffset, direction, Color.black, 10.0f, false); // For debugging purposes
        Debug.DrawRay(playerRB.position + miningOffset, cardinalDirection, Color.green, 10.0f, false); // For debugging purposes

        if (hit.transform != null)
        {
            Ore ore = hit.transform.GetComponent<Ore>();
            ore.TakeDamage(PlayerAttributes.miningDamage);
        }

        playerMovement.canMove = false;

        yield return new WaitForSeconds(PlayerAttributes.miningSpeed - PlayerAttributes.miningSpeedBonus);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    #region Health variables
    [SerializeField]
    [Tooltip("The maximum full health of this enemy.")]
    private float maxHealth;
    private float currentHealth;
    #endregion

    #region Movement variables
    [SerializeField]
    [Tooltip("The max speed at which this enemy will move.")]
    private float speed;
    [SerializeField]
    [Tooltip("The rate at which this enemy accelerates.")]
    private float acceleration;
    [SerializeField]
    [Tooltip("The furthest distance this enemy can detect the player.")]
    private float aggroRange;
    private bool facingRight;
    #endregion

    [SerializeField]
    [Tooltip("The item that this enemy drops when it dies.")]
    private GameObject itemDrop;

    [SerializeField]
    [Tooltip("Amount of damage this enemy's attacks deal to the player.")]
    protected int attackDamage;
    private Transform target;
    private Rigidbody2D EnemyRB;

    private bool takingDamage = false;
    

    // Start is called before the first frame update
    void Start() {
        EnemyRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        facingRight = false;
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, target.position) <= aggroRange) {
            Move();
        }
        updateAnim();  
    }

    private void Update() {
        if (EnemyRB.velocity.magnitude > speed) {
            EnemyRB.velocity = EnemyRB.velocity.normalized * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player")) {
            col.transform.GetComponent<PlayerInteractions>().takeDamage(attackDamage);
        }
    }

    private void Move() {
        if (takingDamage) return;
        Vector2 direction = target.position - transform.position;
        EnemyRB.AddForce(direction.normalized * acceleration, ForceMode2D.Force);
    }
    protected virtual void updateAnim() {
        Vector2 direction = EnemyRB.velocity;
        if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight)) {
            facingRight = !facingRight;
            Vector3 t_scale = transform.localScale;
            t_scale.x *= -1;
            transform.localScale = t_scale;
        }
    }

    public void takeDamage(float dmg, Vector3 origin) {
        if (takingDamage) return;
        Debug.Log("Damage taken!");
        currentHealth -= dmg;
        GetComponent<Rigidbody2D>().AddForce((transform.position - origin).normalized * dmg * 3, ForceMode2D.Impulse);
        StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash() {
        GetComponent<SpriteRenderer>().color = Color.red;
        takingDamage = true;
        yield return new WaitForSeconds(0.2f);
        takingDamage = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        if (currentHealth <= 0) {
            if (itemDrop != null) {
                GameObject drop = Instantiate(itemDrop, transform.position, transform.rotation);
                drop.transform.parent = transform.parent;
            }
            Destroy(gameObject);
        }
    }
    
}

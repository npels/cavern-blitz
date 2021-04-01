using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    #region Health variables
    [SerializeField]
    [Tooltip("The maximum full health of this enemy.")]
    private int maxHealth;
    private int currentHealth;
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
    #endregion

    [SerializeField]
    [Tooltip("Amount of damage this enemy's attacks deal to the player.")]
    private int attackDamage;
    private Transform target;
    private Rigidbody2D EnemyRB;
    

    // Start is called before the first frame update
    void Start() {
        EnemyRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, target.position) <= aggroRange) {
            Move();
        }
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
        Vector2 direction = target.position - transform.position;
        EnemyRB.AddForce(direction.normalized * acceleration, ForceMode2D.Force);
    }

    public void takeDamage(int dmg) {
        Debug.Log("Damage taken!");
        currentHealth -= dmg;
        if (currentHealth <= 0) {
            Destroy(gameObject);
        } else {
            StartCoroutine(DamageFlash());
        }
    }

    private IEnumerator DamageFlash() {
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}

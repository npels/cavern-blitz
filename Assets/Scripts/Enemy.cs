using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int maxHealth;
    int currentHealth;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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

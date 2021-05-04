using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    private AudioSource explosionSound;
    public float range;
    public float damage;

    private void Start() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero);
        explosionSound = GetComponent<AudioSource>();
        explosionSound.Play();
        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.GetComponent<PlayerInteractions>()) {
                hit.transform.GetComponent<PlayerInteractions>().takeDamage(damage);
            } else if (hit.transform.GetComponent<Enemy>()) {
                hit.transform.GetComponent<Enemy>().takeDamage(damage, transform.position);
            } else if (hit.transform.GetComponent<Ore>()) {
                hit.transform.GetComponent<Ore>().TakeDamage(damage);
            }
        }
    }

    public void EndExplosion() {
        Destroy(gameObject);
    }
}

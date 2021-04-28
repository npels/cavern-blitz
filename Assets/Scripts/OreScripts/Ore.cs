using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour {
    public float maxHealth; //The number of hits it takes to break the ore 
    public int numDrops; //The number of ores that drops when broken
    private float currHealth;
    private AudioSource mineAudio;
    private AudioSource popAudio;
    public GameObject drop; 

    private void Start()
    {
        currHealth = maxHealth;
        mineAudio = GetComponents<AudioSource>()[0];
        popAudio = GetComponents<AudioSource>()[1];
    }

    //Decrements the current health of the ore
    public void TakeDamage(float val)
    {
        GetComponent<ParticleSystem>().Play();
        mineAudio.Play();
        if (GetComponent<Animation>()) GetComponent<Animation>().Play();

        currHealth -= val;
        if (currHealth <= 0)
        {
            
            DropOre(this.gameObject);
        }
    }

    public int GetNumDrops()
    {
        return numDrops;
    }

    #region Drop Functions/Animations
    private void DropOre(GameObject g)
    {
        if (numDrops > 0) {
            popAudio.Play();
            Instantiate(drop, transform.position, transform.rotation);
            Destroy(gameObject);
        } else {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            foreach (Collider2D c in GetComponents<Collider2D>()) c.enabled = false;
        }
        
    }
   
    #endregion
}

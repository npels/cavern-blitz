using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour {
    public int maxHealth; //The number of hits it takes to break the ore 
    public int numDrops; //The number of ores that drops when broken
    private int currHealth;
    public Item oreItem;
    private AudioSource mineAudio;
    private AudioSource popAudio;

    private bool isPickupable = false;

    #region Animation Variables
    private ObjectShake objShake; 
    #endregion
    private void Start()
    {
        objShake = GetComponent<ObjectShake>();
        currHealth = maxHealth;
        mineAudio = GetComponents<AudioSource>()[0];
        popAudio = GetComponents<AudioSource>()[1];
    }

    //Decrements the current health of the ore
    public void TakeDamage(int val)
    {
        GetComponent<ParticleSystem>().Play();
        mineAudio.Play();

        //Shake
        Quaternion rotation = transform.rotation;  
        objShake.Shake();
        transform.rotation = rotation; 

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
            g.transform.localScale = new Vector3(0.5f, 0.5f, 0);
            isPickupable = true;
        } else {
            GetComponent<SpriteRenderer>().enabled = false;
            foreach (Collider2D c in GetComponents<Collider2D>()) c.enabled = false;
        }
        
    }
    private void PickupOre()
    {
        if (oreItem == null) {
            isPickupable = false;
            Destroy(gameObject);
        }
        if (GameManager.instance.inventory.TryAddItem(oreItem, 1) != -1) {
            isPickupable = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isPickupable)
        {
            PickupOre();
        }
    }


    #endregion
}

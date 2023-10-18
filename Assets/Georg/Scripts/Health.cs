using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Demo class for testing health
/// </summary>
public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool dead = false;

    BoidManager bm;

    private void Start()
    {
        health = maxHealth;
        bm = GameObject.Find("Boid Manager").GetComponent<BoidManager>();
        
    }

    public void ChangeHealth(int amount) //Used for adding or subtracting health, based on if amount is positive or negative
    {
        if (!dead)
        {
            health += amount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0)
            {
                health = 0;
                Death();
            }   
            Debug.Log(name+" health: "+health);
        }
    }

    public void Death()
    {
        dead = true;
        Debug.Log(name + " killed.");

        if(this.gameObject.tag == "boid")
        {
            bm.deadBoids++;
        }
        StartCoroutine(DestroyObject(this.gameObject));
    }

    IEnumerator DestroyObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(2);
       if (gameObject)
       {
        Destroy(gameObject);
       }
    }
}

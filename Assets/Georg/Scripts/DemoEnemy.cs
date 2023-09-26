using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Demo class for testing health & basic énemy functionality
/// </summary>
public class DemoEnemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool dead = false;

    public GrappleObject grappleAttached; //Grapple that is attached to this enemy

    private void Start()
    {
        health = maxHealth;
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

        DetachGrapple(); //If player is attached to this enemy, stop grapple function
        Destroy(gameObject);
    }

    public void GrappleAttached(GrappleObject grapple)
    {
        grappleAttached = grapple;
    }

    private void DetachGrapple()
    {
        if(grappleAttached != null)
        {
            grappleAttached.CancelGrapple();
        }
    }
}

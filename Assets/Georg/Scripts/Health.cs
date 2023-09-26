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
    }
}
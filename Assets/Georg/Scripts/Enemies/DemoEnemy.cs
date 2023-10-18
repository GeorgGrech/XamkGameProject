using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Demo class for testing health & basic énemy functionality
/// </summary>
public class DemoEnemy : MonoBehaviour
{
    public GrappleObject grappleAttached; //Grapple that is attached to this enemy

    public WaveManager waveManager;

    public void OnDeath()
    {
        waveManager.leftInWave--;
        waveManager.RemoveBoidFromList(GetComponent<Boid>());

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

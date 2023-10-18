using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoidProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime;

    

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Debug.Log("Player Hit");
        }

        StartLifetime();
        
    }

     private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void StartLifetime()
    {
        StartCoroutine(Lifetime());
    }

}
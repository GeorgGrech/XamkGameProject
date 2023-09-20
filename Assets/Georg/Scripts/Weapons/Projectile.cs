using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float range;

    private Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(initPos, transform.position) > range)
        {
            Debug.Log("Out of range. Destroyed.");
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        collision.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);
        Debug.Log("Collided with "+collision.gameObject.name);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Transform trailObject = transform.GetChild(0);
        trailObject.parent = null;
        trailObject.GetComponent<TrailRenderer>().time /= 2 ; //Quicken fade time
        trailObject.GetComponent<TrailRenderer>().emitting = false;
        trailObject.GetComponent<ProjectileTrail>().StartLifetime();
        Destroy(gameObject);
    }
}

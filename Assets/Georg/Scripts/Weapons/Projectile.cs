using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float range;

    private Vector3 initPos;
    private float totalDistance;

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
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);
        Debug.Log("Collided with "+collision.gameObject.name);
        Destroy(gameObject);
    }
}

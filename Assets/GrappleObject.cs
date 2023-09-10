using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleObject : MonoBehaviour
{
    public Vector3 grapplePoint;
    public PlayerGrapple playerGrapple;
    public float grappleSpeed;

    private bool hasConnected = false;

    public Coroutine grappleCoroutine;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasConnected)
        {
            if(collision.gameObject.layer == playerGrapple.grappleableLayer)
            {
                hasConnected = true;
                StopCoroutine(grappleCoroutine);
            }
        }
    }

    public IEnumerator MoveToGrapple()
    {
        transform.LookAt(grapplePoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, grapplePoint, grappleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void ExecMoveToGrapple()
    {
        grappleCoroutine = StartCoroutine(MoveToGrapple());
    }
}

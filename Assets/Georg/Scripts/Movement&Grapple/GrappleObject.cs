using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleObject : MonoBehaviour
{
    public Vector3 grapplePoint;
    public PlayerGrapple playerGrapple;
    public float grappleSpeed;

    public float maxGrappleDistance;

    public Vector3 surfaceNormal;

    public string grappleableLayer;

    private bool hasConnected = false;

    public Coroutine grappleCoroutine; //Used for both throwing grapple and pulling player


    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if(hasConnected)
        {
            playerGrapple.grapplePoint = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision) //Collision with actual surfaces
    {
        if (!hasConnected) //Grapple hasn't connected to anything yet
        {
            if(LayerMask.LayerToName(collision.gameObject.layer) == grappleableLayer) //If collision is grappleable environment
            {
                Debug.Log("Connected to Grappleable surface");

                hasConnected = true;

                GetComponent<Rigidbody>().isKinematic = true;
                StopCoroutine(grappleCoroutine); //Stop grapple Lifetime

                transform.rotation = Quaternion.FromToRotation(Vector3.forward, surfaceNormal);

                grappleCoroutine = StartCoroutine(playerGrapple.PullPlayer()); //Start pulling player

                collision.gameObject.SendMessageUpwards("GrappleAttached", this, SendMessageOptions.DontRequireReceiver);
            }
            else //If collision isn't grappleable environment
            {
                Debug.Log("Hit non-Grappleable surface");
                //Cancel functionality
                CancelGrapple();
            }
        }
    }

    public void ThrowGrapple()
    {
        transform.LookAt(grapplePoint);
        GetComponent<Rigidbody>().AddForce(transform.forward * grappleSpeed);


        grappleCoroutine = StartCoroutine(Lifetime());
    }

    public void CancelGrapple()
    {
        if (grappleCoroutine != null)
        {
            StopCoroutine(grappleCoroutine);
        }

        playerGrapple.grappling = false;
        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        Vector3 startPos = transform.position;

        while (Vector3.Distance(startPos, transform.position) <= maxGrappleDistance)
        {
            yield return null;
        }

        Debug.Log("Grapple out of range, destroyed");
        CancelGrapple();
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleObject : MonoBehaviour
{
    public Vector3 grapplePoint;
    public PlayerGrapple playerGrapple;
    public float grappleSpeed;

    public Vector3 surfaceNormal;

    public string grappleableLayer;

    private bool hasConnected = false;

    public Coroutine grappleCoroutine; //Used for both throwing grapple and pulling player

    [SerializeField] private GameObject stopperPrefab;
    private GameObject stopper;



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

                transform.parent = collision.gameObject.transform;

                Vector3 parentScale = collision.gameObject.transform.localScale;
                transform.localScale = new Vector3(1 / parentScale.x, 1/ parentScale.y, 1 / parentScale.z);

                hasConnected = true;

                GetComponent<Rigidbody>().isKinematic = true;
                StopCoroutine(grappleCoroutine); //Stop grapple move towards target

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GrappleStopper"))
        {
            Debug.Log("No surface hit.");
            CancelGrapple();
        }
    }

    public IEnumerator ThrowGrapple()
    {
        transform.LookAt(grapplePoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, grapplePoint, grappleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void ExecThrowGrapple(bool stopperNeeded)
    {
        if(stopperNeeded) stopper = Instantiate(stopperPrefab,grapplePoint, Quaternion.identity);

        grappleCoroutine = StartCoroutine(ThrowGrapple());
    }

    public void CancelGrapple()
    {
        if (grappleCoroutine != null)
        {
            StopCoroutine(grappleCoroutine);
        }

        if (stopper != null)
        {
            Destroy(stopper);
        }

        playerGrapple.grappling = false;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerGrapple : MonoBehaviour
{
    [SerializeField] private Transform playerCam;
    [SerializeField] private GameObject grapplePrefab;
    [SerializeField] private GameObject grappleViewModel;

    public bool grappling = false;

    [SerializeField] private Transform throwPoint;
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float pullForce;
    [SerializeField] private float maxPullSpeed;

    [Header("Instantiated Grapple Properties")]
    [SerializeField] private float grappleSpeed;
    public Vector3 grapplePoint;
    public LayerMask grappleableLayer;
    public string grappleableLayerName; //Using LayerMask with the grappleObject coughed up errors. So string it is. Cry about it.

    private GrappleObject grappleObject;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;


    [Header("Automatic cancel settings")]

    [SerializeField]
    [InspectorName("Cancel Grapple when view obstructed")]
    private bool cancelWhenObstructed;

    [SerializeField]
    [InspectorName("Automatically cancel grapple when in proximity")]
    private bool cancelWhenClose;
    [SerializeField] private float autoCancelDistance;

    private Rigidbody rb;

    public LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey) && !grappling) StartGrapple();

        if (Input.GetKeyUp(grappleKey) && grappling) grappleObject.CancelGrapple();

        if (grappling) //Auto cancel options
        {
            lr.SetPosition(0, throwPoint.position);
            lr.SetPosition(1, grappleObject.transform.position);

            grappleObject.spawnedGrappleModel.transform.position = grappleObject.transform.position; //Ideally this would be done in GrappleObject. Too bad I don't care.
            if ((cancelWhenObstructed
                && Physics.Linecast(throwPoint.position, grappleObject.transform.position, grappleableLayer)) // Cancel when obstructed
                    
                || (cancelWhenClose
                && Vector3.Distance(grapplePoint,transform.position) <= autoCancelDistance)) //Cancel when in proximity of grapple
            {
                grappleObject.CancelGrapple(); //Break grapple
            }

        }
    }

    private void StartGrapple()
    {
        grappling = true;
        lr.enabled = true;

        RaycastHit hit;
        if (Physics.Raycast(playerCam.position, playerCam.forward, out hit, maxGrappleDistance, grappleableLayer) && !hit.collider.isTrigger)
        {
            grapplePoint = hit.point;
            
        }
        else
        {
            grapplePoint = playerCam.position + playerCam.forward * maxGrappleDistance;
        }

        InstantiateGrapple(hit.normal);
        EnableGrappleModel(false);

        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
    }

    private void InstantiateGrapple(Vector3 surfaceNormal)
    {
        grappleObject = GameObject.Instantiate(grapplePrefab,throwPoint.position,Quaternion.identity).GetComponent<GrappleObject>();

        grappleObject.grappleSpeed = grappleSpeed;
        grappleObject.grapplePoint = grapplePoint;
        grappleObject.playerGrapple = this;

        grappleObject.surfaceNormal = surfaceNormal;
        grappleObject.grappleableLayer = grappleableLayerName;
        grappleObject.maxGrappleDistance= maxGrappleDistance;

        grappleObject.ThrowGrapple();
    }

    public void EnableGrappleModel(bool enable)
    {
        grappleViewModel.SetActive(enable);
    }

    public IEnumerator PullPlayer()
    {
        while (true)
        {
            Vector3 dir = (grapplePoint - transform.position).normalized;
            rb.AddForce(dir * pullForce); //Keep adding force

            if(rb.velocity.magnitude>maxPullSpeed)
                rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxPullSpeed);

            yield return null;
        }
    }
}

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

    public bool grappling = false;

    [SerializeField] private Transform throwPoint;
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float pullForce;

    [Header("Instantiated Grapple Properties")]
    [SerializeField] private float grappleSpeed;
    private Vector3 grapplePoint;
    public LayerMask grappleableLayer;
    public string grappleableLayerName; //Using LayerMask with the grappleObject coughed up errors. So string it is. Cry about it.

    private GrappleObject grappleObject;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey) && !grappling) StartGrapple();

        if (Input.GetKeyUp(grappleKey) && grappling) grappleObject.CancelGrapple();
    }

    private void StartGrapple()
    {
        bool stopperNeeded;
        Debug.Log("StartGrapple");
        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(playerCam.position, playerCam.forward, out hit, maxGrappleDistance, grappleableLayer))
        {
            grapplePoint = hit.point;
            stopperNeeded = false;
            
        }
        else
        {
            grapplePoint = playerCam.position + playerCam.forward * maxGrappleDistance;
            stopperNeeded = true;
        }

        InstantiateGrapple(stopperNeeded,hit.normal);

        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
    }

    private void InstantiateGrapple(bool stopperNeeded, Vector3 surfaceNormal)
    {
        grappleObject = GameObject.Instantiate(grapplePrefab,throwPoint.position,Quaternion.identity).GetComponent<GrappleObject>();

        grappleObject.grappleSpeed = grappleSpeed;
        grappleObject.grapplePoint = grapplePoint;
        grappleObject.playerGrapple = this;

        grappleObject.surfaceNormal = surfaceNormal;
        grappleObject.grappleableLayer = grappleableLayerName;

        grappleObject.ExecThrowGrapple(stopperNeeded);
    }

    public IEnumerator PullPlayer()
    {
        Debug.Log("Pulling player");
        while (true)
        {
            Vector3 dir = (grapplePoint - transform.position).normalized;
            rb.AddForce(dir * pullForce); //Keep adding force
            yield return null;
        }
    }
}

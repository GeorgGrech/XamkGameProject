using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    public Transform playerCam;
    public GameObject grapplePrefab;

    bool grappling = false;


    [Header("Instantiated Grapple Properties")]
    public float grappleSpeed;
    private Vector3 grapplePoint;
    public LayerMask grappleableLayer;



    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey)) StartGrapple();
    }

    private void StartGrapple()
    {
        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(playerCam.position, playerCam.forward, out hit, /*maxGrappleDistance,*/ grappleableLayer))
        {
            grapplePoint = hit.point;
            
        }
        else
        {
            grapplePoint = playerCam.position + playerCam.forward /** maxGrappleDistance*/;
        }


        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
    }

    private IEnumerator ThrowGrapple()
    {
        Quaternion camRotation = playerCam.rotation;
        yield return null;
    }

    private void InstantiateGrapple()
    {
        GrappleObject grappleObject = GameObject.Instantiate(grapplePrefab).GetComponent<GrappleObject>();

        grappleObject.grappleSpeed = grappleSpeed;
        grappleObject.grapplePoint = grapplePoint;

        grappleObject.ExecMoveToGrapple();
    }
}

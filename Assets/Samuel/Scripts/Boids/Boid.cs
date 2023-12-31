﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boid : MonoBehaviour {

    BoidSettings settings;

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    // To update:
    Vector3 acceleration;
    
    public Vector3 avgFlockHeading;
    
    public Vector3 avgAvoidanceHeading;
    
    public Vector3 centreOfFlockmates;
    
    public int numPerceivedFlockmates;

    public GameObject boidTarget;
    public bool areaTrigger;

    // Cached
    Material material;
    Transform cachedTransform;
    Transform target;

    void Awake () {
        material = transform.GetComponentInChildren<MeshRenderer> ().material;
        cachedTransform = transform;
        areaTrigger = false;
    }

    public void Initialize (BoidSettings settings, Transform target) {
        this.target = target;
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    public void SetColour (Color col) {
        if (material != null) {
            material.color = col;
        }
    }

    public void UpdateBoid () {
        
        // if(areaTrigger == false)
        // {
        //     var step =  5 * Time.deltaTime;
        //     transform.position = Vector3.MoveTowards(transform.position, boidTarget.transform.position, step);

        //     Vector3 newDirection = Vector3.RotateTowards(transform.forward, boidTarget.transform.position, step, 0.0f);

        //     this.transform.rotation = Quaternion.LookRotation(newDirection);
        // }        
        // else
        // {
              Vector3 acceleration = Vector3.zero;

                if (target != null) {
                    Vector3 offsetToTarget = (target.position - position);
                    acceleration = SteerTowards (offsetToTarget) * settings.targetWeight;
                }

                if (numPerceivedFlockmates != 0 && numPerceivedFlockmates < 1) {
                    centreOfFlockmates /= numPerceivedFlockmates;

                    Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

                    var alignmentForce = SteerTowards (avgFlockHeading) * settings.alignWeight;
                    var cohesionForce = SteerTowards (offsetToFlockmatesCentre) * settings.cohesionWeight;
                    var seperationForce = SteerTowards (avgAvoidanceHeading) * settings.seperateWeight;

                    acceleration += alignmentForce;
                    acceleration += cohesionForce;
                    acceleration += seperationForce;
                }

                if (IsHeadingForCollision ()) {
                    Vector3 collisionAvoidDir = ObstacleRays ();
                    Vector3 collisionAvoidForce = SteerTowards (collisionAvoidDir) * settings.avoidCollisionWeight;
                    acceleration += collisionAvoidForce;
                }

                velocity += acceleration * Time.deltaTime;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;
                speed = Mathf.Clamp (speed, settings.minSpeed, settings.maxSpeed);
                velocity = dir * speed;

                if(cachedTransform != null)
                {
                    cachedTransform.position += velocity * Time.deltaTime;
                    cachedTransform.forward = dir;
                    position = cachedTransform.position;
                    forward = dir;
                }
                
        
        // }
              

        
    }

    bool IsHeadingForCollision () {
        RaycastHit hit;
        if (Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask)) {
            return true;
        } else { }
        return false;
    }

    Vector3 ObstacleRays () {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            
            if(cachedTransform != null)
            {
                Vector3 dir = cachedTransform.TransformDirection (rayDirections[i]);    
                Ray ray = new Ray (position, dir);
                if (!Physics.SphereCast (ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) 
                {
                    return dir;
                }
            }
            
        }

        return forward;
    }

    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude (v, settings.maxSteerForce);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "boidColliderTrigger")
        {
            StartCoroutine(WaitXAmount(0.5f));
            areaTrigger = true;
        }
    }

    IEnumerator WaitXAmount(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    // public void DisableBoid(GameObject obj)
    // {
    //     foreach(MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>()){
    //     mesh.enabled = false;
    //     }
    // }

   
    
}
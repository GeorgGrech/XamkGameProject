using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    // Timer to track collision time
	float _timeColliding;
	// Time before damage is taken, 1 second default
	public float timeThreshold = 1f;


//    void OnCollisionEnter(Collision collider)
//    {
//     if(collider.gameObject.tag == "Player")
//     {
//         collider.gameObject.SendMessageUpwards("RespawnPlayer", SendMessageOptions.DontRequireReceiver);
//     }
//    }

   void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			_timeColliding = 0f;

			Debug.Log("Enemy started colliding with player.");

			collision.gameObject.SendMessageUpwards("ChangeHealth", -20f, SendMessageOptions.DontRequireReceiver);
		}
    }
   void OnCollisionStay(Collision collision)
   {
        if(collision.gameObject.tag == "Player")
        {
            if (_timeColliding < timeThreshold) {
                    _timeColliding += Time.deltaTime;
                } else {
                    // Time is over theshold, player takes damage
                    collision.gameObject.SendMessageUpwards("ChangeHealth", -20f, SendMessageOptions.DontRequireReceiver);
                    // Reset timer
                    _timeColliding = 0f;
                }
        }
   }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
   void OnCollisionEnter(Collision collider)
   {
    if(collider.gameObject.tag == "Player")
    {
        collider.gameObject.SendMessageUpwards("RespawnPlayer", SendMessageOptions.DontRequireReceiver);
    }
   }
}

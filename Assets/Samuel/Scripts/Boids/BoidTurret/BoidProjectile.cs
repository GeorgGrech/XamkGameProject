using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoidProjectile : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private float lifetime;

    public float shotDamage = 10f;

    public GameObject GlobalVolume;
    public GameObject explosionVfx;
    int LayerObstacle;
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        GlobalVolume = GameObject.Find("Global Volume");
        LayerObstacle = LayerMask.NameToLayer("Environment");
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            audioManager.playSound(4);
            collider.SendMessageUpwards("ChangeHealth", -shotDamage, SendMessageOptions.DontRequireReceiver);
            GlobalVolume.SendMessageUpwards("PlayDamageAnimation", SendMessageOptions.DontRequireReceiver);
            Instantiate(explosionVfx, gameObject.transform);

        }

        if(collider.gameObject.layer == LayerObstacle)
        {
            audioManager.playSound(4);
            Debug.Log("Collided with: " + collider.name);
            Debug.Log("Position of collision: " + gameObject.transform.position);
            Instantiate(explosionVfx, gameObject.transform.position, Quaternion.identity);

        }
        StartLifetime();
        
    }

     private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void StartLifetime()
    {
        StartCoroutine(Lifetime());
    }

}

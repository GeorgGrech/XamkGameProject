using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    //Public properties
    public enum ShotType
    {
        Hitscan,
        Projectile
    }

    public ShotType weaponShotType;

    public float range;
    public float damage; //This is per single "projectile", so shotguns would deal this damage * shotsPerRound

    [Header("Ammo")]
    public float shotsPerRound; //Multiple shots per round. For use with shotguns.
    public float magSize;
    public float currentAmmo;

    [Header("Time")]
    public float rateOfFire;
    public float reloadTime;

    [Space(10)]
    public float accuracy; //So far accuracy will be static. Perhaps later it will be dynamic according to movement, rapid fire, etc.
    public Transform shootSpot;

    [Header("Hitscan shot effect")]
    public GameObject lineEffect;
    public float lineEffectDuration = .05f;

    //Private functional vars
    private float fireTimer;
    private float actualROF;

    // Start is called before the first frame update
    void Start()
    {        
        actualROF = 1f / rateOfFire;

        currentAmmo = magSize;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;

        if (currentAmmo <= 0) //Reload automatically
            Reload();

        UserInput();
    }

    void UserInput()
    {
        if(fireTimer >= actualROF)
        {
            if (Input.GetButton("Fire1"))
            {
                if (weaponShotType == ShotType.Hitscan)
                {
                    HitscanFire();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    void HitscanFire()
    {
        fireTimer = 0f;

        if (currentAmmo <= 0)
        {
            //DryFire();

            return;
        }

        currentAmmo--;

        for (int i = 0; i < shotsPerRound; i++)
        {
            float accuracyVary = (100 - accuracy) / 1000;
            Vector3 direction = shootSpot.forward;
            direction.x += Random.Range(-accuracyVary, accuracyVary);
            direction.y += Random.Range(-accuracyVary, accuracyVary);
            direction.z += Random.Range(-accuracyVary, accuracyVary);

            Ray ray = new Ray(shootSpot.position, direction);
            RaycastHit hit;

            Vector3 effectTarget = shootSpot.forward+direction*range;

            if (Physics.Raycast(ray, out hit, range))
            {
                hit.collider.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);
            }
            StartCoroutine(LineEffect(effectTarget));
        }
    }

    public IEnumerator LineEffect(Vector3 direction)
    {
        GameObject lrObject = Instantiate(lineEffect);
        LineRenderer lr = lrObject.GetComponent<LineRenderer>();

        lr.SetPosition(0, shootSpot.position);
        lr.SetPosition(1, direction * range);

        yield return new WaitForSeconds(lineEffectDuration);
        Destroy(lrObject);
    }

    private void Reload() //This method reloads in one go. Perhaps later for weapons like shotguns, they'll reload one by one?
    {
        Debug.Log("Reloading...");
		currentAmmo = magSize;
        fireTimer = -reloadTime;
    }
}

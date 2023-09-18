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
    public float lineEffectDuration = .1f;
    public Material lineMaterial;
    public float lineWidth = .1f;

    //Private functional vars
    private float fireTimer;
    private float actualROF;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        if(weaponShotType == ShotType.Hitscan)
        {
            lr = GetComponent<LineRenderer>();
        }

        actualROF = 1f / rateOfFire;

        currentAmmo = magSize;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;

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
    }

    void HitscanFire()
    {
        fireTimer = 0f;

        if (currentAmmo <= 0)
        {
            //DryFire();
            return;
        }

        for (int i = 0; i < shotsPerRound; i++)
        {
            float accuracyVary = (100 - accuracy) / 1000;
            Vector3 direction = shootSpot.forward;
            direction.x += Random.Range(-accuracyVary, accuracyVary);
            direction.y += Random.Range(-accuracyVary, accuracyVary);
            direction.z += Random.Range(-accuracyVary, accuracyVary);

            Ray ray = new Ray(shootSpot.position, direction);
            RaycastHit hit;

            //StartCoroutine(LineEffect(direction));

            if (Physics.Raycast(ray, out hit, range))
            {
                hit.collider.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public IEnumerator LineEffect(Vector3 direction)
    {
        LineRenderer lr = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lr.material = lineMaterial;
        lr.startWidth=0; lr.endWidth = 0;
        lr.SetPosition(0, shootSpot.position);
        lr.SetPosition(1, direction * range);
        yield return new WaitForSeconds(lineEffectDuration);
        Destroy(lr);
    }
}

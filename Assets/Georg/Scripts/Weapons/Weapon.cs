using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

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

    private TextMeshProUGUI ammoUI;

    [Header("Time")]
    public float rateOfFire;
    public float reloadTime;

    [Space(10)]
    public float accuracy; //So far accuracy will be static. Perhaps later it will be dynamic according to movement, rapid fire, etc.
    public Transform shootSpot;

    [Header("Hitscan related vars")]
    public GameObject lineEffect;
    public float lineEffectDuration = .05f;

    [Header("Projectile related vars")]
    public GameObject projectilePrefab;
    public float projectileSpeed;

    //Private functional vars
    private float fireTimer;
    private float actualROF;


    private void Awake()
    {
        ammoUI = transform.root.Find("Canvas").Find("AmmoCount").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        actualROF = 1f / rateOfFire;

        currentAmmo = magSize;

        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
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
                Fire();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo<magSize)
            Reload();
    }

    void Fire() //Contains functionality common to both fire types, before choosing between Hitscan and Projectile
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

            if (weaponShotType == ShotType.Hitscan)
            {
                HitscanFire();
            }
            else ProjectileFire();

            UpdateUI();
        }
    }

    void HitscanFire()
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
            //Get GameObject hit
            GameObject gb = hit.collider.gameObject;
            gb.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);

            //Get Rigidbody From gb
            Rigidbody rb = gb.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * 1000f);
            
            Debug.Log(gb);

            if(gb.tag == "boid")
            {
            //    gb.SendMessageUpwards("DisableBoid", gb,SendMessageOptions.DontRequireReceiver);
               StartCoroutine(DestroyObject(gb));
            }
            else
            {
                StartCoroutine(DestroyObject(gb));
            }
        }
        StartCoroutine(LineEffect(effectTarget));
    }

    void ProjectileFire()
    {

        float accuracyVary = (100 - accuracy) / 1000;

        Quaternion randomRotation = shootSpot.rotation;
        randomRotation.x += Random.Range(-accuracyVary, accuracyVary);
        randomRotation.y += Random.Range(-accuracyVary, accuracyVary);

        GameObject projectile = Instantiate(projectilePrefab, shootSpot.position, randomRotation);

        projectile.GetComponent<Projectile>().damage = damage;
        projectile.GetComponent<Projectile>().range = range;

        projectile.GetComponent<Rigidbody>().AddRelativeForce(0, 0, projectileSpeed);
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
        Invoke("UpdateUI",reloadTime);
    }

    //So far I'm just using this to display the debug reload messages, but will be used to update UI later on
    private void UpdateUI()
    {

        if (gameObject.activeSelf) //Don't override if weapon switched
        {
            //Update UI

            ammoUI.SetText(currentAmmo.ToString() + " / " + magSize.ToString());

            Debug.Log(name + " reloaded.");
        }
        
    }

    IEnumerator DestroyObject(GameObject enemyobject)
    {
        yield return new WaitForSeconds(2);
       if (enemyobject)
       {
        Destroy(enemyobject);
       }
    }
}

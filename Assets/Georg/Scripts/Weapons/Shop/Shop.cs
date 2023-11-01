using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private GameObject[] weapons;

    public Transform weaponsContainer;
    public WeaponSwitch weaponSwitch;
    // Start is called before the first frame update
    void Start()
    {
        weaponsContainer = GameObject.Find("WeaponsContainer").transform;
        weaponSwitch = weaponsContainer.GetComponent<WeaponSwitch>();
        PopulateShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateShop()
    {
        foreach (GameObject weapon in weapons)
        {
            GameObject weaponOption = Instantiate(optionPrefab,transform);
            weaponOption.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(weapon.name);
            WeaponOption weaponOptionScript = weaponOption.GetComponent<WeaponOption>();
            weaponOptionScript.weapon = weapon;
            weaponOptionScript.weaponsContainer = weaponsContainer;
            weaponOptionScript.weaponSwitch = weaponSwitch;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class WeaponOption : MonoBehaviour
{
    public GameObject weapon;
    public Transform weaponsContainer;
    public WeaponSwitch weaponSwitch;

    bool mouseOver;

    private int price;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(GameObject weapon,WeaponSwitch weaponSwitch, Transform weaponsContainer)
    {
        this.weapon = weapon;
        transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(weapon.name);
        this.weaponsContainer = weaponsContainer;
        this.weaponSwitch = weaponSwitch;

        price = weapon.GetComponent<Weapon>().price;


        if (ShopManager._instance.unlockedWeapons.Contains(weapon.name))
        {
            price = 0;
            Debug.Log(weapon.name + " unlocked");
        }

        if(price > 0)
        {
            transform.Find("Price").GetComponent<TextMeshProUGUI>().SetText("$"+price.ToString());
        }
        else
        {
            transform.Find("Price").GetComponent<TextMeshProUGUI>().SetText("Unlocked");
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Keypress");
            if(ShopManager._instance.money >= price)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (weaponSwitch.weapons[0])
                    {
                        Destroy(weaponSwitch.weapons[0]);
                    }
                    weaponSwitch.weapons[0] = Instantiate(weapon, weaponsContainer);

                }
                else
                {
                    if (weaponSwitch.weapons[1])
                    {
                        Destroy(weaponSwitch.weapons[1]);
                    }
                    weaponSwitch.weapons[1] = Instantiate(weapon, weaponsContainer);
                }

                if (!ShopManager._instance.unlockedWeapons.Contains(weapon.name))
                {
                    ShopManager._instance.unlockedWeapons.Add(weapon.name);
                    transform.Find("Price").GetComponent<TextMeshProUGUI>().SetText("Unlocked");
                }
            }

            /*
            for(int i = 0; i < weaponsContainer.childCount; i++)
            {
                weaponSwitch.weapons[i] = weaponsContainer.GetChild(i).gameObject;
            }
            */
        }
    }
}

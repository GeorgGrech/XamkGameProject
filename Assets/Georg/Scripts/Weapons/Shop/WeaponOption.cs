using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class WeaponOption : MonoBehaviour
{
    public GameObject weapon;
    public Transform weaponsContainer;
    public WeaponSwitch weaponSwitch;

    bool mouseOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Keypress");
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
            /*
            for(int i = 0; i < weaponsContainer.childCount; i++)
            {
                weaponSwitch.weapons[i] = weaponsContainer.GetChild(i).gameObject;
            }
            */
        }
    }
}

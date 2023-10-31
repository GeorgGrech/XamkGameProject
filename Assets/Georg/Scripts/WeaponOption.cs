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
        Debug.Log("MouseOver");
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Keypress");
            Instantiate(weapon,weaponsContainer);

            for(int i = 0; i < weaponsContainer.childCount; i++)
            {
                weaponSwitch.weapons[i] = weaponsContainer.GetChild(i).gameObject;
            }
        }
    }
}

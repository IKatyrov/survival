using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour 
{

    [SerializeField]
    private WeaponHandler[] weapons;

    private int current_Weapon_Index; //текущее оружие по порядку

	// Use this for initialization
	void Start () 
    {
        current_Weapon_Index = 0; //взятие первого оружия
        weapons[current_Weapon_Index].gameObject.SetActive(true); //активация первого оружия
	}
	
	// Update is called once per frame
	void Update () 
    {
        //KeyCode.Alpha1 - цифры на верхней раскладке

        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            TurnOnSelectedWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            TurnOnSelectedWeapon(1);
        }
    
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            TurnOnSelectedWeapon(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        {
            TurnOnSelectedWeapon(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) 
        {
            TurnOnSelectedWeapon(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) 
        {
            TurnOnSelectedWeapon(5);
        }

    } 

    void TurnOnSelectedWeapon(int weaponIndex) //выбор оружия
    {

        if (current_Weapon_Index == weaponIndex) //проверка на то, что я выбрал
        {
            return;
        }
     
        weapons[current_Weapon_Index].gameObject.SetActive(false); // отключение текущего оружия
      
        weapons[weaponIndex].gameObject.SetActive(true); // выбор нового оружия

        current_Weapon_Index = weaponIndex; // сохранение индекса нового оружия

    }

    public WeaponHandler GetCurrentSelectedWeapon() 
    {
        return weapons[current_Weapon_Index];
    }

}


































using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{

    private WeaponManager weapon_Manager;

    public float fireRate = 15f; 
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim; //анимация во время прицеливания
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair; //прицел

    private bool is_Aiming; //состояние прицела

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;

    [SerializeField]
    private Transform arrow_Bow_StartPosition;

    void Awake() 
    {

        weapon_Manager = GetComponent<WeaponManager>();

        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;

    }

    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot() 
    {

        
        if(weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE) //если в руках винтовка стреляющая очередями
        {

            //если пройденное время больше, чем время между выстрелами
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire) 
            {

                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                 BulletFired();

            }


            // если стреляю из оружия с одиночными выстрелами
        } 
        else 
        {

            if(Input.GetMouseButtonDown(0)) 
            {
                // топор
                if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG) 
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                // одиночный выстрел
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET) 
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                    BulletFired();

                } 
        
                else 
                {
                    // если прицеливаюсь когда в руках стрела или копьё
                    if(is_Aiming) 
                    {

                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                        if(weapon_Manager.GetCurrentSelectedWeapon().bulletType== WeaponBulletType.ARROW) 
                        {
                            // стреляю стрелой
                            ThrowArrowOrSpear(true);

                        } 
                        else if(weapon_Manager.GetCurrentSelectedWeapon().bulletType== WeaponBulletType.SPEAR) 
                        {

                            // стреляю копьём
                            ThrowArrowOrSpear(false);

                        }

                    }

                } 


            } 

        } 

    } 

    void ZoomInAndOut() //приближение и отдаление масштаба прицела
    {
        // если камера прицеливается
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM) 
        {

            // Input.GetMouseButtonDown - удержание правой кнопки мыши
            if(Input.GetMouseButtonDown(1)) 
            {

                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                crosshair.SetActive(false);
            }

            //  Input.GetMouseButtonUp - отпускание кнопки
            if (Input.GetMouseButtonUp(1)) 
            {

                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

                crosshair.SetActive(true);
            }

        } 

        //приближение оружия к себе
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM) 
        {

            if(Input.GetMouseButtonDown(1)) 
            {

                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);

                is_Aiming = true;

            }

            if (Input.GetMouseButtonUp(1)) 
            {

                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);

                is_Aiming = false;

            }

        } 

    } 

    void ThrowArrowOrSpear(bool throwArrow) 
    {

        if(throwArrow) //если бросаю стрелу
        {

            GameObject arrow = Instantiate(arrow_Prefab); //создаю стрелу
            arrow.transform.position = arrow_Bow_StartPosition.position; //задаю направление стреле

            arrow.GetComponent<ArrowBowScript>().Launch(mainCam);

        }
         else //иначе копьё
        {

            GameObject spear = Instantiate(spear_Prefab); //создают копьё
            spear.transform.position = arrow_Bow_StartPosition.position; //задаю направление

            spear.GetComponent<ArrowBowScript>().Launch(mainCam);

        }


    }

    void BulletFired() 
    {

        RaycastHit hit;

        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit)) 
        {

            if(hit.transform.tag == Tags.ENEMY_TAG) 
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }

        }

    } 

} 































using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowScript : MonoBehaviour {

    private Rigidbody myBody;

    public float speed = 30f;

    public float deactivate_Timer = 3f; //время существования объекта

    public float damage = 50f;

    void Awake() 
    {
        myBody = GetComponent<Rigidbody>();
    }

    void Start () 
    {
        Invoke("DeactivateGameObject", deactivate_Timer);
	}

    public void Launch(Camera mainCamera) 
    {

        myBody.velocity = mainCamera.transform.forward * speed; //движение

        transform.LookAt(transform.position + myBody.velocity);

    }
	
    void DeactivateGameObject() 
    {
        if(gameObject.activeInHierarchy) //если объект активен
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider target) 
    {
        if(target.tag == Tags.ENEMY_TAG) 
        {

            target.GetComponent<HealthScript>().ApplyDamage(damage);

            gameObject.SetActive(false);

        }
    }
} 












































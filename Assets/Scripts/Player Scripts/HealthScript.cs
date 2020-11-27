using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour 
{

    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;

    public bool is_Player, is_Boar, is_Cannibal;

    private bool is_Dead;

    private EnemyAudio enemyAudio;

    private PlayerStats player_Stats;

	void Awake () 
    {
	    
        if(is_Boar || is_Cannibal) 
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if(is_Player) 
        {
            player_Stats = GetComponent<PlayerStats>();
        }

	}
	
    public void ApplyDamage(float damage) //нанеселение урона
    {

        //если умер, то выйти 
        if (is_Dead) { return; }

        health -= damage;

        if(is_Player) //удар по игроку
        {
            player_Stats.Display_HealthStats(health); //отобрадение уровня жизней
        }

        if(is_Boar || is_Cannibal) //удар по врагу
        {
            if(enemy_Controller.Enemy_State == EnemyState.PATROL) 
            {
                enemy_Controller.chase_Distance = 50f; //увеличение дистанции обнаружения
            }
        }

        if(health <= 0f) 
        {

            PlayerDied();

            is_Dead = true;
        }

    } 

    void PlayerDied() 
    {

        if(is_Cannibal) //смерть канибала
        {

            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 5f); //толчок врага после смерти

            //отключение компонентов врага
            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;

            StartCoroutine(DeadSound());

            EnemyManager.instance.EnemyDied(true); //создание новых врагов
        }

        if(is_Boar) //смерть кабана
        {
            navAgent.velocity = Vector3.zero; //остановить навмеш
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead(); //отыграть анимацию смерти

            StartCoroutine(DeadSound());

            EnemyManager.instance.EnemyDied(false);
        }

        if(is_Player) //смерть персонажа
        {

            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for (int i = 0; i < enemies.Length; i++) //пересчёт врагов
            {
                enemies[i].GetComponent<EnemyController>().enabled = false; //отключение врагов
            }

            EnemyManager.instance.StopSpawning(); //остановка спавна врагов

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);

        }

        if(tag == Tags.PLAYER_TAG) 
        {

            Invoke("RestartGame", 3f);

        } else 
        {

            Invoke("TurnOffGameObject", 3f);

        }

    } 

    void RestartGame() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject() 
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound() 
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.Play_DeadSound();
    }

} 









































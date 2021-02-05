using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour {

    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 7f; //дистанция для погони
    private float current_Chase_Distance; //текущая дистанция
    public float attack_Distance = 1.8f; //дистанция атаки
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f; //время ожидания перед атакой
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;

    private EnemyAudio enemy_Audio;

    void Awake() {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemy_Audio = GetComponentInChildren<EnemyAudio>();

    }

    // Use this for initialization
    void Start () {

        enemy_State = EnemyState.PATROL; //состояние врага прогулка

        patrol_Timer = patrol_For_This_Time;

        // когда противник впервые попадает в атаку игрока
        attack_Timer = wait_Before_Attack;

        // запомнить значение расстояния погони, чтобы его можно было вернуть
        current_Chase_Distance = chase_Distance;

	}
	
	// Update is called once per frame
	void Update () {
		
        if(enemy_State == EnemyState.PATROL) {
            Patrol();
        }

        if(enemy_State == EnemyState.CHASE) {
            Chase();
        }

        if (enemy_State == EnemyState.ATTACK) {
            Attack();
        }

    }

    void Patrol() {

        // указание агенту, что он может двигаться
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        // добавление таймера к прогулке
        patrol_Timer += Time.deltaTime;

        if(patrol_Timer > patrol_For_This_Time) {

            SetNewRandomDestination();

            patrol_Timer = 0f;

        }

        if(navAgent.velocity.sqrMagnitude > 0) {
        
            enemy_Anim.Walk(true);
        
        } else {

            enemy_Anim.Walk(false);

        }

        // проверка расстояние между игроком и противником
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance) 
        {

            enemy_Anim.Walk(false); //остановить ходьбу

            enemy_State = EnemyState.CHASE; //назначить состояние погони

            
            enemy_Audio.Play_ScreamSound();

        }


    } 

    void Chase() {

        // разрешение агенту двигаться
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;

        //установка позиции игрока в качестве пункта преследования
        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0) {

            enemy_Anim.Run(true);

        } else {

            enemy_Anim.Run(false);

        }

        // если расстояние между противником и игроком меньше дистанции атаки 
        if(Vector3.Distance(transform.position, target.position) <= attack_Distance) 
        {

            // остановка анимаций
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            // сброс дистанции до предыдушей
            if(chase_Distance != current_Chase_Distance) 
            {
                chase_Distance = current_Chase_Distance;
            }

        } 
        else if(Vector3.Distance(transform.position, target.position) > chase_Distance) // игрок убежал от врача
        {
            // остановка анимации бега
            enemy_Anim.Run(false);

            enemy_State = EnemyState.PATROL;

            //сбросить таймер патрулирования, чтобы функция могла сразу рассчитать новый пункт назначения патрулирования 
            patrol_Timer = patrol_For_This_Time;

            //сброс дистанции погони
            if (chase_Distance != current_Chase_Distance) {
                chase_Distance = current_Chase_Distance;
            }


        } // else

    } // chase

    void Attack() {

        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack) {

            enemy_Anim.Attack();

            attack_Timer = 0f;
            
            enemy_Audio.Play_AttackSound();

        }

        if(Vector3.Distance(transform.position, target.position) >
           attack_Distance + chase_After_Attack_Distance) {

            enemy_State = EnemyState.CHASE;

        }


    } // attack

    void SetNewRandomDestination() {

        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        navAgent.SetDestination(navHit.position);

    }

    void Turn_On_AttackPoint() {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint() {
        if (attack_Point.activeInHierarchy) {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State 
    {
        get; set;
    }

} // class



































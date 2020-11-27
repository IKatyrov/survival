using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour 
{

    private PlayerMovement playerMovement;

    public float sprint_Speed = 10f; //бег
    public float move_Speed = 5f; //обынчый шаг
    public float crouch_Speed = 2f; //присед

    private Transform look_Root;
    private float stand_Height = 1.6f; //высота камеры стоя
    private float crouch_Height = 1f; //высота камеры сидя

    private bool is_Crouching;

    private PlayerFootsteps player_Footsteps;

    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.6f;

    private float walk_Step_Distance = 0.4f;
    private float sprint_Step_Distance = 0.25f;
    private float crouch_Step_Distance = 0.5f;

    private PlayerStats player_Stats;

    private float sprint_Value = 100f; //имеющаяся энергия
    public float sprint_Treshold = 10f; //расход энергии

	void Awake () 
    {

        playerMovement = GetComponent<PlayerMovement>();

        look_Root = transform.GetChild(0); //получение камеры

        player_Footsteps = GetComponentInChildren<PlayerFootsteps>();

        player_Stats = GetComponent<PlayerStats>();

	}

    void Start() 
    {
        player_Footsteps.volume_Min = walk_Volume_Min;
        player_Footsteps.volume_Max = walk_Volume_Max;
        player_Footsteps.step_Distance = walk_Step_Distance;
    }


    void Update () 
    {
        Sprint();
        Crouch();
	}

    void Sprint() 
    {
        if(sprint_Value > 0f) //если есть выносливость, то можно бежать
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching) 
            {

                playerMovement.speed = sprint_Speed;

                player_Footsteps.step_Distance = sprint_Step_Distance;
                player_Footsteps.volume_Min = sprint_Volume;
                player_Footsteps.volume_Max = sprint_Volume;

            }

        }

        if(Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching) //остановка бена
        {

            playerMovement.speed = move_Speed;

            player_Footsteps.step_Distance = walk_Step_Distance;
            player_Footsteps.volume_Min = walk_Volume_Min;
            player_Footsteps.volume_Max = walk_Volume_Max;

        }

        if(Input.GetKey(KeyCode.LeftShift) && !is_Crouching)  //если уде бежит
        {

            sprint_Value -= sprint_Treshold * Time.deltaTime; //расход энергии

            if(sprint_Value <= 0f) 
            {

                sprint_Value = 0f;

                // сброс скорости и звука
                playerMovement.speed = move_Speed;
                player_Footsteps.step_Distance = walk_Step_Distance;
                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;


            }

            player_Stats.Display_StaminaStats(sprint_Value);

        } 
        else 
        {
            if(sprint_Value != 100f) //если энергия не полная 
            {
                sprint_Value += (sprint_Treshold / 2f) * Time.deltaTime; //восстановление

                player_Stats.Display_StaminaStats(sprint_Value);

                if(sprint_Value > 100f) //проверка полная ли энергрия
                {
                    sprint_Value = 100f; //остановка на 100%
                }

            }

        }
    }

    void Crouch() 
    {

        if(Input.GetKeyDown(KeyCode.C)) 
        {
            
            if(is_Crouching) 
            {

                look_Root.localPosition = new Vector3(0f, stand_Height, 0f);
                playerMovement.speed = move_Speed;

                player_Footsteps.step_Distance = walk_Step_Distance;
                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;

                is_Crouching = false;

            } 
            else 
            {
               

                look_Root.localPosition = new Vector3(0f, crouch_Height, 0f);
                playerMovement.speed = crouch_Speed;

                player_Footsteps.step_Distance = crouch_Step_Distance;
                player_Footsteps.volume_Min = crouch_Volume;
                player_Footsteps.volume_Max = crouch_Volume;

                is_Crouching = true;

            }

        } 


    } 

} 




























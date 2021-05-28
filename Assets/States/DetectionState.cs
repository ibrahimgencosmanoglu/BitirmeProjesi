using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectionState : IState
{
    float redZonedefault;
    float redZoneRadius;
    float yellowZoneRadius;
    float redZoneIncSpeed;
    float detectionTime;
    float distance;
    float time;
    GameObject player;
    Transform myTransform;
    EnemyAI SM;
    Material enemyMaterial;
    Rigidbody rb;
    NavMeshAgent agent;
    public DetectionState(bool _isAvailable, EnemyAI SM, Material enemyMaterial, Transform myTransform, GameObject player, float yellowZoneRadius, float redZoneRadius, float redZoneIncSpeed, NavMeshAgent agent,float detectionTime)
    {
        //Needs
        this.myTransform = myTransform;
        this.isAvailable = _isAvailable;
        this.SM = SM;
        this.enemyMaterial = enemyMaterial;
        this.player = player;
        this.yellowZoneRadius = yellowZoneRadius;
        this.redZoneRadius = redZoneRadius;
        this.redZoneIncSpeed = redZoneIncSpeed;
        this.agent = agent;
        this.detectionTime = detectionTime;
    }

    //public DetectionState(bool _isAvailable, EnemyAI SM, Material enemyMaterial, Transform myTransform, Rigidbody rb, Transform target, float yellowZoneRadius, float redZoneRadius, float redZoneIncSpeed)
    //{
    //    //Needs
    //    this.myTransform = myTransform;
    //    this.isAvailable = _isAvailable;
    //    this.SM = SM;
    //    this.enemyMaterial = enemyMaterial;
    //    this.rb = rb;
    //    this.target = target;
    //    this.yellowZoneRadius = yellowZoneRadius;
    //    this.redZoneRadius = redZoneRadius;
    //    this.redZoneIncSpeed = redZoneIncSpeed;
    //}

    public bool isAvailable { get; set; }

    public void Enter()
    {
        if (isAvailable) 
        {
            time = 0;
            agent.isStopped = true;
            redZonedefault = this.redZoneRadius;
            enemyMaterial.color = Color.yellow;
            //throw new System.NotImplementedException();
        }
    }

    public void Exit()
    {
        redZoneRadius = redZonedefault;
    }

    public void FixTick()
    {
        //throw new System.NotImplementedException();
    }

    public void Tick()
    {
        if (isAvailable) 
        {
            distance = Vector3.Distance(myTransform.position, player.transform.position);

            if (SM.fieldOfViewYellowCheck(this.yellowZoneRadius))
            {
                time += Time.deltaTime;
                if (time >= detectionTime) 
                {
                    time = (time % detectionTime) - 0.2f;
                    SM.ChangeState(SM.searchState);
                }
            }

            if (SM.fieldOfViewRedCheck(this.redZoneRadius))
            {
                SM.ChangeState(SM.chaseState);
            }
            else if (!SM.fieldOfViewYellowCheck(this.yellowZoneRadius))
            {
                SM.ChangeState(SM.idleState);
            }
        }

        if (!isAvailable) 
        {
            SM.ChangeState(SM.chaseState);
        }
    }
}

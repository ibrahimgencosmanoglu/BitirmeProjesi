using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : IState
{

    GameObject player;
    Transform myTransform;
    EnemyAI SM;
    Material enemyMaterial;
    Rigidbody rb;
    NavMeshAgent agent;
    float redZoneRadius;
    float yellowZoneRadius;
    float redZoneIncSpeed;
    public SearchState(bool _isAvailable, EnemyAI SM, Material enemyMaterial, Transform myTransform, Rigidbody rb, NavMeshAgent agent, GameObject player, float redZoneRadius, float yellowZoneRadius, float redZoneIncSpeed)
    {
        //Needs
        this.myTransform = myTransform;
        this.isAvailable = _isAvailable;
        this.SM = SM;
        this.enemyMaterial = enemyMaterial;
        this.rb = rb;
        this.agent = agent;
        this.player = player;
        this.redZoneRadius = redZoneRadius;
        this.yellowZoneRadius = yellowZoneRadius;
        this.redZoneIncSpeed = redZoneIncSpeed;
    }

    public bool isAvailable { get; set; }

    public void Enter()
    {
        if (isAvailable)
        {
            enemyMaterial.color = Color.yellow;
            agent.isStopped = false;
            //throw new System.NotImplementedException();
        }
    }

    public void Exit()
    {
        agent.isStopped = true;
        //enemyMaterial.color = Color.black;
    }

    public void FixTick()
    {
        //throw new System.NotImplementedException();
    }

    public void Tick()
    {
        if (isAvailable)
        {
            float distance = Vector3.Distance(myTransform.position, player.transform.position);
            agent.SetDestination(EnemyAI.targetPosition);
            if (SM.fieldOfViewRedCheck())
            {
                SM.ChangeState(SM.chaseState);
            }
            Debug.Log(Vector3.Distance(myTransform.position,EnemyAI.targetPosition));
            if (Vector3.Distance(myTransform.position, EnemyAI.targetPosition) <= 5f) 
            {
                Debug.Log("Search!");
                SM.ChangeState(SM.idleState);
            }
            if ((distance <= yellowZoneRadius) && (yellowZoneRadius >= redZoneRadius))
            {
                redZoneRadius += redZoneIncSpeed * Time.deltaTime;
            }
        }
        if (!isAvailable)
        {
            SM.ChangeState(SM.chaseState);
        }
    }
}

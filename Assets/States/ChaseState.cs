using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    GameObject player;
    Transform myTransform;
    EnemyAI SM;
    Material enemyMaterial;
    Rigidbody rb;
    NavMeshAgent agent;
    float lookRadius;
    public ChaseState(bool _isAvailable, EnemyAI SM, Material enemyMaterial, Transform myTransform, Rigidbody rb,NavMeshAgent agent, GameObject player,float lookRadius)
    {
        //Needs
        this.myTransform = myTransform;
        this.isAvailable = _isAvailable;
        this.SM = SM;
        this.enemyMaterial = enemyMaterial;
        this.rb = rb;
        this.agent = agent;
        this.player = player;
        this.lookRadius = lookRadius;
    }

    public bool isAvailable { get ; set; }

    public void Enter()
    {
        if (isAvailable) 
        {
            enemyMaterial.color = Color.red;
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

            if (distance <= lookRadius)
            {
                agent.SetDestination(player.transform.position);
            }

            if (distance <= .1f || distance >= lookRadius)
            {
                SM.ChangeState(SM.idleState);
            }
        }
        if (!isAvailable) 
        {
            SM.ChangeState(SM.idleState);
        }
    }
}

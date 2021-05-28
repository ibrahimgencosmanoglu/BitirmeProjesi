using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    bool initialMove = false;
    EnemyAI SM;
    GameObject player;
    Transform myTransform;
    NavMeshAgent agent;
    float yellowZoneRadius;
    Vector3 startPoint;
    Vector3 endPoint;
    float speed;
    //This part will removed
    Material enemyMaterial;
    Color enemyColor = Color.black;
    float redZoneRadius;
    
    public IdleState(bool _isAvailable, EnemyAI SM, Material enemyMaterial, Transform myTransform, GameObject player, float yellowZoneRadius, Vector3 startPoint, Vector3 endPoint, float speed, NavMeshAgent agent,float redZoneRadius) {
        //Needs
        this.yellowZoneRadius = yellowZoneRadius;
        this.player = player;
        this.myTransform = myTransform;
        this.isAvailable = _isAvailable;
        this.SM = SM;
        this.enemyMaterial = enemyMaterial;
        this.startPoint = startPoint;
        this.endPoint= endPoint;
        this.speed = speed;
        this.agent = agent;
        this.redZoneRadius = redZoneRadius;

    }
    public bool isAvailable { get; set; }

    public void Enter()
    {
        initialMove = false;
        agent.isStopped = true;
        enemyColor = enemyMaterial.color;
        enemyMaterial.color = Color.black;
        //this.redZoneRadius = EnemyAI.redZoneDefault;
    }

    public void Exit()
    {
       //do nothing
    }

    public void FixTick()
    {
        //this method for physics to run it in fixed update
        //do snothing
    }

    public void Tick()
    {
        if (!initialMove) 
        {
            agent.isStopped = false;
            agent.SetDestination(findClosestDestination());
            initialMove = true;
        }
        else if (Vector3.Distance(myTransform.position, endPoint) <= 2f)
        {
            changePosition();
            agent.SetDestination(endPoint);
        }
        //Debug.Log(Vector3.Distance(myTransform.position, endPoint));
        //if (Vector3.Distance(myTransform.position, player.transform.position) <= yellowZoneRadius)
        //{
        //    EnemyAI.targetPosition = player.transform.position;
        //    this.SM.ChangeState(SM.detectionState);
        //}
        if (SM.fieldOfViewRedCheck(this.redZoneRadius))
        {
            EnemyAI.targetPosition = player.transform.position;
            this.SM.ChangeState(SM.chaseState);
        }
        else if (SM.fieldOfViewYellowCheck(this.yellowZoneRadius)) {
            EnemyAI.targetPosition = player.transform.position;
            this.SM.ChangeState(SM.detectionState);
        }
    }

    public void changePosition() 
    {
        
        Vector3 temp = endPoint;
        endPoint = startPoint;
        startPoint = temp;
    }

    public Vector3 findClosestDestination() 
    {
        if (Vector3.Distance(myTransform.position, startPoint) > Vector3.Distance(myTransform.position, endPoint))
        {
            changePosition();
            return endPoint;
        }
        else
            return endPoint;
    }
    // Start is called before the first frame update
}

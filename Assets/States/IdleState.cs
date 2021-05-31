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
    //This part will removed
    Material enemyMaterial;
    Color enemyColor = Color.black;
    float redZoneRadius;
    public bool isAvailable { get; set; }

    // Constructor

    public IdleState(bool _isAvailable, EnemyAI SM, Material enemyMaterial, Transform myTransform, GameObject player, float yellowZoneRadius, Vector3 startPoint, Vector3 endPoint, NavMeshAgent agent,float redZoneRadius) {
        //Needs
        this.yellowZoneRadius = yellowZoneRadius;   // Sarı alan yarı çapı
        this.player = player;                       // player objesi
        this.myTransform = myTransform;             // kendi pozisyonu
        this.isAvailable = _isAvailable;            // state var mı yok mu
        this.SM = SM;                               // YapayZeka objesi
        this.enemyMaterial = enemyMaterial;         // materyali
        this.startPoint = startPoint;               // volta atarken ki başlangıç noktası
        this.endPoint= endPoint;                    // volta atarken ki varış noktası
        this.agent = agent;                         // navmesh agent
        this.redZoneRadius = redZoneRadius;         // kırmızı alan yarı çapı

    }

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
        //do nothing
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
        if (SM.fieldOfViewRedCheck(this.redZoneRadius))
        {
            EnemyAI.targetPosition = player.transform.position;
            this.SM.ChangeState(SM.chaseState);                         // kırmızı alana girerse kovalama durumuna geçer
        }
        else if (SM.fieldOfViewYellowCheck(this.yellowZoneRadius)) {
            EnemyAI.targetPosition = player.transform.position;
            this.SM.ChangeState(SM.detectionState);                     // sarı alana girerse tespit etme alanına girer
        }
    }

    public void changePosition() 
    {
        
        Vector3 temp = endPoint;
        endPoint = startPoint;
        startPoint = temp;
    }

    public Vector3 findClosestDestination()             // yapay zekanın bulunduğu konumda başlangıç ve varış noktalarına olan en yakın mesafeyi bulduğu fonksiyon 
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

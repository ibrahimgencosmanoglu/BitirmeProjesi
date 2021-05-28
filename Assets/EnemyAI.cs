using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : StateMachine
{
    public DetectionState detectionState { get; private set; }
    public IdleState idleState { get; private set; }
    public ChaseState chaseState { get; private set; }
    public SearchState searchState { get; private set; }
    NavMeshAgent agent;
    [SerializeField] public float redZoneRadius;
    [SerializeField] float redZoneIncSpeed;
    [SerializeField] public float yellowZoneRadius;
    [Range(0,360)] public float angle;
    [SerializeField] public bool canSeePlayer;
    [SerializeField] float detectionTime;
   // [SerializeField] Transform target;
    [SerializeField] MeshRenderer enemyMesh = null;
    [SerializeField] Rigidbody rb;
    //[SerializeField] float lookRadius = 10f;
    [SerializeField] bool isIdleState = true;
    [SerializeField] bool isChaseState = true;
    [SerializeField] bool isDetectionState = true;
    [SerializeField] bool isSearchState = true;
    [SerializeField] Vector3 idleStartPoint;
    [SerializeField] Vector3 idleEndPoint;
    [SerializeField] float speed;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] public GameObject playerRef;
    public static Vector3 targetPosition;
    public static float redZoneDefault = new float();
    private void Awake()
    {
        //redZoneDefault = redZoneRadius;
        this.agent = GetComponent < NavMeshAgent>();
        this.agent.speed = speed;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        //target = PlayerManager.instance.player.transform;
        idleState = new IdleState(isIdleState,this,enemyMesh.material,this.transform,this.playerRef,this.yellowZoneRadius,this.idleStartPoint,this.idleEndPoint,this.agent,this.redZoneRadius);
        chaseState = new ChaseState(isChaseState,this,enemyMesh.material,this.transform,this.rb,this.agent,this.playerRef,this.yellowZoneRadius);
        detectionState = new DetectionState(isDetectionState, this, enemyMesh.material, this.transform, this.playerRef, this.yellowZoneRadius, this.redZoneRadius, this.redZoneIncSpeed,this.agent,this.detectionTime);
        searchState = new SearchState(isSearchState, this, enemyMesh.material, this.transform, this.rb, this.agent, this.playerRef, this.redZoneRadius, this.yellowZoneRadius, this.redZoneIncSpeed);
        
    }

    void Start()
    {
        //StartCoroutine(fieldOfViewRoutine());
        ChangeState(idleState);
    }

    

    public bool fieldOfViewYellowCheck(float yellowZone) {
        Collider[] YellowCircleChecks = Physics.OverlapSphere(transform.position, yellowZone, targetMask);
        

        if (YellowCircleChecks.Length != 0)
        {
            Transform target = YellowCircleChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    return true;
                else
                    return false;

            }
            else
                return false;
        }
        else 
            return false;
    }

    public bool fieldOfViewRedCheck(float redZone) {
        Collider[] RedCircleChecks = Physics.OverlapSphere(transform.position, redZone, targetMask);

        if (RedCircleChecks.Length != 0)
        {
            Transform target = RedCircleChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;
    }

}

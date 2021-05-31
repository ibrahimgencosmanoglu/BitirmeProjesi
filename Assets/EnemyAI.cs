using System.Collections;                                       //C# namespace
using System.Collections.Generic;                               //C# namespace
using UnityEngine;                                              //Unitynin bize sunduğu Vector3,GameObject,LayerMask,Transform vs. sınıflarının bulunduğu namespace
using UnityEngine.AI;                                           //NavMesh ve NavMeshAgent için gerekli olan namespace

//EnemyAI Scripti Kullanıcı arayüzü ile

public class EnemyAI : StateMachine         
{
    // Durumların Tanımlandığı yer
    public DetectionState detectionState { get; private set; }
    public IdleState idleState { get; private set; }
    public ChaseState chaseState { get; private set; }
    public SearchState searchState { get; private set; }
    NavMeshAgent agent;

    // Durumların seçileceği yerler

    [SerializeField] bool isIdleState = true;
    [SerializeField] bool isChaseState = true;
    [SerializeField] bool isDetectionState = true;
    [SerializeField] bool isSearchState = true;

    // Unity Componentlerinin kullanıcı arayüzüne ekleneceği yerler

    [SerializeField] MeshRenderer enemyMesh = null;
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 idleStartPoint;
    [SerializeField] Vector3 idleEndPoint;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] public GameObject playerRef;

    // Unity kullanıcı ara yüzünden eklenecek olan nümerik alanlar 

    [SerializeField] public float redZoneRadius;
    [SerializeField] float redZoneIncSpeed;
    [SerializeField] public float yellowZoneRadius;
    [Range(0,360)]   public float angle;
    [SerializeField] float detectionTime;
    [SerializeField] float speed;


    

    public static Vector3 targetPosition;
    public static float redZoneDefault = new float();


    private void Awake()                                // Start metodundan önce çağrılır bütün durumlar bu metodun içerisinde başlatılır
    {
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
        ChangeState(idleState);                         // ilk durum başlangıcı
    }

    

    public bool fieldOfViewYellowCheck(float yellowZone) {          // Sarı Alan Görüş Açısını belirlediğimiz metod
        Collider[] YellowCircleChecks = Physics.OverlapSphere(transform.position, yellowZone, targetMask);      // Yapay Zekanın etrafında yarı çapı yellowZone değişkeni olan görünmez bir küre oluşturur
                                                                                                                // ve bu küre içerisinde olan ve targetMask ile aynı katmanda olan objeleri dönderir.
        

        if (YellowCircleChecks.Length != 0)                         // yukarıda oluşturulan küreden bir değer döndüyse
        {
            Transform target = YellowCircleChecks[0].transform;     // kürenin içerisine giren nesnenin pozisyonu belirlenir.
            Vector3 directionToTarget = (target.position - transform.position).normalized; //yapay zeka ve oyuncu arasındaki mesafe alınıp normalize (0-1 arasında bir değere normalleştirilir.) edilir.

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)                            // yapay zekanın z ekseni ve düşman arasındaki açı hesaplanır
                                                                                                            // tanımlanan görüş açısının yarısından ufaksa işlem yapılır.
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) // Unity Physics sınıfının metodu olan Raycast kullanılarak 
                    return true;                                                                                // karakterin bulunduğu konumdan ışın yollar yolladığı ışın
                                                                                                                // obstructionMask katmanıyla aynı katmanda olan bir objeye çarparsa orada sonlanır
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

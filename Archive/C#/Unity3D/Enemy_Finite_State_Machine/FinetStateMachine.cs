using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FinetStateMachine : MonoBehaviour {

    enum EnemyState
    {
        Initialization,
        Idle,
        Walk,
        See,
        Attack,
        Strike,
        Find,
        Dead
    }
    EnemyState enemyState;
    Coroutine currentCoroutine;

    Transform playerTransform;
    ChunkSpawn chunk;
    [SerializeField]
    NavMeshAgent enemy;

    float seeDistance = 15f;
    float attackDistance = 10f;
    float strikeDistance = 1.5f;

    float rotationSpeed = 10f;
    [SerializeField]
    float minIdleTime = 5f;
    [SerializeField]
    float maxIdleTime = 10f;
    float timeToIdle = 0f;
    float curretIdleTime = 0f;

    float timeToSee = 2f;                       // time, which the enemy "sees" the player and then goes to him
    float curretSeeTime = 0f;

    float fov = 60f;

    int layerMask = 0;                          // gets value in the Awake method

    Vector3 lastWalkedPosition = Vector3.zero;
    Vector3 currentWalkedPosition = Vector3.zero;
    Vector3 currentStatePosition = Vector3.zero;

    Vector3 lastTargetPosition = Vector3.zero;

    public ChunkSpawn ChunkSpawn {
        get { return chunk; }
        set { chunk = value; }
    }
    Vector3 playerDirection
    {
        get { return playerTransform.position - transform.position; }
    }
    float PlayerDistance
    {
        get { return Vector3.Distance(transform.position, playerTransform.position); }
    }
    bool findPath = false;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = EnemyState.Initialization;
        layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Mesh");
    }

    void Start () {
        currentCoroutine = StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while(true)
        {
            switch (enemyState)
            {
                case EnemyState.Initialization:
                    Initialization();
                    break;
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Walk:
                    Walk();
                    break;
                case EnemyState.See:
                    See();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Strike:
                    Strike();
                    break;
                case EnemyState.Find:
                    Find();
                    break;
                case EnemyState.Dead:
                    Dead();
                    break;
            }
            yield return null;
        }
    }

    void Initialization()
    {
        enemyState = EnemyState.Idle;
    }

    void Idle()
    {
        Debug.Log("Idle");
        curretIdleTime += Time.deltaTime;
        if(curretIdleTime < timeToIdle)
        {
            timeToIdle = Random.Range(minIdleTime, maxIdleTime);
            // looks for the player
            float playerAngle = GetPlayerAngle();
            // if the player gets in the enemy fov
            if (playerAngle < fov && playerAngle > 0 && FindPlayerSeeRayCast())
            {
                // if the enemy sees the player
                    curretIdleTime = 0;
                    enemyState = EnemyState.See;
                    return;
            }
        }
        else
        {
            // if wasn't found the player while idle - walk
            curretIdleTime = 0;
            timeToIdle = Random.Range(minIdleTime, maxIdleTime);
            enemyState = EnemyState.Walk;
        }
    }

    void Walk()
    {
        Debug.Log("Walk");
        // prevents the enemy from walking only between two points
        // TODO make it works with offset
        if (!findPath)
        {
            while ((int)currentWalkedPosition.magnitude == (int)lastWalkedPosition.magnitude || currentWalkedPosition == currentStatePosition)
            {
                var index = Random.Range(0, chunk.walkPonts.Count);
                currentWalkedPosition = chunk.walkPonts[index].position;
            }
            lastWalkedPosition = enemy.transform.position;
            findPath = true;
        }
        // if the enemy gets to the position
        else if (enemy.remainingDistance <= 0.01)
        {
            findPath = false;
            currentStatePosition = currentWalkedPosition;
            enemyState = EnemyState.Idle;
            return;
        }
        // looks for the player
        var playerAngle = GetPlayerAngle();
        // if the player gets in the enemy fov
        if (playerAngle < fov && playerAngle > 0 && FindPlayerSeeRayCast())
        {
            // if the enemy sees the player
                curretIdleTime = 0;
                enemyState = EnemyState.See;
                enemy.destination = enemy.transform.position; // stops the enemy
                return;
        }
        // if wasn't found the player - continue walking
        enemy.destination = currentWalkedPosition;
    }

    void See()
    {
        Debug.Log("See");
        curretSeeTime += Time.deltaTime;
        // rotates the enemy to the player direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection), rotationSpeed * Time.deltaTime);
        var playerAngle = GetPlayerAngle();
        if (curretSeeTime > timeToSee)
        {
            lastTargetPosition = playerTransform.position;
            enemyState = EnemyState.Find;
            curretSeeTime = 0;
            return;
        }
        else if(FindPlayerAttackRayCast())
        {
            enemyState = EnemyState.Attack;
            return;
        }
        else if(playerAngle > fov || playerAngle < 0 && !FindPlayerSeeRayCast())
        {
            enemyState = EnemyState.Idle;
        }
    }

    void Find()
    {
        Debug.Log("Find");
        enemy.destination = lastTargetPosition;
        var playerAngle = GetPlayerAngle();
        if (playerAngle < fov && playerAngle > 0 && FindPlayerAttackRayCast())
        {
            enemyState = EnemyState.Attack;
            return;
        }
        else if (playerAngle < fov && playerAngle > 0 && FindPlayerSeeRayCast())
        {
            lastTargetPosition = playerTransform.position;
        }
        else if (enemy.velocity.magnitude < 0.1f)
        {
            enemyState = EnemyState.Idle;
            return;
        }
    }

    void Attack()
    {
        Debug.Log("Attack");
        enemy.destination = playerTransform.position;
        var playerAngle = GetPlayerAngle();
        if (playerAngle < fov && playerAngle > 0 && FindPlayerAttackRayCast() && PlayerDistance <= strikeDistance)
        {
            enemyState = EnemyState.Strike;
            return;
        }
        else if(!FindPlayerAttackRayCast() || PlayerDistance > strikeDistance && !(playerAngle < fov && playerAngle > 0))
        {
            enemyState = EnemyState.Find;
            return;
        }
    }

    void Strike()
    {
        Debug.Log("Strike");
        if(PlayerDistance <= strikeDistance && FindPlayerAttackRayCast())
        {
                // strike code
        }
        else
        {
            enemyState = EnemyState.Find;
        }
    }

    void Dead()
    {
        // death code
        StopCoroutine(currentCoroutine);
    }

    #region PLAYER_LOOKING_FUNCTIONS

    private float GetPlayerAngle()
    {
        return Vector3.Angle(transform.forward, (playerTransform.position - transform.position));
    }

    private bool FindPlayerSeeRayCast()
    {
        RaycastHit hitObject;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hitObject, seeDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hitObject.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private bool FindPlayerAttackRayCast()
    {
        RaycastHit hitObject;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hitObject, attackDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hitObject.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seeDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, strikeDistance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//swicth to using enums 
public enum EnemyState
{
    idle,
    attack,
    runaway,
    chase,
    protect
}
public class EnemyAI : MonoBehaviour
{

    public Text stateText;
    public enum States { idle = 1 , attack = 2, runaway , chase, protect};

    public float lookRadius = 3f;
    public Transform target;


    public float timeBetweenShots;
    public float startTimeBetweenShots;
    public GameObject projectile;

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    public Transform player;

    public int level = 1;

    public static EnemyAI instance;

    public EnemyState currentState;

    
    private void Start()
    {
        //currentState = 
        stateText.text = "State: " + currentState;
    }

    
    private void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);

        float outerView = (float)(lookRadius * 0.75) + lookRadius;

        if (distance >= lookRadius && distance <= outerView && PlayerMovement.instance.level < level)
        {
            Debug.Log("Player is within range and i should attack");
            currentState = EnemyState.chase;
            Chase();
        }

        if (distance <= lookRadius && PlayerMovement.instance.level < level)
        {
            Debug.Log("Player is within range and i should attack");
            currentState = EnemyState.attack;
            Attack();
        }

        if (distance <= lookRadius && PlayerMovement.instance.level > level)
        {
            Debug.Log("Player is within range but i should run");
            currentState = EnemyState.runaway;
            RunAway();
        }
     
       if(distance >= outerView)
        {
            Debug.Log("Player not withing range");
            currentState = EnemyState.idle;
            Idle();
        }

        if(Treasure.instance.isInDistress == true)
        {
            currentState = EnemyState.protect;
            TrackTreasure();
        }

        if(Treasure.instance.isInDistress != true)
        {
            Idle();
        }

        stateText.text = "State: " + currentState;


        timeBetweenShots -= Time.deltaTime;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);


        float outerView = (float)(lookRadius * 0.75) + lookRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, outerView);
    }

    void Attack()
    {
        if (timeBetweenShots <=0)
        {
            
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    public void Chase()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void RunAway()
    {     
        transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
    }
    
    void TrackTreasure()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        currentState = EnemyState.protect;
        if (PlayerMovement.instance.level > level)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
    }

    void Idle()
    {        
        transform.position = transform.position;
    }

}

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
      
        
    }

    
    private void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            if(PlayerMovement.instance.level < level)
            {
                
                Chase();
            }
            else if(PlayerMovement.instance.level > level)
            {
                
                RunAway();
            }
        }
        else
        {
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


        timeBetweenShots -= Time.deltaTime;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void Attack()
    {
        currentState = EnemyState.attack;
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
        currentState = EnemyState.chase;
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            Attack();
        }
        else
        {
            
        }
    }

    void RunAway()
    {     
        transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        currentState = EnemyState.runaway;
    }
    
    void TrackTreasure()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //Pass protect state to UI, somehow
        currentState = EnemyState.protect;
        if (PlayerMovement.instance.level > level)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
       
    }

    void Idle()
    {
        //if enemy is not moving then set state to idle
        currentState = EnemyState.idle;
        transform.position = transform.position;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public bool isInDistress;
    public Transform player;
    public float lookRadius = 1.5f;





    public static Treasure instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance <= lookRadius)
        {
            isInDistress = true;
        }

        else if(distance > lookRadius)
        {
            isInDistress = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}

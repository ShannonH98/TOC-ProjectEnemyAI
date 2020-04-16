using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 9;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    private Vector2 startLocation;

    public int level = 5;

    public static PlayerMovement instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();

        //store the start locatiton
        startLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Bullet")
        {
            slowdown();
        } 
        else if (collision.CompareTag("Enemy"))
        {
            transform.position = startLocation;
        }
    }


    private void slowdown()
    {     
        this.speed--;
    }
}

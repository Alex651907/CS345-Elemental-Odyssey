using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour
{
    public Transform player;
    public float chaseDistance = 10f;
    public float moveSpeed = 2f;
    private Rigidbody2D myRigidbody2D;
    private float wanderTimer;
    private Vector2 currentDirection;
    private int terrainLayer = 6;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,
       1f)).normalized;
        wanderTimer = Random.Range(3, 6);
        speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody2D.velocity = currentDirection * speed;
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg *
        Mathf.Atan2(currentDirection.y, currentDirection.x), Vector3.forward);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance)
        {
            speed = moveSpeed + 0.5f;
            currentDirection = (player.position - transform.position).normalized;
        } 
        else
        {
            speed = moveSpeed;
            if (wanderTimer <= 0f)
            {
                currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,
       1f)).normalized;
                wanderTimer = Random.Range(3, 6);
            }
        }

        wanderTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == terrainLayer)
        {
            currentDirection = -currentDirection;
        }
    }
}

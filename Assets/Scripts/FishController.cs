using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float speed;
    public float turnDistance;
    private Vector2 currentDirection;
    private Rigidbody2D myRigidbody2D;
    private float wanderTimer;
    private int terrainLayer = 6;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,
       1f)).normalized;
        wanderTimer = Random.Range(3, 6);
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody2D.velocity = currentDirection * speed;
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg *
        Mathf.Atan2(currentDirection.y, currentDirection.x), Vector3.forward);

        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            currentDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,
       1f)).normalized;
            wanderTimer = Random.Range(2, 8);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == terrainLayer)
        {
            currentDirection = -currentDirection;
        }
    }
}

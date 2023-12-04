using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    public float speed = 4f;
    private Vector2 currentPosition;
    private Vector2 currentDirection;
    private Vector2 startingPos;

    void Start()
    {
        currentPosition = transform.position;
        currentDirection = Vector2.left;
        startingPos = transform.position;
    }

    void Update()
    {
        transform.Translate(currentDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Beach")
        {
            currentDirection = -currentDirection;
        }
    }

    public void ResetPosition()
    {
        transform.position = startingPos;
    }
}

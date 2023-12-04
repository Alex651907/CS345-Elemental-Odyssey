using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 initialPosition;

    // Adjust these parameters to fit your game
    public float jumpForce = 10f;
    public float jumpInterval = 4f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        // Call the Jump method every jumpInterval seconds, starting from the beginning
        InvokeRepeating("Jump", 0f, jumpInterval);
    }

    void Jump()
    {
        // Reset the position to the initial position
        transform.position = initialPosition;

        // Add an upward force to make the fireball jump
        rb2d.velocity = new Vector2(0f, jumpForce);
        StartCoroutine(ResetFireball());
    }

    IEnumerator ResetFireball() {
        yield return new WaitForSeconds(2);
        transform.position = initialPosition;
        rb2d.velocity = new Vector2(0f, 0f);
    }

}

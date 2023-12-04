using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceController : MonoBehaviour
{
    private float wobbleSpeed = 30f;
    private float wobbleAmount = 15f;

    // Start is called before the first frame update
    void Start()
    {
        // Invoke the Wobble method after 2 seconds
        Invoke("Wobble", 2f);

        // Invoke the DestroySelf method after 4 seconds
        Invoke("DestroySelf", 4f);
    }

    // Update is called once per frame
    void Update()
    {
        // You can add additional logic here if needed
    }

    // Custom method to wobble the ice block
    void Wobble()
    {
        StartCoroutine(WobbleAnimation());
    }

    // Custom method to destroy the game object
    void DestroySelf()
    {
        // Destroy the game object this script is attached to
        Destroy(gameObject);
    }

    // Custom method for the wobbling animation
    IEnumerator WobbleAnimation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f) // Wobble for 2 seconds
        {
            float angle = Mathf.Sin(elapsedTime * wobbleSpeed) * wobbleAmount;

            // Rotate the ice block back and forth
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the ice block is upright when the animation is done
        transform.rotation = Quaternion.identity;
    }

    // Handle collisions with other 2D objects
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("lava"))
        {
            // Destroy the object tagged as "lava"
            Destroy(col.gameObject);
        }
    }
}

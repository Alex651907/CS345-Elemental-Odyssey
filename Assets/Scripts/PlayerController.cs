using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public Transform groundCheck;
    public LayerMask whatIsTerrain;
    public LayerMask whatIsWater;
    public Animator animator;
    private Rigidbody2D rb;
    private bool grounded;
    private bool wet;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
        animator.SetFloat("speed", rb.velocity.magnitude);
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsTerrain);
        wet = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsWater);

        animator.SetBool("grounded", grounded);

        if(Input.GetButtonDown("Jump") && (grounded || wet))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            animator.SetBool("grounded", false);
        }
    }
}

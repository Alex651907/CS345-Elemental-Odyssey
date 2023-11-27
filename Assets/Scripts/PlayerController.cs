using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float jumpSpeed;
    public float defaultMoveSpeed;
    public float defaultJumpSpeed;
    public Transform groundCheck;
    public LayerMask whatIsTerrain;
    public LayerMask whatIsWater;
    public Animator animator;
    private Rigidbody2D rb;
    public PlayerLives playerLives;
    public Vector2 startingPos = Vector2.zero;
    public bool grounded;
    public bool wet;
    private int lives;
    public string gameOverScene;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        lives = 3;
        moveSpeed = defaultMoveSpeed;
        jumpSpeed = defaultJumpSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsTerrain);
        wet = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsWater);

        if(wet)
        {
            moveSpeed = 2;
            jumpSpeed = 3;
            rb.mass = 4;
            rb.gravityScale = 0.5f;
        }
        else if(!wet)
        {
            moveSpeed = defaultMoveSpeed;
            jumpSpeed = defaultJumpSpeed;
            rb.mass = 1;
            rb.gravityScale = 1;
        }

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

        animator.SetBool("grounded", grounded);

        if(Input.GetButtonDown("Jump") && (grounded || wet))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            animator.SetBool("grounded", false);
        }

        if(lives <= 0)
        {
            SceneManager.LoadScene(gameOverScene);
        }
    }
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "lava" || co.tag == "enemy"){
            lives -= 1;
            transform.position = startingPos;
            playerLives.updateLives(lives);
        }
        if (lives <= 0) {
            SceneManager.LoadScene(gameOverScene);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float jumpSpeed;
    private float sprintSpeed;
    public float defaultMoveSpeed;
    public float defaultJumpSpeed;
    public float defaultSprintSpeed;
    public Transform groundCheck;
    public LayerMask whatIsTerrain;
    public LayerMask whatIsWater;
    public Animator animator;
    private Rigidbody2D rb;
    public PlayerLives playerLives;
    public Vector2 startingPos = Vector2.zero;
    public bool grounded;
    public bool wet;
    public bool sprinting;
    private int lives;
    public string gameOverScene;
    public GameObject[] powerUpAssets;
    public AudioSource powerUpCollect;
    public AudioSource jumpSound;
    public GameObject[] crabs;
    public GameObject powerUpItem;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        lives = 3;
        moveSpeed = defaultMoveSpeed;
        jumpSpeed = defaultJumpSpeed;
        sprintSpeed = defaultSprintSpeed;
        foreach (GameObject asset in powerUpAssets)
        {
            asset.SetActive(false);
        }
        sprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsTerrain);
        wet = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsWater);
        animator.SetBool("wet", wet);

        if (wet)
        {
            moveSpeed = defaultMoveSpeed - 2;
            sprintSpeed = defaultSprintSpeed - 2;
            jumpSpeed = 3;
            rb.mass = 4;
            rb.gravityScale = 0.5f;
            
        }
        else if(!wet)
        {
            moveSpeed = defaultMoveSpeed;
            sprintSpeed = defaultSprintSpeed;
            jumpSpeed = defaultJumpSpeed;
            rb.mass = 1f;
            rb.gravityScale = 1f;
        }

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * (sprinting ? sprintSpeed : moveSpeed), rb.velocity.y);
        animator.SetFloat("speed", rb.velocity.magnitude);
        animator.SetBool("sprinting", sprinting && (grounded || wet));

        animator.SetBool("grounded", grounded);

        if (Input.GetButtonDown("Jump") && (grounded || wet))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            animator.SetBool("grounded", false);
            if (!wet)
            {
                jumpSound.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

            foreach(GameObject asset in powerUpAssets)
            {
                bool facingLeft = asset.GetComponent<SpriteRenderer>().flipX;
                asset.GetComponent<SpriteRenderer>().flipX = true;

                if (asset.name == "scubaTank" && !facingLeft)
                {
                    asset.transform.localPosition = new Vector3(-asset.transform.localPosition.x, asset.transform.localPosition.y, 0.0f);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;

            foreach (GameObject asset in powerUpAssets)
            {
                bool facingRight = !asset.GetComponent<SpriteRenderer>().flipX;
                asset.GetComponent<SpriteRenderer>().flipX = false;

                if (asset.name == "scubaTank" && !facingRight)
                {
                    asset.transform.localPosition = new Vector3(-asset.transform.localPosition.x, asset.transform.localPosition.y, 0.0f);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            sprinting = true;

        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            sprinting = false;
        }

        if (lives <= 0)
        {
            SceneManager.LoadScene(gameOverScene);
        }
    }
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "lava" || co.tag == "enemy"){
            lives -= 1;

            foreach (GameObject crab in crabs)
            {
                crab.GetComponent<CrabController>().ResetPosition();
            }

            powerUpItem.SetActive(true);

            foreach (GameObject asset in powerUpAssets)
            {
                asset.SetActive(false);
            }

            transform.position = startingPos;
            playerLives.updateLives(lives);
        }
        if (lives <= 0) {
            SceneManager.LoadScene(gameOverScene);
        }

        if (co.tag == "power")
        {
            powerUpCollect.Play();
            co.gameObject.SetActive(false);
            foreach (GameObject asset in powerUpAssets)
            {
                asset.SetActive(true);
            }
        }

        if (co.tag == "level_up")
        {
            co.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}

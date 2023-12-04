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
    private Vector2 startingPos;
    private Vector3 startingScale;
    private Quaternion startingRotation;
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
    public AudioSource playerDieAudio;
    public AudioSource waterDieAudio;
    public AudioSource gameOverAudio;
    public AudioSource backgroundAudio;
    private bool deathTimerStart = false;
    private float deathAnimationTimer = 0f;
    private float dieRoationSpeed = 100f;
    private float dieScaleDownRate = .3f;
    public GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        startingRotation = transform.rotation;
        startingScale = transform.localScale;
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
        if (!deathTimerStart)
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
            else if (!wet)
            {
                moveSpeed = defaultMoveSpeed;
                sprintSpeed = defaultSprintSpeed;
                jumpSpeed = defaultJumpSpeed;
                rb.mass = 1;
                rb.gravityScale = 1;
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

                foreach (GameObject asset in powerUpAssets)
                {
                    bool facingLeft = asset.GetComponent<SpriteRenderer>().flipX;
                    asset.GetComponent<SpriteRenderer>().flipX = true;

                    if (asset.name == "scubaTank" && !facingLeft)
                    {
                        asset.transform.localPosition = new Vector3(-asset.transform.localPosition.x, asset.transform.localPosition.y, 0.0f);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
        } 
        else if (!(Vector3.Distance(transform.localScale, new Vector3(.1f, .1f, 1f)) < 0.01f))
        {
            foreach (GameObject crab in crabs)
            {
                crab.GetComponent<CrabController>().ResetPosition();
                crab.SetActive(false);
            }

            if (deathAnimationTimer == 0f)
            {
                if (wet)
                {
                    waterDieAudio.Play();
                }
                else
                {
                    playerDieAudio.Play();
                }
            }

            transform.rotation *= Quaternion.Euler(0f, 0f, dieRoationSpeed * Time.deltaTime);
            transform.localScale = transform.localScale - new Vector3(dieScaleDownRate, dieScaleDownRate, 0) * Time.deltaTime;

            deathAnimationTimer += 1f;
        }
        else
        {
            foreach (GameObject crab in crabs)
            {
                crab.SetActive(true);
            }

            powerUpItem.SetActive(true);

            foreach (GameObject asset in powerUpAssets)
            {
                asset.SetActive(false);
            }

            transform.position = startingPos;
            transform.localScale = startingScale;
            transform.rotation = startingRotation;

            deathAnimationTimer = 0f;
            deathTimerStart = false;
        }

       
    }
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "lava" || co.tag == "enemy"){
            lives -= 1;
            playerLives.updateLives(lives);
            deathTimerStart = true;
        }
        if (lives <= 0) {
            gameOverAudio.Play();
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

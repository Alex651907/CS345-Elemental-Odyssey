using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    public string gameOverScene;
    public GameObject[] powerUpAssets;
    public GameObject[] crabs;
    public GameObject powerUpItem;
    private float dieRoationSpeed = 100f;
    private float dieScaleDownRate = .3f;
    public GameObject cameraObject;
    public GameObject breathMeter;
    public float maxBreath;
    private float breath;
    public AudioController audioController;
    public bool controlsSuspended;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        startingRotation = transform.rotation;
        startingScale = transform.localScale;
        moveSpeed = defaultMoveSpeed;
        jumpSpeed = defaultJumpSpeed;
        sprintSpeed = defaultSprintSpeed;
        foreach (GameObject asset in powerUpAssets)
            asset.SetActive(false);
        controlsSuspended = false;
        sprinting = false;
        breath = maxBreath;
        playerLives.updateLives(Lives.GetLives());
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsTerrain);
        wet = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, whatIsWater);
        animator.SetBool("wet", wet);

        if (wet)
        {
            breathMeter.SetActive(true);
            breath -= Time.deltaTime;
            if(breath < 0.001f)
            {
                Lives.LoseLife();
                audioController.playDeathWater();
                controlsSuspended = true;
                StartCoroutine(playDeathAnimation(3.0f));
                playerLives.updateLives(Lives.GetLives());
            }
            moveSpeed = defaultMoveSpeed - 2;
            sprintSpeed = defaultSprintSpeed - 2;
            jumpSpeed = 3;
            rb.mass = 4;
            rb.gravityScale = 0.5f;
        }
        else if(!wet)
        {
            if(breath < maxBreath)
                breath += Time.deltaTime;
            else
                breathMeter.SetActive(false);
            moveSpeed = defaultMoveSpeed;
            sprintSpeed = defaultSprintSpeed;
            jumpSpeed = defaultJumpSpeed;
            rb.mass = 1f;
            rb.gravityScale = 1f;
        }
        float breathNorm = Mathf.Clamp01(breath / maxBreath);
        breathMeter.GetComponent<Slider>().value = breathNorm;

        if(!controlsSuspended)
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
                audioController.playJump();
            }
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            foreach (GameObject asset in powerUpAssets)
                asset.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            foreach (GameObject asset in powerUpAssets)
                asset.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKeyDown(KeyCode.Q) && grounded)
        {
            sprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            sprinting = false;
        }
    } 
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "lava" || co.tag == "enemy"){
            Lives.LoseLife();
            playerLives.updateLives(Lives.GetLives());
            audioController.playDeath();
            controlsSuspended = true;
            StartCoroutine(playDeathAnimation(3.0f));
        }
        if (Lives.GetLives() <= 0) {
            SceneManager.LoadScene(gameOverScene);
        }

        if (co.tag == "power")
        {
            audioController.playPowerUpCollect();
            powerUpItem.SetActive(false);
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

    IEnumerator playDeathAnimation(float duration)
    {
        if(crabs.Length > 0)
        {
            foreach (GameObject crab in crabs)
            {
                crab.GetComponent<CrabController>().ResetPosition();
                crab.SetActive(false);
            }
        }
        float timer = Time.time;
        float iniTime = timer;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        while(timer < iniTime + duration){
            transform.rotation *= Quaternion.Euler(0f, 0f, dieRoationSpeed * Time.deltaTime);
            transform.localScale = transform.localScale - new Vector3(dieScaleDownRate, dieScaleDownRate, 0) * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        respawn();
    }

    void respawn()
    {
        transform.position = startingPos;
        transform.localScale = startingScale;
        transform.rotation = startingRotation;
        breath = maxBreath;
        if(crabs.Length > 0)
        {
            foreach (GameObject crab in crabs) 
                crab.SetActive(true);
        }
        if(powerUpAssets.Length > 0){
            powerUpItem.SetActive(true);
            foreach (GameObject asset in powerUpAssets)
            {
                asset.SetActive(false);
            }
        }
        controlsSuspended = false;
    }
}

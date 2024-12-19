using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    float horiMovement;
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float movementSpeed;
    private float jumpHeight;

    [Header("Base Stats")]
    public float baseSpeed = 6f;
    public float baseHeight = 7f;
    public float baseScale = 1.25f;

    [Header("Jumper Stats")]
    public float jumperSpeed = 8f;
    public float jumperHeight = 9f;
    public float jumperScale = 1.25f;

    [Header("Floater Stats")]
    public float floaterSpeed = 5f;
    public float floaterHeight = 7f;
    public float floaterScale = 0.50f;

    [Header("Sprite States")]
    public Sprite baseState;
    public Sprite jumperState;
    public Sprite floaterState;

    [Header("Animator Controllers")]
    public RuntimeAnimatorController baseController;
    public RuntimeAnimatorController jumperController;
    public RuntimeAnimatorController floaterController;

    [Header("Jump Checks")]
    public bool jumped;
    public bool grounded;
    public int jumpCount;

    [Header("Dash Checks")]
    public bool canDash;
    public bool isDashing;
    public float dashSpeed = 20f; 
    public float dashTime = 0.15f;
    public float dashCooldown = 1f;
    public Vector2 dashDir;
    public bool dashed; 

    [Header("Checks")]
    public PlayerStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        grounded = true;
        jumped = false;
        jumpCount = 0;
        dashed = false;

        canDash = true;
        isDashing = false;

        anim = GetComponent<Animator>();    
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentState = PlayerStates.BASE;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                jumped = true;
                jumpCount++;
            }
            else if (currentState == PlayerStates.JUMPER && !grounded && jumpCount == 1)
            {
                jumped = true;
                jumpCount++;
            }
        }

        if (currentState == PlayerStates.BASE)
        {
            if (Input.GetKeyDown(KeyCode.E) && canDash)
            {
                dashed = true;
            }
        }

        SwitchStates();

        switch (currentState)
        {
            case PlayerStates.BASE:
                Base();
                break;
            case PlayerStates.JUMPER:
                Jumper();
                break;
            case PlayerStates.FLOATER:
                Floater();
                break;
        }

    }

    void FixedUpdate()
    {
        Movement();

        if (jumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            grounded = false;
            jumped = false;
        }

        if (dashed)
        {
            StartCoroutine(Dash());
        }
    }

    void SwitchStates()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentState = PlayerStates.BASE;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentState = PlayerStates.JUMPER;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentState = PlayerStates.FLOATER;
        }
    }

    void Base()
    {
        sr.sprite = baseState;
        anim.runtimeAnimatorController = baseController;
        
        movementSpeed = baseSpeed;
        jumpHeight = baseHeight;
        rb.gravityScale = baseScale;
    }

    void Jumper()
    {
        sr.sprite = jumperState;
        anim.runtimeAnimatorController = jumperController;

        movementSpeed = jumperSpeed;
        jumpHeight = jumperHeight;
        rb.gravityScale = jumperScale;
    }

    void Floater()
    {
        sr.sprite = floaterState;
        anim.runtimeAnimatorController = floaterController;

        movementSpeed = floaterSpeed;
        jumpHeight = floaterHeight;
        rb.gravityScale = floaterScale;
    }

    void Movement()
    {
        horiMovement = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horiMovement * movementSpeed, rb.velocity.y);


        if (horiMovement < 0f)
        {
            anim.SetBool("isWalking", true);
            sr.flipX = false;
        }
        else if (horiMovement > 0f)
        {
            anim.SetBool("isWalking", true);
            sr.flipX= true;
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float origScale = rb.gravityScale;
        rb.gravityScale = 0f;
        anim.SetBool("isWalking", false);

        dashDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (dashDir == Vector2.zero)
        {
            dashDir = new Vector2(transform.localScale.x * dashSpeed, 0f);
        }

        rb.velocity = dashDir.normalized * dashSpeed;

        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = origScale;
        isDashing = false;
        dashed = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Platform"))
        {
            grounded = true;
            jumpCount = 0;
        }
    }

    public enum PlayerStates
    {
        BASE,
        JUMPER,
        FLOATER
    }
}

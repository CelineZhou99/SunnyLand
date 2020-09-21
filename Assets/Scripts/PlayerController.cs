using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    // start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    

    // finite state machine
    private enum State { idle, running, jumping, falling, hurt, climb };
    private State state = State.idle;

    // Ladder Variables
    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    public Ladder ladder;
    [SerializeField] float climbSpeed = 5f;

    // inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource gem;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.climb)
        {
            Climb();
        } else if (state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state); // set animation based on Enumerator state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.numCherries.text = PermanentUI.perm.cherries.ToString();
        }

        if (collision.tag == "Powerup")
        {
            gem.Play();
            jumpForce = 19f;
            GetComponent<SpriteRenderer>().color = new Color32(255, 203, 238, 255);
            StartCoroutine(ResetPower()); // coroutine allows us to use timers
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            } else
            {
                state = State.hurt;
                HandleHealth();
                if (other.gameObject.transform.position.x > transform.position.x) // enemy is to my right therefore i should be damaged and move left
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else // enemy is to my left therefore i should be damaged and move right 
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }

        }
    }

    // Reset level if health reaches 0
    private void HandleHealth()
    {
        PermanentUI.perm.health -= 20;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }

        if (hDirection < 0) // moving left
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0) // moving right 
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) // jumping
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.climb)
        {

        } else if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        } else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        } else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        } else if (Mathf.Abs(rb.velocity.x) > 2f) // moving
        {
            state = State.running;
        } else
        {
            state = State.idle;
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(8); // will wait for 8 seconds
        jumpForce = 16f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Climb()
    {
        if (Input.GetButtonDown("Jump")) // jumping
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = 3;
            anim.speed = 1f;
            Jump();
            return;
        }

        float vDirection = Input.GetAxis("Vertical");

        if (vDirection > .1f && !topLadder) // climbing up
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        } else if (vDirection < -.1f && !bottomLadder) // climbing down
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        } else // staying still
        {
            anim.speed = 0f;
            rb.velocity = Vector2.zero;
        }
    }
}

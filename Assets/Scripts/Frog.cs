using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength = 3f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private LayerMask ground;

    private Collider2D coll;

    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start(); // base is what you inherite from
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (anim.GetBool("Jumping")) // transition from jump to fall
        {
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling")) // transition from fall to idle 
        {
            anim.SetBool("Falling", false);
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap) // test to see if we are beyond the leftCap
            {
                if (transform.localScale.x != 1) // make sure sprite is facing the right direction
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (coll.IsTouchingLayers(ground)) // test to see if frog is on the ground, if so then jump
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else // otherwise face right
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap) // test to see if we are beyond the leftCap
            {
                if (transform.localScale.x != -1) // make sure sprite is facing the right direction
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (coll.IsTouchingLayers(ground)) // test to see if frog is on the ground, if so then jump
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else // otherwise face right
            {
                facingLeft = true;
            }
        }
    }
}

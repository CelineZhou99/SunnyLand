using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim; // protected so the inherited classes can access
    protected Rigidbody2D rb;
    protected AudioSource death;

    protected virtual void Start() // virtual means your children can take this and make it your own
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        death.Play();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // stops moving when dying
        GetComponent<Collider2D>().enabled = false; // can't collide into it anymore after death
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}

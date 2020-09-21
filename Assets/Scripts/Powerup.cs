using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("Collect");
            GetComponent<Collider2D>().enabled = false; // can't collide into it anymore after death
        }
    }

    private void Collected()
    {
        Destroy(this.gameObject);
    }
}

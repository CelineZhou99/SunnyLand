using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentUI : MonoBehaviour
{
    // player stats
    public int cherries = 0;
    public int health = 100;

    public TextMeshProUGUI numCherries;
    public Text healthAmount;

    // static means this variable actually belongs to the class itself vs the instance, something everything can get access to
    // belonging to the class itself means you can't make an instance of it
    // so everytime there's an instance of it, its not going to make a new one
    public static PermanentUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // singleton
        if (!perm)
        {
            perm = this; // this = current instance
        } else
        {
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        cherries = 0;
        numCherries.text = cherries.ToString();
        health = 100;
        healthAmount.text = health.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] protected string sceneName;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

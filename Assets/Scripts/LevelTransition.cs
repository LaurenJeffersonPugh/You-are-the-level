using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelTransition : MonoBehaviour
{
    void OnTriggerEnter2D()
    {
        Physics2D.gravity = new Vector2(0, -9.81f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}


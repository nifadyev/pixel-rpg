using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    Animator animator;
    public Image[] hearts;
    public int maxHealth;
    int currentHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        GetHealth();
    }

    void Update()
    {
        Movement();
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        GetHealth();
    }

    void Movement()
    {
        // if there is else if the player will be able to move only in 1 direction
        // if only if's - in multiple directions
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            animator.SetInteger("Direction", 0);
            animator.speed = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            animator.SetInteger("Direction", 1);
            animator.speed = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            animator.SetInteger("Direction", 2);
            animator.speed = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            animator.SetInteger("Direction", 3);
            animator.speed = 1f;
        }
        else
        {
            animator.speed = 0f;
        }
    }

    void GetHealth()
    {
        for (int i = 0; i <= hearts.Length - 1; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i <= currentHealth - 1; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
}

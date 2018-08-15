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
    public int currentHealth;
    public GameObject sword;
    public float thrustPower;
    public bool canMove;
    public bool canAttack;
    public bool iniFrames; // Invincibility - мигание при получении урона
    SpriteRenderer spriteRenderer;
    float iniTimer = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        GetHealth();
        canMove = true;
        canAttack = true;
        iniFrames = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (iniFrames)
        {
            iniTimer -= Time.deltaTime;
            int rn = Random.Range(0, 100);
            if (rn < 50)
            {
                spriteRenderer.enabled = false;
            }
            else if (rn > 50)
            {
                spriteRenderer.enabled = true;
            }
            if (iniTimer <= 0)
            {
                iniTimer = 1f;
                iniFrames = false;
                spriteRenderer.enabled = true;
            }
        }
        GetHealth();
    }

    void Movement()
    {
        if (!canMove)
        {
            return;
        }

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

    //TODO: Add enum for directions

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        canMove = false;
        canAttack = false;
        GameObject newSword = Instantiate(sword, transform.position, transform.rotation);
        if (currentHealth == maxHealth)
        {
            newSword.GetComponent<Sword>().special = true;
            canMove = true;
            thrustPower = 500;
        }
        int swordDirection = animator.GetInteger("Direction");
        animator.SetInteger("AttackingDirection", swordDirection);
        #region //SwordRotation

        if (swordDirection == 0)
        {
            newSword.transform.Rotate(0, 0, 0);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        if (swordDirection == 1)
        {
            newSword.transform.Rotate(0, 0, 180);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
        }
        if (swordDirection == 2)
        {
            newSword.transform.Rotate(0, 0, 90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        }
        if (swordDirection == 3)
        {
            newSword.transform.Rotate(0, 0, -90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
        #endregion


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            if (!iniFrames)
            {
                iniFrames = true;
                currentHealth--;
            }
            other.gameObject.GetComponent<Bullet>().CreateParticle();
            Destroy(other.gameObject);
        }
    }
}

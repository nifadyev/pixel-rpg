using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    Animator animator;
    public float speed;
    int direction;
    float directionTimer = 0.7f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = 0.2f;
    bool shouldChange;

    void Start()
    {
        animator = GetComponent<Animator>();
        direction = Random.Range(0, 3);
        canAttack = false;
        shouldChange = false;
    }


    void Update()
    {
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0)
        {
            directionTimer = 0.7f;
            direction = Random.Range(0, 3);
        }
        Movement();

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = 0.2f;
            }
        }
    }

    void Movement()
    {
        if (direction == 0)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            animator.SetInteger("Direction", direction);
        }
        else if (direction == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            animator.SetInteger("Direction", direction);
        }
        else if (direction == 2)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            animator.SetInteger("Direction", direction);
        }
        else if (direction == 3)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            animator.SetInteger("Direction", direction);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sword")
        {
            health--;
            other.gameObject.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(other.gameObject);
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            health--;
            if (!other.gameObject.GetComponent<Player>().iniFrames)
            {
                other.gameObject.GetComponent<Player>().currentHealth--;
                other.gameObject.GetComponent<Player>().iniFrames = true;
            }
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.tag == "Wall")
        {
            if (shouldChange)
            {
                return;
            }

            if (direction == 0)
            {
                direction = 2;
            }
            else if (direction == 1)
            {
                direction = 3;
            }
            else if (direction == 2)
            {
                direction = 0;
            }
            else if (direction == 3)
            {
                direction = 1;
            }
            shouldChange = true;
        }
    }

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        canAttack = false;

        if (direction == 0)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (direction == 1)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);
        }
        else if (direction == 2)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);
        }
        else if (direction == 3)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
    }
}

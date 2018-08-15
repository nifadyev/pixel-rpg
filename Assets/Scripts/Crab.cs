using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    int direction;
    float timer = 1.5f;
    public int health;
    public float speed;
    public GameObject particleEffect;
    SpriteRenderer spriteRenderer;
    public Sprite facingUp;
    public Sprite facingDown;
    public Sprite facingRight;
    public Sprite facingLeft;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1.5f;
            direction = Random.Range(0, 3);
        }
        Movement();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            other.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            Destroy(other.gameObject);
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
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.tag == "Wall")
        {
            direction = Random.Range(0, 3);
        }
    }

    void Movement()
    {
        if (direction == 0)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingDown;
        }
        else if (direction == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingLeft;
        }
        else if (direction == 2)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingRight;
        }
        else if (direction == 3)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingUp;
        }
    }
}

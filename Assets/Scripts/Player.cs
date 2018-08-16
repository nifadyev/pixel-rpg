using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public float thrustPower;
    public int maxHealth;
    public int currentHealth;
    public Image[] hearts;
    public GameObject sword;
    public bool canMove;
    public bool canAttack;
    public bool iniFrames; // Invincibility - мигание при получении урона

    Animator animator;
    SpriteRenderer spriteRenderer;
    float iniTimer = 1f;

    void Start()
    {
        if (PlayerPrefs.HasKey("maxHealth"))
        {
            LoadGame();
        }
        else
        {
            currentHealth = maxHealth;
        }

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetHealth();
        canMove = true;
        canAttack = true;
        iniFrames = false;
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
            int rn = Random.Range(0, 100);
            spriteRenderer.enabled = (rn < 50) ? false : true;

            iniTimer -= Time.deltaTime;
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
        // Reset hearts before activating
        for (int i = 0; i <= hearts.Length - 1; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i <= currentHealth - 1; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }

        canMove = false;
        canAttack = false;
        thrustPower = 250;

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
        // Restart the game
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }

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
        if (other.gameObject.tag == "Potion")
        {
            currentHealth = maxHealth;
            Destroy(other.gameObject);
            // Restrict to increase health more than 5
            if (maxHealth >= 5)
            {
                return;
            }
            maxHealth++;
            currentHealth = maxHealth;
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("maxHealth", currentHealth);
        PlayerPrefs.SetInt("currentHealth", currentHealth);
    }

    void LoadGame()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        currentHealth = PlayerPrefs.GetInt("currentHealth");
    }
}

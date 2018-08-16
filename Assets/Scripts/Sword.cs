using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool special;
    public GameObject swordParticle;

    float timer = 0.15f;
    float specialTimer = 1f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetInteger("AttackingDirection", 5);
        }

        if ((!special) && (timer <= 0))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            Destroy(gameObject);
        }

        specialTimer -= Time.deltaTime;
        if (specialTimer <= 0)
        {
            Instantiate(swordParticle, transform.position, transform.rotation);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            Destroy(gameObject);
        }
    }

    public void CreateParticle()
    {
        Instantiate(swordParticle, transform.position, transform.rotation);
    }
}

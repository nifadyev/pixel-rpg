using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject particleEffect;

    float timer = 2f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            CreateParticle();
            Destroy(gameObject);
        }
    }

    public void CreateParticle()
    {
        Instantiate(particleEffect, transform.position, transform.rotation);
    }
}

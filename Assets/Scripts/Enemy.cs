﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    #region movement_variable
    public float movespeed;
    #endregion

    #region physics_component
    Rigidbody2D enemyRB;
    #endregion

    #region targeting_variables
    public Transform player;
    #endregion

    #region attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region helth_valuables
    public float maxHealth;
    float currentHealth;
    #endregion

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;



    }
    private void Update()
    {
        if (player == null)
        {
            return;
        }
        Move();

    }

    #region movement_functions
    private void Move()
    {
        Vector2 direction = player.position - transform.position;
        enemyRB.velocity = direction.normalized * movespeed;


    }
    #endregion

    #region attack_functions
    private void Explode()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
     
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
                Instantiate(explosionObj, transform.position, transform.rotation);
               

            }
           


        }

        Destroy(this.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Explode();
        }
    }
    #endregion

    #region health_function
    public void TakeDamage(float value)
    {
        FindObjectOfType<AudioManager>().Play("BatHurt");
        print("huuurt");
        currentHealth = currentHealth - value;
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : SlimeCreature
{
    public LivesBar livesBar;
    public float attackDistance;
    RayWeapon rayWeapon;
   // public bool isAttacking;

    public new void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        RefreshLivesBar();
    }
    protected void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        health = 5;
        speed = 10;
        attack = 1;
        attackDistance = 10;
        //isAttacking = false;
        rayWeapon = GetComponentInChildren<RayWeapon>();
    }

    public void RefreshLivesBar()
    {
        if (livesBar != null)
        {
            livesBar.Refresh();
        }
    }

    void Move()
    {
        Vector3 vectorH = Vector3.right * Input.GetAxis("Horizontal");
        Vector3 vectorV = Vector3.up * Input.GetAxis("Vertical");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + vectorH, speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + vectorV, speed * Time.deltaTime);
        State = AnimationState.Move;
    }

    void Attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistanse = attackDistance + 1;
        int id = 0;
        for(int i = 0; i < enemies.Length; i++)
        {
            if ((enemies[i].transform.position - transform.position).magnitude < minDistanse) {
                minDistanse = (enemies[i].transform.position - transform.position).magnitude;
                id = i;
            }
        }
        if (minDistanse <= attackDistance) {
            StartCoroutine(rayWeapon.Shoot(enemies[id].transform.position));
        }
        
    }

    void Update()
    {
        State = AnimationState.Idle;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Move();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
}

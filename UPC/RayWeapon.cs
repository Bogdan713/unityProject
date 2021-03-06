﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayWeapon : MonoBehaviour
{
    public Transform player;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public LineRenderer sublineRenderer;

    public IEnumerator Shoot(Vector2 targetPosition)
    {

            //if (Input.GetButtonDown("Fire1"))

            //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        Vector2 playerPosition = player.position;
        float playerRadius = player.GetComponent<CircleCollider2D>().radius*20;// real player Radius with scale

        float dx = targetPosition.x - playerPosition.x;
        float dy = targetPosition.y - playerPosition.y;
        float xAdd = dx * playerRadius / (Mathf.Abs(dy) + Mathf.Abs(dx));
        float yAdd = dy * playerRadius / (Mathf.Abs(dy) + Mathf.Abs(dx));

        Vector2 origin = new Vector3(playerPosition.x + xAdd, playerPosition.y + yAdd);

        RaycastHit2D hitInfo = Physics2D.Raycast(origin, new Vector2(dx, dy));
        if (hitInfo)
        {
            lineRenderer.SetPosition(0, new Vector3(origin.x, origin.y, -2.1f));
            lineRenderer.SetPosition(1, new Vector3(hitInfo.point.x, hitInfo.point.y, -2.1f));

            sublineRenderer.SetPosition(0, new Vector3(origin.x, origin.y, -2f));
            sublineRenderer.SetPosition(1, new Vector3(hitInfo.point.x, hitInfo.point.y, -2f));

            if (hitInfo.transform.tag.Equals("Enemy")) {
                hitInfo.transform.gameObject.GetComponent<Enemy>().TakeDamage(player.GetComponent<Character>().attack);
            }
        }
        lineRenderer.enabled = true;
        sublineRenderer.enabled = true;
        yield return new WaitForSeconds(0.04f);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(0.03f);
        
        sublineRenderer.enabled = false;
        Instantiate(impactEffect, new Vector3(hitInfo.point.x, hitInfo.point.y, -3f), Quaternion.identity);
    }
}

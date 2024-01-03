using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class memberManager : MonoBehaviour
{
    public Animator character_animator;
    public GameObject Particle_Death;
    private Transform Boss;
    public int Health;
    public float MinDistanceOfEnemy, MaxDistanceOfEnemy, moveSpeed;
    public bool fight, member;
    private Rigidbody rb;
    private CapsuleCollider _capsuleCollider;

    private void Start()
    {
        character_animator = GetComponent<Animator>();
        Boss = GameObject.FindWithTag("boss").transform;
        Health = 5;
        rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

    }

    private void Update()
    {
        var bossDistance = Boss.position - transform.position;

        if (!fight)
        {
            if (bossDistance.sqrMagnitude <= MaxDistanceOfEnemy * MaxDistanceOfEnemy)
            {
                PlayerManager.PlayerManagerCls.attackToTheBoss = true;
                PlayerManager.PlayerManagerCls.gameState = false;
            }

            if (PlayerManager.PlayerManagerCls.attackToTheBoss && member)
            {
                transform.position = Vector3.MoveTowards(transform.position, Boss.position, moveSpeed * Time.deltaTime);

                var stickManRotation = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) - transform.position;

                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion.LookRotation(stickManRotation, Vector3.up), 10f * Time.deltaTime);

                character_animator.SetFloat("run", 1f);

                rb.velocity = Vector3.zero;
            }
        }

        if (bossDistance.sqrMagnitude <= MinDistanceOfEnemy * MinDistanceOfEnemy)
        {

            fight = true;

            var stickManRotation = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion.LookRotation(stickManRotation, Vector3.up), 10f * Time.deltaTime);

            character_animator.SetBool("fight", true);

            MinDistanceOfEnemy = MaxDistanceOfEnemy;

            rb.velocity = Vector3.zero;
        }

        else
        {

            fight = false;
        }
    }
    public void ChangeTheAttackMode()
    {
        character_animator.SetFloat("attackmode", Random.Range(0, 2));

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("damage"))
        {
            Health--;

            if (Health <= 0)
            {
                Instantiate(Particle_Death, transform.position, Quaternion.identity);

                gameObject.SetActive(false);
               

                for(int i = 0; i <bossManager.BossManagerCls.Enemies.Count; i++)
                {
                    if(bossManager.BossManagerCls.Enemies.ElementAt(i).name == gameObject.name)
                    {
                        bossManager.BossManagerCls.Enemies.RemoveAt(i);
                        break;

                    }
                }
                bossManager.BossManagerCls.LockOnTarget = false;
            }

        }
    }


}

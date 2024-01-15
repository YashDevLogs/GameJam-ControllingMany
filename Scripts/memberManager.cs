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

    [SerializeField] private Transform Boss;
    public int Health;
    public float MinDistanceOfEnemy, MaxDistanceOfEnemy, moveSpeed;
    public bool IsFighiting, Ismember;
    [SerializeField]private Rigidbody rb;
    [SerializeField]private CapsuleCollider _capsuleCollider;

    private void Start()
    {
        character_animator = GetComponent<Animator>();
        Health = 5;

    }

    private void Update()
    {
        var bossDistance = Boss.position - transform.position;

        if (!IsFighiting)
        {
            if (bossDistance.sqrMagnitude <= MaxDistanceOfEnemy * MaxDistanceOfEnemy)
            {
                PlayerManager.PlayerManagerCls.attackToTheBoss = true;
                PlayerManager.PlayerManagerCls.gameState = false;
            }

            if (PlayerManager.PlayerManagerCls.attackToTheBoss && Ismember)
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

            IsFighiting = true;

            var stickManRotation = new Vector3(Boss.position.x, transform.position.y, Boss.position.z) - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion.LookRotation(stickManRotation, Vector3.up), 10f * Time.deltaTime);

            character_animator.SetBool("fight", true);

            MinDistanceOfEnemy = MaxDistanceOfEnemy;

            rb.velocity = Vector3.zero;
        }

        else
        {

            IsFighiting = false;
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

                if (gameObject.name != PlayerManager.PlayerManagerCls.Rblst.ElementAt(0).name)
                {
                    gameObject.SetActive(false);
                    transform.parent = null;
                }
                else
                {
                    _capsuleCollider.enabled = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                }


                bossManager.BossManagerCls.RemoveEnemiesFromList();
                bossManager.BossManagerCls.LockOnTarget = false;
            }

        }
        if (other.collider.CompareTag("obstacle"))
        {
            Instantiate(Particle_Death, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            transform.parent = null;
            bossManager.BossManagerCls.RemoveEnemiesFromList();
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletpos;
    public float timerInterval;
    private float timer;
    public bool canAttack;

    private Transform player; // 플레이어의 Transform

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 태그를 통해 찾기
        canAttack = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > timerInterval && canAttack == true)
        {
            timer = 0;
            Shoot();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            canAttack = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canAttack = false;
        }
    }
    private void Shoot()
    {
        GameObject bulletInstance = Instantiate(bullet, bulletpos.position, Quaternion.identity);
        EnemyBullet enemyBullet = bulletInstance.GetComponent<EnemyBullet>();
        enemyBullet.speed = 4;
        animator.SetTrigger("Attack");
        if (enemyBullet != null)
        {
            enemyBullet.target = player; // 플레이어의 Transform을 설정
        }
    }
}

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

    private Transform player; // �÷��̾��� Transform

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // �÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� �±׸� ���� ã��
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
            enemyBullet.target = player; // �÷��̾��� Transform�� ����
        }
    }
}

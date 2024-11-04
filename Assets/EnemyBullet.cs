using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rigid;
    public float speed; // 총알 속도
    public Transform target; // 플레이어의 Transform

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        target = GameManager.instance.Player.transform;
    }

    private void Start()
    {
        // 초기 방향 설정
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            rigid.velocity = direction.normalized * speed; // 초기 속도 설정

            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 회전 각도 계산
            transform.rotation = Quaternion.Euler(0, 0, rot); // 방향으로 회전
        }
    }

    private void Update()
    {
        // 플레이어를 추적
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            rigid.velocity = direction.normalized * speed; // 지속적으로 속도 설정
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 피해를 입히는 로직
            other.SendMessage("TakeDamage", 1f); // 예시로 1의 피해를 입힘
            Destroy(gameObject); // 총알 삭제
        }
        else if (other.CompareTag("Platform"))
        {
            Destroy(gameObject); // 플랫폼과 충돌 시 총알 삭제
        }
    }
}

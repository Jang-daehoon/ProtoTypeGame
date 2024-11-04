using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rigid;
    public float speed; // �Ѿ� �ӵ�
    public Transform target; // �÷��̾��� Transform

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
        // �ʱ� ���� ����
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            rigid.velocity = direction.normalized * speed; // �ʱ� �ӵ� ����

            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ȸ�� ���� ���
            transform.rotation = Quaternion.Euler(0, 0, rot); // �������� ȸ��
        }
    }

    private void Update()
    {
        // �÷��̾ ����
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            rigid.velocity = direction.normalized * speed; // ���������� �ӵ� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� ���ظ� ������ ����
            other.SendMessage("TakeDamage", 1f); // ���÷� 1�� ���ظ� ����
            Destroy(gameObject); // �Ѿ� ����
        }
        else if (other.CompareTag("Platform"))
        {
            Destroy(gameObject); // �÷����� �浹 �� �Ѿ� ����
        }
    }
}

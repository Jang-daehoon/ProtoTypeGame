using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLockMechanism : MonoBehaviour
{
    public GameObject needKeyObj;
    public bool isKeyGet;
    public bool isPotalReady;

    private void Awake()
    {
        isKeyGet = false;
        isPotalReady = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isPotalReady == false && isKeyGet == false)
        {
            Debug.Log("���� ���踦 ã�� ���� ��Ż�� ����ֽ��ϴ�.");
        }
        else if(collision.CompareTag("Player") && isPotalReady == true && isKeyGet == true)
        {
            Debug.Log("���� é�ͷ� �̵��մϴ�.");
        }
    }
}

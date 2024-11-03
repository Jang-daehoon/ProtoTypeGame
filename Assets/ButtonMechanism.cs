using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMechanism : MonoBehaviour
{
    public GameObject NeedObject;    // ��ư�� ������ ������Ʈ
    public GameObject DeleteObject;  // ��ư�� ������ �ı��� ������Ʈ
    public bool isBtnPush;           // ��ư�� ���� (���� ����)

    private void Awake()
    {
        isBtnPush = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == NeedObject)
        {
            isBtnPush = true;

            // DeleteObject�� ������ ���� �ı�
            if (DeleteObject != null)
            {
                Destroy(DeleteObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == NeedObject)
        {
            isBtnPush = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == NeedObject)
        {
            isBtnPush = false;
        }
    }
}

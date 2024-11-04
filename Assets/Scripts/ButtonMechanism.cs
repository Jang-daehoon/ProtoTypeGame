using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMechanism : MonoBehaviour
{
    public GameObject NeedObject;    // 버튼을 눌러줄 오브젝트
    public GameObject DeleteObject;  // 버튼이 눌리면 파괴할 오브젝트
    public bool isBtnPush;           // 버튼의 상태 (눌림 여부)

    private void Awake()
    {
        isBtnPush = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == NeedObject)
        {
            isBtnPush = true;

            // DeleteObject가 존재할 때만 파괴
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

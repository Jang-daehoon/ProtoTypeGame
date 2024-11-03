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
            Debug.Log("아직 열쇠를 찾지 못해 포탈이 잠겨있습니다.");
        }
        else if(collision.CompareTag("Player") && isPotalReady == true && isKeyGet == true)
        {
            Debug.Log("다음 챕터로 이동합니다.");
        }
    }
}

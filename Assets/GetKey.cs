using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKey : MonoBehaviour
{
    public PadLock_Trigger ConnectPadLockObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetKeyItem();
        }
    }
    private void GetKeyItem()
    {
        ConnectPadLockObj.isKeyGet = true;
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLockKeyScripts : MonoBehaviour
{
    public PadLockMechanism PadLock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PadLock.isKeyGet = true;
            PadLock.isPotalReady = true;
            PadLock.PadLockImage.GetComponent<Animator>().SetBool("isOpenPotal", true);
            Destroy(gameObject);
        }
    }
}

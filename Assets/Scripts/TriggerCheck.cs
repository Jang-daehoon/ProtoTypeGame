using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    public bool isFirstTriggerDone;
    private void Awake()
    {
        isFirstTriggerDone = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isFirstTriggerDone = true;
        }
        
    }
}

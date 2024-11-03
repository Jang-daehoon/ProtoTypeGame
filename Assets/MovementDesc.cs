using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDesc : MonoBehaviour
{
    public GameObject DescObj;

    private void Start()
    {
        DescObj.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            DescObj.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DescObj.SetActive(false);
        }
    }
}

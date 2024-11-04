using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.2f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private GameObject FallObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        FallObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        FallObject.GetComponent<Rigidbody2D>().gravityScale = 10;
        Destroy(FallObject, destroyDelay);
    }
}

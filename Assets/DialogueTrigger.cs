using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool isStroyDone;
    public GameObject DoTutorial;

    private void Awake()
    {
        isStroyDone = false;
        DoTutorial.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isStroyDone == false)
        {
            isStroyDone = true;
            DoTutorial.SetActive(true);
        }
    }
}

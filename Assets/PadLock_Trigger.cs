using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PadLock_Trigger : MonoBehaviour
{
    [Header("PadTrigger에 상호작용 되는 오브젝트")]
    [SerializeField] private GameObject padLockTriggerObj;
    [SerializeField] private GameObject KeyObj;
    [SerializeField] private Tilemap tileMapObj;
    [SerializeField] private bool isPadLockOpen;
    public bool isKeyGet;

    private void Start()
    {
        isKeyGet = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(padLockTriggerObj.GetComponent<TriggerCheck>().isFirstTriggerDone == true)
        {
            TutorialManager.Instance.SecondTutorial.SetActive(true);
            KeyObj.SetActive(true);
        }
        else if(padLockTriggerObj.GetComponent<TriggerCheck>().isFirstTriggerDone == true && isKeyGet == true)
        {
            isPadLockOpen = true;
        }

        if (isPadLockOpen == true)
        {
            StartCoroutine(FadeOutTilemap());
        }
    }
    private IEnumerator FadeOutTilemap()
    {
        SpriteRenderer spriteRenderer = tileMapObj.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;

        for (float t = 0; t < 3; t += Time.deltaTime)
        {
            float normalizedTime = t / 3; 
            color.a = Mathf.Lerp(1, 0, normalizedTime); 
            spriteRenderer.color = color;
            yield return null; 
        }

        color.a = 0; 
        spriteRenderer.color = color;
    }
}


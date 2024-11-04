using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class PadLockMechanism : MonoBehaviour
{
    public GameObject needKeyObj;
    public GameObject PadLockImage;
    public GameObject potalOpenParticle;

    public GameObject Forth_Tutorial;

    public bool isKeyGet;
    public bool isPotalReady;

    public Transform moveToNextPos;
    public Collider2D nextConfiner;

    [Header("����")]
    public GameObject DescObj;
    [SerializeField] TextMeshProUGUI descText;

    private void Awake()
    {
        isKeyGet = false;
        isPotalReady = false;
    }
    private void Update()
    {
        if (isKeyGet == true)
        {
            potalOpenParticle.SetActive(true);
        }
        else
            potalOpenParticle.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isPotalReady == false && isKeyGet == false)
        {
            Debug.Log("���� ���踦 ã�� ���� ��Ż�� ����ֽ��ϴ�.");

            descText.text = "���� ���踦 ã�� ���� ��Ż�� ����ֽ��ϴ�.";
            DescObj.SetActive(true);

        }
        else if(collision.CompareTag("Player") && isPotalReady == true && isKeyGet == true)
        {
            StartCoroutine(MoveToNextChapter());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isPotalReady == false && isKeyGet == false)
        {
            DescObj.SetActive(false);
        }
    }
    private IEnumerator MoveToNextChapter()
    {
        Debug.Log("���� é�ͷ� �̵��մϴ�.");
        Forth_Tutorial.SetActive(true);

        yield return new WaitForSeconds(3f);
        GameManager.instance.Player.transform.position = moveToNextPos.position;
        GameManager.instance.Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameManager.instance.Player.GetComponent<Animator>().SetFloat("Speed", 0);
        GameManager.instance.vCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = nextConfiner;
        nextConfiner.gameObject.SetActive(true);
    }
}

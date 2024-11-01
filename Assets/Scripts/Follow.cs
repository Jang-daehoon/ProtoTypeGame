using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    public float heightOffset = 50f; // ���� �������� ������ �� �ִ� ����

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(GameManager.instance.Player.transform.position);
        screenPosition.y += heightOffset; // Y ��ǥ�� ������ �߰�
        rect.position = screenPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

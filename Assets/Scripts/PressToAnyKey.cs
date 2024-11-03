using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressToAnyKey : MonoBehaviour
{
    void Update()
    {
        // PC�� ����� �Է��� ��� ����
        if (IsAnyInputDetected())
        {
            StartGame();
        }
    }

    // �Է� ���� �Լ�: PC�� ����� ȯ���� ��� üũ
    bool IsAnyInputDetected()
    {
        // 1. Ű���� �Է� ���� (PC)
        if (Input.anyKeyDown)
        {
            return true;
        }

        // 2. ���콺 Ŭ�� ���� (PC)
        if (Input.GetMouseButtonDown(0)) // ���� Ŭ��
        {
            return true;
        }

        // 3. ��ġ �Է� ���� (�����)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }

        // �Է��� ������ false ��ȯ
        return false;
    }

    // ���� ���� �Լ� (���ϴ� ���� ����)
    void StartGame()
    {
        Debug.Log("���� ����!");
        UIManager.Instance.StartSceneUI.SetActive(false);
        UIManager.Instance.TitleSceneUI.SetActive(true);
    }
}

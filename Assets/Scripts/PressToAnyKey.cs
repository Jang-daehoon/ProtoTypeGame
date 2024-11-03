using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressToAnyKey : MonoBehaviour
{
    void Update()
    {
        // PC와 모바일 입력을 모두 감지
        if (IsAnyInputDetected())
        {
            StartGame();
        }
    }

    // 입력 감지 함수: PC와 모바일 환경을 모두 체크
    bool IsAnyInputDetected()
    {
        // 1. 키보드 입력 감지 (PC)
        if (Input.anyKeyDown)
        {
            return true;
        }

        // 2. 마우스 클릭 감지 (PC)
        if (Input.GetMouseButtonDown(0)) // 왼쪽 클릭
        {
            return true;
        }

        // 3. 터치 입력 감지 (모바일)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }

        // 입력이 없으면 false 반환
        return false;
    }

    // 게임 시작 함수 (원하는 동작 수행)
    void StartGame()
    {
        Debug.Log("게임 시작!");
        UIManager.Instance.StartSceneUI.SetActive(false);
        UIManager.Instance.TitleSceneUI.SetActive(true);
    }
}

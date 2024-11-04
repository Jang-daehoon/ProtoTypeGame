using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutScene : MonoBehaviour
{
    public static bool isCutSceneOn;    //CutScene 활성화시 움직임 불가
    private bool isCutSceneDone;        //CutScene 종료되었는지 확인

    public Animator camAnim;
    private float originalMoveSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isCutSceneDone == false)
        {
            isCutSceneOn = true;
            isCutSceneDone = true;
            //활성화 시 플레이어의 rb의 이동속드를 0으로 만들어 멈추도록 작성
            originalMoveSpeed = collision.GetComponent<CharacterMovement>().moveSpeed;
            collision.GetComponent<CharacterMovement>().moveSpeed = 0;
            Debug.Log("StartCutScene");
            camAnim.SetBool("CutScene1", true);
            Invoke(nameof(StopCutScene), 3f);   //3초 후 원래 카메라로 복귀
        }
    }
    private void StopCutScene()
    {
        isCutSceneOn = false;
        GameManager.instance.Player.moveSpeed = originalMoveSpeed;
        camAnim.SetBool("CutScene1", false);
    }
}

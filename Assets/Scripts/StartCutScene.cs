using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutScene : MonoBehaviour
{
    public static bool isCutSceneOn;    //CutScene Ȱ��ȭ�� ������ �Ұ�
    private bool isCutSceneDone;        //CutScene ����Ǿ����� Ȯ��

    public Animator camAnim;
    private float originalMoveSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isCutSceneDone == false)
        {
            isCutSceneOn = true;
            isCutSceneDone = true;
            //Ȱ��ȭ �� �÷��̾��� rb�� �̵��ӵ带 0���� ����� ���ߵ��� �ۼ�
            originalMoveSpeed = collision.GetComponent<CharacterMovement>().moveSpeed;
            collision.GetComponent<CharacterMovement>().moveSpeed = 0;
            Debug.Log("StartCutScene");
            camAnim.SetBool("CutScene1", true);
            Invoke(nameof(StopCutScene), 3f);   //3�� �� ���� ī�޶�� ����
        }
    }
    private void StopCutScene()
    {
        isCutSceneOn = false;
        GameManager.instance.Player.moveSpeed = originalMoveSpeed;
        camAnim.SetBool("CutScene1", false);
    }
}

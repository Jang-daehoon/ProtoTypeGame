using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //Array�� ����
    public GameObject[] Stage;
    public int curStage;

    //�������� ����
    public void StageSelect(int curStage)
    {
        Stage[curStage].SetActive(true);
        StageInit(curStage);

    }
    //���õ� �������� �ʱ�ȭ
    public void StageInit(int curStage)
    {
        Stage[curStage].transform.position = new Vector3(0, 0, 0);
        //��Ȱ��ȭ �Ǿ��� ��� �ڽĿ�����Ʈ Ȱ��ȭ
        foreach(Transform child in Stage[curStage].transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}

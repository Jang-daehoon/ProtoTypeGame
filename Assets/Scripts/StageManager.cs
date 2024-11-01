using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //Array로 관리
    public GameObject[] Stage;
    public int curStage;

    //스테이지 선택
    public void StageSelect(int curStage)
    {
        Stage[curStage].SetActive(true);
        StageInit(curStage);

    }
    //선택된 스테이지 초기화
    public void StageInit(int curStage)
    {
        Stage[curStage].transform.position = new Vector3(0, 0, 0);
        //비활성화 되었던 모든 자식오브젝트 활성화
        foreach(Transform child in Stage[curStage].transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}

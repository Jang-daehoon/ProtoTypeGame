using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//튜토리얼 전체를 관리하는 스크립트
public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;   //현재 튜토리얼에서 순차적으로 진행할 튜토리얼 행동을 저장
    [SerializeField]
    private string nextSceneName = "";

    private TutorialBase currentTutorial = null;
    [SerializeField]
    private int currentIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        //시작 시 튜토리얼 행동을 설정하고 플레이하는 메서드 호출
        SetNextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        //현재 튜토리얼의 Exit()메서드 호출
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }
        //마지막 튜토리얼을 진행했다면 CompletedAllTutorials() 메서드 호출
        if (currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }
        //다음 튜토리얼 과정을 currentTutorial로 등록
        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        //새로바뀐 튜토리얼의 Enter()메서드 호출
        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;
        //행동양식이 여러 종류가 되었을 때 코드 추가 작성
        //현재는 씬 전환

        Debug.Log("Complete All");
        /*
                if (!nextSceneName.Equals(""))
                {
                    SceneManager.LoadScene(nextSceneName);
                }
        */
    }
}

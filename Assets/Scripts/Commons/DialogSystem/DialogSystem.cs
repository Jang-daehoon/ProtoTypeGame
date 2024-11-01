using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Speaker { Player, Friends }

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Dialog[] dialogs;                       // 현재 분기의 대사 목록
    [SerializeField]
    private Image[] CharacterImage;
    [SerializeField]
    private Image[] imageDialogs;                   // 대화창 Image UI
    [SerializeField]
    private TextMeshProUGUI[] textNames;            // 현재 대사중인 캐릭터 이름 출력 Text UI
    [SerializeField]
    private TextMeshProUGUI[] textDialogues;        // 현재 대사 출력 Text UI
    [SerializeField]
    private GameObject[] objectArrows;              // 대사가 완료되었을 때 출력되는 커서 오브젝트
    [SerializeField]
    private float typingSpeed;                       // 텍스트 타이핑 효과의 재생 속도
    [SerializeField]
    private KeyCode keyCodeSkip = KeyCode.Space;    // 타이핑 효과를 스킵하는 키
    
    private float originalMoveSpeed;

    private int currentIndex = -1;
    [SerializeField]private bool isTypingEffect = false;            // 텍스트 타이핑 효과를 재생중인지

    public void Setup()
    {
        GameManager.instance.isDialogStart = true;

        originalMoveSpeed = GameManager.instance.Player.moveSpeed;
        //활성화 시 플레이어의 rb의 이동속드를 0으로 만들어 멈추도록 작성
        GameManager.instance.Player.moveSpeed = 0;
        for (int i = 0; i < CharacterImage.Length; ++i)
        {
            InActiveObjects(i);
        }

        SetNextDialog();
    }

    public bool UpdateDialog()
    {
        if (Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0))
        {
            if (isTypingEffect == true)
            {
                StopCoroutine("TypingText");
                isTypingEffect = false;
                ShowFullDialogue();
                return false;
            }

            // 대사가 끝났을 때만 다음 대사로 넘어갑니다.
            if (dialogs.Length > currentIndex + 1)
            {
                SetNextDialog();
            }
            else
            {
                HideAllDialogObjects();
                GameManager.instance.isDialogStart = false;
                GameManager.instance.Player.moveSpeed = originalMoveSpeed;
                return true;
            }
        }

        return false;
    }


    private void SetNextDialog()
    {
        currentIndex++;

        Dialog currentDialog = dialogs[currentIndex];

        // 캐릭터 이미지에 스프라이트 설정
        CharacterImage[(int)currentDialog.speaker].sprite = currentDialog.CharacterSprite;

        // 대화 관련 오브젝트 활성화
        CharacterImage[(int)currentDialog.speaker].gameObject.SetActive(true);
        imageDialogs[(int)currentDialog.speaker].gameObject.SetActive(true);
        textNames[(int)currentDialog.speaker].gameObject.SetActive(true);
        textNames[(int)currentDialog.speaker].text = currentDialog.Name; // Dialog의 Name 사용
        textDialogues[(int)currentDialog.speaker].gameObject.SetActive(true);

        // 대사 텍스트를 타이핑 효과로 출력
        StartCoroutine(TypingText());
    }

    private void ShowFullDialogue()
    {
        if (currentIndex < dialogs.Length)
        {
            textDialogues[(int)dialogs[currentIndex].speaker].text = dialogs[currentIndex].dialogue;
            objectArrows[(int)dialogs[currentIndex].speaker].SetActive(true);
        }
    }

    private void HideAllDialogObjects()
    {
        for (int i = 0; i < CharacterImage.Length; ++i)
        {
            InActiveObjects(i);
        }
    }

    private void InActiveObjects(int index)
    {
        CharacterImage[index].gameObject.SetActive(false);
        imageDialogs[index].gameObject.SetActive(false);
        textNames[index].gameObject.SetActive(false);
        textDialogues[index].gameObject.SetActive(false);
        objectArrows[index].SetActive(false);
    }

    private IEnumerator TypingText()
    {
        int index = 0;
        isTypingEffect = true;

        //텍스트를 한글자씩 타이핑치듯 재생
        while (index < dialogs[currentIndex].dialogue.Length)
        {
            textDialogues[currentIndex].text = dialogs[currentIndex].dialogue.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTypingEffect = false;

        //대사가 완료되었을 때 출력되는 커서 활성화
        objectArrows[currentIndex].SetActive(true);
    }
}

[System.Serializable]
public struct Dialog
{
    public Speaker speaker; // 화자
    public Sprite CharacterSprite;   // 캐릭터 스프라이트
    public string Name; // 캐릭터 이름
    [TextArea(3, 5)]
    public string dialogue;  // 대사
}

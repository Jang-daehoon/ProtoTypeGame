using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharacterMovement Player;
    public bool isDialogStart;  //대화 진행중인지 확인하는 변수
    public GameObject StartControlDescObj;

    [Header("PlayerStatus")]
    public int hp;
    public int maxHp;
    public int mana;
    public int maxMana;
    public int Gold;
    public bool isDead;

    private void Awake()
    {
        instance = this;
    }
    public void GameStart()
    {
        hp = maxHp;
        mana = maxMana;
        isDead = false;
        TutorialManager.Instance.Tutorials[0].SetActive(true);
        UIManager.Instance.TitleSceneUI.SetActive(false);
        StartControlDescObj.SetActive(true);
    }
}

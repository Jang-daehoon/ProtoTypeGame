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
    public bool isDialogStart;  //��ȭ ���������� Ȯ���ϴ� ����

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
    private void Start()
    {
        hp = maxHp;
        mana = maxMana;
        isDead = false;
        TutorialManager.Instance.firstTutorial.SetActive(true);
    }
}

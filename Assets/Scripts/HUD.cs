using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum UiType { Hp, Mana, Time, Gold, DodgeGauge, GetGold}
    public UiType uiType;
    public float playTime = 0f; // 플레이 타임을 기록할 변수
    public int getGold = 0;
    public float curDodgeGauge { get; set; }
    public float maxDodgeGauge = 100;
    public float increDodgeGauge = 0;
    public bool canDodge;

    TextMeshProUGUI myText;
    Slider mySlider;
     
    private void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        mySlider = GetComponent<Slider>();
    }
    private void Start()
    {
        curDodgeGauge = maxDodgeGauge;
        canDodge = true;
    }
    private void Update()
    {

        if (curDodgeGauge == maxDodgeGauge)
        {
            canDodge = true;
        }
        else
        {
            canDodge = false;
        }
    }

    private void LateUpdate()
    {
        switch(uiType)
        {
            case UiType.Hp:
                float curHp = GameManager.instance.hp;
                float maxHp = GameManager.instance.maxHp;
                mySlider.value = curHp / maxHp;
                break;
            case UiType.Mana:
                float curMana = GameManager.instance.mana;
                float maxMana = GameManager.instance.maxMana;
                mySlider.value = curMana / maxMana;
                break;
            case UiType.Time:
                int minutes = Mathf.FloorToInt(playTime / 60); // 분 계산
                int seconds = Mathf.FloorToInt(playTime % 60); // 초 계산

                myText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
                break;
            case UiType.Gold:
                int CurGold = GameManager.instance.Gold;
                myText.text = CurGold.ToString();
                break;
            case UiType.DodgeGauge:
                mySlider.value = curDodgeGauge / maxDodgeGauge;
                break;
            case UiType.GetGold:
                break;
        }
    }
    public void IncreDodgeGauge()
    {
        if (curDodgeGauge < maxDodgeGauge)
        {
            curDodgeGauge += increDodgeGauge;
        }
        else
            Debug.Log("현재 회피게이지가 최대치입니다.");
    }
}

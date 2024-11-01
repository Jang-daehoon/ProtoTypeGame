using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Bronze, Silver, Gold, Potion, ManaPotion}
    public ItemType itemType;
    //int AddGold;    //������ �� ��ũ���ͺ� ������Ʈ ���� �� ��� ����� �ּ�ó��

    public void GetItems()
    {
        //�ӽ÷� ȹ�� ��差 �ּ�ó��
        switch (itemType)
        {
            case ItemType.Bronze:
                Debug.Log("50 ��� ȹ��!!");

                break;
            case ItemType.Silver:
                Debug.Log("100 ��� ȹ��!!");

                break;
            case ItemType.Gold:
                Debug.Log("200 ��� ȹ��!!");

                break;
            case ItemType.Potion:
                Debug.Log("ü�� 1 ȸ��!");
                GameManager.instance.hp += 1;
                break;
            case ItemType.ManaPotion:
                Debug.Log("���� 5 ȸ��");
                GameManager.instance.mana += 5;
                break;
        }
    }
}

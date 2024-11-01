using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Bronze, Silver, Gold, Potion, ManaPotion}
    public ItemType itemType;
    //int AddGold;    //ÇÁ¸®Æé ¹× ½ºÅ©¸³ÅÍºí ¿ÀºêÁ§Æ® °ü¸® ½Ã »ç¿ë ÇöÀç´Â ÁÖ¼®Ã³¸®

    public void GetItems()
    {
        //ÀÓ½Ã·Î È¹µæ °ñµå·® ÁÖ¼®Ã³¸®
        switch (itemType)
        {
            case ItemType.Bronze:
                Debug.Log("50 °ñµå È¹µæ!!");

                break;
            case ItemType.Silver:
                Debug.Log("100 °ñµå È¹µæ!!");

                break;
            case ItemType.Gold:
                Debug.Log("200 °ñµå È¹µæ!!");

                break;
            case ItemType.Potion:
                Debug.Log("Ã¼·Â 1 È¸º¹!");
                GameManager.instance.hp += 1;
                break;
            case ItemType.ManaPotion:
                Debug.Log("¸¶³ª 5 È¸º¹");
                GameManager.instance.mana += 5;
                break;
        }
    }
}

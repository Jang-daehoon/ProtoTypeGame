using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueActive : MonoBehaviour
{
    public ButtonMechanism connBtn;
    public GameObject statueParticle;

    // Update is called once per frame
    void Update()
    {
        if(connBtn.isBtnPush == true)
        {
            statueParticle.SetActive(true);
        }
        else
            statueParticle.SetActive (false);   
    }
}

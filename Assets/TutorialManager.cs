using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public GameObject firstTutorial;
    public GameObject SecondTutorial;


    private void Awake()
    {
        Instance = this;
    }

}

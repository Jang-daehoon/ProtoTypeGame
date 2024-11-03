using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("UIObject")]
    public GameObject StartSceneUI;
    public GameObject TitleSceneUI;

    [Header("ButtonObject")]
    public Button gameStartBtn;
    public Button optionBtn;
    public Button exitBtn;
    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameStartBtn.onClick.AddListener(GameManager.instance.GameStart);
    }
}

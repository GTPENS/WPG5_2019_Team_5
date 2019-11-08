using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject playButtonObject;
    GameManager manager;
    
    void Start()
    {
        playButtonObject.GetComponent<Button>().onClick.AddListener(onClickPlay);
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }
    
    void onClickPlay()
    {
        manager.joinGame();
    }
}

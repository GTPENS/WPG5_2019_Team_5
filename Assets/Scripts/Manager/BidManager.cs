using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BidManager : MonoBehaviour
{
    [SerializeField] GameObject sliderObject;
    [SerializeField] GameObject bidText;
    [SerializeField] GameObject bidButtonObject;

    GameManager manager;
    Slider slider;
    int value;

    void Start()
    {
        bidButtonObject.GetComponent<Button>().onClick.AddListener(onClickBid);

        slider = sliderObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(onValueChanged);

        value = (int) ((slider.value / 100f) * manager.getPlayer().gold);
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    private void onValueChanged(float value)
    {
        int playerGold = manager.getPlayer().gold;
        this.value = (int) ((value / 100f) * playerGold);

        bidText.GetComponent<Text>().text = this.value.ToString();
    }

    void onClickBid()
    {
        manager.doBidding(this.value);
    }
}

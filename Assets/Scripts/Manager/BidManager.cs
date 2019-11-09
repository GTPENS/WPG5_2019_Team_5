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
    int timer;
    int value;

    void Start()
    {
        bidButtonObject.GetComponent<Button>().onClick.AddListener(onClickBid);

        slider = sliderObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(onValueChanged);

        value = (int) ((slider.value / 100f) * manager.getPlayer().gold);

        StartCoroutine(startTimer());
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setTimer(int timer)
    {
        this.timer = timer;
    }

    private void onValueChanged(float value)
    {
        int playerGold = manager.getPlayer().gold;
        this.value = (int) ((value / 100f) * playerGold);

        bidText.GetComponent<Text>().text = this.value.ToString();
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(timer);
        onClickBid();
    }

    void onClickBid()
    {
        manager.doBidding(this.value);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BidManager : MonoBehaviour
{
    [SerializeField] GameObject timerTextObject;
    [SerializeField] GameObject sliderObject;
    [SerializeField] GameObject bidText;
    [SerializeField] GameObject bidButtonObject;

    GameManager manager;
    Slider slider;
    Button button;
    Text timerText, bidButtonText;
    Coroutine coroutine;
    int timer, value;

    void Start()
    {
        timerText = timerTextObject.GetComponent<Text>();

        button = bidButtonObject.GetComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(onClickBid);

        bidButtonText = bidButtonObject.GetComponentInChildren<Text>();
        bidButtonText.text = "Bid";

        slider = sliderObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(onValueChanged);

        value = (int) ((slider.value / 100f) * manager.getPlayer().gold);

        timerText.text = timer.ToString();
        coroutine = StartCoroutine(startTimer());
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

        bidText.GetComponent<Text>().text = $"${this.value}";
    }

    IEnumerator startTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
            timerText.text = timer.ToString();

            if (timer <= 0)
            {
                onClickBid();
                break;
            }
        }
    }

    void onClickBid()
    {
        StopCoroutine(coroutine);

        manager.doBidding(this.value);
        bidButtonText.text = "Waiting other Player";
        button.GetComponent<Button>().interactable = false;
    }
}

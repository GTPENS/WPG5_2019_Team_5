using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BidManager : MonoBehaviour
{
    [SerializeField] GameObject sliderObject;
    [SerializeField] GameObject bidObject;

    Slider slider;
    Text bidText;

    void Start()
    {
        slider = sliderObject.GetComponent<Slider>();
        bidText = bidObject.GetComponent<Text>();

        slider.onValueChanged.AddListener(onValueChanged);
    }

    private void onValueChanged(float value)
    {
        Debug.Log($"value: {value}");
    }
}

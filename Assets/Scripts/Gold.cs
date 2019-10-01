using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    Text goldText;
    
    void Start()
    {
        goldText = GetComponent<Text>();

        updateValue();
    }

    void updateValue() {
        goldText.text = Config.getInstance.gold.ToString();
    }
}

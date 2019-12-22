using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogButton : MonoBehaviour
{
    public string type;
    public bool selected;

    DialogHandler dialogHandler;

    void Start()
    {
        dialogHandler = GetComponentInParent<DialogHandler>();

        GetComponent<Button>().onClick.AddListener(onClick);
    }

    void changeButton()
    {
        Color color;
        ColorUtility.TryParseHtmlString(selected ? "#FFFFFF" : "#4F4F4F", out color);

        GetComponent<Image>().color = color;
    }

    void onClick()
    {
        if (selected)
            selected = !dialogHandler.unSelectButton(this.type);
        else
            selected = dialogHandler.selectButton(this.type);

        changeButton();
    }

    public void resetButton()
    {
        selected = false;
        changeButton();
    }
}

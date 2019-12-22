using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] GameObject selectDialog;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject nextButton;

    List<string> activeButtons;

    void Start()
    {
        activeButtons = new List<string>();
        nextButton.GetComponent<Button>().onClick.AddListener(onNext);

        resetButton();
    }

    public void resetButton()
    {
        foreach (var item in buttons)
        {
            item.GetComponent<DialogButton>().resetButton();
        }
    }

    void onNext()
    {
        onFinish();
    }

    List<string> onFinish()
    {
        gameObject.SetActive(false);
        return activeButtons;
    }

    public bool selectButton(string type)
    {
        if (activeButtons.Count >= 2) return false;

        activeButtons.Add(type);
        return true;
    }

    public bool unSelectButton(string type)
    {
        if (activeButtons.Count <= 0) return false;

        activeButtons.Remove(type);
        return true;
    }
}

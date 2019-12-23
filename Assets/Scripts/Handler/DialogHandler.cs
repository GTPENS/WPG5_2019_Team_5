using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject nextButton;
    
    ActionManager actionManager;
    List<string> activeButtons;

    string spellName;

    void Start()
    {
        activeButtons = new List<string>();
        nextButton.GetComponent<Button>().onClick.AddListener(onFinish);

        resetButton();
    }

    public void setActionManager(ActionManager actionManager)
    {
        this.actionManager = actionManager;
    }

    public void setSpellName(string spellName)
    {
        this.spellName = spellName;
    }

    public void resetButton()
    {
        foreach (var item in buttons)
        {
            item.GetComponent<DialogButton>().resetButton();
        }
    }

    void onFinish()
    {
        if (activeButtons.Count < 2) return;

        actionManager.onSelected(this.spellName, activeButtons[0], activeButtons[1]);

        var parent = gameObject.transform.parent;
        parent.gameObject.SetActive(false);
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

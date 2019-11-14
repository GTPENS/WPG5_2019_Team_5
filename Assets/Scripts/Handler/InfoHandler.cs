using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject infoDialog;
    [SerializeField] GameObject turnTextObject;
    [SerializeField] GameObject goldTextObject;

    Text turnText, goldText;
    Player player;

    bool mouseDown;

    void Start()
    {
        turnText = turnTextObject.GetComponent<Text>();
        goldText = goldTextObject.GetComponent<Text>();

        updateText();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        infoDialog.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        infoDialog.SetActive(false);
    }

    public Player getPlayer()
    {
        return player;
    }

    public void setPlayer(Player player)
    {
        this.player = player;

        if (turnText != null)
            updateText();
    }

    void updateText()
    {
        turnText.text = $"Turn {player.turn}";
        goldText.text = $"Rp. {player.gold}";
    }
}

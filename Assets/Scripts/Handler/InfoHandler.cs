using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject indicator;
    [SerializeField] GameObject infoDialog;
    [SerializeField] GameObject turnTextObject;
    [SerializeField] GameObject goldTextObject;

    Text turnText, goldText;
    Player player;

    bool mouseDown, ready;
    int turn;

    void Start()
    {
        turnText = turnTextObject.GetComponent<Text>();
        goldText = goldTextObject.GetComponent<Text>();

        updateInfo();
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
    }

    public void setTurn(int turn)
    {
        this.turn = turn;

        if (turnText != null)
            updateInfo();
    }

    public void setReady(bool ready)
    {
        this.ready = ready;
    }

    void updateInfo()
    {
        indicator.SetActive(ready && player.turn == turn);

        turnText.text = $"Turn {player.turn + 1}";
        goldText.text = $"Rp. {player.gold}";
    }
}

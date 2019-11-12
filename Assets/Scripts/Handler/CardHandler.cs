using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    GameManager manager;
    Card card;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onCardClick);
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setCardData(Card card)
    {
        this.card = card;
    }

    public void onCardClick()
    {
        manager.doSelect(card);
        Destroy(gameObject);
    }
}

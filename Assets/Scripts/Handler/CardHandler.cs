using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    GameManager manager;
    CollectManager collectManager;
    Card card;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onCardClick);
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setCollectManager(CollectManager collectManager)
    {
        this.collectManager = collectManager;
    }

    public void setCardData(Card card)
    {
        this.card = card;
    }

    public int getCardId()
    {
        return this.card.id;
    }

    public void onCardClick()
    {
        manager.doSelect(card);
        collectManager.onCardDestroy(card.id);
        Destroy(gameObject);
    }
}

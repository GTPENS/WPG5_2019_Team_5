using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    GameManager manager;
    CollectManager collectManager;
    ActionManager actionManager;
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

    public void setActionManager(ActionManager actionManager)
    {
        this.actionManager = actionManager;
    }

    public void setCardData(Card card)
    {
        this.card = card;
    }

    public int getCardId()
    {
        return this.card.id;
    }

    public string getCardType()
    {
        return this.card.type;
    }

    public void onCardClick()
    {
        if (collectManager != null) {
            manager.doSelect(card);
            collectManager.onCardDestroy(card.id);
            Destroy(gameObject);
        }

        if (actionManager != null) {
            manager.doSpell(card);
            actionManager.onCardDestroy(card.id);
            Destroy(gameObject);
        }
    }
}

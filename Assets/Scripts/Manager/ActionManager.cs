using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] GameObject cardGrid;
    [SerializeField] GameObject[] specialCards;
    [SerializeField] GameObject selectDialog;
    [SerializeField] GameObject luckyDialog;
    [SerializeField] GameObject actionButton;
    GameManager manager;
    Player player;
    List<GameObject> cardObjects;
    Coroutine coroutine;
    int timer;

    void Start()
    {
        player = manager.getPlayer();
        cardObjects = new List<GameObject>();
        
        showSpecialCards();
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setTimer(int timer)
    {
        this.timer = timer;
    }

    void showSpecialCards()
    {
        if (player.cardList != null)
        foreach (var card in player.cardList)
        {
            if (!card.special) continue;

            var cardObject = Instantiate(manager.cards[GameManager.getCardIndex(card.type)], 
                new Vector2(), Quaternion.identity);
            cardObject.transform.SetParent(cardGrid.transform, false);
            cardObject.transform.localScale = Vector3.one;

            CardHandler handler = cardObject.GetComponent<CardHandler>();
            handler.setManager(manager);
            handler.setCardData(card);
            handler.setActionManager(this);

            cardObjects.Add(cardObject);
        }
    }

    public void onCardDestroy(int id)
    {
        for (int i = 0; i < cardObjects.Count; i++) {
            CardHandler cardHandler = cardObjects[i].GetComponent<CardHandler>();

            if (cardHandler.getCardId() == id) {
                cardObjects.RemoveAt(i);
            }
        }
    }

    IEnumerator startTimer()
    {
        while (true)
        {
            timer -= 1;
            manager.updateTimer(timer);
            
            yield return new WaitForSeconds(1f);

            if (timer <= 0)
            {
                randomClick();
                break;
            }
        }
    }

    void randomClick()
    {
        StopCoroutine(coroutine);

        int random = Random.Range(0, cardObjects.Count - 1);
        cardObjects[random].GetComponent<CardHandler>().onCardClick();
    }
}

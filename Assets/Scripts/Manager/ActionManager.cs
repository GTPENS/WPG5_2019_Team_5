﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int timer, tempIndex;

    void Start()
    {
        player = manager.getPlayer();
        cardObjects = new List<GameObject>();

        actionButton.GetComponent<Button>().onClick.AddListener(skip);
        
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
        Debug.Log($"cardList count: {player.cardList.Count}");

        foreach (var card in player.cardList)
        {
            if (!card.special) continue;

            var cardObject = Instantiate(manager.produceCard(card), 
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

    void skip()
    {
        manager.doSkip("skipAction");
    }

    public void saveState(int tempIndex)
    {
        this.tempIndex = tempIndex;
    }

    public void checkState()
    {
        if (tempIndex >= manager.getTurnIndex())
            manager.sellFunction();
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

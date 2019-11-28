using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectManager : MonoBehaviour
{
    [SerializeField] GameObject timerTextObject;
    [SerializeField] GameObject cardGrid;

    GameManager manager;
    List<Card> cardPool;
    List<GameObject> cardObjects;
    Coroutine coroutine;
    Text timerText;
    int turn, timer;

    void Start()
    {
        timerText = timerTextObject.GetComponent<Text>();
        cardObjects = new List<GameObject>();

        for (var i = 0; i < cardPool.Count; i++)
        {
            Card card = cardPool[i];

            var cardObject = Instantiate(manager.cards[GameManager.getCardIndex(card.type)], 
                new Vector2(), Quaternion.identity);
            cardObject.transform.SetParent(cardGrid.transform, false);
            cardObject.transform.localScale = Vector3.one;

            CardHandler handler = cardObject.GetComponent<CardHandler>();
            handler.setManager(manager);
            handler.setCardData(card);
            handler.setCollectManager(this);

            cardObjects.Add(cardObject);
        }

        timerText.text = timer.ToString();
        coroutine = StartCoroutine(startTimer());
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setCards(List<Card> cardPool)
    {
        this.cardPool = cardPool;
    }

    public void setTurn(int turn)
    {
        this.turn = turn;

        if (gameObject.activeSelf && manager.getPlayer().turn == turn) {
            if (coroutine != null)
                StopCoroutine(coroutine);
            
            coroutine = StartCoroutine(startTimer());
        }
    }

    public void setTimer(int timer)
    {
        this.timer = timer;
    }

    public void onCardDestroy(int id)
    {
        for (int i = 0; i < cardObjects.Count; i++) {
            CardHandler cardHandler = cardObjects[i].GetComponent<CardHandler>();

            if (cardHandler.getCardId() == id) {
                manager.addToDeck(cardHandler);
                cardObjects.RemoveAt(i);
            }
        }
    }

    IEnumerator startTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
            timerText.text = timer.ToString();

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

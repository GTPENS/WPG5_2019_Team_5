using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectManager : MonoBehaviour
{
    [SerializeField] GameObject playerGrid;
    [SerializeField] GameObject cardGrid;
    [SerializeField] GameObject[] cards;

    GameManager manager;
    List<Card> cardPool;
    List<GameObject> cardObjects;
    int timer;

    void Start()
    {   
        for (var i = 0; i < cardPool.Count; i++)
        {
            Card card = cardPool[i];

            var cardObject = Instantiate(cards[getCardIndex(card.type)], 
                new Vector2(), Quaternion.identity);
            cardObject.transform.SetParent(cardGrid.transform, false);
            cardObject.transform.localScale = Vector3.one;

            CardHandler handler = cardObject.GetComponent<CardHandler>();
            handler.setManager(manager);
            handler.setCardData(card);
            cardObjects.Add(cardObject);
        }

        StartCoroutine(startTimer());
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setCards(List<Card> cardPool)
    {
        this.cardPool = cardPool;
    }

    public void setTimer(int timer)
    {
        this.timer = timer;
    }

    int getCardIndex(string type)
    {
        switch (type)
        {
            case "Marine": return 0;
            case "Finance": return 1;
            case "Commerce": return 2;
            case "Agriculture": return 3;
            default: return -1;
        }
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(timer);
        randomClick();
    }

    void randomClick()
    {
        int random = Random.Range(0, cardObjects.Count - 1);
        
        cardObjects[random].GetComponent<CardHandler>().onCardClick();
        cardObjects.RemoveAt(random);
    }
}

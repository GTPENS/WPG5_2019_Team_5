using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour
{
    [SerializeField] GameObject deckGrid;
    [SerializeField] GameObject cartGrid;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject priceObject;
    GameManager manager;
    List<GameObject> cardObjects;
    List<GameObject> cartObjects;
    Text priceText;
    int priceTotal;

    void Start()
    {
        cardObjects = new List<GameObject>();
        cartObjects = new List<GameObject>();

        priceText = priceObject.GetComponent<Text>();

        nextButton.GetComponent<Button>().onClick.AddListener(onNext);

        // populateCards();
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    void onNext()
    {
        manager.requestReset();
    }

    void populateCards()
    {
        foreach (var card in manager.getPlayer().cardList)
        {
            var cardObject = Instantiate(manager.produceCard(card), 
                new Vector2(), Quaternion.identity);
            cardObject.transform.SetParent(deckGrid.transform, false);
            cardObject.transform.localScale = Vector3.one;

            CardHandler handler = cardObject.GetComponent<CardHandler>();
            handler.setCardData(card);
            handler.setSellManager(this);

            cardObjects.Add(cardObject);
        }
    }

    public void addCard(CardHandler handler)
    {
        var cardObject = Instantiate(manager.produceCard(handler.getCard()), 
            new Vector2(), Quaternion.identity);
        cardObject.transform.SetParent(cartGrid.transform, false);
        cardObject.transform.localScale = Vector3.one;

        cardObject.GetComponent<CardHandler>().setCardHandler(handler);
        cartObjects.Add(cardObject);

        priceTotal += getPrice(handler.getCardType());
        priceText.text = $"${priceTotal}";
    }

    int getPrice(string type)
    {
        var stocks = manager.getStocks();
        var minor = stocks.Where(x => x.name == type);

        if (minor.Count() > 0)
            return minor.First().price;

        return 0;
    }
}

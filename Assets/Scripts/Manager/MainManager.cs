using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] GameObject goldTextObject;
    [SerializeField] GameObject playerGrid;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject cardsGrid;
    [SerializeField] GameObject stocksGrid;

    GameManager manager;
    Text goldText;
    List<GameObject> playerObjects;
    List<GameObject> cardObjects;
    StockHandler stockHandler;

    void Start()
    {
        goldText = goldTextObject.GetComponent<Text>();
        playerObjects = new List<GameObject>();
        cardObjects = new List<GameObject>();

        stockHandler = stocksGrid.GetComponent<StockHandler>();

        setGoldText(manager.getPlayer().gold);
        populatePlayers();
        updateStockInfo();
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setGoldText(int gold)
    {
        goldText.text = $"$ {gold}";
    }

    public void updateStockInfo()
    {
        stockHandler.updateStocksPrice(manager.getStocks());
    }

    void populatePlayers()
    {
        foreach (var player in manager.getPlayers())
        {
            var playerObject = Instantiate(playerPrefab, 
                new Vector2(), Quaternion.identity);
            playerObject.transform.SetParent(playerGrid.transform, false);
            playerObject.transform.localScale = Vector3.one;

            InfoHandler infoHandler = playerObject.GetComponent<InfoHandler>();
            infoHandler.setPlayer(player);

            playerObjects.Add(playerObject);
        }
    }

    public void updatePlayersInfo()
    {
        var players = manager.getPlayers();

        for (int i = 0; i < players.Count; i++)
        {
            InfoHandler infoHandler = playerObjects[i].GetComponent<InfoHandler>();

            if (infoHandler.getPlayer().id == players[i].id)
                infoHandler.setPlayer(players[i]);
        }
    }

    public void addCard(CardHandler handler)
    {
        int index = GameManager.getCardIndex(handler.getCardType());
        
        var cardObject = Instantiate(manager.cards[index], 
            new Vector2(), Quaternion.identity);
        cardObject.transform.SetParent(cardsGrid.transform, false);
        cardObject.transform.localScale = Vector3.one;

        cardObject.GetComponent<CardHandler>().setCardHandler(handler);
        cardObjects.Add(cardObject);
    }

    public void syncDeck(List<Card> cardPool)
    {
        if (cardObjects == null || cardPool == null) return;

        var cards = cardObjects.Where(x => !cardPool.Contains(x.GetComponent<CardHandler>().getCard()));

        if (cards.Count() > 0)
            cardObjects.Remove(cards.First());
    }

    public List<GameObject> getCardObjects()
    {
        return cardObjects;
    }
}

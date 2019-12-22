using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] GameObject goldTextObject;
    [SerializeField] GameObject timerTextObject;
    [SerializeField] GameObject playerGrid;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject cardsGrid;
    [SerializeField] GameObject stocksGrid;

    GameManager manager;
    Text goldText, timerText;
    List<GameObject> playerObjects;
    List<GameObject> deckObjects;
    StockHandler stockHandler;

    void Start()
    {
        goldText = goldTextObject.GetComponent<Text>();
        timerText = timerTextObject.GetComponent<Text>();
        
        playerObjects = new List<GameObject>();
        deckObjects = new List<GameObject>();

        stockHandler = stocksGrid.GetComponent<StockHandler>();
        cardsGrid.SetActive(true);

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

    public void setTimerText(int timer)
    {
        timerText.text = $"{timer}";
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
            infoHandler.setTurn(manager.getTurnIndex());

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
            {
                infoHandler.setReady(manager.isReady());
                infoHandler.setPlayer(players[i]);
                infoHandler.setTurn(manager.getTurnIndex());
            }
        }
    }

    public void addCard(CardHandler handler)
    {
        var cardObject = Instantiate(manager.produceCard(handler.getCard()), 
            new Vector2(), Quaternion.identity);
        cardObject.transform.SetParent(cardsGrid.transform, false);
        cardObject.transform.localScale = Vector3.one;

        cardObject.GetComponent<CardHandler>().setCardHandler(handler);
        deckObjects.Add(cardObject);
    }

    public List<GameObject> getCardObjects()
    {
        return deckObjects;
    }

    public void setGridActive(bool status)
    {
        cardsGrid.SetActive(status);
    }
}

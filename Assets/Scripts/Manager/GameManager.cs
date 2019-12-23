using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{   
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject bidCanvas;
    [SerializeField] GameObject collectCanvas;
    [SerializeField] GameObject actionCanvas;
    [SerializeField] GameObject sellCanvas;
    public GameObject[] cards;
    public GameObject[] luckyCards;
    public GameObject[] infoCards;
    public GameObject[] plusCards;
    public GameObject[] minusCards;

    MainManager mainManager;
    MenuManager menuManager;
    BidManager bidManager;
    CollectManager collectManager;
    ActionManager actionManager;
    SellManager sellManager;

    NetworkManager network;
    bool isServerOn, ready;
    Player player;
    List<Stock> stockList;
    List<Player> playerList;

    int turnIndex = 0;

    void Awake()
    {
        network = GetComponent<NetworkManager>();
        player = new Player();
    }

    void Start()
    {
        mainManager = mainCanvas.GetComponent<MainManager>();

        menuManager = menuCanvas.GetComponent<MenuManager>();
        menuManager.setManager(this);

        bidManager = bidCanvas.GetComponent<BidManager>();
        bidManager.setManager(this);

        collectManager = collectCanvas.GetComponent<CollectManager>();
        collectManager.setManager(this);

        actionManager = actionCanvas.GetComponent<ActionManager>();
        actionManager.setManager(this);

        sellManager = sellCanvas.GetComponent<SellManager>();
        sellManager.setManager(this);
    }

    public Player getPlayer()
    {
        return player;
    }

    public List<Stock> getStocks()
    {
        return stockList;
    }

    public List<Player> getPlayers()
    {
        return playerList;
    }

    public int getTurnIndex()
    {
        return turnIndex;
    }

    public bool isReady()
    {
        return ready;
    }

    public static int getCardIndex(string type)
    {
        switch (type)
        {
            case "Kelautan": return 0;
            case "Perdagangan": return 1;
            case "Pertanian": return 2;
            case "Keuangan": return 3;
            default: return -1;
        }
    }

    public GameObject produceCard(Card card)
    {
        int index = GameManager.getCardIndex(card.type);

        if (card.special)
        {
            if (card.spell == "Beruntung")
                return luckyCards[index];
            else if (card.spell == "Info Bursa")
                return infoCards[index];
            else if (card.spell == "Investor Plus")
                return plusCards[index];
            else if (card.spell == "Investor Min")
                return minusCards[index];
        }
        else
            return cards[index];

        return null;
    }

    public void updateTimer(int timer)
    {
        mainManager.setTimerText(timer);
    }

    public void joinGame()
    {
        Data data = new Data("join");
        string jsonString = JsonUtility.ToJson(data);
        isServerOn = network.setupSocket();

        if (!isServerOn) {
            Debug.Log("Server is not Running");
            return;
        }

        Debug.Log("Connecting to Server");
        Debug.Log($"Send {jsonString} to Server");
            
        network.writeSocket(jsonString);
        network.readSocket<string>(filterData);

        mainManager.setManager(this);
    }

    public Player getPlayerDetail(int id)
    {
        return playerList.Where(p => p.id == id).First();
    }

    public void doBidding(int bidValue)
    {
        Data data = new Data("bid");
        data.playerId = player.id;
        data.bidValue = bidValue;
        
        player.gold -= bidValue;
        mainManager.setGoldText(player.gold);

        string jsonString = JsonUtility.ToJson(data);
        network.writeSocket(jsonString);
    }

    public void doSelect(Card card)
    {
        Data data = new Data("select");
        data.playerId = player.id;
        data.cardId = card.id;

        string jsonString = JsonUtility.ToJson(data);
        network.writeSocket(jsonString);
    }

    public void doSkip(string command)
    {
        Data data = new Data(command);
        data.playerId = player.id;

        string jsonString = JsonUtility.ToJson(data);
        network.writeSocket(jsonString);
    }

    public void doSpell(string spellName, string stock1, string stock2)
    {
        Data data = new Data("spell");
        data.playerId = player.id;
        data.spell = new Spell(spellName, stock1, stock2);

        string jsonString = JsonUtility.ToJson(data);
        network.writeSocket(jsonString);
    }

    void filterData(string result)
    {
        Data data = JsonUtility.FromJson<Data>(result);

        stockList = data.stockList;
        playerList = data.playerList;
        
        switch (data.command)
        {
            case "preBid":
                player = getPlayerDetail(data.playerId);
                break;

            case "bid":
                bidManager.setTimer(data.timer);

                menuCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                bidCanvas.SetActive(true);
                break;

            case "collect":
                ready = true;
                turnIndex = data.turnIndex;

                player = getPlayerDetail(player.id);

                collectManager.setTimer(data.timer);
                collectManager.setCards(data.cardPool);
                collectManager.setTurn(turnIndex);

                mainManager.updatePlayersInfo();
                collectManager.syncDeck(data.cardPool);

                bidCanvas.SetActive(false);
                collectCanvas.SetActive(true);
                break;

            case "waitCollect":
                turnIndex = data.turnIndex;

                mainManager.updatePlayersInfo();

                collectManager.syncDeck(data.cardPool);
                collectManager.setTimer(data.timer);
                collectManager.resetTimer();
                break;

            case "action":
                player = getPlayerDetail(player.id);

                mainManager.updatePlayersInfo();
                mainManager.setGridActive(false);
                actionManager.setTimer(data.timer);

                collectCanvas.SetActive(false);
                actionCanvas.SetActive(true);
                break;

            case "skipAction":
                actionManager.saveState(turnIndex);
                turnIndex = data.turnIndex;

                mainManager.updatePlayersInfo();
                actionManager.checkState();
                break;

            case "sell":
                sellFunction();
                break;

            case "economy":
                mainManager.updatePlayersInfo();
                mainManager.updateStockInfo();
                break;

            case "reset":
                resetGame();
                break;
        }
    }

    public void sellFunction()
    {
        mainManager.updatePlayersInfo();
        mainManager.updateStockInfo();
        mainManager.setGridActive(true);
                
        collectCanvas.SetActive(false);
        actionCanvas.SetActive(false);
        sellCanvas.SetActive(true);
    }

    public void requestReset()
    {
        Data data = new Data("reset");

        string jsonString = JsonUtility.ToJson(data);
        network.writeSocket(jsonString);
    }

    public void resetGame()
    {
        mainManager.updatePlayersInfo();
        mainManager.updateStockInfo();

        bidManager.reset();
        bidManager.setTimer(10);

        sellCanvas.SetActive(false);
        bidCanvas.SetActive(true);
    }

    public void addToDeck(CardHandler handler)
    {
        mainManager.addCard(handler);
    }
}

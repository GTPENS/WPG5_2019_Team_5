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

    MainManager mainManager;
    MenuManager menuManager;
    BidManager bidManager;
    CollectManager collectManager;
    ActionManager actionManager;

    NetworkManager network;
    bool isServerOn;
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

    public void doSpell(Card card)
    {
        Data data = new Data("spell");
        data.playerId = player.id;
        data.cardId = card.id;
        data.cardSpell = card.spell;

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

                collectManager.syncDeck(data.cardPool);
                collectManager.setTimer(data.timer);
                collectManager.resetTimer();

                // Debug.Log($"waitCollect timer count: {data.timer}");
                break;

            case "action":
                mainManager.updatePlayersInfo();
                actionManager.setTimer(data.timer);

                collectCanvas.SetActive(false);
                actionCanvas.SetActive(true);
                break;

            case "sell":
                mainManager.updatePlayersInfo();
                
                collectCanvas.SetActive(false);
                actionCanvas.SetActive(false);
                sellCanvas.SetActive(true);
                break;

            case "economy":
                mainManager.updatePlayersInfo();
                mainManager.updateStockInfo();
                break;
        }

        // if (mainManager != null)
        //     mainManager.updateDebug();
    }

    public void addToDeck(CardHandler handler)
    {
        mainManager.addCard(handler);
    }
}

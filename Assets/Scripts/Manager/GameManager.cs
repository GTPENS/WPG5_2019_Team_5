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

    MainManager mainManager;
    MenuManager menuManager;
    BidManager bidManager;
    CollectManager collectManager;
    ActionManager actionManager;

    NetworkManager network;
    bool isServerOn;
    Player player;
    List<Player> playerList;

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

    public List<Player> GetPlayers()
    {
        return playerList;
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

    void filterData(string result)
    {
        Data data = JsonUtility.FromJson<Data>(result);

        playerList = data.playerList;
        player = getPlayerDetail(data.playerId);
        
        switch (data.command)
        {
            case "bid":
                bidManager.setTimer(data.timer);

                menuCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                bidCanvas.SetActive(true);
                break;

            case "collect":
                collectManager.setTimer(data.timer);
                collectManager.setCards(data.cardPool);
                collectManager.setTurn(data.turnIndex);

                mainManager.updatePlayersInfo();

                bidCanvas.SetActive(false);
                collectCanvas.SetActive(true);
                break;

            case "action":
                actionManager.setTimer(data.timer);

                collectCanvas.SetActive(false);
                actionCanvas.SetActive(true);
                break;

            case "sell":
                collectCanvas.SetActive(false);
                actionCanvas.SetActive(false);
                sellCanvas.SetActive(true);
                break;
        }
    }
}

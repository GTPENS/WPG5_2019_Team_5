﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject bidCanvas;
    [SerializeField] GameObject collectCanvas;
    [SerializeField] GameObject actionCanvas;
    [SerializeField] GameObject sellCanvas;
    [SerializeField] GameObject goldText;

    MenuManager menuManager;
    BidManager bidManager;
    CollectManager collectManager;

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
        menuManager = menuCanvas.GetComponent<MenuManager>();
        menuManager.setManager(this);

        bidManager = bidCanvas.GetComponent<BidManager>();
        bidManager.setManager(this);

        collectManager = collectCanvas.GetComponent<CollectManager>();
        collectManager.setManager(this);
    }

    public Player getPlayer()
    {
        return player;
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

        string result = network.readSocket();
        Debug.Log($"Receive {result} from Server");
        filterData(result);
    }

    public void doBidding(int bidValue)
    {
        Data data = new Data("bid");
        data.playerId = player.id;
        data.bidValue = bidValue;
        
        player.gold -= bidValue;
        updateGold();

        string jsonString = JsonUtility.ToJson(data);

        network.writeSocket(jsonString);
        string result = network.readSocket();

        Debug.Log($"Receive {result} from Server");
        filterData(result);
    }

    public void doSelect(Card card)
    {
        Data data = new Data("select");
        data.playerId = player.id;
        data.cardId = card.id;

        string jsonString = JsonUtility.ToJson(data);

        network.writeSocket(jsonString);
        string result = network.readSocket();

        Debug.Log($"Receive {result} from Server");
        filterData(result);
    }

    void filterData(string result)
    {
        Data data = JsonUtility.FromJson<Data>(result);
        
        switch (data.command)
        {
            case "bid":
                player.id = data.playerId;
                playerList = data.playerList;

                bidManager.setTimer(data.timer);

                menuCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                bidCanvas.SetActive(true);
                break;

            case "collect":
                collectManager.setTimer(data.timer);
                collectManager.setCards(data.cardPool);
                collectManager.setTurn(data.turnIndex);

                bidCanvas.SetActive(false);
                collectCanvas.SetActive(true);
                break;

            case "action":
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

    void updateGold()
    {
        goldText.GetComponent<Text>().text = player.gold.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] GameObject goldTextObject;
    [SerializeField] GameObject playerGrid;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject cardsGrid;

    GameManager manager;
    Text goldText;
    List<GameObject> playerObjects;

    void Start()
    {
        goldText = goldTextObject.GetComponent<Text>();
        playerObjects = new List<GameObject>();

        setGoldText(manager.getPlayer().gold);
        populatePlayers();
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setGoldText(int gold)
    {
        goldText.text = $"Rp. {gold}";
    }

    void populatePlayers()
    {
        foreach (var player in manager.GetPlayers())
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
        var players = manager.GetPlayers();

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
    }
}

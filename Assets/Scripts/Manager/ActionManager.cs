using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] GameObject cardGrid;
    [SerializeField] GameObject[] specialCards;
    GameManager manager;
    Player player;
    int timer;

    void Start()
    {
        player = manager.getPlayer();

        Debug.Log(player.cardList.Count);
    }

    public void setManager(GameManager manager)
    {
        this.manager = manager;
    }

    public void setTimer(int timer)
    {
        this.timer = timer;
    }

    void populatePlayers()
    {
        foreach (var card in player.cardList)
        {
            if (!card.special) continue;

            var cardObject = Instantiate(specialCards[0], 
                new Vector2(), Quaternion.identity);
            cardObject.transform.SetParent(cardGrid.transform, false);
            cardObject.transform.localScale = Vector3.one;
        }
    }
}

using System;
using System.Collections.Generic;

[System.Serializable]
public class Data
{
    public string command;
    public string datetime;
    public int playerId;
    public int bidValue;
    public int timer;
    public List<Stock> stockList;
    public List<Player> playerList;
    public List<Card> cardPool;
    public int cardId;
    public Spell spell;
    public int turnIndex;

    public Data(string command)
    {
        this.command = command;

        DateTime now = DateTime.Now;
        this.datetime = now.ToString("dd-MM-yyyy hh:mm:ss");
    }
}
using System;
using System.Collections.Generic;

public class Data
{
    public string command;
    public string datetime;
    public int playerId;
    public int bidValue;
    public int timer;
    public List<Player> playerList;

    public Data(string command)
    {
        this.command = command;

        DateTime now = DateTime.Now;
        this.datetime = now.ToString("dd-MM-yyyy hh:mm:ss");
    }
}
using System.Collections.Generic;

public class Data
{
    public string command;
    public int playerId;
    public int bidValue;
    public List<Player> playerList;

    public Data(string command)
    {
        this.command = command;
    }
}
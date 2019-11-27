using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public int id;
    public int gold;
    public int turn;
    public List<Card> cardList;
}
[System.Serializable]
public class Card
{
    public int id;
    public string type;
    public bool special;
    public string spell;

    public Card()
    {
        this.special = false;
        this.spell = "";
    }
}
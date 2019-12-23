using System;
using System.Collections.Generic;

[System.Serializable]
public class Spell
{
    public string name;
    public string stock1;
    public string stock2;

    public Spell(string name)
    {
        this.name = name;
    }

    public Spell(string name, string stock1, string stock2)
    {
        this.name = name;
        this.stock1 = stock1;
        this.stock2 = stock2;
    }
}
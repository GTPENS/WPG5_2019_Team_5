#include "card.h"

int random(int min, int max)
{
  return min + (rand() % ( max - min + 1 ));
}

Card::Card(int id, char const *type)
{
    this->id = id;
    this->type = type;
    this->special = false;
}

Card::Card(int id, char const *type, bool special, char const *spell)
{
    this->id = id;
    this->type = type;
    this->special = special;
    this->spell = spell;
}

int Card::getId()
{
    return this->id;
}

char const *Card::getType()
{
    return this->type;
}

bool Card::isSpecial()
{
    return this->special;
}

char const *Card::getSpell()
{
    return this->spell;
}

char const *Card::getRandomType()
{
    char const *Type[] = {"Kelautan", "Perdagangan", "Pertanian", "Keuangan"};
    int max = sizeof(Type) / sizeof(Type[0]);

    return Type[random(0, max - 1)];
}

bool Card::randomSpecial()
{
    return random(0, 1);
}

char const *Card::getRandomSpell()
{
    char const *Spell[] = {"Investor Plus", "Investor Min", "Info Bursa", "Beruntung"};
    int max = sizeof(Spell) / sizeof(Spell[0]);

    return Spell[random(0, max - 1)];
}

Json::Value Card::toArray()
{
    Json::Value value;
    value["id"] = this->id;
    value["type"] = this->type;
    value["special"] = this->special;

    if (this->special)
        value["spell"] = this->spell;
    
    return value;
}
#include "card.h"

int random(int min, int max)
{
  return min + (rand() % ( max - min + 1 ));
}

Card::Card(char const *type)
{
    this->type = type;
    this->special = false;
}

Card::Card(char const *type, bool special, char const *spell)
{
    this->type = type;
    this->special = special;
    this->spell = spell;
}

char const *Card::getRandomType()
{
    char const *Type[] = {"Marine", "Finance", "Commerce", "Agriculture"};
    int max = sizeof(Type) / sizeof(Type[0]);

    return Type[random(0, max - 1)];
}

char const *Card::getRandomSpell()
{
    char const *Spell[] = {"Double UP", "Double DOWN", "Commerce", "Agriculture"};
    int max = sizeof(Spell) / sizeof(Spell[0]);

    return Spell[random(0, max - 1)];
}

Json::Value Card::toArray()
{
    Json::Value value;
    value["type"] = this->type;
    value["special"] = this->special;

    if (this->special)
        value["spell"] = this->spell;
    
    return value;
}
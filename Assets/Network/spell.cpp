#include "spell.h"
#include <stdio.h>
#include <iostream>

using namespace std;

Spell::Spell()
{
    this->name = "";   
    this->stock1 = "";   
    this->stock2 = "";   
}

Spell::Spell(Json::Value value)
{
    this->name = value["name"].asCString();   
    this->stock1 = value["stock1"].asCString();   
    this->stock2 = value["stock2"].asCString();   
}

Spell::Spell(char const *name)
{
    this->name = name;
}

char const *Spell::getName()
{
    return this->name;
}

void Spell::setStocks(char const *stock1, char const *stock2)
{
    this->stock1 = stock1;
    this->stock2 = stock2;
}

char const *Spell::getStock1()
{
    return this->stock1;
}

char const *Spell::getStock2()
{
    return this->stock2;
}

Json::Value Spell::toArray()
{
    Json::Value value;
    value["name"] = this->name;
    value["stock1"] = this->stock1;
    value["stock2"] = this->stock2;
    
    return value;
}
#include "data.h"
#include <stdio.h>
#include <iostream>

using namespace std;

Data::Data(string command, vector<Stock> stockList, vector<Player> playerList)
{
    this->command = command;
    this->stockList = stockList;
    this->playerList = playerList;

    this->playerId = 0;
    this->timer = 0;
    this->turnIndex = 0;
}

int Data::getPlayerId()
{
    return this->playerId;
}

void Data::setPlayerId(int playerId)
{
    this->playerId = playerId;
}

int Data::getTimer()
{
    return this->timer;
}

void Data::setTimer(int timer)
{
    this->timer = timer;
}

int Data::getTurn()
{
    return this->turnIndex;
}

void Data::setTurn(int turnIndex)
{
    this->turnIndex = turnIndex;
}

void Data::setCards(vector<Card> cards)
{
    cardPool = cards;
}

Json::Value Data::toArray()
{
    Json::Value value;
    value["command"] = this->command;
    value["playerId"] = this->playerId;
    value["timer"] = this->timer;
    value["turnIndex"] = this->turnIndex;

    for (int i = 0; i < this->stockList.size(); i++)
    {
        value["stockList"][i] = this->stockList[i].toArray();
    }

    for (int i = 0; i < this->playerList.size(); i++)
    {
        value["playerList"][i] = this->playerList[i].toArray();
    }

    for (int i = 0; i < this->cardPool.size(); i++)
    {
        value["cardPool"][i] = this->cardPool[i].toArray();
    }
    
    return value;
}
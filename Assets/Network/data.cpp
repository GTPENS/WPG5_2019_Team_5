#include "data.h"
#include <stdio.h>
#include <iostream>

using namespace std;

Data::Data(string command, vector<Player> playerList)
{
    this->command = command;
    this->playerList = playerList;

    this->playerId = 0;
    this->timer = 0;
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

Json::Value Data::toArray()
{
    Json::Value value;
    value["command"] = this->command;
    value["timer"] = this->timer;

    for (int i = 0; i < this->playerList.size(); i++)
    {
        value["playerList"][i] = this->playerList[i].toArray();
    }
    
    return value;
}
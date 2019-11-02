#include "data.h"
#include <stdio.h>
#include <iostream>

using namespace std;

Data::Data(bool status, string command, int playerId, vector<Player> playerList)
{
    this->status = status;
    this->command = command;
    this->playerId = playerId;
    this->playerList = playerList;
    this->timer = 0;
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
    value["status"] = this->status;
    value["command"] = this->command;
    value["timer"] = this->timer;

    int index = 0;

    for (int i = 0; i < this->playerList.size(); i++)
    {
        if (this->playerList[i].getId() != playerId) {
            value["enemyList"][index] = this->playerList[i].toArray();
            index++;
        }
        else
            value["player"] = this->playerList[i].toArray();
    }
    
    return value;
}
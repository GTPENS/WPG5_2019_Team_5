#include "data.h"
#include <stdio.h>
#include <iostream>

using namespace std;

Data::Data(bool status, int playerId, int timer, vector<Player> playerList)
{
    this->status = status;
    this->playerId = playerId;
    this->timer = timer;
    this->playerList = playerList;
}

Json::Value Data::toArray()
{
    Json::Value value;
    value["status"] = this->status;
    value["playerId"] = this->playerId;
    value["timer"] = this->timer;

    for (int i = 0; i < this->playerList.size(); i++)
    {
        value["playerList"][0] = this->playerList[i].toArray();
    }
    
    return value;
}
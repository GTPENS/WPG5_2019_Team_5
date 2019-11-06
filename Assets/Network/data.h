#pragma once

#include <vector>
#include "player.h"
#include <jsoncpp/json/json.h>

using namespace std;

class Data
{
    public:
        Data(string, vector<Player>);

        int getPlayerId();
        void setPlayerId(int);
        int getTimer();
        void setTimer(int);

        Json::Value toArray();

    private:
        string command;
        int playerId;
        int timer;

        vector<Player> playerList;
};
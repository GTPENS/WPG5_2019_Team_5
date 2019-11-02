#pragma once

#include <vector>
#include "player.h"
#include <jsoncpp/json/json.h>

using namespace std;

class Data
{
    public:
        Data(bool, string, int, vector<Player>);

        int getTimer();
        void setTimer(int);

        Json::Value toArray();

    private:
        bool status;
        string command;
        int playerId;
        int timer;

        vector<Player> playerList;
};
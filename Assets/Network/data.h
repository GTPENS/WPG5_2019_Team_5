#pragma once

#include <vector>
#include "player.h"
#include <jsoncpp/json/json.h>

using namespace std;

class Data
{
    public:
        Data(bool, int, int, vector<Player>);

        Json::Value toArray();

    private:
        bool status;
        int playerId;
        int timer;

        vector<Player> playerList;
};
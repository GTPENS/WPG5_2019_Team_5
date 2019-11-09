#pragma once

#include <vector>
#include "player.h"
#include "card.h"
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
        void addCard(Card);

        Json::Value toArray();

    private:
        string command;
        int playerId;
        int timer;

        vector<Player> playerList;
        vector<Card> cardPool;
};
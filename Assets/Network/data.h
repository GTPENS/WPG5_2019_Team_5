#pragma once

#include <vector>
#include "stock.h"
#include "player.h"
#include "card.h"
#include "spell.h"
#include <jsoncpp/json/json.h>

using namespace std;

class Data
{
    public:
        Data(string, vector<Stock>, vector<Player>);

        int getPlayerId();
        void setPlayerId(int);
        int getTimer();
        void setTimer(int);
        int getTurn();
        void setTurn(int);
        void setCards(vector<Card>);
        void setSpell(Spell);

        Json::Value toArray();

    private:
        string command;
        int playerId;
        int timer;
        int turnIndex;

        vector<Stock> stockList;
        vector<Player> playerList;
        vector<Card> cardPool;
        Spell spell;
};
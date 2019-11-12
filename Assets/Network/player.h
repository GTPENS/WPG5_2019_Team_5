#pragma once

#include <vector>
#include "card.h"
#include <jsoncpp/json/json.h>

using namespace std;

class Player
{
    public:
        Player(int);
        
        int getId();
        int getTurn();
        void setTurn(int);
        int getGold();
        void addGold(int);
        void reduceGold(int);
        void addCard(Card);
        int getCardTotal();
        Card getCard(int);

        Json::Value toArray();

    private:
        int id;
        int turn;
        int gold;

        vector<Card> cardList;
};
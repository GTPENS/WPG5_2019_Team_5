#pragma once

#include <jsoncpp/json/json.h>

class Player
{
    public:
        Player(int);
        
        int getId();
        int getTurn();
        void setTurn(int turn);
        int getGold();
        void addGold(int gold);
        void reduceGold(int gold);

        Json::Value toArray();

    private:
        int id;
        int turn;
        int gold;
};
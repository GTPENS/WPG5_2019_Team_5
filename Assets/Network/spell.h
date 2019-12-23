#pragma once

#include <vector>
#include "stock.h"
#include "player.h"
#include "card.h"
#include <jsoncpp/json/json.h>

class Spell
{
    public:
        Spell();
        Spell(Json::Value);
        Spell(char const *);

        char const *getName();
        void setStocks(char const *, char const *);
        char const *getStock1();
        char const *getStock2();

        Json::Value toArray();

    private:
        char const *name;
        char const *stock1;
        char const *stock2;
};
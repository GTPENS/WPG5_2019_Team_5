#pragma once

#include <jsoncpp/json/json.h>

class Card
{
    public:
        Card(int, char const *);
        Card(int, char const *, bool, char const *);

        int getId();
        bool isSpecial();
        char const *getSpell();

        static char const *getRandomType();
        static char const *getRandomSpell();

        Json::Value toArray();

    private:
        int id;
        char const *type;
        bool special;
        char const *spell;
};
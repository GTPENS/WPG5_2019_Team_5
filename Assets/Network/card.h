#pragma once

#include <jsoncpp/json/json.h>

class Card
{
    public:
        Card(char const *);
        Card(char const *, bool, char const *);

        static char const *getRandomType();
        static char const *getRandomSpell();

        Json::Value toArray();

    private:
        char const *type;
        bool special;
        char const *spell;
};
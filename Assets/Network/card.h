#pragma once

#include <jsoncpp/json/json.h>

class Card
{
    public:
        Card(int, char const *);
        Card(int, char const *, bool, char const *);

        int getId();
        char const *getType();
        bool isSpecial();
        char const *getSpell();

        static char const *getRandomType();
        static bool randomSpecial();
        static char const *getRandomSpell();

        Json::Value toArray();

    private:
        int id;
        char const *type;
        bool special;
        char const *spell;
};
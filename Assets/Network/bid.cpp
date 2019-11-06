#include "bid.h"

Bid::Bid(int id, int value)
{
    this->id = id;
    this->value = value;
}

int Bid::getId()
{
    return this->id;
}

int Bid::getValue()
{
    return this->value;
}
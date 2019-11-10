#include "player.h"

Player::Player(int id)
{
    this->id = id;
    this->turn = 0;
    this->gold = 0;
}

int Player::getId()
{
    return this->id;
}

int Player::getTurn()
{
    return this->turn;
}

void Player::setTurn(int turn)
{
    this->turn = turn;
}

int Player::getGold()
{
    return this->gold;
}

void Player::addGold(int gold)
{
    this->gold += gold;
}

void Player::reduceGold(int gold)
{
    this->gold -= gold;
}

void Player::addCard(Card card)
{
    cardList.push_back(card);
}

Json::Value Player::toArray()
{
    Json::Value value;
    value["id"] = this->id;
    value["turn"] = this->turn;
    value["gold"] = this->gold;

    for (int i = 0; i < this->cardList.size(); i++)
    {
        value["cardList"][i] = this->cardList[i].toArray();
    }
    
    return value;
}
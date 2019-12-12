#include "stock.h"

Stock::Stock(string name)
{
    this->name = name;
    this->price = 20;
}

string Stock::getName()
{
    return this->name;
}

int Stock::getPrice()
{
    return this->price;
}

void Stock::upPrice()
{
    this->price += 20;
}

void Stock::upPrice(int price)
{
    this->price += price;
}

void Stock::downPrice()
{
    this->price -= 20;
}

void Stock::downPrice(int price)
{
    this->price -= price;
}

Json::Value Stock::toArray()
{
    Json::Value value;
    value["name"] = this->name;
    value["price"] = this->price;
    
    return value;
}
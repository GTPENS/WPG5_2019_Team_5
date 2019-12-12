#pragma once

#include <iostream>
#include <jsoncpp/json/json.h>

using namespace std;

class Stock {
    public:
        Stock(string);

        string getName();
        int getPrice();
        void upPrice();
        void upPrice(int);
        void downPrice();
        void downPrice(int);

        Json::Value toArray();
    
    private:
        string name;
        int price;
};
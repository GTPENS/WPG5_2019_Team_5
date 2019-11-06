#pragma once

class Bid {
    public:
        Bid(int, int);

        int getId();
        int getValue();
    
    private:
        int id;
        int value;
};
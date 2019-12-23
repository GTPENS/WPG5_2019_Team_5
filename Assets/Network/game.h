#pragma once

#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <arpa/inet.h>
#include <pthread.h>
#include <iostream>
#include <vector>
#include <algorithm>
#include <random>
#include <jsoncpp/json/json.h>
#include "data.h"
#include "player.h"
#include "bid.h"

using namespace std;

class Game
{
    public:
        Game();

        int index;
        
        void setMaxPlayer(int);
        void economyShuffle();
        void run(void (*) (Game *, char *, int));
        void addPlayer(Player, int);
        void doBid(int, int, int);
        void populateCards(Data *);
        void sortBid();
        void doSelect(int, int, int);
        void action();
        void doSkipAction(int, int);
        void doSpell(int, Spell, int);
        void increaseStock(const char *, const char *);
        void decreaseStock(const char *, const char *);
        void doReset(int, int);

    private:
        int my_sock, their_sock, len, maxPlayer, turnIndex, cardIndex;
        char ip[INET_ADDRSTRLEN];
        bool firstJoin, firstBid, firstCollect;

        struct sockaddr_in my_addr, their_addr;
        socklen_t their_addr_size;
        pthread_t sendt, recvt;
        
        vector<Stock> stockList;
        vector<Player> playerList;
        vector<Bid> bidList;
        vector<Card> randomCards;

        vector<string> economyLaut;
        vector<string> economyDagang;
        vector<string> economyTani;
        vector<string> economyUang;
};
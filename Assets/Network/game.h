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
        void run(void (*) (Game *, char *, int));
        void addPlayer(Player, int);
        void doBid(int, int, int);
        void populateCards(Data *);
        void sortBid();
        void doSelect(int, int, int);

    private:
        int my_sock, their_sock, len, maxPlayer, turnIndex, cardIndex;
        char ip[INET_ADDRSTRLEN], msg[500];

        struct sockaddr_in my_addr, their_addr;
        socklen_t their_addr_size;
        pthread_t sendt, recvt;
        
        vector<Player> playerList;
        vector<Bid> bidList;
        vector<Card> randomCards;
};
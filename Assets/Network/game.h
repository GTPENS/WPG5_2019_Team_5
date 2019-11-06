#pragma once

#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <netinet/in.h>
#include <string.h>
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
        
        void run(void (*) (Game *, char *));
        void sendBack(Data);
        void addPlayer(Player);
        void doBid(int, int);
        void updatePosition();

    private:
        int server_fd, new_socket, valread;
        struct sockaddr_in address;
        int opt = 1;
        int addrlen = sizeof(address);
        char buffer[1024] = {0};
        char const *hello = "Hello from server";

        vector<Player> playerList;
        vector<Bid> bidList;
};
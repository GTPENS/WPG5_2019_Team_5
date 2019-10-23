#pragma once

#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <netinet/in.h>
#include <string.h>
#include <iostream>
#include <vector>
#include <jsoncpp/json/json.h>
#include "player.h"

using namespace std;

class Game
{
    public:
        Game();
        
        void run(void (*) (Game *, char *));
        void addPlayer(Player);

    private:
        int server_fd, new_socket, valread;
        struct sockaddr_in address;
        int opt = 1;
        int addrlen = sizeof(address);
        char buffer[1024] = {0};
        char const *hello = "Hello from server";

        vector<Player> playerList;
};
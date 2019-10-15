#include "game.h"

#define PORT 8080 

Game::Game()
{
    
}

void Game::run(void (*onRequest) (Game *, char *))
{
    // Creating socket file descriptor 
	if ((server_fd = socket(AF_INET, SOCK_STREAM, 0)) == 0) 
	{ 
		perror("socket failed"); 
		exit(EXIT_FAILURE); 
	} 
	
	// Forcefully attaching socket to the port 8080 
	if (setsockopt(server_fd, SOL_SOCKET, SO_REUSEADDR | SO_REUSEPORT, &opt, sizeof(opt))) 
	{ 
		perror("setsockopt"); 
		exit(EXIT_FAILURE); 
	}

	address.sin_family = AF_INET; 
	address.sin_addr.s_addr = INADDR_ANY; 
	address.sin_port = htons( PORT ); 
	
	// Forcefully attaching socket to the port 8080 
	if (bind(server_fd, (struct sockaddr *)&address, sizeof(address)) < 0) 
	{ 
		perror("bind failed"); 
		exit(EXIT_FAILURE); 
	}

	cout << "The Server is running on port " << PORT << endl;

	if (listen(server_fd, 3) < 0) 
	{ 
		perror("listen"); 
		exit(EXIT_FAILURE); 
	}

	if ((new_socket = accept(server_fd, (struct sockaddr *)&address, (socklen_t*)&addrlen)) < 0) 
	{ 
		perror("accept"); 
		exit(EXIT_FAILURE); 
	}

	valread = read( new_socket , buffer, 1024); 
	onRequest(this, buffer);
}

void Game::addPlayer(Player player)
{
	playerList.push_back(player);
	cout << "* Add Player with id " << player.getId() << endl;

	char const *success = "Player Successfully Connected";
	send(new_socket, success, strlen(success), 0);
	
	// cout << playerList.at(0).getGold() << endl;
}
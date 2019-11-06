#include "game.h"

#define PORT 8080 

Game::Game()
{
    this->index = 0;
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
	address.sin_port = htons(PORT);
	
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

	while(true)
	{
		valread = recv( new_socket, buffer, sizeof(buffer), 0);

		if (valread != -1) {
			onRequest(this, buffer);
		}
	}
}

void Game::sendBack(Data data)
{
	Json::FastWriter fastwriter;
	string jsonData = fastwriter.write(data.toArray());

	char const *converted = jsonData.c_str();
	
	int result = send(new_socket, converted, strlen(converted), 0);
	delete converted;

	if (result == -1)
		cout << "* Send Feedback to Client Failed" << endl;
}

void Game::addPlayer(Player player)
{
	playerList.push_back(player);
	cout << "* Add Player with id " << player.getId() << endl;

	Data data("bid", playerList);
	data.setPlayerId( player.getId());

	this->sendBack(data);
}

bool compare(Bid data1, Bid data2) 
{ 
    return (data1.getValue() > data2.getValue()); 
}

void Game::doBid(int playerId, int bidValue)
{
	cout << "* Player " << playerId << " bid " << bidValue << " gold" << endl;
	bidList.push_back(Bid(playerId, bidValue));
	
	sort(bidList.begin(), bidList.end(), compare);
	updatePosition();

	Data data("play", playerList);
	this->sendBack(data);
}

void Game::updatePosition()
{
	for (int i = 0; i < bidList.size(); i++)
	{
		Bid bid = bidList[i];

		for (int j = 0; j < playerList.size(); j++)
		{
			if (playerList[j].getId() == bid.getId()) {
				playerList[j].setTurn(i);
			}
		}

		cout << "* Bidding Phase Complete" << endl;
		cout << "    => Turn " << i << " is Player " << bid.getId() << endl;
	}
}
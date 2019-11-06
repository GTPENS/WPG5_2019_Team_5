#include "game.h"
#define PORT 8080 

struct client_info {
	Game *context;
	void (*onRequest) (Game *, char *, int);

	int sockno;
	char ip[INET_ADDRSTRLEN];
};

int clients[100], n = 0;

struct client_info cl;
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

// =============== Server Created by Rahmat Ansori ===============

Game::Game()
{
    this->index = 0;
}

void sendToAll(Data data, int current)
{
	pthread_mutex_lock(&mutex);

	Json::FastWriter fastwriter;
	string jsonData = fastwriter.write(data.toArray());

	char const *converted = jsonData.c_str();

	for (int i = 0; i < n; i++) {
		if (clients[i] != current) 
		{
			if (send(clients[i], converted, strlen(converted), 0) < 0) 
			{
				perror("Sending Failed");
				continue;
			}
		}
	}

	pthread_mutex_unlock(&mutex);
}

void sendBack(Data data, int current)
{
	pthread_mutex_lock(&mutex);

	Json::FastWriter fastwriter;
	string jsonData = fastwriter.write(data.toArray());

	char const *converted = jsonData.c_str();

	if (send(current, converted, strlen(converted), 0) < 0) 
	{
		perror("Sending Failed");
	}

	pthread_mutex_unlock(&mutex);
}

void filterSend(char *message, client_info info)
{
	int current = info.sockno;
	info.onRequest(info.context, message, current);
}

void *receive(void *sock)
{
	struct client_info cl = *((struct client_info *) sock);
	char message[500];
	int len, j;

	while ((len = recv(cl.sockno, message, 500, 0)) > 0) 
	{
		message[len] = '\0';
		filterSend(message, cl);
		memset(message, '\0', sizeof(message));
	}

	pthread_mutex_lock(&mutex);
	printf("%s Disconnected\n", cl.ip);
	
	for (int i = 0; i < n; i++) {
		if (clients[i] == cl.sockno) {
			j = i;
			while (j < n-1) {
				clients[j] = clients[j+1];
				j++;
			}
		}
	}

	n--;
	pthread_mutex_unlock(&mutex);
}

void Game::run(void (*onRequest) (Game *, char *, int))
{
	my_sock = socket(AF_INET,SOCK_STREAM, 0);
	memset(my_addr.sin_zero, '\0', sizeof(my_addr.sin_zero));
	
	my_addr.sin_family = AF_INET;
	my_addr.sin_addr.s_addr = INADDR_ANY;
	my_addr.sin_port = htons(PORT);
	their_addr_size = sizeof(their_addr);

	if (bind(my_sock, (struct sockaddr *) &my_addr, sizeof(my_addr)) != 0) {
		perror("Binding Failed");
		exit(1);
	}

	if (listen(my_sock, 5) != 0) {
		perror("Listening Failed");
		exit(1);
	}

	while (true) 
	{
		if ((their_sock = accept(my_sock, (struct sockaddr *) &their_addr, &their_addr_size)) < 0) {
			perror("Accept Failed");
			exit(1);
		}

		pthread_mutex_lock(&mutex);
		inet_ntop(AF_INET, (struct sockaddr *) &their_addr, ip, INET_ADDRSTRLEN);
		printf("%s Connected\n", ip);

		cl.context = this;
		cl.onRequest = onRequest;
		cl.sockno = their_sock;
		strcpy(cl.ip, ip);
		clients[n] = their_sock;
		n++;
		
		pthread_create(&recvt, NULL, receive, &cl);
		pthread_mutex_unlock(&mutex);
	}
}

void Game::addPlayer(Player player, int target)
{
	playerList.push_back(player);
	cout << "* Add Player with id " << player.getId() << endl;

	Data data("bid", playerList);
	data.setPlayerId( player.getId());

	sendBack(data, target);
}

bool compare(Bid data1, Bid data2) 
{ 
    return (data1.getValue() > data2.getValue()); 
}

void Game::doBid(int playerId, int bidValue, int target)
{
	cout << "* Player " << playerId << " bid " << bidValue << " gold" << endl;
	bidList.push_back(Bid(playerId, bidValue));
	
	sort(bidList.begin(), bidList.end(), compare);
	updatePosition();

	Data data("play", playerList);
	sendBack(data, target);
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
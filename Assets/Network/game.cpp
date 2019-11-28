#include "game.h"

#define PORT 8080 
#define TO_ALL -1

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
	this->maxPlayer = 4;
	this->turnIndex = 0;
	this->cardIndex = 0;

	vector<string> pool{"Investor Plus", "Investor Min", "Info Bursa", "Beruntung"};

	this->economyLaut = pool;
	this->economyDagang = pool;
	this->economyTani = pool;
	this->economyUang = pool;

	cout << "* Initialize Game" << endl;
}

void Game::setMaxPlayer(int max)
{
	this->maxPlayer = max;
	cout << "* Max Player set to " << maxPlayer << endl;
}

void Game::economyShuffle()
{
	auto rng = default_random_engine{};
	shuffle(begin(economyLaut), end(economyLaut), rng);
	shuffle(begin(economyDagang), end(economyDagang), rng);
	shuffle(begin(economyTani), end(economyTani), rng);
	shuffle(begin(economyUang), end(economyUang), rng);
}

void sendToAll(Data data, int current = TO_ALL)
{
	pthread_mutex_lock(&mutex);

	Json::FastWriter fastwriter;
	string jsonData = fastwriter.write(data.toArray());

	char const *converted = jsonData.c_str();

	for (int i = 0; i < n; i++) {
		if (current != TO_ALL && clients[i] == current) continue;

		if (send(clients[i], converted, strlen(converted), 0) < 0) 
		{
			perror("Sending Failed");
			continue;
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
	// printf("* Client with ip %s is Disconnected\n", cl.ip);
	
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
		// printf("* Client with ip %s is Connected\n", ip);

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
	if (!firstJoin) {
		cout << "* Waiting for players to join" << endl;
		firstJoin = true;
	}

	cout << "    * Add Player with id " << player.getId() << endl;
	playerList.push_back(player);

	if (playerList.size() < maxPlayer) {
		cout << "* Waiting other player" << endl;

		Data data("wait", playerList);
		data.setPlayerId(player.getId());
		sendBack(data, target);
	}
	else
	{
		cout << "* Max Player Reached, the Game Starts" << endl << endl;

		Data data("bid", playerList);
		data.setTimer(10);
		sendToAll(data, TO_ALL);
	}
}

bool compare(Bid data1, Bid data2) 
{ 
    return (data1.getValue() > data2.getValue()); 
}

void Game::doBid(int playerId, int bidValue, int target)
{
	if (!firstBid) {
		cout << "* Bidding Phase" << endl;
		firstBid = true;
	}
	
	cout << "    * Player " << playerId << " => " << bidValue << " gold" << endl;
	bidList.push_back(Bid(playerId, bidValue));
	
	sort(bidList.begin(), bidList.end(), compare);

	if (bidList.size() != playerList.size()) 
	{
		cout << "* Waiting other player" << endl;

		Data data("wait", playerList);
		sendBack(data, target);
	}
	else 
	{
		sortBid();
			
		Data data("collect", playerList);
		data.setTimer(10);

		populateCards(&data);
		sendToAll(data, TO_ALL);
	}
}

void Game::populateCards(Data *data)
{
	for (int i = 0; i < 4; i++)
	{
		char const *type = Card::getRandomType();

		if (Card::randomSpecial())
			// randomCards.push_back(Card(cardIndex, type, true, Card::getRandomSpell()));
			randomCards.push_back(Card(cardIndex, type, true, "Beruntung"));
		else
			randomCards.push_back(Card(cardIndex, type));
		
		cardIndex++;
	}
	
	data->setCards(randomCards);
}

void Game::sortBid()
{
	cout << "* Bidding Phase Complete" << endl;

	for (int i = 0; i < bidList.size(); i++)
	{
		Bid bid = bidList[i];

		for (int j = 0; j < playerList.size(); j++)
		{
			if (playerList[j].getId() == bid.getId()) {
				playerList[j].reduceGold(bid.getValue());
				playerList[j].setTurn(i);
			}
		}

		cout << "    * Turn " << (i + 1) << " => Player " 
			<< bid.getId() << endl;
	}
}

void Game::doSelect(int playerId, int cardId, int target)
{
	if (!firstCollect) {
		cout << "* Collect Phase" << endl;
		firstCollect = true;
	}
	
	cout << "    * Player " << playerId << " => Card " << cardId << endl;

	// Do remove card from cardPool
	// Do add card to player cardList
	// Do change turn

	for (int i = 0; i < randomCards.size(); i++)
	{
		if (randomCards[i].getId() != cardId) continue;

		for (int j = 0; j < playerList.size(); j++)
		{
			if (playerList[j].getId() == playerId) {
				playerList[j].addCard(randomCards[i]);
				randomCards.erase(randomCards.begin() + i);
				turnIndex++;
				break;
			}
		}
	}

	if (turnIndex + 1 >= playerList.size())
		turnIndex = 0;

	Data data("collect", playerList);
	data.setCards(randomCards);
	data.setTimer(10);
	data.setTurn(turnIndex);
	
	sendToAll(data, TO_ALL);

	if (randomCards.size() <= 0)
		action();
}

void Game::action()
{
	bool isFound;

	// player with null special card will skip
	// player with special card can skip this phase

	for (int i = 0; i < playerList.size(); i++)
	{
		if (isFound)
			break;

		for (int j = 0; j < playerList[i].getCardTotal(); j++)
		{
			Card card = playerList[i].getCard(j);

			if (card.isSpecial()) {
				turnIndex = i;
				isFound = true;
				break;
			}
		}
	}

	if (isFound)
	{
		cout << "* Action Phase" << endl;
		
		Data data("action", playerList);
		data.setTurn(turnIndex);

		sendToAll(data, TO_ALL);
	}
	else
	{
		cout << "* Player with Special Card not Found, Action Phase Skipped" << endl;

		Data data("sell", playerList);
		sendToAll(data, TO_ALL);
	}
}

void Game::doSpell(int playerId, int cardId, const char *cardSpell, int target)
{
	cout << "    * Player " << playerId << " => " << cardSpell << " Special Card" << endl;

	if (strcmp(cardSpell, "Investor Plus") == 0) {
		
	}
	else if (strcmp(cardSpell, "Investor Min") == 0) {
		
	}
	else if (strcmp(cardSpell, "Info Bursa") == 0) {
		
	}
	else if (strcmp(cardSpell, "Beruntung") == 0) {
		// Get two additional card
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < playerList.size(); j++)
			{
				char const *type = Card::getRandomType();

				if (playerList[j].getId() == playerId) {
					playerList[j].addCard(Card(cardIndex, type));
					cardIndex++;
				}
			}
		}
	}

	if (index < playerList.size()) 
	{
		cout << "* Waiting other player" << endl;

		Data data("wait", playerList);
		sendBack(data, target);
	}
	else 
	{
		cout << "* Action Phase Complete" << endl;

		Data data("sell", playerList);
		sendToAll(data, TO_ALL);
	}
}

void Game::doSkip()
{
	
}
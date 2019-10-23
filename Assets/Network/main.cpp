#include "game.h"

int random(int min, int max)
{
	srand(time(NULL));
	return min + (rand() % ( max - min + 1 ));
}

void onRequest(Game *game, char *buffer)
{
	Json::Value root;
    Json::Reader reader;
    bool parsed = reader.parse(buffer, root);

	if (!parsed) return;

	if (root["command"] == "join")
	{
		Player player(random(1111, 9999));
		game->addPlayer(player);
	}
}

int main(int argc, char const *argv[]) 
{ 
	Game game;
	game.run(onRequest);
}
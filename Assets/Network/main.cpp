#include "game.h"

void onRequest(Game *game, char *buffer)
{
	Json::Value root;
    Json::Reader reader;
    bool parsed = reader.parse(buffer, root);

	if (!parsed) return;

	if (root["command"] == "join")
	{
		Player player(game->index);
		game->addPlayer(player);
		game->index++;
	}
}

int main(int argc, char const *argv[]) 
{
	srand(time(NULL));
	
	Game game;
	game.run(onRequest);
}
#include "game.h"

void onRequest(Game *game, char *message, int target)
{
	Json::Value root;
    Json::Reader reader;
    bool parsed = reader.parse(message, root);

	if (!parsed) return;
	cout << root << endl;

	if (root["command"] == "join")
	{
		Player player(game->index);
		game->addPlayer(player, target);
		game->index++;
	}
	else if (root["command"] == "bid") 
	{
		game->doBid(root["playerId"].asUInt(), root["bidValue"].asUInt(), target);
	}
}

int main(int argc, char const *argv[]) 
{
	srand(time(NULL));
	
	Game game;
	game.run(onRequest);
}
#include "game.h"

void onRequest(Game *game, char *message, int target)
{
	Json::Value root;
    Json::Reader reader;
    bool parsed = reader.parse(message, root);

	if (!parsed) return;

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
	else if (root["command"] == "select") 
	{
		game->doSelect(root["playerId"].asUInt(), root["cardId"].asUInt(), target);
	}
	else if (root["command"] == "spell") 
	{
		game->doSpell(root["playerId"].asUInt(), root["cardId"].asUInt(), root["cardSpell"].asCString(), target);
	}
}

int main(int argc, char *argv[]) 
{
	srand(time(NULL));
	
	Game game;
	game.setMaxPlayer(argc > 1 ? atoi(argv[1]) : 4);
	game.economyShuffle();
	game.run(onRequest);
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Lobby
{
    public Lobby(string id, int maxPlayers)
    {
        this.id = id;
        this.maxPlayers = maxPlayers;
        this.lobbyState = 0;
    }
    public long clock = 0;
    public int botCount = 0;
    public List<Player> players;
    [JsonIgnore]
    public Bot bot;
    public int lobbyState;
    public GameState game;
    public int numPlayers = 1;
    public string id;
    public int maxPlayers;
    public Board board;
    public int score = 0;
}
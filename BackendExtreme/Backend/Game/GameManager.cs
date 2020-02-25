using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GameManager
{
    Dictionary<string, Lobby> lobbies;
    private Thread thread;
    private int[][] data;
    public GameManager(Dictionary<string, Lobby> lobbies)
    {
        this.lobbies = lobbies;
        thread = new Thread(stateUpdate);
        data = new int[][] {
                new int[] {0, 0, 1, 0},
                new int[] {0, 0, 1, 0},
                new int[] {0, 0, 1, 0},
                new int[] {0, 0, 0, 0},
        };
    }

    public void startGame()
    {
        thread.Start();
    }

    private bool checkCollision(Player player, Board board)
    {

        return false;
    }

    public void stateUpdate()
    {
        while (true)
        {
            Thread.Sleep(1000); // tick rate
            foreach (string lobbyID in lobbies.Keys)
            {
                Lobby lobby = lobbies[lobbyID];
                if (lobby.lobbyState == LobbyState.PLAYING)
                {
                    // update board
                    for (int j = 0; j < lobby.players.Count; j++)
                    {
                        if (lobby.players[j].currentBlock == null)
                        {
                            // spawn block
                            lobby.players[j].currentBlock = new Block(data, 5);
                            lobby.players[j].currentBlockPosition = new Tuple<int, int>(5, 5);
                        }
                        else
                        {
                            if (checkCollision(lobby.players[j], lobby.board))
                            {
                                // place block
                                // set player's current block to null
                            }
                            // block falls 1 space
                            lobby.players[j].currentBlockPosition = new Tuple<int, int>(lobby.players[j].currentBlockPosition.Item1 - 1, lobby.players[j].currentBlockPosition.Item2);
                        }
                    }
                    // send game state to all players in lobby
                    for (int j = 0; j < lobby.players.Count; j++)
                    {
                        lobby.players[j].webSocket.Send(JsonConvert.SerializeObject(lobby.game));
                    }
                }
            }
        }
    }
}
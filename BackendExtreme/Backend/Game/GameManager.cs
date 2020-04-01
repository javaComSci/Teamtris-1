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
    private List<Block> blocks;

    private List<List<Block>> allBlocks;
    private int[][] data;
    public GameManager(Dictionary<string, Lobby> lobbies)
    {
        this.lobbies = lobbies;
        thread = new Thread(stateUpdate);
        int[][] b1 = new int[][] {
                new int[] {1, 1, 0, 0},
                new int[] {1, 1, 0, 0},
                new int[] {0, 0, 0, 0},
                new int[] {0, 0, 0, 0},
            };
        int[][] b2 = new int[][] {
                new int[] {1, 1, 0, 0},
                new int[] {1, 1, 0, 0},
                new int[] {0, 0, 0, 0},
                new int[] {0, 0, 0, 0},
            };
        int[][] b3 = new int[][] {
                new int[] {0, 0, 0, 0},
                new int[] {1, 0, 0, 0},
                new int[] {0, 0, 0, 0},
                new int[] {0, 0, 0, 0},
            };

        Block block1 = new Block(b1, 1);
        Block block2 = new Block(b2, 1);
        Block block3 = new Block(b3, 1);

        blocks = new List<Block>();

        blocks.Add(block1);
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
                    foreach (Bot bot in lobby.bots)
                    {
                        Console.WriteLine("making bot move");
                        try
                        {
                            List<List<Tuple<int, int>>> allBobs = bot.GetMove(lobby.game.board, allBlocks);
                            List<Tuple<int, int>> bob = allBobs[0];
                            if (bob == null)
                            {
                                Console.WriteLine("no place to place piece");
                            }
                            foreach (Tuple<int, int> tup in bob)
                            {
                                // lobby.game.board.board[tup.Item1, tup.Item2] = 1;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("bot error");
                        }
                    }
                    // update board
                    for (int j = 0; j < lobby.players.Count; j++)
                    {
                        if (lobby.players[j].currentBlock == null)
                        {
                            // spawn block
                            // lobby.players[j].currentBlock = new Block(data, 5);
                            // lobby.players[j].currentBlockPosition = new Tuple<int, int>(5, 5);
                        }
                        else
                        {
                            if (checkCollision(lobby.players[j], lobby.game.board))
                            {
                                // place block
                                // set player's current block to null
                            }
                            // block falls 1 space
                            // lobby.players[j].currentBlockPosition = new Tuple<int, int>(lobby.players[j].currentBlockPosition.Item1 - 1, lobby.players[j].currentBlockPosition.Item2);
                        }
                    }
                    lobby.game.current_time = DateTime.Now.Millisecond;
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
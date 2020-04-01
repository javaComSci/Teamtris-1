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

    public List<List<Block>> allBlocks;
    private int[][] data;
    public GameManager(Dictionary<string, Lobby> lobbies)
    {
        this.lobbies = lobbies;
        thread = new Thread(stateUpdate);
        List<Block> bot1Blocks = new List<Block>();
        List<Block> bot2Blocks = new List<Block>();
        List<Block> bot3Blocks = new List<Block>();

        int[][] block11 = new int[][] {
            new int[] {0, 0, 1, 0},
            new int[] {0, 0, 1, 0},
            new int[] {0, 0, 1, 0},
            new int[] {0, 0, 1, 0},
        };
        int[][] block21 = new int[][] {
            new int[] {0, 1, 0, 0},
            new int[] {0, 0, 0, 0},
            new int[] {0, 0, 0, 0},
            new int[] {0, 0, 0, 0},
        };
        int[][] block31 = new int[][] {
            new int[] {0, 1, 0, 0},
            new int[] {0, 0, 0, 0},
            new int[] {0, 0, 0, 0},
            new int[] {0, 0, 0, 0},
        };
        bot1Blocks.Add(new Block(block11, 1));
        bot2Blocks.Add(new Block(block21, 1));
        bot3Blocks.Add(new Block(block31, 1));
        allBlocks = new List<List<Block>>();
        allBlocks.Add(bot1Blocks);
        allBlocks.Add(bot2Blocks);
        allBlocks.Add(bot3Blocks);

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
                    Bot bot = lobby.bot;
                    Console.WriteLine("making bot move");

                    List<List<Tuple<int, int>>> allBobs = bot.GetMove(lobby.game.board, allBlocks);
                    List<Tuple<int, int>> bob = allBobs[0];
                    if (bob == null)
                    {
                        Console.WriteLine("no place to place piece");
                    }
                    foreach (Tuple<int, int> tup in bob)
                    {
                        lobby.game.board.board[tup.Item1, tup.Item2] = 1;
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
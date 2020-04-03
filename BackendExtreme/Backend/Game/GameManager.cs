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

    RandomPiece randomPiece;

    public List<List<Block>> allBlocks;
    private int[][] data;
    public GameManager(Dictionary<string, Lobby> lobbies)
    {
        this.lobbies = lobbies;
        thread = new Thread(stateUpdate);
        // List<Block> bot1Blocks = new List<Block>();
        // List<Block> bot2Blocks = new List<Block>();
        // List<Block> bot3Blocks = new List<Block>();

        // int[][] block11 = new int[][] {
        //     new int[] {0, 0, 1, 0},
        //     new int[] {0, 0, 1, 0},
        //     new int[] {0, 0, 1, 0},
        //     new int[] {0, 0, 1, 0},
        // };
        // int[][] block21 = new int[][] {
        //     new int[] {0, 1, 0, 0},
        //     new int[] {0, 0, 0, 0},
        //     new int[] {0, 0, 0, 0},
        //     new int[] {0, 0, 0, 0},
        // };
        // int[][] block31 = new int[][] {
        //     new int[] {0, 1, 0, 0},
        //     new int[] {0, 0, 0, 0},
        //     new int[] {0, 0, 0, 0},
        //     new int[] {0, 0, 0, 0},
        // };
        // bot1Blocks.Add(new Block(block11, 1));
        // bot2Blocks.Add(new Block(block21, 1));
        // bot3Blocks.Add(new Block(block31, 1));
        // allBlocks = new List<List<Block>>();
        // allBlocks.Add(bot1Blocks);
        // allBlocks.Add(bot2Blocks);
        // allBlocks.Add(bot3Blocks);


        List<Block> bot1Blocks = new List<Block>();
        List<Block> bot2Blocks = new List<Block>();
        List<Block> bot3Blocks = new List<Block>();

        randomPiece = new RandomPiece();

        for (int i = 0; i < 100; i++)
        {
            int[][] block11 = randomPiece.GenerateRandomPiece();
            int[][] block21 = randomPiece.GenerateRandomPiece();
            int[][] block31 = randomPiece.GenerateRandomPiece();
            bot1Blocks.Add(new Block(block11, 1));
            bot2Blocks.Add(new Block(block21, 1));
            bot3Blocks.Add(new Block(block31, 1));
        }

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
                    if (lobby.bot != null)
                    {
                        Bot bot = lobby.bot;
                        Console.WriteLine("making bot move");
                        Board modifiedBoard = new Board(lobby.game.board.height, lobby.game.board.width);
                        for (int i = 0; i < lobby.game.board.height; i++)
                        {
                            for (int j = 0; j < lobby.game.board.width; j++)
                            {
                                if (lobby.game.board.board[i, j] >= 1)
                                {
                                    // Console.WriteLine("THE INDEX IS " + i + " " + j + " " + lobby.game.board.board[i, j]);
                                    modifiedBoard.board[i, j] = 1;
                                }
                                else
                                {
                                    modifiedBoard.board[i, j] = 0;
                                }
                            }
                        }

                        Prints botInfoPrinter = new Prints();
                        // Console.WriteLine("BEFORE BOT BOARD");
                        // botInfoPrinter.PrintMultiDimArr(modifiedBoard.board);
                        allBlocks[0].RemoveAt(0);
                        allBlocks[0].Add(new Block(randomPiece.GenerateRandomPiece(), 1));
                        allBlocks[1].RemoveAt(0);
                        allBlocks[1].Add(new Block(randomPiece.GenerateRandomPiece(), 1));
                        allBlocks[2].RemoveAt(0);
                        allBlocks[2].Add(new Block(randomPiece.GenerateRandomPiece(), 1));

                        List<List<Tuple<int, int>>> allBobs = bot.GetMove(modifiedBoard, allBlocks);
                        List<Tuple<int, int>> bob = allBobs[0];
                        if (bob == null)
                        {
                            Console.WriteLine("no place to place piece");
                            return;
                        }
                        else
                        {
                            bool moveValid = true;
                            foreach (Tuple<int, int> tup in bob)
                            {
                                foreach (Player player in lobby.players)
                                {
                                    if (player.currentBlockPosition != null)
                                    {
                                        for (int i = 0; i < player.currentBlockPosition.Length; i++)
                                        {
                                            if (tup.Item1 == player.currentBlockPosition[i][0] && tup.Item2 == player.currentBlockPosition[i][1])
                                            {
                                                moveValid = false;
                                            }
                                        }
                                    }
                                }
                            }
                            if (moveValid)
                            {
                                foreach (Tuple<int, int> tup in bob)
                                {
                                    lobby.game.board.board[tup.Item1, tup.Item2] = 1;
                                }
                            }
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
                    lobby.game.current_time += 1;
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
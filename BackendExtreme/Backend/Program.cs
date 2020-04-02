using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Generic;
using Newtonsoft.Json;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Teamtris
{
    /**
    * #class Program |
    * @author JavaComSci, Colholt | 
    * @language csharp | 
    * @desc TODO |
    */
    class Program
    {
        static void Main(string[] args)
        {

            // // initialize game state
            // GameState game = new GameState(6, 7);
            // game.players = new Dictionary<int, Player>();
            Dictionary<string, Lobby> lobbies = new Dictionary<string, Lobby>();

            // // printing
            Prints infoPrinter = new Prints();
            
            // // CHANGE - RECIEVED FROM THE FRONTEND - REPLACEMENT
            // int numBots = 3;

            // switch(numBots) {
            //     case 1: 
            //         game.bot = new SingleBot();
            //         break;
            //     case 2:
            //         game.bot = new DoubleBot();
            //         break;
            //     case 3:
            //         game.bot = new TripleBot();
            //         break;
            //     default:
            //         game.bot = null;
            //         break;
            // }
            

            // List<Block> bot1Blocks = new List<Block>();
            // List<Block> bot2Blocks = new List<Block>();
            // List<Block> bot3Blocks = new List<Block>();
            // // game.board.board =  new int[,]{
            // //     {0, 0, 0, 0, 0, 0},
            // //     {0, 0, 0, 0, 0, 0},
            // //     {0, 0, 0, 0, 0, 0},
            // //     {0, 0, 0, 0, 0, 1},
            // //     {0, 0, 0, 1, 1, 1},
            // //     {1, 0, 1, 1, 1, 1}
            // // };

            // game.board.board = new int[,]{
            //     {0, 0, 1, 0, 0, 0, 0},
            //     {1, 1, 1, 0, 1, 1, 1},
            //     {1, 0, 1, 1, 1, 1, 1},
            //     {1, 0, 1, 0, 1, 1, 1},
            //     {0, 1, 1, 1, 1, 1, 1},
            //     {1, 0, 1, 1, 1, 1, 1},
            // };
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
            // List<List<Block>> blocks = new List<List<Block>>();
            // blocks.Add(bot1Blocks);
            // blocks.Add(bot2Blocks);
            // blocks.Add(bot3Blocks);

            // game.bot.GetMove(game.board, blocks);
            // try {
            //     game.bot.GetMove(game.board, blocks);
            // } catch (Exception e) {
            //     Console.WriteLine("Recieved error: "  + e.Message);
            // }

            // connection and adding to the db scores
            // List<string> players = new List<string>();
            // players.Add("modified");
            // players.Add("hi");
            // players.Add(null);
            // players.Add(null);
            // ScoresInfo scoresInfo = new ScoresInfo("Team HIHIOWE", players, 1, 6000);
            // SQLConnection.AddTeamScore(scoresInfo);   
            // Tuple<List<ScoresInfo>, ScoresInfo> retrievedInfo = SQLConnection.GetTopTeamsAndCurrentTeam("Team HIHIOWE");
            // Console.WriteLine("Top teams");
            // infoPrinter.PrintScoreList(retrievedInfo.Item1);
            // Console.WriteLine("Current team");
            // infoPrinter.PrintScoreInfo(retrievedInfo.Item2);


            // List<ScoresInfo> retrieved = SQLConnection.GetTopTeams();
            // infoPrinter.PrintScoreList(retrieved);


            // create localhost web socket server on port 5202
            var wssv = new WebSocketServer("ws://0.0.0.0:5202");
            wssv.Start();
            wssv.AddWebSocketService<LobbyManager>("/lobby", () => new LobbyManager(lobbies));
            wssv.AddWebSocketService<Play>("/play", () => new Play(lobbies));
            wssv.AddWebSocketService<ScoresManager>("/scores", () => new ScoresManager());
            wssv.AddWebSocketService<ScoresDirectManager>("/scoresDirect", () => new ScoresDirectManager());
            GameManager gameManager = new GameManager(lobbies);
            Console.WriteLine("Starting to check for sockets");
            // start game broadcasting service
            gameManager.startGame();
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}

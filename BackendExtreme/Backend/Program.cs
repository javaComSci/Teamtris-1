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
    * @author ??? | 
    * @language csharp | 
    * @desc TODO |
    */
    class Program
    {
        static void Main(string[] args)
        {
            // initialize game state
            GameState game = new GameState(6, 6);
            game.players = new Dictionary<int, Player>();
            Dictionary<string, Lobby> lobbies = new Dictionary<string, Lobby>();

            // printing
            Prints infoPrinter = new Prints();

            // currently just have a single bot
            game.bot = new SingleBot();
            List<Block> blocks = new List<Block>();
            // game.board.board =  new int[,]{
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 1},
            //     {0, 0, 0, 1, 1, 1},
            //     {1, 0, 1, 1, 1, 1}
            // };

            // game.board.board = new int[,]{
            //     {0, 0, 0, 0, 0, 0, 1},
            //     {0, 0, 1, 0, 0, 0, 1},
            //     {0, 0, 1, 0, 0, 1, 1},
            //     {0, 0, 1, 0, 0, 1, 1},
            //     {0, 1, 1, 1, 1, 1, 1},
            //     {0, 1, 1, 1, 1, 1, 1},
            // };
            // int[][] data = new int[][] {
            //     new int[] {0, 0, 1, 1}, 
            //     new int[] {0, 0, 1, 1}, 
            //     new int[] {0, 0, 1, 0}, 
            //     new int[] {0, 1, 0, 0}, 
            // };



            // // DEMO USER STORY 13 - a - shows placement as is
            // game.board.board = new int[,]{
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 1, 0, 0, 0},
            //     {0, 0, 1, 0, 0, 1},
            //     {0, 0, 1, 0, 0, 1},
            //     {0, 1, 1, 1, 1, 1},
            //     {0, 1, 1, 1, 1, 1},
            // };
            // int[][] b1 = new int[][] {
            //     new int[] {1, 1, 0, 0},
            //     new int[] {1, 1, 0, 0},
            //     new int[] {1, 0, 0, 0},
            //     new int[] {1, 0, 0, 0},
            // };
            // Block block1 = new Block(b1, 1);
            // blocks.Add(block1);



            // // DEMO USER STORY 13 - b - shows placement with rotation
            // game.board.board = new int[,]{
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 1},
            //     {1, 0, 0, 0, 0, 1},
            //     {1, 0, 0, 1, 1, 1},
            //     {1, 0, 1, 1, 1, 1},
            // };
            // int[][] b1 = new int[][] {
            //     new int[] {1, 1, 1, 0},
            //     new int[] {0, 1, 0, 0},
            //     new int[] {0, 0, 0, 0},
            //     new int[] {0, 0, 0, 0},
            // };
            // Block block1 = new Block(b1, 1);
            // blocks.Add(block1);
            


            // // DEMO USER STORY 13 - c - shows placement with area covered
            // game.board.board = new int[,]{
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 1},
            //     {1, 0, 0, 1, 0, 1},
            //     {1, 0, 0, 1, 1, 1},
            //     {1, 0, 1, 1, 0, 1},
            // };
            // int[][] b1 = new int[][] {
            //     new int[] {0, 1, 0, 0},
            //     new int[] {0, 1, 0, 0},
            //     new int[] {0, 1, 0, 0},
            //     new int[] {0, 0, 0, 0},
            // };
            // Block block1 = new Block(b1, 1);
            // blocks.Add(block1);


            // // DEMO USER STORY 13 - d - fit piece on board with invalid piece
            // game.board.board = new int[,]{
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 1},
            //     {1, 0, 0, 1, 0, 1},
            //     {1, 0, 0, 1, 1, 1},
            //     {1, 0, 1, 1, 0, 1},
            // };
            // int[][] b1 = new int[][] {
            //     new int[] {0, 1, 0, 0},
            //     new int[] {0, 0, 1, 0},
            //     new int[] {0, 0, 0, 0},
            //     new int[] {0, 0, 0, 0},
            // };
            // Block block1 = new Block(b1, 1);
            // blocks.Add(block1);


            // // DEMO USER STORY 15 - 1 shape ahead
            // game.board.board = new int[,]{
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 1, 0, 0, 0},
            //     {0, 0, 1, 0, 0, 1},
            //     {0, 0, 1, 0, 0, 1},
            //     {0, 1, 1, 1, 1, 1},
            //     {0, 1, 1, 1, 1, 1},
            // };
            // int[][] b1 = new int[][] {
            //     new int[] {1, 1, 0, 0},
            //     new int[] {1, 1, 0, 0},
            //     new int[] {0, 0, 0, 0},
            //     new int[] {0, 0, 0, 0},
            // };
            // int[][] b2 = new int[][] {
            //     new int[] {1, 1, 0, 0},
            //     new int[] {1, 1, 0, 0},
            //     new int[] {1, 0, 0, 0},
            //     new int[] {1, 0, 0, 0},
            // };
            // Block block1 = new Block(b1, 1);
            // Block block2 = new Block(b2, 1);
            // blocks.Add(block1);
            // blocks.Add(block2);


            // DEMO USER STORY 15 - 2 shapes ahead
            game.board.board = new int[,]{
                {0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0},
                {0, 0, 1, 0, 0, 1},
                {0, 0, 1, 0, 0, 1},
                {0, 1, 1, 1, 1, 1},
                {0, 1, 1, 1, 1, 1},
            };
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
            blocks.Add(block1);
            // blocks.Add(block2);
            // blocks.Add(block3);

            try {
                game.bot.GetMove(game.board, blocks);
            } catch (Exception e) {
                Console.WriteLine("Recieved error: "  + e.Message);
            }

            // connection and adding to the db scores
            List<string> players = new List<string>();
            players.Add("p1");
            players.Add("p2");
            players.Add(null);
            players.Add(null);
            ScoresInfo scoresInfo = new ScoresInfo("Team HI", players, 1, 60);
            long id = SQLConnection.AddTeamScore(scoresInfo);   
            Tuple<List<ScoresInfo>, ScoresInfo> retrievedInfo = SQLConnection.GetTopTeamsAndCurrentTeam(id);
            Console.WriteLine("Top teams");
            infoPrinter.PrintScoreList(retrievedInfo.Item1);
            Console.WriteLine("Current team");
            infoPrinter.PrintScoreInfo(retrievedInfo.Item2);



            // create localhost web socket server on port 5202
            var wssv = new WebSocketServer("ws://0.0.0.0:5202");
            wssv.Start();
            wssv.AddWebSocketService<LobbyManager>("/lobby", () => new LobbyManager(lobbies));
            wssv.AddWebSocketService<Play>("/play", () => new Play(lobbies));
            GameManager gameManager = new GameManager(lobbies);
            Console.WriteLine("Starting to check for sockets");
            // start game broadcasting service
            gameManager.startGame();
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}

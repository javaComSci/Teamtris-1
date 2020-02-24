﻿using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Teamtris
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize game state
            GameState game = new GameState(6, 6);
            game.players = new Dictionary<int, Player>();
            Dictionary<string, Lobby> lobbies = new Dictionary<string, Lobby>();

            // currently just have a single bot
            game.bot = new SingleBot();
            // game.board.board =  new int[,]{
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 0},
            //     {0, 0, 0, 0, 0, 1},
            //     {0, 0, 0, 1, 1, 1},
            //     {1, 0, 1, 1, 1, 1}
            // };
            game.board.board = new int[,]{
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {1, 0, 0, 1, 0, 1},
                {1, 0, 0, 1, 1, 1},
                {1, 0, 1, 1, 1, 1}
            };
            // int[][] data = new int[][] {
            //     new int[] {0, 0, 1, 1}, 
            //     new int[] {0, 0, 1, 1}, 
            //     new int[] {0, 0, 1, 0}, 
            //     new int[] {0, 1, 0, 0}, 
            // };
            int[][] data = new int[][] {
                new int[] {0, 0, 1, 0},
                new int[] {0, 0, 1, 0},
                new int[] {0, 0, 1, 0},
                new int[] {0, 0, 0, 0},
            };
            Block block = new Block(data, 1);
            List<Block> blocks = new List<Block>();
            blocks.Add(block);
            game.bot.GetMove(game.board, blocks);


            // create localhost web socket server on port 5202
            var wssv = new WebSocketServer("ws://0.0.0.0:5202");
            wssv.Start();
            wssv.AddWebSocketService<LobbyManager>("/lobby", () => new LobbyManager(lobbies));

            // Gonna need something like this for when a player clicks "Create token"
            // wssv.AddWebSocketService<Play>("/creategame", () => new Game(game));

            Console.WriteLine("Starting to check for sockets");
            // create thread to broadcast message every x milliseconds
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    foreach (string lobbyID in lobbies.Keys)
                    {
                        Lobby lobby = lobbies[lobbyID];
                        if (lobby.lobbyState == LobbyState.PLAYING)
                        {
                            for (int j = 0; j < lobby.players.Count; j++)
                            {
                                lobby.players[j].webSocket.Send(JsonConvert.SerializeObject(game));
                            }
                        }
                    }
                }
            });

            // thread.Start();
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}

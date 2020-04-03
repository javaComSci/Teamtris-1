using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Generic;
using Newtonsoft.Json;
using MySql.Data;
using MySql.Data.MySqlClient;


public class RandomPiece {

    public int GetSquarePriority(int i, int j, int[][] piece) {
        // Console.WriteLine("HERE WITH " + i + " " + j);
        if(i < 0 || i >= 4 || j < 0 || j >= 4) {
            return 0;
        }

        if(piece[i][j] == 1){
            return 0;
        }

        Random random = new Random();
        return random.Next(0,2);
    }

    public int[][] GenerateRandomPiece(){
        Prints botInfoPrinter = new Prints();
        int[][] piece = new int[][] {
                new int[] {0, 0, 0, 0}, 
                new int[] {0, 0, 0, 0}, 
                new int[] {0, 0, 0, 0}, 
                new int[] {0, 0, 0, 0}, 
            };
        int[] gen = {2,3,4};

        Random random = new Random();
        int startIndex = random.Next(0, gen.Length);
        // Console.WriteLine("THE INDEX IS " + startIndex);
        int count = gen[startIndex];
        // Console.WriteLine("COUNT IS " + count);

        int i = random.Next(0, 4);
        int j = random.Next(0, 4);

        piece[i][j] = 1;

        count = count - 1;

        Dictionary<string, Tuple<int, int, int>> values = new Dictionary<string, Tuple<int, int, int>>();

        while(count > 0) {
            values["left"] = new Tuple<int, int, int>(GetSquarePriority(i, j - 1, piece), i, j - 1);
            values["right"] = new Tuple<int, int, int>(GetSquarePriority(i, j + 1, piece), i, j + 1);
            values["up"] = new Tuple<int, int, int>(GetSquarePriority(i - 1, j, piece), i - 1, j);
            values["down"] = new Tuple<int, int, int>(GetSquarePriority(i + 1, j, piece), i + 1, j);
            var maxVal = -1;
            Tuple<int, int, int> bestVal = null;
            foreach(string v in values.Keys) {
                if(maxVal == -1) {
                    maxVal = values[v].Item1;
                    bestVal = values[v];
                }
                if(values[v].Item1 >= maxVal) {
                    // Console.WriteLine("HERE " + maxVal + " " + values[v].Item2);
                    maxVal = values[v].Item1;
                    bestVal = values[v];
                }
            }
            if(maxVal == 1) {
                // Console.WriteLine("MAX VAL " + maxVal);
                piece[bestVal.Item2][bestVal.Item3] = 1;
                count = count - 1;
            } else {
                break;
            }
        }
        // Console.WriteLine("PIECE ");
        // botInfoPrinter.PrintJaggedArr(piece);
        return piece;   
    }
}
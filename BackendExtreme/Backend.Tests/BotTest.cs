using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        private GameState game;
        private List<Block> blocks;


        /* 
         Setup the board and pieces for testing
         */
        [SetUp]
        public void Setup()
        {
            game = new GameState(6, 6);
            game.bot = new SingleBot();
            game.board.board =  new int[,]{
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {1, 0, 0, 1, 0, 1},
                {1, 1, 0, 1, 1, 1},
                {1, 0, 1, 1, 1, 1}
            };

            int[][] b1 = new int[][] {
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 1, 0}, 
            };

            int[][] b2 = new int[][] {
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 0, 0}, 
            };

            Block block1 = new Block(b1, 1);
            Block block2 = new Block(b2, 1);
            blocks = new List<Block>();
            blocks.Add(block1);
            blocks.Add(block2);
        }

    /*
        Assert that a move can be taken without erroring
     */
    [Test]
        public void MakeMoves() {
            try {
                foreach(Block block in blocks) {
                    List<Block> newBlocks = new List<Block>();
                    newBlocks.Add(block);
                    game.bot.GetMove(game.board, newBlocks); 
                }
            } catch (Exception e) {
                Assert.Fail("Expected no exception " + e.Message);
            }        
        }
    }
}
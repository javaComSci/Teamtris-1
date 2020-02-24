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
        Prints botInfoPrinter;

        /* 
         Setup the board and pieces for testing
         */
        [SetUp]
        public void Setup()
        {
            botInfoPrinter = new Prints();

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

             int[][] b3 = new int[][] {
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 0, 1, 1}, 
                new int[] {0, 0, 1, 0}, 
            };

            int[][] b4 = new int[][] {
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 1, 1, 0}, 
                new int[] {0, 0, 1, 1}, 
                new int[] {0, 0, 1, 0}, 
            };

            int[][] b5 = new int[][] {
                new int[] {0, 1, 1, 0}, 
                new int[] {0, 1, 1, 0}, 
                new int[] {0, 0, 1, 1}, 
                new int[] {0, 0, 1, 0}, 
            };

            int[][] b6 = new int[][] {
                new int[] {0, 1, 1, 0}, 
                new int[] {0, 1, 1, 0}, 
                new int[] {0, 0, 0, 0}, 
                new int[] {0, 0, 0, 0}, 
            };

            int[][] b7 = new int[][] {
                new int[] {0, 0, 1, 0}, 
                new int[] {0, 1, 1, 1}, 
                new int[] {0, 0, 0, 0}, 
                new int[] {0, 0, 0, 0}, 
            };

            int[][] b8 = new int[][] {
                new int[] {0, 1, 1, 0}, 
                new int[] {0, 1, 1, 1}, 
                new int[] {0, 0, 0, 0}, 
                new int[] {0, 0, 0, 0}, 
            };

            int[][] b9 = new int[][] {
                new int[] {0, 1, 1, 1}, 
                new int[] {0, 1, 1, 1}, 
                new int[] {0, 0, 0, 0}, 
                new int[] {0, 0, 0, 0}, 
            };

            Block block1 = new Block(b1, 1);
            Block block2 = new Block(b2, 1);
            Block block3 = new Block(b3, 1);
            Block block4 = new Block(b4, 1);
            Block block5 = new Block(b5, 1);
            Block block6 = new Block(b6, 1);
            Block block7 = new Block(b7, 1);
            Block block8 = new Block(b8, 1);
            Block block9 = new Block(b9, 1);

            blocks = new List<Block>();

            blocks.Add(block1);
            blocks.Add(block2);
            blocks.Add(block3);
            blocks.Add(block4);
            blocks.Add(block5);
            blocks.Add(block6);
            blocks.Add(block7);
            blocks.Add(block8);
            blocks.Add(block9);
        }


        /*
            Helper for placing the pieces on the  board
         */
        public Tuple<int[,], int[,]> PlacePieceOnBoard(int [,] board, List<Tuple<int, int>> dots) {
            int[,] newBoard = new int[board.GetLength(0),board.GetLength(1)];

            for(int i = 0; i < board.GetLength(0); i++){
                for(int j = 0; j < board.GetLength(1); j++){
                    newBoard[i,j] = board[i,j];
                }
            }

            foreach(Tuple<int, int> dot in dots) {
                board[dot.Item1, dot.Item2] = 1;
                newBoard[dot.Item1, dot.Item2] = 2;
            }

            
            Tuple<int[,], int[,]> allBoards = new Tuple<int[,], int[,]>(board, newBoard);
            return allBoards;
        }


        /*
            Assert that a move can be taken without erroring for trying multiple different pieces
        */
        [Test]
        public void MakeDiffPieceMoves() {
            TestContext.Progress.WriteLine("\n\n\n\n\n--------------------------------------MAKE DIFF PIECE MOVES--------------------------------------");
            try {
                foreach(Block block in blocks) {
                    TestContext.Progress.WriteLine("--------------------------------------");
                    List<Block> newBlocks = new List<Block>();
                    newBlocks.Add(block);
                    TestContext.Progress.Write("Piece to be placed");
                    botInfoPrinter.PrintJaggedArr(block.data, false);
                    List<Tuple<int, int>> piecePlaced = game.bot.GetMove(game.board, newBlocks); 
                    TestContext.Progress.Write("Board with piece");
                    botInfoPrinter.PrintBoardWithPiece(game.board.board, piecePlaced, false);
                    TestContext.Progress.WriteLine("--------------------------------------");
                }
            } catch (Exception e) {
                Assert.Fail("Expected no exception recieved " + e.Message);
            }        
        }


        /*
            Assert that a move can be taken without erroring when same piece placed twice on the board
        */
        [Test]
        public void MakeMoveWithSamePieceTwice() {
            TestContext.Progress.WriteLine("\n\n\n\n\n--------------------------------------MOVE WITH SAME PIECE TWICE--------------------------------------");
            try {
                List<Block> newBlocks = new List<Block>();
                newBlocks.Add(blocks[0]);
                TestContext.Progress.WriteLine("--------------------------------------");
                TestContext.Progress.Write("Piece to be placed");
                botInfoPrinter.PrintJaggedArr(newBlocks[0].data, false);

                // point it to new board
                game.board.board = new int[,]{
                    {0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0},
                    {1, 0, 0, 1, 0, 1},
                    {1, 1, 0, 1, 1, 1},
                    {1, 0, 1, 1, 1, 1}
                };

                for(int i = 0; i < 2; i++) {
                    List<Tuple<int, int>> piecePlaced = game.bot.GetMove(game.board, newBlocks); 

                    Tuple<int[,], int[,]> allBoards = PlacePieceOnBoard(game.board.board, piecePlaced);

                    game.board.board = allBoards.Item1;
                    
                    TestContext.Progress.Write("Board after piece placed");
                    botInfoPrinter.PrintMultiDimArr(allBoards.Item2, false);
                }
                TestContext.Progress.WriteLine("--------------------------------------");

                // point back to old board
                game.board.board = new int[,]{
                    {0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0},
                    {1, 0, 0, 1, 0, 1},
                    {1, 1, 0, 1, 1, 1},
                    {1, 0, 1, 1, 1, 1}
                };
            } catch (Exception e) {
                Assert.Fail("Expected no exception but recieved" + e.Message);
            }        
        }
    }

}
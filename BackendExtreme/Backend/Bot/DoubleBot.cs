using System;
using System.Collections.Generic;
using System.Linq;


/**
 * #class DoubleBot |
 * @author JavaComSci | 
 * @language csharp | 
 * @desc DoubleBot bot extends the abstract bot class defined here:
 * @link{BotDefVar}
 * The DoubleBot class will be made if the player requires two
 * bots in their game. |
 */
public class DoubleBot : Bot {
    Prints botInfoPrinter;

    public DoubleBot() {
        // create a board for this bot
        Console.WriteLine("I AM A SINGLE BOT");
        botInfoPrinter = new Prints();
    }

    public List<Tuple<int [][], int [][]>> GetAllOrientations(Block bot1Block, Block bot2Block) {
        // contains all the orientaitons of the rotated blocks
        List<Tuple<int [][], int [][]>> allOrientations = new List<Tuple<int [][], int [][]>>();

        // try all orientations of each of the blocks
        for(int i = 0; i < 3; i++) {
            bot1Block.data = bot1Block.RotateMatrix();
            for(int j = 0; j < 3; j++) {
                bot2Block.data = bot2Block.RotateMatrix();
                allOrientations.Add(new Tuple<int [][], int [][]>(bot1Block.data, bot2Block.data));
            }
            bot2Block.data = bot2Block.RotateMatrix();
        }

        return allOrientations;
    }

    public override List<Tuple<int, int>> GetMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false) {
        // get each of the bot's blocks
        List<Block> bot1Blocks = allBotBlocks[0];
        List<Block> bot2Blocks = allBotBlocks[1];

        // print the information
        Console.WriteLine("BOARD");
        botInfoPrinter.PrintMultiDimArr(board.board);
        Console.WriteLine("PIECE 1 BOT 1");
        botInfoPrinter.PrintJaggedArr(bot1Blocks[0].data);
        Console.WriteLine("PIECE 2 BOT 2");
        botInfoPrinter.PrintJaggedArr(bot2Blocks[0].data);

        // check to make sure that each block is valid
        bool blockValid1 = bot1Blocks[0].CheckValidity();
        bool blockValid2 = bot2Blocks[0].CheckValidity();
        // block is invalid
        if(!blockValid1 || !blockValid2) {
            throw new Exception("Shape formation is incorrect");
        }

        // all orientations of the 2 blocks
        List<Tuple<int [][], int [][]>> allOrientations = GetAllOrientations(bot1Blocks[0], bot2Blocks[0]);

        // place bot 2 piece then bot 1 piece
        // GetFit(board, bot1Blocks[0], bot2Blocks[0]);
        
        return null;
    }
}
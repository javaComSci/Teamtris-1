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

    public List<Tuple<Block, Block>> GetAllOrientations(Block bot1Block, Block bot2Block) {
        // contains all the orientaitons of the rotated blocks
        BlockEqualityComparer blockEqualityComparer = new BlockEqualityComparer();
        List<Tuple<Block, Block>> allOrientations = new List<Tuple<Block, Block>>();

        // orientations of each block
        HashSet<Block> bot1BlockOrientations = new HashSet<Block>(blockEqualityComparer);
        HashSet<Block> bot2BlockOrientations = new HashSet<Block>(blockEqualityComparer);

        Console.WriteLine("PIECE 1 BOT 1");
        botInfoPrinter.PrintJaggedArr(bot1Block.data);
        Console.WriteLine("PIECE 2 BOT 2");
        botInfoPrinter.PrintJaggedArr(bot2Block.data);

        for(int i = 0; i < 3; i++) {
            bot1Block.data = bot1Block.RotateMatrix();
            Block newBot1Block = new Block(bot1Block.data.Select(s => s.ToArray()).ToArray(), bot1Block.color);
            bot1BlockOrientations.Add(newBot1Block);

            bot2Block.data = bot2Block.RotateMatrix();
            Block newBot2Block = new Block(bot2Block.data.Select(s => s.ToArray()).ToArray(), bot2Block.color);
            bot2BlockOrientations.Add(newBot2Block);            
        }

        Console.WriteLine("BLOCKS " + bot1BlockOrientations.Count);
        allOrientations = (from bot1List in bot1BlockOrientations
								from bot2List in bot2BlockOrientations
								select new Tuple<Block, Block>(bot1List, bot2List)).ToList();

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
        List<Tuple<Block, Block>> allOrientations = GetAllOrientations(bot1Blocks[0], bot2Blocks[0]);
        Console.WriteLine("ALL ORIENTATIONS");
        botInfoPrinter.PrintAllOrientations(allOrientations);


        // place bot 2 piece then bot 1 piece
        // GetFit(board, bot1Blocks[0], bot2Blocks[0]);
        
        return null;
    }
}
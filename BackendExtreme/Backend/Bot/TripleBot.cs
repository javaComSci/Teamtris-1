using System;
using System.Collections.Generic;
using System.Linq;

/**
 * #class TripleBot |
 * @author JavaComSci | 
 * @language csharp | 
 * @desc TripleBot extends the abstract bot class defined here:
 * @link{BotDefVar}
 * The TripleBot class will be made if the player requires three 
 * bots in their game. |
 */
public class TripleBot : Bot {
    Prints botInfoPrinter;

    public TripleBot() {
        // create a board for this bot
        Console.WriteLine("I AM A TRIPLE BOT");
        botInfoPrinter = new Prints();
    }


     /**
     * #function Triplebot::GetAllOrientations |
     * @author JavaComSci |
	 * @desc gets all the orientations of the blocks that are possible |
     * @header public List<Tuple<Block, Block, int>> GetAllOrientations(Block bot1Block, Block bot2Block) | 
	 * @param Block bot1Block: first block |
     * @param Block bot2Block: second block |
     * @param Block bot3Block: third block |
	 * @returns List<Tuple<Block, Block, int>> : contains the list of the orientations of the blocks |
	 */
    public List<Tuple<Block, Block, int>> GetAllOrientations(Block bot1Block, Block bot2Block, Block bot3Block) {
        return null;
    }

    public override List<Tuple<int, int>> GetMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false) {
        return null;
    }
}



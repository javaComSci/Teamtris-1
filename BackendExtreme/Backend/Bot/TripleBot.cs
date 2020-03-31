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
     * #function Triplebot::CheckBlockValididty |
     * @author JavaComSci |
	 * @desc check valididty of blocks |
     * @header public void CheckBlockValididty(List<List<Block>> allBotBlocks) | 
	 * @param List<List<Block>> allBotBlocks: List of all the blocks
	 * @returns void : checked valididty |
	 */
    public void CheckBlockValididty(List<List<Block>> allBotBlocks) {
        // check that there are 3 blocks
        if((allBotBlocks.Count != 3 || allBotBlocks[0] == null || allBotBlocks[1] == null || allBotBlocks[2] == null)){
            throw new Exception("Not enough blocks");
        }
        // check to make sure that each block is valid
        bool blockValid1 = allBotBlocks[0][0].CheckValidity();
        // block is invalid
        if(!blockValid1) {
            throw new Exception("Shape formation is incorrect");
        }
        bool blockValid2 = allBotBlocks[1][0].CheckValidity();
        // block is invalid
        if(!blockValid2) {
            throw new Exception("Shape formation is incorrect");
        }
        bool blockValid3 = allBotBlocks[2][0].CheckValidity();
        // block is invalid
        if(!blockValid3) {
            throw new Exception("Shape formation is incorrect");
        }
    }


     /**
     * #function Triplebot::GetAllOrientations |
     * @author JavaComSci |
	 * @desc gets all the orientations of the blocks that are possible |
     * @header public List<Tuple<Block, Block, Block>> GetAllOrientations(Block bot1Block, Block bot2Block, Block bot3Block) | 
	 * @param Block bot1Block: first block |
     * @param Block bot2Block: second block |
     * @param Block bot3Block: third block |
	 * @returns List<Tuple<Block, Block, int>> : contains the list of the orientations of the blocks |
	 */
    public List<Tuple<Block, Block, Block>> GetAllOrientations(Block bot1Block, Block bot2Block, Block bot3Block) {
        List<Block> blockOrientationsForBot1 = new List<Block>();
        List<Block> blockOrientationsForBot2 = new List<Block>();
        List<Block> blockOrientationsForBot3 = new List<Block>();

        Console.WriteLine("PIECE 1 BOT 1");
        botInfoPrinter.PrintJaggedArr(bot1Block.data);
        Console.WriteLine("PIECE 1 BOT 2");
        botInfoPrinter.PrintJaggedArr(bot2Block.data);
        Console.WriteLine("PIECE 1 BOT 3");
        botInfoPrinter.PrintJaggedArr(bot3Block.data);

        for(int i = 0; i < 4; i++) {
            bot1Block.data = bot1Block.RotateMatrix();
            Block newBot1Block = new Block(bot1Block.data.Select(s => s.ToArray()).ToArray(), bot1Block.color);
            newBot1Block.ShiftDataBottom();
            bool bot1BlockExists = false;
            for(int j = 0; j < blockOrientationsForBot1.Count; j++) {
                if(newBot1Block.Equals(blockOrientationsForBot1[j])){
                    bot1BlockExists = true;
                }
            }
            if(!bot1BlockExists){
                blockOrientationsForBot1.Add(newBot1Block);
            }

            bot2Block.data = bot2Block.RotateMatrix();
            Block newBot2Block = new Block(bot2Block.data.Select(s => s.ToArray()).ToArray(), bot2Block.color);
            newBot2Block.ShiftDataBottom();
            bool bot2BlockExists = false;
            for(int j = 0; j < blockOrientationsForBot2.Count; j++) {
               if(newBot2Block.Equals(blockOrientationsForBot2[j])){
                    bot2BlockExists = true;
                }
            }
            if(!bot2BlockExists){
                blockOrientationsForBot2.Add(newBot2Block);
            }


            bot3Block.data = bot3Block.RotateMatrix();
            Block newBot3Block = new Block(bot3Block.data.Select(s => s.ToArray()).ToArray(), bot3Block.color);
            newBot3Block.ShiftDataBottom();
            bool bot3BlockExists = false;
            for(int j = 0; j < blockOrientationsForBot3.Count; j++) {
               if(newBot3Block.Equals(blockOrientationsForBot3[j])){
                    bot3BlockExists = true;
                }
            }
            if(!bot3BlockExists){
                blockOrientationsForBot3.Add(newBot3Block);
            }
        }

        List<Tuple<Block, Block, Block>> allOrientations = (from block1 in blockOrientationsForBot1
                                   from block2 in blockOrientationsForBot2
                                   from block3 in blockOrientationsForBot3
                                   select new Tuple<Block, Block, Block>(block1, block2, block3)).ToList();
        Console.WriteLine("NUMBER OF ORIENTATIONS " + allOrientations.Count);
        return allOrientations;
    }


    /**
     * #function Triplebot::GenerateAllOrientations |
     * @author JavaComSci |
	 * @desc gets all the orientations of the blocks that are possible |
     * @header public List<Tuple<Block, Block, Block>> GetAllOrientations(Block bot1Block, Block bot2Block, Block bot3Block) | 
	 * @param List<List<Block>> allBotBlocks: List of all the blocks
	 * @returns List<Tuple<Block, Block, int>> : contains the list of the orientations of the blocks |
	 */
    public List<Tuple<Block, Block, Block>> GenerateAllOrientations(List<List<Block>> allBotBlocks) {
       // generate all combinations of orientations
        List<Tuple<int, int, int>> combos = new List<Tuple<int, int, int>>();
        combos.Add(Tuple.Create(0,1,2));
        combos.Add(Tuple.Create(0,2,1));
        combos.Add(Tuple.Create(1,0,2));
        combos.Add(Tuple.Create(1,2,0));
        combos.Add(Tuple.Create(2,1,0));
        combos.Add(Tuple.Create(2,0,1));

        List<Tuple<Block, Block, Block>> allOrientations = new List<Tuple<Block, Block, Block>>();

        foreach(Tuple<int, int,int> combo in combos){
            allOrientations.AddRange(GetAllOrientations(allBotBlocks[combo.Item1][0],allBotBlocks[combo.Item2][0], allBotBlocks[combo.Item3][0]));
        }

        return allOrientations;
    }


     /**
     * #function TripleBot::GetFitThreeBlocks |
     * @author JavaComSci |
	 * @desc gets the fit of a both block onboard |
     * @header publicList<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> GetFitThreeBlocks(Board board, List<Tuple<Block, Block, Block>> blocksWithOrientations) | 
	 * @param Board board: board to do placing on |
     * @param List<Tuple<Block, Block, Block>> blocksWithOrientations: blockss to be placed|
	 * @returns List<CompatiblePiece, CompatiblePiece, CompatiblePiece> : contains list of compatible positions for both blocks |
	 */
    public List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> GetFitThreeBlocks(Board board, List<Tuple<Block, Block, Block>> blocksWithOrientations) {
        // list of all the compatible pieces that are on the board
        List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> allCompatiblePieces = new List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>>();

        foreach(Tuple<Block, Block, Block> blockWithOrientation in blocksWithOrientations) {
            // get the fit of the current board given this first piece and orientation
            List<CompatiblePiece> compatibleFirstPieces = GetFit(board, blockWithOrientation.Item1);

            foreach(CompatiblePiece compatibleFirstPiece in compatibleFirstPieces) {
                // set up next board for next piece
                Board boardCopy = new Board(board.board.GetLength(0), board.board.GetLength(1));
                boardCopy.board = board.CopyBoard(board.board);
                boardCopy.board = board.FillBoardWithPiece(boardCopy.board, compatibleFirstPiece.locationOnBoard);
                boardCopy.FindMaxHeights();

                // get the next piece on the board
                List<CompatiblePiece> compatibleSecondPieces = GetFit(boardCopy, blockWithOrientation.Item2);

                foreach(CompatiblePiece compatibleSecondPiece in compatibleSecondPieces) {
                    // set up next board for next piece
                    Board boardCopyCopy = new Board(boardCopy.board.GetLength(0), boardCopy.board.GetLength(1));
                    boardCopyCopy.board = boardCopy.CopyBoard(boardCopy.board);
                    boardCopyCopy.board = boardCopy.FillBoardWithPiece(boardCopyCopy.board, compatibleSecondPiece.locationOnBoard);
                    boardCopyCopy.FindMaxHeights();

                    // get the next piece on the board
                    List<CompatiblePiece> compatibleThirdPieces = GetFit(boardCopyCopy, blockWithOrientation.Item3);

                    foreach(CompatiblePiece compatibleThirdPiece in compatibleThirdPieces){
                        allCompatiblePieces.Add(Tuple.Create<CompatiblePiece, CompatiblePiece, CompatiblePiece>(compatibleFirstPiece, compatibleSecondPiece, compatibleThirdPiece));
                    }
                }

            }
        }
        
        // Console.WriteLine("ALL COMPATIBLE PIECES ON BOARD");
        // botInfoPrinter.PrintAllCompatiblePiecesTriple(board.board, allCompatiblePieces);
        return allCompatiblePieces;
    }


    /**
     * #function DoubleBot::GetbestFit |
     * @author JavaComSci |
	 * @desc gets the fit of a all three block onboard |
     * @header public Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece> GetBestFit(List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> allCompatiblePieces)  | 
	 * @param List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> allCompatiblePieces: all the pieces to find best fit for|
	 * @returns List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> : contains position for three blocks |
	 */
     public Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece> GetBestFit(List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> allCompatiblePieces) {
        List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> ordered = allCompatiblePieces.OrderBy(t => t.Item3).ThenBy(t => t.Item2).ThenBy(t => t.Item1).ToList();
        Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece> bestPiece = ordered[0];
        return bestPiece;
     }


     /**
     * #function TripleBot::GetMove |
     * @author JavaComSci |
	 * @desc gets the fit of a three block onboard |
     * @header public override List<Tuple<int, int>> GetMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false)  | 
	 * @param Board board: board to do placing on |
     * @param List<List<Block>> allBotBlocks: blocks to be placed|
	 * @returns List<List<Tuple<int, int>>> : contains position for both blocks |
	 */
    public override List<List<Tuple<int, int>>> GetMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false) {
        
        try {
            CheckBlockValididty(allBotBlocks);
        } catch (Exception e) {
            throw e;
        }
        
        // get each of the bot's blocks
        List<Block> bot1Blocks = allBotBlocks[0];
        List<Block> bot2Blocks = allBotBlocks[1];
        List<Block> bot3Blocks = allBotBlocks[2];

        // get the max height of each column of the baord 
        board.FindMaxHeights();

        // print the information
        Console.WriteLine("BOARD");
        botInfoPrinter.PrintMultiDimArr(board.board);
        Console.WriteLine("PIECE 1 BOT 1");
        botInfoPrinter.PrintJaggedArr(bot1Blocks[0].data);
        Console.WriteLine("PIECE 1 BOT 2");
        botInfoPrinter.PrintJaggedArr(bot2Blocks[0].data);
        Console.WriteLine("PIECE 1 BOT 3");
        botInfoPrinter.PrintJaggedArr(bot3Blocks[0].data);
        
        // get all the orientations
        List<Tuple<Block, Block, Block>> allOrientations = GenerateAllOrientations(allBotBlocks);
        if(allOrientations == null){
            return null;
        }
        // Console.WriteLine("ALL ORIENTATIONS");
        botInfoPrinter.PrintAllOrientationsThreeBlocksAsList(allOrientations);

        // get fit three blocks
        List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> allCompatiblePieces = GetFitThreeBlocks(board, allOrientations);
        if(allCompatiblePieces == null || allCompatiblePieces.Count == 0) {
            return null;
        }
        Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece> bestPieces = GetBestFit(allCompatiblePieces);
         if(bestPieces == null || bestPieces.Item1 == null || bestPieces.Item2 == null || bestPieces.Item3 == null){
            return null;
        }

        // printing the info
        Console.WriteLine("BEST COMPATIBLE PIECES ON BOARD");
        List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>> bestPiecesList = new List<Tuple<CompatiblePiece, CompatiblePiece, CompatiblePiece>>();
        bestPiecesList.Add(bestPieces);
        botInfoPrinter.PrintAllCompatiblePieces(board.board, bestPiecesList);


        // return setup
        List<Tuple<int, int>> compatiblePiece1 = bestPieces.Item1.locationOnBoard;
        List<Tuple<int, int>> compatiblePiece2 = bestPieces.Item2.locationOnBoard;
        List<Tuple<int, int>> compatiblePiece3 = bestPieces.Item3.locationOnBoard;
        List<List<Tuple<int, int>>> allMoves = new List<List<Tuple<int, int>>>();
        allMoves.Add(compatiblePiece1);
        allMoves.Add(compatiblePiece2);
        allMoves.Add(compatiblePiece3);
        if(compatiblePiece1 == null || compatiblePiece2 == null || compatiblePiece3 == null) {
            return null;
        }
        return allMoves;
    }

    public override List<Tuple<int, int>> GetSingleMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false){
       return GetMove(board, allBotBlocks, allRotations)[0];
    }
}



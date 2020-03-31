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
        Console.WriteLine("I AM A DOUBLE BOT");
        botInfoPrinter = new Prints();
    }

    /**
     * #function DoubleBot::GetAllOrientations |
     * @author JavaComSci |
	 * @desc gets all the orientations of the blocks that are possible |
     * @header public List<Tuple<Block, Block, int>> GetAllOrientations(Block bot1Block, Block bot2Block) | 
	 * @param Block bot1Block: first block |
     * @param Block bot2Block: second block |
	 * @returns List<Tuple<Block, Block, int>> : contains the list of the orientations of the blocks |
	 */
    public List<Tuple<Block, Block, int>> GetAllOrientations(Block bot1Block, Block bot2Block) {
        // contains all the orientations of the rotated blocks
        BlockEqualityComparer blockEqualityComparer = new BlockEqualityComparer();
        List<Tuple<Block, Block, int>> allOrientationsBlock1Block2 = new List<Tuple<Block, Block, int>>();
        List<Tuple<Block, Block, int>> allOrientationsBlock2Block1 = new List<Tuple<Block, Block, int>>();

        // orientations of each block
        List<Block> blockOrientationsForBot1 = new List<Block>();
        List<Block> blockOrientationsForBot2 = new List<Block>();

        Console.WriteLine("PIECE 1 BOT 1");
        botInfoPrinter.PrintJaggedArr(bot1Block.data);
        Console.WriteLine("PIECE 2 BOT 2");
        botInfoPrinter.PrintJaggedArr(bot2Block.data);

        for(int i = 0; i < 4; i++) {
            bot1Block.data = bot1Block.RotateMatrix();
            Block newBot1Block = new Block(bot1Block.data.Select(s => s.ToArray()).ToArray(), bot1Block.color);
            newBot1Block.ShiftDataBottom();
            
            bot2Block.data = bot2Block.RotateMatrix();
            Block newBot2Block = new Block(bot2Block.data.Select(s => s.ToArray()).ToArray(), bot2Block.color);
            newBot2Block.ShiftDataBottom();
            
            bool bot1BlockExists = false;
            bool bot2BlockExists = false;

            for(int j = 0; j < blockOrientationsForBot1.Count; j++) {
                if(newBot1Block.Equals(blockOrientationsForBot1[j])){
                    bot1BlockExists = true;
                }
            }
            for(int j = 0; j < blockOrientationsForBot2.Count; j++) {
               if(newBot2Block.Equals(blockOrientationsForBot2[j])){
                    bot2BlockExists = true;
                }
            }
    
            if(!bot1BlockExists) {
                blockOrientationsForBot1.Add(newBot1Block);
            }
            if(!bot2BlockExists) {
                blockOrientationsForBot2.Add(newBot2Block);
            }
        }

        allOrientationsBlock1Block2 = (from bot1List in blockOrientationsForBot1
								from bot2List in blockOrientationsForBot2
								select new Tuple<Block, Block, int>(bot1List, bot2List, 1)).ToList();

        allOrientationsBlock2Block1 = (from bot1List in blockOrientationsForBot1
								from bot2List in blockOrientationsForBot2
								select new Tuple<Block, Block, int>(bot2List, bot1List, 2)).ToList();
        
        allOrientationsBlock1Block2.AddRange(allOrientationsBlock2Block1);

        return allOrientationsBlock1Block2;
    }



    /**
     * #function DoubleBot::GetFit |
     * @author JavaComSci |
	 * @desc gets the fit of a single block onboard |
     * @header public List<CompatiblePiece> GetFit(Board board, Block block) | 
	 * @param Board board: board to do placing on |
     * @param Block block: block to be placed|
	 * @returns List<CompatiblePiece> : contains list of compatible positions for the block |
	 */
    public List<CompatiblePiece> GetFit(Board board, Block block) {
        // Console.WriteLine("GET FIT");

        List<CompatiblePiece> compatiblePieces = new List<CompatiblePiece>();

        int[][] shiftedOverPiece = block.data;
        botInfoPrinter.PrintJaggedArr(block.data);
        int[] bottomBlocks = block.GetBottomBlocksAsJaggedArray(block.data);

        // Console.WriteLine("AFTER JAGGED");

        // to calculate the width of the piece
        int minCol = 5;
        int maxCol = -1;        

        // positions of where the piece exists in the data in a tuple with both the ints for row and column
        List<Tuple<int, int>> dotPositions = new List<Tuple<int, int>>();
        
        // go through all the rows and get all the places where there is a true
        for(int row = 0; row < block.data.Length; row++) {
            dotPositions.AddRange(block.data[row].Select((b,i) => b == 1 ? i : -1).Where(i => i != -1).Select(index => new Tuple<int, int>(row, index)));
        }

        // shift over the dot positions
        foreach(Tuple<int, int>  positionOfDot in dotPositions) {
            // dot to be tested
            int dotRowOnPiece = positionOfDot.Item1;
            int dotColOnPiece = positionOfDot.Item2;
            // calculate the min and max of the columns
            minCol = Math.Min(minCol, dotColOnPiece);
            maxCol = Math.Max(maxCol, dotColOnPiece);
        }

        // find width of piece
        int widthOfPiece = maxCol - minCol + 1;

        for(int startingCol = 0; startingCol < board.board.GetLength(1) - widthOfPiece + 1; startingCol++) {
            for(int startingRow = board.maxHeights[startingCol] + 1; startingRow <= board.height; startingRow++) {

                // compatible board info
                List<Tuple<int, int>> compatibleBoard = new List<Tuple<int, int>>();


                // modified board that is getting filled by these dots
                int[,] modifiedBoardWithOnlyPieces = new int[board.height,board.width];

                for(int i = 0; i < board.height; i++){
                    for(int j = 0; j < board.width; j++){
                        modifiedBoardWithOnlyPieces[i,j] = board.board[i,j];
                    }
                }

                // dots nearby for area covered
                HashSet<Tuple<int, int>> dotsNearby = new HashSet<Tuple<int,int>>();

                // dots that fill the floor
                int dotsFillingFloor = 0;

                // see if all dots can be placed on the board without indexing issues in the column space
                foreach(Tuple<int, int> shiftedDotPosition in dotPositions) {
                    int shiftedDotRow = shiftedDotPosition.Item1;
                    int shiftedDotCol = shiftedDotPosition.Item2;

                    // shifted on the board size for the dot to be on the board
                    int shiftedForBoardRow = board.height + (shiftedDotRow - bottomBlocks[0]) - startingRow;
                    int shiftedForBoardCol = startingCol + shiftedDotCol;

                    // make sure that the shifted piece is not below the possibile pieces already there
                    if(board.height - board.maxHeights[startingCol + shiftedDotCol] <=  shiftedForBoardRow) {
                        compatibleBoard = null;
                        break;
                    }

                    // check whether the 2 heights are more than height of the board, if yes, then should not continue with the piece in this or any of the following rows
                    if(shiftedForBoardRow < 0 || shiftedForBoardRow >= board.height) {
                        compatibleBoard = null;
                        break;
                    } 

                    // check if the dot is overriding an exising dot
                    if(board.board[shiftedForBoardRow, shiftedForBoardCol] == 1) {
                        compatibleBoard = null;
                        break;
                    }

                    // add to the board information
                    compatibleBoard.Add(Tuple.Create(shiftedForBoardRow, shiftedForBoardCol));
                    modifiedBoardWithOnlyPieces[shiftedForBoardRow, shiftedForBoardCol] = 2;

                    // see which dots are nearby
                    // up
                    if(shiftedForBoardRow - 1 >= 0 && board.board[shiftedForBoardRow - 1,shiftedForBoardCol] == 1) {
                        dotsNearby.Add(Tuple.Create(shiftedForBoardRow - 1, shiftedForBoardCol));
                    }
                    // down
                    if(shiftedForBoardRow + 1 < board.height && board.board[shiftedForBoardRow + 1,shiftedForBoardCol] == 1) {
                        dotsNearby.Add(Tuple.Create(shiftedForBoardRow + 1, shiftedForBoardCol));
                    }
                    // left
                    if(shiftedForBoardCol - 1 >= 0 && board.board[shiftedForBoardRow,shiftedForBoardCol - 1] == 1) {
                        dotsNearby.Add(Tuple.Create(shiftedForBoardRow, shiftedForBoardCol - 1));
                    }
                    // right
                    if(shiftedForBoardCol + 1 < board.width && board.board[shiftedForBoardRow,shiftedForBoardCol + 1] == 1) {
                        dotsNearby.Add(Tuple.Create(shiftedForBoardRow, shiftedForBoardCol + 1));
                    }
                    // check for touching the floor
                    if(shiftedForBoardRow + 1 == board.height) {
                        dotsNearby.Add(Tuple.Create(shiftedForBoardRow + 1, shiftedForBoardCol));
                        dotsFillingFloor += 1;
                    }

                }
                
                 // piece starting at this column and row is actually compatible then add its information
                if(compatibleBoard != null) {
                    // check for the number of rows that it fills
                    int rowsFilled = 0;
                    for(int k = 0; k < board.height; k++) {
                        bool allFilled = true;
                        for(int l = 0; l < board.width; l++) {
                            if(modifiedBoardWithOnlyPieces[k, l] != 1 && modifiedBoardWithOnlyPieces[k, l] != 2){
                                allFilled = false;
                            }
                        }
                        if(allFilled) {
                            rowsFilled += 1;
                        }
                    }
                    
                    compatibleBoard = compatibleBoard.OrderBy(c => c.Item1).ThenBy(c => c.Item2).ToList();
                    CompatiblePiece compatiblePiece = new CompatiblePiece(compatibleBoard, dotsNearby.Count, rowsFilled);
                    compatiblePieces.Add(compatiblePiece);
                    break;
                }
            }
        }
        return compatiblePieces;
    }
    


    /**
     * #function DoubleBot::GetFitBothBlocks |
     * @author JavaComSci |
	 * @desc gets the fit of a both block onboard |
     * @header publicList<Tuple<CompatiblePiece, CompatiblePiece>> GetFitBothBlocks(Board board, List<Tuple<Block, Block, int>> blocksWithOrientations) | 
	 * @param Board board: board to do placing on |
     * @param List<Tuple<Block, Block, int>> blocksWithOrientations: blockss to be placed|
	 * @returns List<CompatiblePiece> : contains list of compatible positions for both blocks |
	 */
    public List<Tuple<CompatiblePiece, CompatiblePiece>> GetFitBothBlocks(Board board, List<Tuple<Block, Block, int>> blocksWithOrientations) {
        // list of all the compatible pieces that are on the board
        List<Tuple<CompatiblePiece, CompatiblePiece>> allCompatiblePieces = new List<Tuple<CompatiblePiece, CompatiblePiece>>();

        foreach(Tuple<Block, Block, int> blockWithOrientation in blocksWithOrientations) {
            // get the fit of the current board given this first piece and orientation
            List<CompatiblePiece> compatibleFirstPieces = GetFit(board, blockWithOrientation.Item1);

            // // sort the compatible first pieces
            // compatibleFirstPieces.Sort((x, y) => {
            //         // sort by whether a line has been cleared and puts those first if there is
            //         int result = y.numLinesCleared.CompareTo(x.numLinesCleared);
            //         // sort by the area covered
            //         return result == 0 ? y.area.CompareTo(x.area) : result;
            //     });

            // Console.WriteLine("BLOCK WITH ORIENTIATION");
            // botInfoPrinter.PrintCompatiblePieces(board.board, compatibleFirstPieces);

            // get the next pieces to fit
            foreach(CompatiblePiece compatibleFirstPiece in compatibleFirstPieces) {
                // make a copy of the boards to send to the current board
                Board boardCopy = new Board(board.board.GetLength(0), board.board.GetLength(1));
                boardCopy.board = board.CopyBoard(board.board);

                // add the pieces to this board to where it has the first piece
                boardCopy.board = board.FillBoardWithPiece(boardCopy.board, compatibleFirstPiece.locationOnBoard);

                // Console.WriteLine("BOARD WITH PIECE CONSIDERED");
                // botInfoPrinter.PrintMultiDimArr(boardCopy.board);

                // get the max heights on the board computed for the next round
                boardCopy.FindMaxHeights();

                // get the next piece on the board
                List<CompatiblePiece> compatibleSecondPieces = GetFit(boardCopy, blockWithOrientation.Item2);
                
                // // sort the compatible second pieces
                // compatibleSecondPieces.Sort((x, y) => {
                //     // sort by whether a line has been cleared and puts those first if there is
                //     int result = y.numLinesCleared.CompareTo(x.numLinesCleared);
                //     // sort by the area covered
                //     return result == 0 ? y.area.CompareTo(x.area) : result;
                // });

                // Console.WriteLine("BLOCK WITH ORIENTIATION");
                // botInfoPrinter.PrintCompatiblePieces(boardCopy.board, compatibleSecondPieces);

                foreach(CompatiblePiece compatibleSecondPiece in compatibleSecondPieces){
                    allCompatiblePieces.Add(Tuple.Create<CompatiblePiece, CompatiblePiece>(compatibleFirstPiece, compatibleSecondPiece));
                }
            }
        }
        Console.WriteLine("ALL COMPATIBLE PIECES ON BOARD");
        botInfoPrinter.PrintAllCompatiblePieces(board.board, allCompatiblePieces);
        return allCompatiblePieces;
    }



    /**
     * #function DoubleBot::GetbestFit |
     * @author JavaComSci |
	 * @desc gets the fit of a both block onboard |
     * @header public Tuple<CompatiblePiece, CompatiblePiece> GetBestFit(List<Tuple<CompatiblePiece, CompatiblePiece>> allCompatiblePieces)  | 
	 * @param List<Tuple<CompatiblePiece, CompatiblePiece>> allCompatiblePieces: all the pieces to find best fit for|
	 * @returns List<Tuple<CompatiblePiece, CompatiblePiece>> : contains position for both blocks |
	 */
     public Tuple<CompatiblePiece, CompatiblePiece> GetBestFit(List<Tuple<CompatiblePiece, CompatiblePiece>> allCompatiblePieces) {

        // sort the compatible second pieces and get the one that is the best fit
        List<CompatiblePiece> secondPieces = allCompatiblePieces.Select(t => t.Item2).ToList();
        secondPieces.Sort((x, y) => {
            // sort by whether a line has been cleared and puts those first if there is
            int result = y.numLinesCleared.CompareTo(x.numLinesCleared);
            // sort by the area covered
            return result == 0 ? y.area.CompareTo(x.area) : result;
        });

        // get the best second piece
        CompatiblePiece bestSecondPiece = secondPieces[0];

        // best first piece
        CompatiblePiece firstPiece = null;

        // get the corresponding first piece
        foreach(Tuple<CompatiblePiece, CompatiblePiece> compatiblePieces in allCompatiblePieces){
            CompatiblePiece potentialFirstPiece = compatiblePieces.Item1;
            CompatiblePiece potentialSecondPiece = compatiblePieces.Item2;

            bool match = true;
            if(potentialSecondPiece.area != bestSecondPiece.area) {
                match = false;
            }
            if(potentialSecondPiece.numLinesCleared != bestSecondPiece.numLinesCleared) {
                match = false;
            }
            if(potentialSecondPiece.locationOnBoard.Count != bestSecondPiece.locationOnBoard.Count) {
                match = false;
            } else {
                for(int i = 0; i < potentialSecondPiece.locationOnBoard.Count; i++) {
                    if(!potentialSecondPiece.locationOnBoard[i].Equals(bestSecondPiece.locationOnBoard[i])) {
                        match = false;
                        break;
                    }
                }
            }

            if(match) {
                firstPiece = potentialFirstPiece;
                break;
            }
        }

        Tuple<CompatiblePiece, CompatiblePiece> bestPieces = new Tuple<CompatiblePiece, CompatiblePiece>(firstPiece, bestSecondPiece);
        return bestPieces;
     }


    /**
     * #function DoubleBot::GetMove |
     * @author JavaComSci |
	 * @desc gets the fit of a both block onboard |
     * @header public override List<Tuple<int, int>> GetMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false)  | 
	 * @param Board board: board to do placing on |
     * @param List<List<Block>> allBotBlocks: blocks to be placed|
	 * @returns List<Tuple<int, int>> : contains position for both blocks |
	 */
    public override List<List<Tuple<int, int>>> GetMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false) {
        // get each of the bot's blocks
        List<Block> bot1Blocks = allBotBlocks[0];
        List<Block> bot2Blocks = allBotBlocks[1];

        // get the max height of each column of the baord 
        board.FindMaxHeights();

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

        // all orientations of the 2 blocks in each one going first on the board
        List<Tuple<Block, Block, int>> allOrientations = GetAllOrientations(bot1Blocks[0], bot2Blocks[0]);

        // Console.WriteLine("ALL ORIENTATIONS");
        // botInfoPrinter.PrintAllOrientationsAsList(allOrientations);

        // get the best fit of blocks
        List<Tuple<CompatiblePiece, CompatiblePiece>> allCompatiblePieces = GetFitBothBlocks(board, allOrientations);
        Tuple<CompatiblePiece, CompatiblePiece> bestPieces = GetBestFit(allCompatiblePieces);

        // printing the info
        Console.WriteLine("BEST COMPATIBLE PIECES ON BOARD");
        List<Tuple<CompatiblePiece, CompatiblePiece>> bestPiecesList = new List<Tuple<CompatiblePiece, CompatiblePiece>>();
        bestPiecesList.Add(bestPieces);
        botInfoPrinter.PrintAllCompatiblePieces(board.board, bestPiecesList);


        // return setup
        List<Tuple<int, int>> compatiblePiece1 = bestPieces.Item1.locationOnBoard;
        List<Tuple<int, int>> compatiblePiece2 = bestPieces.Item2.locationOnBoard;
        List<List<Tuple<int, int>>> allMoves = new List<List<Tuple<int, int>>>();
        allMoves.Add(compatiblePiece1);
        allMoves.Add(compatiblePiece2);
        return allMoves;
    }
}
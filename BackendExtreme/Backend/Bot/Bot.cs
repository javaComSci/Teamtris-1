using System;
using System.Collections.Generic;
using System.Linq;


// #code BotDefVar csharp
public abstract class Bot {
    public abstract List<List<Tuple<int, int>>> GetMove(
        Board board, 
        List<List<Block>> allBotBlocks, 
        bool allRotations = false
    );

    public abstract List<Tuple<int, int>> GetSingleMove(
        Board board, 
        List<List<Block>> allBotBlocks, 
        bool allRotations = false
    );

    /**
     * #function Bot::GetFit |
     * @author JavaComSci |
	 * @desc gets the fit of a single block onboard |
     * @header public List<CompatiblePiece> GetFit(Board board, Block block) | 
	 * @param Board board: board to do placing on |
     * @param Block block: block to be placed|
	 * @returns List<CompatiblePiece> : contains list of compatible positions for the block |
	 */
    public List<CompatiblePiece> GetFit(Board board, Block block) {
        Prints botInfoPrinter = new Prints();

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
}
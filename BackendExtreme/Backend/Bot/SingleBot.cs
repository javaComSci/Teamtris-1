using System;
using System.Collections.Generic;
using System.Linq;

/**
 * #class SingleBot |
 * @author JavaComSci | 
 * @language csharp | 
 * @desc Single bot extends the abstract bot class defined here:
 * @link{BotDefVar}
 * The SingleBot class will be made if the player requires only one 
 * bot in their game. |
 */
public class SingleBot : Bot {
    Prints botInfoPrinter;

	/**
     * #function SingleBot::SingleBot |
     * @author JavaComSci |
	 * @desc Creates a new board for the bot.  |
     * @header SingleBot() | 
	 * @param void : SingleBot takes no params |
	 * @returns SingleBot : An object of single bot class | 
	 */
    public SingleBot() {
        // create a board for this bot
        // Console.WriteLine("I AM A SINGLE BOT");
        botInfoPrinter = new Prints();
    }

    /**
     * #function SingleBot::getFit |
     * @author JavaComSci |
	 * @desc need desc here TODO |
     * @header List<...> getFit(Board board, Block block, int rotation) | 
	 * @param Board board : contains the the board that we want to make the move on |
     * @param Block block : contains the block that we want to fit |
     * @param int rotation : which roation we are trying to fit for |
	 * @returns List<...> compatiblePieces : information about the pieces that are compatible on the board |
	 */
    public List<Tuple<int, List<Tuple<int, int>>, int, int>> getFit(Board board, Block block, int rotation) {
        // has all the positions compatible for this piece with the rotation, location on board, area covered, and number of lines it can clear
        List<Tuple<int, List<Tuple<int, int>>, int, int>> compatiblePieces = new List<Tuple<int, List<Tuple<int, int>>, int, int>>();
        
        // positions of where the piece exists in the data in a tuple with both the ints for row and column
        List<Tuple<int, int>> dotPositions = new List<Tuple<int, int>>();
        
        // go through all the rows and get all the places where there is a true
        for(int row = 0; row < block.data.Length; row++) {
            dotPositions.AddRange(block.data[row].Select((b,i) => b == 1 ? i : -1).Where(i => i != -1).Select(index => new Tuple<int, int>(row, index)));
        }


        // print out the board
        // Console.WriteLine("BOARD");
        // botInfoPrinter.PrintMultiDimArr(board.board);

        // print the piece to be fit
        // Console.WriteLine("PIECE");
        // botInfoPrinter.PrintJaggedArr(block.data);

        // print the positions of the pieces
        // botInfoPrinter.PrintPositions(dotPositions);

        // find the position at the bottommost left corner of the piece because that is where we start placing the piece on the board
        Tuple<int, int> bottomLeft = block.FindBottomLeft();
        int bottomLeftRow = bottomLeft.Item1;
        int bottomLeftCol = bottomLeft.Item2;
        // Console.WriteLine("THE BOTTOM LEFT ROW OF PIECE: " + bottomLeftRow);
        // Console.WriteLine("THE BOTTOM LEFT COLUMN OF PIECE: " + bottomLeftCol);

        // shifted over piece information
        int[,] shiftedOverPiece = new int[4,4];
        List<Tuple<int, int>> shiftedDotPositions = new List<Tuple<int, int>>();

        // to calculate the width of the piece
        int minCol = 5;
        int maxCol = -1;        

        // shift over the dot positions
        foreach(Tuple<int, int>  positionOfDot in dotPositions) {
            // dot to be tested
            int dotRowOnPiece = positionOfDot.Item1;
            int dotColOnPiece = positionOfDot.Item2;

            // shift over the dot to get rid of extra space
            int modRowOnPiece = 3 - (bottomLeftRow - dotRowOnPiece);
            int modDotCol = dotColOnPiece - bottomLeftCol;

            // fill in new piece with this dot and this is the piece we have to fit on the board at this specific column
            shiftedOverPiece[modRowOnPiece, modDotCol] = 1;

            // add to the list of shifted dot positions
            shiftedDotPositions.Add(Tuple.Create(modRowOnPiece, modDotCol));

            // calculate the min and max of the columns
            minCol = Math.Min(minCol, dotColOnPiece);
            maxCol = Math.Max(maxCol, dotColOnPiece);
        }

        // find width of piece
        int widthOfPiece = maxCol - minCol + 1;
        // Console.WriteLine("WIDTH OF PIECE " + widthOfPiece);

        // get the shifted over piece
        // Console.WriteLine();
        // Console.WriteLine("SHIFTED OVER PIECE");
        // botInfoPrinter.PrintMultiDimArr(shiftedOverPiece);

        int[] bottomBlocks = block.GetBottomBlocks(shiftedOverPiece);
        // Console.WriteLine("BOTTOM ");
        // botInfoPrinter.PrintArr(bottomBlocks);

        // go through all starting positions in columns and rows
        for(int startingCol = 0; startingCol < board.board.GetLength(1) - widthOfPiece + 1; startingCol++) {
        // for(int startingCol = 0; startingCol < 1; startingCol++) { 
            for(int startingRow = board.maxHeights[startingCol] + 1; startingRow <= board.height; startingRow++) {
                // Console.WriteLine("STARTING COL " + startingCol + " AND ROW " + startingRow);

                // compatible board info
                List<Tuple<int, int>> compatibleBoard = new List<Tuple<int, int>>();


                // modified board that is getting filled by these dots
                int[,] modifiedBoardWithOnlyPieces = new int[board.height,board.width];
                // Console.WriteLine("HERE THE PEICE "+ board.height, width);
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
                foreach(Tuple<int, int> shiftedDotPosition in shiftedDotPositions) {
                    int shiftedDotRow = shiftedDotPosition.Item1;
                    int shiftedDotCol = shiftedDotPosition.Item2;

                    // Console.WriteLine("\n\nON THE PIECE " + shiftedDotRow + " " + shiftedDotCol);

                    // shifted on the board size for the dot to be on the board
                    int shiftedForBoardRow = board.height + (shiftedDotRow - bottomBlocks[0]) - startingRow;
                    // Console.WriteLine("SHIFTED DOT ROW " + bottomBlocks[0]);
                    // Console.WriteLine("CURRENT ROW " + shiftedDotRow);
                    // Console.WriteLine("IIIII " + startingRow);
                    int shiftedForBoardCol = startingCol + shiftedDotCol;
                    // Console.WriteLine("SHIFTED DOT POSITION ON BOARD(" + shiftedForBoardRow + "," + shiftedForBoardCol + ")");

                    // Console.Write("\n\n\nAAAHH" + board.maxHeights[startingCol + shiftedDotCol] + " " + shiftedForBoardRow);
                    // make sure that the shifted piece is not below the possibile pieces already there
                    if(board.height - board.maxHeights[startingCol + shiftedDotCol] <=  shiftedForBoardRow) {
                        // Console.WriteLine("INCOMPATIBLE PIECE WITH MAX HEIGHTS");
                        compatibleBoard = null;
                        break;
                    }

                    // check whether the 2 heights are more than height of the board, if yes, then should not continue with the piece in this or any of the following rows
                    if(shiftedForBoardRow < 0 || shiftedForBoardRow >= board.height) {
                        // Console.WriteLine("INCOMPATIBLE PIECE BECAUSE OUT OF BOUNDS WITH INFO " + (board.height - startingRow - 1) + " " + (4 - shiftedDotRow - 1));
                        compatibleBoard = null;
                        break;
                    } 

                    // check if the dot is overriding an exising dot
                    if(board.board[shiftedForBoardRow, shiftedForBoardCol] == 1) {
                        // Console.WriteLine("INCOMPATIBLE PIECE BECAUSE OVERRIDING DOTS");
                        compatibleBoard = null;
                        break;
                    }

                    // add to the board information
                    compatibleBoard.Add(Tuple.Create(shiftedForBoardRow, shiftedForBoardCol));
                    modifiedBoardWithOnlyPieces[shiftedForBoardRow, shiftedForBoardCol] = 2;

                    // see which dots are nearby
                    // up
                    if(shiftedForBoardRow - 1 > 0 && board.board[shiftedForBoardRow - 1,shiftedForBoardCol] == 1) {
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

                    // check for touching the ceiling
                    if(shiftedForBoardRow - 1 == 0) {
                        dotsNearby.Add(Tuple.Create(shiftedForBoardRow - 1, shiftedForBoardCol));
                    }

                }
                
                 // piece starting at this column and row is actually compatible then add its information
                if(compatibleBoard != null) {
                    // Console.WriteLine("MODIFIED BOARD");
                    // botInfoPrinter.PrintMultiDimArr(modifiedBoardWithOnlyPieces);

                    // Console.WriteLine("DOTS NEARBY");
                    // botInfoPrinter.PrintSetTuples(dotsNearby);

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
                    // Console.WriteLine("ROWS FILLED WITH THIS PIECE IS " + rowsFilled);
                
                    compatiblePieces.Add(Tuple.Create(rotation, compatibleBoard, dotsNearby.Count, rowsFilled));
                    break;
                }
            }
        }

        return compatiblePieces;
    }

    /**
     * #function SingleBot::GetSingleMove |
     * @author JavaComSci |
	 * @desc need desc here TODO |
     * @header public override List<...> GetSingleMove(Board, List<Block>, bool) | 
	 * @param int[][] board : current enviornment |
     * @param List<Block> blocks : contains the list of all the blocks to try to fit in this location |
	 * @returns List<...> bestPiecePlacementOfCurrentBlock : contains the list of the indicies 
     * of where the piece would be on the board |
	 */
    public List<Tuple<int, int>> GetSingleMove1(Board board, List<List<Block>> allBotBlocks, bool allRotations = false) {
        List<Block> blocks = allBotBlocks[0];
        Console.WriteLine("BOARD");
        botInfoPrinter.PrintMultiDimArr(board.board);
        // Console.WriteLine("PIECE");
        // botInfoPrinter.PrintJaggedArr(blocks[0].data);

        // get the max height of each column of the baord 
        board.FindMaxHeights();

        // has the information about all the compatible pieces for a given block
        Dictionary<Block, List<Tuple<int, List<Tuple<int, int>>, int, int>>> allBlocksPossible = new Dictionary<Block, List<Tuple<int, List<Tuple<int, int>>, int, int>>>();

        int blockCount = 0;

        // test out each of the pieces
        foreach(Block block in blocks) {

            // check to make sure that each block is valid
            bool blockValid = block.CheckValidity();

            // block is invalid
            if(!blockValid) {
                throw new Exception("Shape formation is incorrect");
            }

            // compatible pieces for a single block
            // has all the positions compatible for this piece with the rotation, location on board, area covered, and number of lines that can be cleared
            List<Tuple<int, List<Tuple<int, int>>, int, int>> compatiblePieces = new List<Tuple<int, List<Tuple<int, int>>, int, int>>();

            // get the fit of the board with the area and whether a piece can clear a line
            compatiblePieces.AddRange(getFit(board, block, 1));
            block.data = block.RotateMatrix();
            compatiblePieces.AddRange(getFit(board, block, 2));
            block.data = block.RotateMatrix();
            compatiblePieces.AddRange(getFit(board, block, 3));
            block.data = block.RotateMatrix();
            compatiblePieces.AddRange(getFit(board, block, 4));

            if(allRotations) {
                block.data = block.RotateMatrix();
            }

            // compatible pieces has all the pieces that are compatible with the board and has the information about the rotation, location on board, area covered, and if that piece can clear a line
            compatiblePieces.Sort((x, y) => {
                // sort by whether a line has been cleared and puts those first if there is
                int result = y.Item4.CompareTo(x.Item4);
                // sort by the area covered
                return result == 0 ? y.Item3.CompareTo(x.Item3) : result;
            });

            // the current piece cannot even be placed so must return null
            if(compatiblePieces.Count == 0 && blockCount == 0) {
                return null;
            }
            blockCount++;

            allBlocksPossible[block] = compatiblePieces;
        }

        // get the best piece of the block that is the first one
        Tuple<int, List<Tuple<int, int>>, int, int> bestPieceOfCurrentBlock = allBlocksPossible[blocks[0]][0];

        // placement of this block that is the best of the current block
        List<Tuple<int, int>> bestPiecePlacementOfCurrentBlock = bestPieceOfCurrentBlock.Item2;

        // if the best current piece doesnt clear any lines, need to find one that does clear lines
        if(bestPieceOfCurrentBlock.Item4 == 0 && blocks.Count > 1) {
            // does not clear any lines so need to look for another candidate and choose the next best spot for this block
            
            // find the next piece that clears lines
            Tuple<int, List<Tuple<int, int>>, int, int> bestPieceOfNextBlock = allBlocksPossible[blocks[1]][0];

            // next next piece that clears lines
            Tuple<int, List<Tuple<int, int>>, int, int> bestPieceOfNextNextBlock = allBlocksPossible.Count > 2 ? allBlocksPossible[blocks[2]][0] : null;
            // Console.WriteLine("NEXT PIECE " + bestPieceOfNextNextBlock.Item4);

            // the next piece is able to clear lines
            if(bestPieceOfNextBlock.Item4 > 0) {
                // Console.WriteLine("I AM HEREHEHEH LOOKING AT THE NEXT PIECE!\n");
                // create a new copy of the board so that it can run through the steps for clearing lines again
                int [,] copiedBoard = board.CopyBoard(board.board);

                // add the pieces to this board to where it has the new information
                copiedBoard = board.FillBoardWithPieceConsidered(copiedBoard, bestPieceOfNextBlock.Item2);

                // check to see if there is anything the current piece can place so do another search with the new info
                List<Tuple<int, List<Tuple<int, int>>, int, int>> compatiblePiecesForCurrentPieceAfterPiece1 = new List<Tuple<int, List<Tuple<int, int>>, int, int>>();

                // set up new board
                Board cBoard = new Board(board.board.GetLength(0), board.board.GetLength(1));
                cBoard.board = copiedBoard;
                cBoard.FindMaxHeights();

                // get the fit of the board with the area and whether a piece can clear a line
                compatiblePiecesForCurrentPieceAfterPiece1.AddRange(getFit(cBoard, blocks[0], 1));
                blocks[0].data = blocks[0].RotateMatrix();
                compatiblePiecesForCurrentPieceAfterPiece1.AddRange(getFit(cBoard, blocks[0], 2));
                blocks[0].data = blocks[0].RotateMatrix();
                compatiblePiecesForCurrentPieceAfterPiece1.AddRange(getFit(cBoard, blocks[0], 3));
                blocks[0].data = blocks[0].RotateMatrix();
                compatiblePiecesForCurrentPieceAfterPiece1.AddRange(getFit(cBoard, blocks[0], 4));

                if(allRotations) {
                    blocks[0].data = blocks[0].RotateMatrix();
                }

                // compatible pieces has all the pieces that are compatible with the board and has the information about the rotation, location on board, area covered, and if that piece can clear a line
                compatiblePiecesForCurrentPieceAfterPiece1.Sort((x, y) => {
                    // sort by whether a line has been cleared and puts those first if there is
                    int result = y.Item4.CompareTo(x.Item4);
                    // sort by the area covered
                    return result == 0 ? y.Item3.CompareTo(x.Item3) : result;
                });

                // placement of this block that is the best of the current block
                bestPiecePlacementOfCurrentBlock= compatiblePiecesForCurrentPieceAfterPiece1[0].Item2;
            } else if(bestPieceOfNextNextBlock != null && bestPieceOfNextNextBlock.Item4 > 0) {
                // Console.WriteLine("I AM A NEXT NEXT SHAPE\n\n\n\n");

                // check 2 shapes from the current shape

                // create a new copy of the board so that it can run through the steps for clearing lines again
                int [,] copiedBoard = board.CopyBoard(board.board);

                // add the pieces to this board to where it has the new information
                copiedBoard = board.FillBoardWithPieceConsidered(copiedBoard, bestPieceOfNextNextBlock.Item2);

                // check to see if there is anything the current piece can place so do another search with the new info
                List<Tuple<int, List<Tuple<int, int>>, int, int>> compatiblePiecesForCurrentPieceAfterPiece2 = new List<Tuple<int, List<Tuple<int, int>>, int, int>>();

                // set up new board
                Board cBoard = new Board(board.board.GetLength(0), board.board.GetLength(1));
                cBoard.board = copiedBoard;
                cBoard.FindMaxHeights();

                // get the fit of the board with the area and whether a piece can clear a line
                compatiblePiecesForCurrentPieceAfterPiece2.AddRange(getFit(cBoard, blocks[0], 1));
                blocks[0].data = blocks[0].RotateMatrix();
                compatiblePiecesForCurrentPieceAfterPiece2.AddRange(getFit(cBoard, blocks[0], 2));
                blocks[0].data = blocks[0].RotateMatrix();
                compatiblePiecesForCurrentPieceAfterPiece2.AddRange(getFit(cBoard, blocks[0], 3));
                blocks[0].data = blocks[0].RotateMatrix();
                compatiblePiecesForCurrentPieceAfterPiece2.AddRange(getFit(cBoard, blocks[0], 4));

                if(allRotations) {
                    blocks[0].data = blocks[0].RotateMatrix();
                }

                // compatible pieces has all the pieces that are compatible with the board and has the information about the rotation, location on board, area covered, and if that piece can clear a line
                compatiblePiecesForCurrentPieceAfterPiece2.Sort((x, y) => {
                    // sort by whether a line has been cleared and puts those first if there is
                    int result = y.Item4.CompareTo(x.Item4);
                    // sort by the area covered
                    return result == 0 ? y.Item3.CompareTo(x.Item3) : result;
                });

                // placement of this block that is the best of the current block
                bestPiecePlacementOfCurrentBlock= compatiblePiecesForCurrentPieceAfterPiece2[0].Item2;

            }
        } 

        // Console.WriteLine("BEST PIECE FOR BOARD");
        // botInfoPrinter.PrintPositions(bestPiecePlacementOfCurrentBlock);
        // Console.WriteLine("BOARD WITH PIECE");
        // botInfoPrinter.PrintBoardWithPiece(board.board, bestPiecePlacementOfCurrentBlock);

        return bestPiecePlacementOfCurrentBlock;
    }

    public override List<List<Tuple<int, int>>> GetMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false){
        Console.WriteLine("IN GET MOVE");
        List<Tuple<int, int>> singleMove = GetSingleMove1(board, allBotBlocks, allRotations);
        List<List<Tuple<int, int>>> allMoves = new List<List<Tuple<int, int>>>();
        allMoves.Add(singleMove);
        return allMoves;
    }

    
    public override List<Tuple<int, int>> GetSingleMove(Board board, List<List<Block>> allBotBlocks, bool allRotations = false){
       return GetMove(board, allBotBlocks, allRotations)[0];
    }
}


 /* deprecated for more efficient algorithm
                // go through all the parts of the data that we recieve starting at the bottom left corner and make sure it fits
                // ---|--
                // number of rows and the number of rows is fixed at 20
                for(int j = data.Length - 1; j >= 0 && j < board[i] + ROWS; j--) {
                    // number of columns to go through and only go as much as the board allows to
                    for(int k = 0; k < data[0].Length && k + i < board.Length; k++) {
                        // a dot exists there
                        if(data[j][k]) {
                        }
                    }
                }
             */


// methodology for sorting appraoch - adopted different way to do this
/*  find the best piece of all
        // see if any have a line done
        bool anyLineDone = compatiblePieces.Any(a => a.Item4 == 1);

        if(anyLineDone) {
            // a line was completed in one of the pieces, so need to lead with the pieces that lead to tye line being done
            Console.WriteLine("A line was done");
            // order by the completion and then the area covered
            compatiblePieces.Sort((x, y) => {
                int result = y.Item4.CompareTo(x.Item4);
                return result == 0 ? y.Item3.CompareTo(x.Item3) : result;
            });
        } else {
            compatiblePieces.Sort((x, y) => y.Item3.CompareTo(x.Item3));
            Console.WriteLine("No line can be done, find the best piece");
        }
*/
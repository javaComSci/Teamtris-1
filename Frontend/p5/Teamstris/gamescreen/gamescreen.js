/**
 * @classDesc Controls all necessary information about the gamescreen
 */
class GameScreen {
  constructor(
    playerCount = 1,
    playerID = 1,
    botCount = 0,
    xOffset = 0,
    yOffset = 0,
    CustomWindowWidth = windowWidth,
    CustomWindowHeight = windowHeight
  ) {
    if (gamescreen_constructor) console.log("Creating GameScreen Object");

    // number of players in the game (real and bot inclusive)
    this.NumPlayers = playerCount;
    this.NumBots = botCount

    // ID of the current player
    this.PlayerID = playerID;

    // size of the game board, determined by this.NumPlayers.
    this.BoardSquareSize = [20, 5 + 5 * (this.NumPlayers + this.NumBots)];

    // Screen dimensions
    this.CustomWindowHeight = CustomWindowHeight
    this.CustomWindowWidth = CustomWindowWidth

    // length of the edge of each of the squares on the game board
    this.SquareEdgeLength = Math.min(
      this.CustomWindowHeight / this.BoardSquareSize[0],
      this.CustomWindowWidth / this.BoardSquareSize[1]
    );

    // scales the length of the edges to the desired ratio of the screen.
    this.SquareScalingFactor = 0.8;
    this.SquareEdgeLength = this.SquareEdgeLength * this.SquareScalingFactor;

    // base of the edge length of each square, we center the grid.
    this.WidthTranslation =
      (this.CustomWindowWidth - this.SquareEdgeLength * this.BoardSquareSize[1]) / 2;

    // push the grid closer to the bottom of the screen
    this.HeightTranslation =
      (this.CustomWindowHeight - this.SquareEdgeLength * this.BoardSquareSize[0]) / 1.1;

    this.GridTranslation = [
      xOffset + this.WidthTranslation,
      yOffset + this.HeightTranslation
    ];

    // Default grid stroke of all shapes in the grid (width of edge)
    this.DefaultGridStroke = 8;

    // Create GameArray, which runs the logic for the game
    this.GameArray = new GameArray(
      this.BoardSquareSize[0],
      this.BoardSquareSize[1],
      this.SquareEdgeLength,
      this.NumPlayers,
      this.PlayerID
    );

    // variables used for updating the game grid
    this.PreviousTime = 0;

    // number of milliseconds between every update
    this.GameSpeed = 200;

    // total time in game
    this.totalGameTime = 0;

    // game score
    this.gameScore = 0;
  }

  SetPlayerCount() {
    this.NumPlayers = team.numPlayers
  }

  /**
   * @description Called 60 times a second to draw the gamescreen
   *
   * @return void
   */
  draw() {
    if (gamescreen_draw) console.log("Drawing on GameScreen");
    this.TimeStepUpdate(); // perform a timestep update if necessary
    this.GameArray.Draw(this.GridTranslation[0], this.GridTranslation[1]);

    // draw score
    this.DrawScore()

    // draw sidebar
    this.DrawSidebar()

    // text('word', 10, 60);
    // fill(0, 102, 153, 51);
    // text('word', 10, 90);
  }

  /**
   * @description Draws the scoring for the game
   *
   * @return void
   */
  DrawScore() {
    textSize(32);
    fill(0, 255, 255);
    text("Score: " + (this.gameScore + this.GameArray.gameArrayScore), this.CustomWindowWidth*0.1, this.CustomWindowHeight*0.05);
  }

  /**
   * @description Draws the sidebar displaying next shapes
   *
   * @return void
   */
  DrawSidebar() {
    push();

    rectMode(CORNER)
    var scaleFactor = 0.6
    var DisplayShape = this.GameArray.DisplayShape
    var widthOffset = this.GridTranslation[0] * 0.5
    var heightOffset = this.GridTranslation[1]
    translate(widthOffset, heightOffset);
    DisplayShape.DrawShape(this.SquareEdgeLength*scaleFactor)

    // var DisplayShapes = this.GameArray.DisplayArray
    // if (DisplayShapes == null) {
    //   return;
    // }

    // var scaleFactor = 0.6
    
    // for (var i = 0; i < Math.min(DisplayShapes.length, 2); i++) {
    //   push();
    //   var widthOffset = (this.GridTranslation[0] / 2) - this.SquareEdgeLength*2*scaleFactor
    //   var heightOffset = this.GridTranslation[1] + this.SquareEdgeLength * this.BoardSquareSize[0] * (0.2 * (i+1))
    //   translate(widthOffset, heightOffset);
    //   DisplayShapes[i].DrawShape(this.SquareEdgeLength*scaleFactor)
    //   pop();
    // }
    

    
    // for (var i = 2; i < DisplayShapes.length; i++) {
    //   push();
    //   var widthOffset = (((this.GridTranslation[0] + this.BoardSquareSize[1] * this.SquareEdgeLength)
    //                     + this.CustomWindowWidth) / 2)
    //                     - this.SquareEdgeLength*2*scaleFactor
    //   var heightOffset = this.GridTranslation[1] + this.SquareEdgeLength * this.BoardSquareSize[0] * (0.2 * (i+1))
    //   translate(widthOffset, heightOffset);
    //   DisplayShapes[i].DrawShape(this.SquareEdgeLength*scaleFactor)
    //   pop();
    // }
    

    pop();
  }

  /**
   * @description Sets the socket for listening in the game screen.
   *
   * @return void
   */
  SetupSocket() {
    /* Going to handle all the connections from the backend */
    socket.onmessage = event => {
      var e = JSON.parse(event.data);
      if (e.type == 8) {
        if (e.move == "left") {
          this.GameArray.ForceMoveShape(e.playerID, 1, 0, 0);
        } else if (e.move == "right") {
          this.GameArray.ForceMoveShape(e.playerID, 0, 1, 0);
        } else if (e.move == "down") {
          this.GameArray.ForceMoveShape(e.playerID, 0, 0, 1);
        } else if (e.move == "rotate") {
          this.GameArray.RotateShape(e.playerID, false);
          this.GameArray.ForceMoveShape(e.playerID, 0, 0, 0);
        } else if (e.move == "freeze") {
          // console.log("freezing someone: ")
          // console.log(e.playerID)
          if (e.playerID == this.PlayerID) {
            console.log("fake news")
            return
          }
          this.GameArray.RemoveShapeByID(e.playerID)
          this.GameArray.FreezeIndices(e.shapeIndices)
          //this.GameArray.ForceMoveShape(e.playerID, 0, 0, 0);
        }
      } else if (e.type == 11) {
        // when a player gets a new shape, we force update to re-make the shape on the fronted
        this.GameArray.ForceUpdatePlayer(e.playerID, e.shapeBlueprint)
      } else if (e.type == 100) { 
        // force update based on game board
        var newBoard = e.board.board
        //console.log(newBoard)
        for (var i = 0; i < newBoard.length; i++) {
          for (var j = 0; j < newBoard[0].length; j++) {
            // remove local squares if they conflict with the main board
            if (this.GameArray.GetSquare(i,j).IsFrozen() && newBoard[i][j] == 0) {
              this.GameArray.GetSquare(i,j).SetEmpty()
            }

            // Set all squares on the board that need to be set based on the main board
            if (newBoard[i][j] != 0) {
              this.GameArray.GetSquare(i,j).SetFrozen(newBoard[i][j])
            }

          }
        }

        this.totalGameTime = e.current_time
        this.GameArray.totalGameTime = this.totalGameTime
        this.UpdateGameSpeed()
      } else if (e.type == 666) {
        gameState = 3
      }

      // e.type == 11
    };
  }

  UpdateGameSpeed() {
    this.GameSpeed = 200 * (1 + Math.log(this.totalGameTime))
  }

  // receive generated shape for other players
  // send power cube data

  /**
   * @description returns true if it is time to perform a timestep update, moving each shape down 1.
   *
   * @return boolean
   */
  CheckTimeStepUpdate() {
    var TruncUpdate = int(millis() / this.GameSpeed); // every milliseconds % this.GameSpeed the timestep update
    if (this.PreviousTime != TruncUpdate) {
      if (gamescreen_updateflag) console.log("Updating Game");
      this.PreviousTime = TruncUpdate;
      this.gameScore += 1;
      return true;
    } else {
      return false;
    }
  }

  /**
   * @description Updates all necessary objects on the game board
   *
   * @return void
   */
  TimeStepUpdate() {
    if (this.CheckTimeStepUpdate()) {
      for (var i = 0; i < 1; i++) {
        this.GameArray.MoveAllShapes(0, 0, 1);
      } //endfor
    } //endif
  }

  /**
   * @description Handles user inputs for keyboard inputs
   *
   * @return void
   */
  keyPressedGame(realKeyCode = keyCode) {
    if (realKeyCode === LEFT_ARROW || realKeyCode == 65) { // < or a
      this.GameArray.MoveShape(this.PlayerID, 1, 0, 0);
    } else if (realKeyCode === RIGHT_ARROW || realKeyCode == 68) { // > or d
      this.GameArray.MoveShape(this.PlayerID, 0, 1, 0);
    } else if (realKeyCode === DOWN_ARROW || realKeyCode == 83) { // down or s
      this.GameArray.MoveShape(this.PlayerID, 0, 0, 1);
    } else if (realKeyCode === 82) { // r
      this.GameArray.RotateShape(this.PlayerID);
      this.GameArray.MoveShape(this.PlayerID, 0, 0, 0);
    } else if (realKeyCode === 73) {
      gameState = 3
    } else if (realKeyCode == 32) { // SPACEBAR
      this.GameArray.HardDrop(this.PlayerID);
    }
  }
}
/* This export is used for testing*/
module.exports = [GameScreen];

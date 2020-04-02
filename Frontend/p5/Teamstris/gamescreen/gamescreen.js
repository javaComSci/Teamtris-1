/**
 * @classDesc Controls all necessary information about the gamescreen
 */
class GameScreen {
  constructor(
    playerCount,
    playerID,
    xOffset = 0,
    yOffset = 0,
    CustomWindowWidth = windowWidth,
    CustomWindowHeight = windowHeight
  ) {
    if (gamescreen_constructor) console.log("Creating GameScreen Object");

    // number of players in the game (real and bot inclusive)
    this.NumPlayers = playerCount;

    // ID of the current player
    this.PlayerID = playerID;

    // size of the game board, determined by this.NumPlayers.
    this.BoardSquareSize = [20, 5 + 5 * this.NumPlayers];

    // length of the edge of each of the squares on the game board
    this.SquareEdgeLength = Math.min(
      CustomWindowHeight / this.BoardSquareSize[0],
      CustomWindowWidth / this.BoardSquareSize[1]
    );

    // scales the length of the edges to the desired ratio of the screen.
    this.SquareScalingFactor = 0.8;
    this.SquareEdgeLength = this.SquareEdgeLength * this.SquareScalingFactor;

    // base of the edge length of each square, we center the grid.
    this.WidthTranslation =
      (CustomWindowWidth - this.SquareEdgeLength * this.BoardSquareSize[1]) / 2;
    this.HeightTranslation =
      (CustomWindowHeight - this.SquareEdgeLength * this.BoardSquareSize[0]) /
      2;
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
    textSize(32);
    text('word', 10, 30);
    fill(0, 102, 153);
    text('word', 10, 60);
    fill(0, 102, 153, 51);
    text('word', 10, 90);
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
          console.log("received freeze")
          this.GameArray.FreezeShape(e.playerID, false)
        }
      } else if (e.type == 11) {
        this.GameArray.ForceUpdatePlayer(e.playerID, e.shapeBlueprint)
      } else if (e.type == 100) { // force update based on game board
        var newBoard = e.board.board
        console.log(newBoard)
        for (var i = 0; i < newBoard.length; i++) {
          for (var j = 0; j < newBoard[0].length; j++) {
            if (newBoard[i][j] != 0) {
              //this.GameArray.arr[i][j].SetFrozen(newBoard[i][j])
            }
          }
        }
      }

      // e.type == 11
    };
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
    if (realKeyCode === LEFT_ARROW) {
      this.GameArray.MoveShape(this.PlayerID, 1, 0, 0);
    } else if (realKeyCode === RIGHT_ARROW) {
      this.GameArray.MoveShape(this.PlayerID, 0, 1, 0);
    } else if (realKeyCode === DOWN_ARROW) {
      this.GameArray.MoveShape(this.PlayerID, 0, 0, 1);
    } else if (realKeyCode === 65) {
      //a
      this.GameArray.RotateShape(this.PlayerID);
      this.GameArray.MoveShape(this.PlayerID, 0, 0, 0);
    } else if (realKeyCode === 73) {
      gameState = 3
    }
  }
}
/* This export is used for testing*/
module.exports = [GameScreen];

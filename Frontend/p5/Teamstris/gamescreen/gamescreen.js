class GameScreen {
  constructor(xOffset=0, yOffset=0, CustomWindowWidth=windowWidth, CustomWindowHeight=windowHeight) {
    if(gamescreen_constructor) console.log("Creating GameScreen Object");

    // number of players in the game (real and bot inclusive)
    this.NumPlayers = 1

    // ID of the current player
    this.PlayerID = 1

    // size of the game board, determined by this.NumPlayers.
    this.BoardSquareSize = [20,5+5*this.NumPlayers]

    // length of the edge of each of the squares on the game board
    this.SquareEdgeLength = Math.min(CustomWindowHeight / this.BoardSquareSize[0], CustomWindowWidth / this.BoardSquareSize[1])

    // scales the length of the edges to the desired ratio of the screen.
    this.SquareScalingFactor = 0.8
    this.SquareEdgeLength = this.SquareEdgeLength * this.SquareScalingFactor

    // base of the edge length of each square, we center the grid.
    this.WidthTranslation = (CustomWindowWidth - this.SquareEdgeLength * this.BoardSquareSize[1]) / 2
    this.HeightTranslation = (CustomWindowHeight - this.SquareEdgeLength * this.BoardSquareSize[0]) / 2
    this.GridTranslation = [xOffset + this.WidthTranslation,yOffset + this.HeightTranslation]

    // Default grid stroke of all shapes in the grid (width of edge)
    this.DefaultGridStroke = 8

    // Instantiate all squares
    //this.InstantiateSquares()
    this.GameArray = new GameArray(this.BoardSquareSize[0], this.BoardSquareSize[1], this.SquareEdgeLength, this.NumPlayers)
    //this.GameArray.InstantiateSquares(this.SquareEdgeLength)

    // variables used for updating the game grid
    this.PreviousTime = 0

    // number of milliseconds between every update
    this.GameSpeed = 1000

    // Array of taken squares that will be used to calculate collision. Max one shape per player
    // this.ShapeArray = new Array(this.NumPlayers)
    // this.ShapeArray[0] = this.GameArray.InstantiateShape(this.PlayerID)
    // this.ShapeArray[1] = this.GameArray.InstantiateShape(2,0,5)
    // this.GameArray.PlaceShape(this.ShapeArray[0])
    // this.GameArray.PlaceShape(this.ShapeArray[1])
    
  }

  draw() {
    if(gamescreen_draw) console.log("Drawing on GameScreen");
    this.TimeStepUpdate() // perform a timestep update if necessary
    this.GameArray.Draw(this.GridTranslation[0], this.GridTranslation[1])
    
  }

  // // Sets the initial positions of each square in the grid
  // InstantiateSquares() {
  //   //draw each square of the game board
  //   for (var i = 0; i < this.BoardSquareSize[0]; i++) {
  //     for (var j = 0; j < this.BoardSquareSize[1]; j++) {
  //       this.GameArray[i][j] = new Square(this.SquareEdgeLength)
  //       this.GameArray[i][j].SetPosition(i,j)
  //     }
  //   }
  // }

  // // Create shape on the gamescrean. Spawnlocation denotes the top left, x-axis, offset.
  // InstantiateShape(SpawnLocation=0, owner=this.PlayerID) {
  //   var NewShape = new Shape(owner)

  //   // ensure the shape can be placed along the x-axis given the width of the board and the shape's starting point
  //   if (SpawnLocation + NewShape.ShapeDimensions[2] > this.BoardSquareSize[1]) {
  //     console.log("Invalid shape at specified offset. Out of bounds.")
  //     return
  //   }

  //   for (var i = 0; i < 4; i++) {
  //     for (var j = 0; j < 4; j++) {
  //       // if the blueprint has a square at this location, we attempt to place it
  //       if (NewShape.ShapeBlueprint[i][j] == 1) {
  //         var iOffset = i-NewShape.ShapeDimensions[0]
  //         var jOffset = j-NewShape.ShapeDimensions[1]+SpawnLocation

  //         // if this spot is not empty, then we cannot spawn a square here
  //         if (!this.GameArray.IsEmpty(iOffset,jOffset)) {
  //           console.log("GAME OVER")
  //         } else {
  //           // Always place the shape as if it were in a bounding box
  //           //this.PlaceSquare(iOffset,jOffset,NewShape.ID,NewShape.Color)
  //           NewShape.AddSquare(this.GameArray.GetSquare(iOffset,jOffset))
  //         }
  //       }
  //     }
  //   }
  //   console.log(NewShape)
  //   return NewShape
  // }

  // Sets a flag to perform a timestep update
  CheckTimeStepUpdate() {
    var TruncUpdate = int(millis() / this.GameSpeed) // every milliseconds % this.GameSpeed the timestep update
    if (this.PreviousTime != TruncUpdate) {
      if(gamescreen_updateflag) console.log("Updating Game")
      this.PreviousTime = TruncUpdate
      return true
    } else {
      return false
    }
  }

  // updates all necessary objects on the game board
  TimeStepUpdate() {
    if (this.CheckTimeStepUpdate()) {
      for (var i = 0; i < 1; i++) {
        this.GameArray.MoveAllShapes(0,0,1)
      }//endfor
    } //endif
  }

  // SetSquare(i,j,ID,Color) {
  //   this.GameArray[i][j].ChangeOwner(ID,Color)
  // }

  // GetSquare(i,j) {
  //   return this.GameArray[i][j]
  // }


  // PlaceShape(Shape) {
  //   for (var i = 0; i < Shape.Squares.length; i++) {
  //     var s = Shape.Squares[i]
  //     this.GameArray[s.i][s.j].ChangeOwner(s.ID,s.Color)
  //   }
  // }

  // return true if the provided coordinates have no occupancy
  // IsEmpty(i,j) {
  //   if (this.GameArray.GetSquare(i,j).ID == 0) {
  //     return true
  //   }
  // }

  // draws the game grid based on this.GameArray
  // DrawGameGrid() {
  //   push();
  //   translate(this.GridTranslation[0], this.GridTranslation[1])
  //   stroke(this.DefaultGridStroke)
  //   for (var i = 0; i < this.BoardSquareSize[0]; i++) {
  //     for (var j = 0; j < this.BoardSquareSize[1]; j++) {
  //       this.GameArray.GetSquare(i,j).Draw()
  //     }
  //   }
  //   pop();
  // }

  keyPressedGame(realKeyCode=keyCode){
    if (realKeyCode === LEFT_ARROW) {
      this.GameArray.MoveShape(this.PlayerID,1,0,0)
    } else if (realKeyCode === RIGHT_ARROW) {
      this.GameArray.MoveShape(this.PlayerID,0,1,0)
    } else if (realKeyCode === DOWN_ARROW) {
      this.GameArray.MoveShape(this.PlayerID,0,0,1)
    } else if (realKeyCode === 65) { //a
      this.GameArray.RotateShape(this.PlayerID)
      this.GameArray.MoveShape(this.PlayerID,0,0,0)
    }
  }
}
/* This export is used for testing*/
module.exports = [GameScreen]

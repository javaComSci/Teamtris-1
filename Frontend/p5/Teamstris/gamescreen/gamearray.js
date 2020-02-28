/** 
  * @classDesc Represents the game itself, controls shape movement/resets/collision/coloring
  */
class GameArray {
    constructor(rows, columns, SquareEdgeLength, NumPlayers) {
        // array object to represent the game, filled with squares
        this.row_count = rows
        this.column_count = columns
        this.arr = new Array(this.row_count)
        for (var r = 0; r < this.row_count; r++) {
            this.arr[r] = new Array(columns)
        }

        this.SquareEdgeLength = SquareEdgeLength
        this.InstantiateSquares()

        this.NumPlayers = NumPlayers
        this.ShapeArray = new Array(this.NumPlayers)
        this.ShapeArray[0] = this.InstantiateShape(1,null,0,5, false)
        this.ShapeArray[1] = this.InstantiateShape(2,null,0,10,false)
        this.ShapeArray[2] = this.InstantiateShape(3,null,0,15,false)
        this.ShapeArray[3] = this.InstantiateShape(4,null,0,20,false)
        //this.PlaceShape(this.ShapeArray[1])

        // allows easy determination of whenv to freeze an object
        this.CollisionType = {
            OutOfBounds : 1,
            FrozenObject : 2,
            OtherPlayer : 3,
            NoCollision : 4
        }

        //console.log(this.CollisionType)
    }//end constructor

    /** 
     * @description Simply calls InstantiateSqures
     * 
     * @return void
     */
    ResetGameBoard() {
        this.InstantiateSquares()
    }

    /** 
     * @description Replaces all squares on the board with empty squares, initializes position of squares
     * 
     * @return void
     */
    InstantiateSquares() {
        for (var i = 0; i < this.row_count; i++) {
            for (var j = 0; j < this.column_count; j++) {
                this.arr[i][j] = new Square(this.SquareEdgeLength)
                this.arr[i][j].SetPosition(i,j)
            }
        }
    }

    /** 
     * @description returns the square at the provided indices
     * 
     * @param i - row index
     * @param j - col index
     * 
     * @return Square
     */
    GetSquare(i,j) {
        return this.arr[i][j]
    }

    /** 
     * @description sets the square at the provided indices to be a specific ID and color
     * 
     * @param i - row index
     * @param j - col index
     * @param ID - ID to be set to the square
     * @param Color - color the square will be displayed as
     * 
     * @return void
     */
    SetSquare(i,j,ID,Color) {
        this.arr[i][j].ChangeOwner(ID,Color)
    }

    /** 
     * @description returns true if the ID of the square at the provided indices is 0
     * 
     * @param i - row index
     * @param j - col index
     * 
     * @return boolean
     */
    IsEmpty(i,j) {
        return this.arr[i][j].ID == 0
    }

    /** 
     * @description For every square the provided shape points to, change those squares to be under
     * the ownership of the shape
     * 
     * @param Shape - A Shape object
     * 
     * @return void
     */
    PlaceShape(Shape) {
        Shape.RemoveShape()
        for (var i = 0; i < Shape.Squares.length; i++) {
            var s = Shape.Squares[i]
            this.arr[s.i][s.j].ChangeOwner(s.ID,s.Color)
        }
    }

    /** 
     * @description Sets all squares pointed to by this shape to be empty
     * 
     * @param Shape - A Shape object
     * 
     * @return void
     */
    RemoveShape(Shape) {
        for (var i = 0; i < Shape.Squares.length; i++) {
            var s = Shape.Squares[i]
            this.arr[s.i][s.j].SetEmpty()
        }
    }

    /** 
     * @description Check to see if the provided shape can move in the provided directions
     * 
     * @param Shape - A Shape object
     * @param left - integer representing how far left the shape should move
     * @param right - integer representing how far right the shape should move
     * @param down - integer representing how far down the shape should move
     * 
     * @return CollisionType (Type of collision, defined in constructor)
     */
    IsValidMovement(Shape, left, right, down) {
        var sqs = Shape.Squares
        for (var s = 0; s < sqs.length; s++) {
            var new_row = sqs[s].i + down
            var new_col = sqs[s].j - left + right

            var ColType = this.IsValidSquare(Shape.ID,new_row,new_col)
            if (ColType != this.CollisionType.NoCollision) {
                return ColType
            }
        }
        return this.CollisionType.NoCollision
    }

    /** 
     * @description Checks to see if the square at the provided indices can be moved to
     * 
     * @param ID - ID of the shape attempting to move
     * @param i - row index
     * @param j - col index
     * 
     * @return CollisionType (Type of collision, defined in constructor)
     */
    IsValidSquare(ID,i,j) {
        // check out of bounds
        if (i < 0 || i >= this.row_count || j < 0 || j >= this.column_count) {
            return this.CollisionType.OutOfBounds
        }

        if (this.arr[i][j].IsEmpty() || this.arr[i][j].ID == ID) {
            return this.CollisionType.NoCollision
        }

        if (this.arr[i][j].IsFrozen()) {
            return this.CollisionType.FrozenObject
        }

        // check square ownership
        return this.CollisionType.OtherPlayer
    }

    /** 
     * @description Rotates the shape with the provided ID 90 degrees clockwise, does nothing if
     * the shape cannot be rotated
     * 
     * @param ID - ID of the shape attempting to move
     * 
     * @return void
     */
    async RotateShape(ID, reply=true) {
        var Shape = this.ShapeArray[ID-1]

        // returns in the form [new squares, blueprint, dimensions]
        var narr = Shape.RotateIndices(this.row_count,this.column_count)

        // for all the new provided indices, check to ensure that they can be used
        var newSquares = []
        var boardIndices = []
        for (var i = 0; i < narr[0].length; i++) {
            if (this.IsValidSquare(Shape.ID,narr[0][i][0],narr[0][i][1]) == this.CollisionType.NoCollision) {
                boardIndices.push([narr[0][i][0],narr[0][i][1]])
                newSquares.push(this.arr[narr[0][i][0]][narr[0][i][1]])
            } else {
                // couldn't rotate the shape, so just return
                return
            }
        }

        // reply to the server if it was a player-made move
        if (reply) {
            this.SendAction(ID, boardIndices, "rotate")
        }
        
        Shape.UpdateAfterRotate(newSquares, narr[1], narr[2])
        this.PlaceShape(Shape)
    }

    /** 
     * @description Attempts to move the shape at the provided index in the provided directions
     * 
     * @param ID - ID of a shape object
     * @param left - integer representing how far left the shape should move
     * @param right - integer representing how far right the shape should move
     * @param down - integer representing how far down the shape should move
     * 
     * @return void
     */
    async MoveShape(ID, left, right, down) {
        var Shape = this.ShapeArray[ID-1]
        var ColType = this.IsValidMovement(Shape,left,right,down)
        if (ColType == this.CollisionType.NoCollision) {
            Shape.MoveShape(this.arr,left,right,down)
        } else {
            this.CheckFreeze(Shape,down,ColType)
        }
    }

    /** 
     * @description Forces the shape to move in the specified direction, does not send movement back to
     * the server, and does not check collision
     * 
     * @param ID - ID of a shape object
     * @param left - integer representing how far left the shape should move
     * @param right - integer representing how far right the shape should move
     * @param down - integer representing how far down the shape should move
     * 
     * @return void
     */
    async ForceMoveShape(ID, left, right, down) {
        this.ShapeArray[ID-1].MoveShape(this.arr, left, right, down, false)
    }

    /** 
     * @description Check to see if a shape should be frozen and removed from the owner's control.
     * It will replace the shape with a new one if the current shape is frozen
     * 
     * @param Shape - A Shape object
     * @param down - integer representing how far down the shape should move
     * @param ColType - Collision type of the objects current collision
     * 
     * @return void
     */
    CheckFreeze(Shape, down, ColType) {
        if (down == 1 && (ColType == this.CollisionType.OutOfBounds || ColType == this.CollisionType.FrozenObject)) {
            Shape.Freeze()
            this.ShapeArray[Shape.ID - 1] = this.InstantiateShape(Shape.ID,null,0,Shape.ID*5,false)
        }
    }

    /** 
     * @description Move every shape on the game board in the specified directions
     * 
     * @param left - integer representing how far left the shape should move
     * @param right - integer representing how far right the shape should move
     * @param down - integer representing how far down the shape should move
     * 
     * @return void
     */
    async MoveAllShapes(left, right, down) {
        for (var s = 0; s < this.ShapeArray.length; s++) {
            // check collision type
            var ColType = this.IsValidMovement(this.ShapeArray[s],left,right,down)
            if (ColType == this.CollisionType.NoCollision) {
                this.ShapeArray[s].MoveShape(this.arr,left,right,down,false)
            } else {
                // if shape cannot move, decide if it is removed from owners control
                this.CheckFreeze(this.ShapeArray[s],down, ColType)
            }
        }
    }

    /** 
     * @description Creates a shape object
     * 
     * @param ID - ID of a shape object
     * @param ShapeBlueprint - Square 2D array with 0 values for empty spots and 1s where the shape occurs
     * @param RowOffset - integer representing how far down the shape should spawn
     * @param ColOffset - integer representing how far across the shape should spawn
     * @param RandomOffset - boolean, if set to true, the shape will start at a random column offset
     * 
     * @return Shape
     */
    InstantiateShape(ID, ShapeBlueprint=null, RowOffset=0, ColOffset=0, RandomOffset=false) {
        var NewShape;
        if (ShapeBlueprint == null) {
            NewShape = new Shape(ID)
        } else {
            NewShape = new Shape(ID, ShapeBlueprint)
        }

        // set a random column offset
        if (RandomOffset) {
            ColOffset = Math.floor(Math.random()*(this.column_count - NewShape.ShapeDimensions[2]))
        }

        // ensure the shape can be placed along the x-axis given the width of the board and the shape's starting point
        if (ColOffset + NewShape.ShapeDimensions[2] > this.column_count) {
            console.log("Invalid shape at specified offset. Out of bounds.")
            return
        }

        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 4; j++) {
            // if the blueprint has a square at this location, we attempt to place it
            if (NewShape.ShapeBlueprint[i][j] == 1) {
                var iOffset = i-NewShape.ShapeDimensions[0]+RowOffset
                var jOffset = j-NewShape.ShapeDimensions[1]+ColOffset

                // if this spot is not empty, then we cannot spawn a square here
                if (!this.arr[iOffset][jOffset].IsEmpty()) {
                    //console.log("GAME OVER")
                } else {
                // Always place the shape as if it were in a bounding box
                //this.PlaceSquare(iOffset,jOffset,NewShape.ID,NewShape.Color)
                NewShape.AddSquare(this.arr[iOffset][jOffset])
                }
            }
            }
        }
        return NewShape
    }
    
    /** 
     * @description Called 60 times a second and draws the gameArray
     * 
     * @param RowTranslation - How many rows the drawing should be translated down
     * @param ColTranslation - How many columns the drawing should be translated down
     * 
     * @return void
     */
    Draw(RowTranslation, ColTranslation) {
        push();
        translate(RowTranslation, ColTranslation)
        rectMode(CORNER)
        for (var i = 0; i < this.row_count; i++) {
            for (var j = 0; j < this.column_count; j++) {
                this.arr[i][j].Draw()
            }
        }

        // draw bounding rectangle
        var strokeW = 10
        noFill();
        stroke("pink")
        strokeWeight(strokeW)
        rect(0-strokeW/2, 0-strokeW/2, this.SquareEdgeLength*this.column_count+strokeW, this.SquareEdgeLength*this.row_count+strokeW,10)

        
        pop();
    }

    /** 
     * @description Forces the shape of player with id 'ID' to be replaced with a new shape. Usefull for testing
     * 
     * @param ID - ID of a shape object
     * @param ShapeBlueprint - Square 2D array with 0 values for empty spots and 1s where the shape occurs
     * @param RowOffset - integer representing how far down the shape should spawn
     * @param ColOffset - integer representing how far across the shape should spawn
     * @param RandomOffset - boolean, if set to true, the shape will start at a random column offset
     * 
     * @return void
     */
    ForceChangeShape(ID, ShapeBlueprint, rowOffset, columnOffset, randomOffset) {
        this.ShapeArray[ID-1] = this.InstantiateShape(ID, ShapeBlueprint, rowOffset, columnOffset, randomOffset)
        this.PlaceShape(this.ShapeArray[ID-1])
    }

    /** 
     * @description Sends a user action to the server
     * 
     * @param ID - ID of a shape object
     * @param boardIndices - Indices in the game array that the shape will take after performing the action
     * @param action - action the shape is taking
     * 
     * @return void
     */
    SendAction(ID, boardIndices, action) {
        var data = JSON.stringify({"lobbyID":(team.lobbyToken).toLowerCase(),"playerID":ID,"shapeIndices": boardIndices, "move": action})
        socket.send(JSON.stringify({"type": "6", "data": data}))
    }
}

/* This export is used for testing*/
module.exports = [GameArray]
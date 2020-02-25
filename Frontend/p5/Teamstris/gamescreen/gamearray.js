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
        this.InstantiateSquares(this.SquareEdgeLength)

        this.NumPlayers = NumPlayers
        this.ShapeArray = new Array(this.NumPlayers)
        this.ShapeArray[0] = this.InstantiateShape(1,0,0)
        this.ShapeArray[1] = this.InstantiateShape(2,0,5)
        this.PlaceShape(this.ShapeArray[0])
        this.PlaceShape(this.ShapeArray[1])

        // allows easy determination of whenv to freeze an object
        this.CollisionType = {
            OutOfBounds : 1,
            FrozenObject : 2,
            OtherPlayer : 3,
            NoCollision : 4
        }

        //console.log(this.CollisionType)
    }//end constructor

    // Set every indice within the game array to be a square object
    InstantiateSquares(SquareEdgeLength) {
        for (var i = 0; i < this.row_count; i++) {
            for (var j = 0; j < this.column_count; j++) {
                this.arr[i][j] = new Square(SquareEdgeLength)
                this.arr[i][j].SetPosition(i,j)
            }
        }
    }

    // get the square at the provided indices
    GetSquare(i,j) {
        return this.arr[i][j]
    }

    // set the square at the provided indices
    SetSquare(i,j,ID,Color) {
        this.arr[i][j].ChangeOwner(ID,Color)
    }

    // returns true if the ID of the square is 0
    IsEmpty(i,j) {
        return this.arr[i][j].ID == 0
    }

    // for every square pointed to by the provided shape, set that square to be owned by the shape
    PlaceShape(Shape) {
        Shape.RemoveShape()
        for (var i = 0; i < Shape.Squares.length; i++) {
            var s = Shape.Squares[i]
            this.arr[s.i][s.j].ChangeOwner(s.ID,s.Color)
        }
    }

    // remove the provided shape from the game board
    RemoveShape(Shape) {
        for (var i = 0; i < Shape.Squares.length; i++) {
            var s = Shape.Squares[i]
            this.arr[s.i][s.j].SetEmpty()
        }
    }
    // check to see if the square can move in the provided direction
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

    // you can move to a square if it is empty or contains your own squares
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

    RotateShape(ID) {
        var Shape = this.ShapeArray[ID-1]

        // returns in the form [new squares, blueprint, dimensions]
        var narr = Shape.RotateIndices(this.row_count,this.column_count)

        // for all the new provided indices, check to ensure that they can be used
        var newSquares = []
        for (var i = 0; i < narr[0].length; i++) {
            if (this.IsValidSquare(Shape.ID,narr[0][i][0],narr[0][i][1]) == this.CollisionType.NoCollision) {
                newSquares.push(this.arr[narr[0][i][0]][narr[0][i][1]])
            } else {
                // couldn't rotate the shape, so just return
                return
            }
        }

        Shape.UpdateAfterRotate(newSquares, narr[1], narr[2])
        this.PlaceShape(Shape) 
    }

    // move the shape in the provided direction if it is valid
    MoveShape(ID, left, right, down) {
        var Shape = this.ShapeArray[ID-1]
        var ColType = this.IsValidMovement(Shape,left,right,down)
        if (ColType == this.CollisionType.NoCollision) {
            Shape.MoveShape(this.arr,left,right,down)
        } else {
            this.CheckFreeze(Shape,down,ColType)
        }
    }

    // freezes the object if necessary
    CheckFreeze(Shape, down, ColType) {
        if (down == 1 && (ColType == this.CollisionType.OutOfBounds || ColType == this.CollisionType.FrozenObject)) {
            Shape.Freeze()
            this.ShapeArray[Shape.ID - 1] = this.InstantiateShape(Shape.ID,0,0,true)
        }
    }

    // move every shape on the game board in the provided direction if valid
    MoveAllShapes(left, right, down) {

        for (var s = 0; s < this.ShapeArray.length; s++) {
            // check collision type
            var ColType = this.IsValidMovement(this.ShapeArray[s],left,right,down)
            if (ColType == this.CollisionType.NoCollision) {
                this.ShapeArray[s].MoveShape(this.arr,left,right,down)
            } else {
                // if shape cannot move, decide if it is removed from owners control
                this.CheckFreeze(this.ShapeArray[s],down, ColType)
            }
        }
    }

    // Create shape on the gamearray. Spawnlocation denotes the top left, x-axis, offset.
    InstantiateShape(owner, RowOffset=0, ColOffset=0, RandomOffset=false) {
        var NewShape = new Shape(owner)

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
                    console.log("GAME OVER")
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

    Draw(row_translation, col_translation) {
        push();
        translate(row_translation, col_translation)
        for (var i = 0; i < this.row_count; i++) {
            for (var j = 0; j < this.column_count; j++) {
                this.arr[i][j].Draw()
            }
        }
        pop();
    }
}
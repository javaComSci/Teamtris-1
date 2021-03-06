/** 
  * @classDesc Represents lowest functional unit of teamtris
  */
class Square {
	constructor (SquareEdgeLength, ID=0, Color="grey", RandomPowerCube=true, eps=0.3) {
		// edge length of the square
		this.SquareEdgeLength = SquareEdgeLength

		// ID of the player who 'owns' this square
		this.ID = ID

		// Color of the square
		this.DefaultColor = "grey";
		this.Color = Color

		// edge color of the square
		//this.DefaultGridStroke = DefaultGridStroke

		// i, j location of the square in the game array
		this.i = -1
		this.j = -1

		this.eps = eps

		this.PowerCubeTypes = {
            DEFAULT : 1,
            DESTROYCOL : 2,
            DESTROYAREA : 3,
		}

		this.PowerCubeType = this.PowerCubeTypes.DEFAULT
		// if (RandomPowerCube && Math.random() < this.eps) {
		// 	this.PowerCubeType = Math.max(Math.floor(Math.random()+1 * 2),3)
		// 	this.PowerCubeType = 3;
		// }

		// once this power cube has been used, set applied to true
		this.applied = false
		

		// // define the power cube for this square
		// this.PowerCube = new PowerCube(this.PowerCubeTypes.DEFAULT)
	}

	/** 
     * @description Sets class variables representing this square's position in the game array
	 * 
	 * @param i - row index
	 * @param j - column index
	 * 
     * @return void
     */
	SetPosition(i,j) {
		this.i = i
		this.j = j
	}

	/** 
     * @description Sets this square to be an empty square
	 * 
     * @return void
     */
	SetEmpty() {
		this.ID = 0
		this.Color = this.DefaultColor
		this.PowerCubeType = this.PowerCubeTypes.DEFAULT
		this.applied = false;
	}

	/** 
     * @description Sets this square to be a frozen square
	 * 
	 * @param pct - Power cube type
	 * 
     * @return void
     */
	SetFrozen(pct=null) {
		if (pct != null) {
			this.PowerCubeType = pct
		}
		this.ID = 5
		this.Color = "grey"
		this.applied = false
	}

	/** 
     * @description Sets the power cube type of this square
	 * 
	 * @param PowerCube - Type of powercube, defined under this.PowerCubeTypes
	 * 
     * @return void
     */
	SetPowerCube(PowerCube) {
		this.PowerCubeType = PowerCube
	}

	/** 
     * @description Randomly sets the PowerCubeType of the square
	 * 
     * @return void
     */
	SetRandomPowerCube() {
		this.PowerCubeType = this.PowerCubeTypes.DEFAULT
		if (Math.random() < this.eps) {
			this.PowerCubeType = this.PowerCubeTypes.DESTROYCOL
		}
	}

	GetPowerCubeColor() {
		switch (this.PowerCubeType) {
			case this.PowerCubeTypes.DESTROYCOL:
				return "green"
			case this.PowerCubeTypes.DESTROYAREA:
				return "purple"
		}
		return "black"
	}

	/** 
     * @description Returns true if this square is frozen
	 * 
     * @return boolean
     */
	IsFrozen() {
		return this.ID == 5
	}

	/** 
     * @description Returns true if this square is empty (ID == 0)
	 * 
     * @return boolean
     */
	IsEmpty() {
		return (this.ID == 0)
	}

	/** 
     * @description Sets the applied variable of the square
	 * 
     * @return void
     */
	SetApplied() {
		this.applied = true
	}

	/** 
     * @description returns if the square has been applied
	 * 
     * @return boolean
     */
	IsApplied() {
		return this.applied
	}

	/** 
     * @description Changes the owner of the square
	 * 
	 * @param ID - ID of the owner of the square
	 * @param Color - color to set the square to
	 * 
     * @return boolean
     */
	ChangeOwner(ID, Color, PowerCubeType=this.PowerCubeTypes.DEFAULT) {
		this.ID = ID
		this.Color = Color
		this.PowerCubeType = PowerCubeType
		this.applied = false
	}

	/** 
     * @description Called everytime we want to draw a square
	 * 
     * @return void
     */
	Draw(alpha=255) {
		push();
		stroke("silver")
		strokeWeight(1)
		if (this.ID == 0) {
			noFill()
		} else {
			let c = color(this.Color)
			c.setAlpha(alpha)
			fill(c)
		}
		rect(this.j * this.SquareEdgeLength, this.i * this.SquareEdgeLength, this.SquareEdgeLength, this.SquareEdgeLength)
		//console.log(this.PowerCubeType)
		if (this.PowerCubeType != this.PowerCubeTypes.DEFAULT) {
			fill(this.GetPowerCubeColor())
			rect(this.j * this.SquareEdgeLength + this.SquareEdgeLength*0.25, this.i * this.SquareEdgeLength+this.SquareEdgeLength*0.25, this.SquareEdgeLength*0.5, this.SquareEdgeLength*0.5)
		}
		pop();
	}
}

/* This export is used for testing*/
module.exports = [Square]

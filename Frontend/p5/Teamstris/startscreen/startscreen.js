
class StartScreen {
	/**
	 * constructor: Init setup for the launch screen. This will add the buttons and
	 * create the default values for everything.
	 * 
	 * @param void
	 * 
	 * @returns StartScreen
	 */
	constructor() {
		if (startscreen_constructor) console.log("Creating StartScreen Object");
		/* 							X, Y 				 , W  			  , H 				 , gamestate, default color	*/
		buttonList.push(new Buttons(0, windowHeight / 2.8, windowWidth / 5, windowHeight / 10, 0, "blue"));
		buttonList[buttonList.length - 1].text = "Create Game"; // Text to put in the button
		buttonList[buttonList.length - 1].hoverColor = "yellow"; // What color to make the button on mouse hover
		buttonList[buttonList.length - 1].id = "createGame"; // ID of the button

		buttonList.push(new Buttons(0, windowHeight / 4, windowWidth / 5, windowHeight / 10, 0, "red"));
		buttonList[buttonList.length - 1].text = "Join game"; // Text to put in the button
		buttonList[buttonList.length - 1].hoverColor = "yellow"; // What color to make the button on mouse hover
		buttonList[buttonList.length - 1].id = "joinGame"; // ID of the button

		this.TokenBoxText = ""; // default token

		this.usernameBoxStroke = false; // true: highlight box red. false: go back to normal

		this.usernameText = "username"; // default username
		this.usernameTextTouched = false; // checks to see if the box has been touched by the user yet

		this.gameStateStartScreen = 0; // where in the start screen you are. @draw has a good comment on this.

		this.titleAnimation = [300, 500, 400, 700] //drops the peices 
	}

	/**
	 * draw: This funcion will be ran at 60 frames a second and will call all the functions
	 * 	     needed to draw the launch screen.
	 */
	draw() {
		this.drawTitle(); // Draws the title
		this.animateTitle(); // Draws the T's dropping
		this.drawHighScoreButton();
		// this.titleVarible -= 4;
		if (startscreen_draw) console.log("Drawing on Startscreen");
		Buttonloop();
		/**
		 * This switch statment will tell us where we are on the launch screen i.e
		 * 0 - username box field and join/create game buttons are active
		 * 1 - token field and accept button fields are active
		 */
		switch (this.gameStateStartScreen) {
			case 0:
				this.drawUsernameBox(); // Draws the usernameBox
				break;
			case 1:
				this.drawTokenBox();
				break;
		}
	}

	/**
	 * animateTitle: Will check and add/subtract the locations of the T's falling when you
	 * 				 go to the launch screen. Once the animation is done, this function will 
	 * 				 return instantly.
	 * 
	 * @param void
	 * 
	 * @returns void
	 */
	animateTitle() {
		for (let i = 0; i < this.titleAnimation.length; i++) {
			if (this.titleAnimation[i] > 0) {
				this.titleAnimation[i] -= 10;
			}
		}
	}

	/**
	 * drawUsernameBox: This function will draw the username bot onto the screen 
	 * 
	 * @param void
	 * 
	 * @returns void
	 */
	drawUsernameBox() {
		push(); // Push my settings
		translate(windowWidth / 2, windowHeight / 2); // translate cord plane to center of screen
		fill(255); // setting my color to white
		if(this.usernameBoxStroke == true) {
			stroke("red"); // change to red
			strokeWeight(4); // make it thicker
		}
		rect(0, windowHeight / 15, windowWidth / 3, windowHeight / 12, 15); // drawing my username textbox
		stroke("black"); // reset my old stroke color
		strokeWeight(0); //reset my old strokeweight
		textSize(windowWidth / 30);
		fill(0, 0, 0, 100); // Set alpha to 100
		if (this.usernameTextTouched) { // If they are touching it then make it black.
			fill(0, 0, 0, 255); // Black with 100% alpha
		}
		text(this.usernameText, 0, windowHeight / 15); // draw username into text box. 
		pop(); // restore my settings
	}

	/**
	 * drawTitle: This function will draw the title (Teamtris) onto the 
	 *            launch screen.
	 * 
	 * @param void
	 * 
	 * @returns void
	 * 
	 * @BUG Somehow the first square is sometimes below or above the other letters.
	 * This is only a problem with significantly different screen sizes
	 */
	drawTitle() {
		push(); // Push my settings
		let eamPosY; // the words 'eam' y position 
		let squareSize = windowWidth / 28; // The size of all the squares making up the T's
		let spaceBetweenSquares = windowWidth / 19;
		translate(windowWidth / 2, 0); //Translate to the top middle of the screen
		fill(255, 0, 0); // Fill red
		textSize(windowWidth / 7); // Set text size relative to windowWidth 
		text("eam", -windowWidth / 20, eamPosY = windowHeight / 3); // draw the "eam" in red on the launch screen
		fill(0, 0, 255); // fill blue
		text("ris", windowWidth / 4.2, eamPosY); // draw the "ris" in blue on the screen
		/**
		 * These recs are for the First T
		 */
		let yStart; // Bottom box of the T's
		rect(-windowWidth / 4.3, (yStart = windowHeight / 2.6) - this.titleAnimation[0], squareSize, squareSize) // first T, top blue
		rect(-windowWidth / 4.3, (yStart - (spaceBetweenSquares)) - this.titleAnimation[0], squareSize, squareSize) // first T, bot blue
		fill(255, 0, 0) // fill red
		rect(-windowWidth / 4.3, yStart - (2 * spaceBetweenSquares) - this.titleAnimation[1], squareSize, squareSize) // first T, top red middle
		rect(-windowWidth / 4.3 - spaceBetweenSquares, yStart - (2 * spaceBetweenSquares) - this.titleAnimation[1], squareSize, squareSize) // first T, top red right
		rect(-windowWidth / 4.3 + spaceBetweenSquares, yStart - (2 * spaceBetweenSquares) - this.titleAnimation[1], squareSize, squareSize) // first T, top red left
		/**
		 * These recs are for the second T
		 */
		fill(0, 0, 255) // fill blue
		rect(windowWidth / 8, yStart - this.titleAnimation[2], squareSize, squareSize) // second T, bot blue
		rect(windowWidth / 8, yStart - (spaceBetweenSquares) - this.titleAnimation[3], squareSize, squareSize) // first T, top blue
		fill(255, 0, 0) // fill red
		rect(windowWidth / 8, yStart - (2 * spaceBetweenSquares) - this.titleAnimation[3], squareSize, squareSize) // second T, top red middle
		rect(windowWidth / 8 - spaceBetweenSquares, yStart - (2 * spaceBetweenSquares) - this.titleAnimation[3], squareSize, squareSize) // second T, top red left
		rect(windowWidth / 8 + spaceBetweenSquares, yStart - (2 * spaceBetweenSquares) - this.titleAnimation[3], squareSize, squareSize) // second T, top red right
		pop(); // reset settings
	}

	drawTokenBox() {
		push();
		let ret = false;
		translate(windowWidth / 2, windowHeight / 2)
		fill(255);
		rect(0, 0, windowWidth / 4, windowWidth / 4, 50)
		fill(0)
		textSize(windowWidth / 30)
		text("Enter Token", 0, -windowWidth / 15)
		text(this.TokenBoxText, 0, windowHeight / 15)
		strokeWeight(10)
		line(-windowWidth / 13, windowHeight / 10, windowWidth / 13, windowHeight / 10);
		fill(0)
		strokeWeight(5)
		if ((mouseX - (windowWidth / 2) >= (0 - (windowWidth / 8) / 2)) && (mouseX - (windowWidth / 2) <= (0 + (windowWidth / 8) / 2))) {
			if ((mouseY - (windowHeight / 2) >= (windowHeight / 5.5 - (windowHeight / 15) / 2)) && (mouseY - (windowHeight / 2) <= (windowHeight / 5.5 + (windowHeight / 15) / 2))) {
				fill(255)
				ret = false;
			}
		}

		rect(0, windowHeight / 5.5, windowWidth / 8, windowHeight / 15, 5)
		fill(255)
		if ((mouseX - (windowWidth / 2) >= (0 - (windowWidth / 8) / 2)) && (mouseX - (windowWidth / 2) <= (0 + (windowWidth / 8) / 2))) {
			if ((mouseY - (windowHeight / 2) >= (windowHeight / 5.5 - (windowHeight / 15) / 2)) && (mouseY - (windowHeight / 2) <= (windowHeight / 5.5 + (windowHeight / 15) / 2))) {
				fill(0)
				ret = true;
			}
		}
		textSize(30)
		text("Accept", 0, windowHeight / 5.5)
		pop();
		return ret;
	}

	mouseClickedStart() {
		switch (this.gameStateStartScreen) { // Switch statment to tell us what we are looking at
			case 0:
				if (ClickedLoop() == "joinGame") {
					/* Check to see if they trying to go into game with a bad username */
					if (!this.usernameTextTouched || this.usernameText == "") {
						this.usernameBoxStroke = true;
					} else {
						this.gameStateStartScreen = 1;
						buttonList[FindButtonbyID("joinGame")].invalid = true;
						buttonList[FindButtonbyID("createGame")].invalid = true;
					}
				} else if (ClickedLoop() == "createGame") {
					/* Check to see if they trying to go into game with a bad username */
					if (!this.usernameTextTouched || this.usernameText == "") {
						this.usernameBoxStroke = true;
					} else {
						/* Creating my lobbyscreen object */
  						mLobbyScreen = new LobbyScreen(new Player(this.usernameText, Math.floor(Math.random() * 100), true));
						gameState = 1; // Switch to lobby screen
					}
				} else if (this.drawHighScoreButtonCheckMouse() == true) { // if they click highscore
					this.gameStateStartScreen = -1;
					gameState = 3; //send to score screen.
				} else {
				}
				break;
			case 1:
				if (this.drawTokenBox() == true) { // Checks to see if you clicked on the accept button
					/** Need to check token @todo */
					var randomID = Math.floor(Math.random() * 100);
					var data = JSON.stringify({"lobbyID":this.TokenBoxText,"name": this.usernameText,"playerID": randomID})
					socket.send(JSON.stringify({"type": "0", "data": data}))
					socket.onmessage = (event) => {
						if(event.data == "bad"){
							this.TokenBoxText = "";
						}else {
							mLobbyScreen = new LobbyScreen(new Player(this.usernameText, randomID, false));
							mLobbyScreen.team.lobbyToken = this.TokenBoxText;
							gameState = 1;
						}
					}
				} else if (this.drawHighScoreButtonCheckMouse() == true) { // if they click highscore
					gameState = 3; //send to score screen.
				}
				break; // run
		}
	}

	/**
	 * drawHighScoreButtonCheckMouse: Will tell whoever calls it if the mouse if over the highscore screen. 
	 * 								  This will mostly be used when a user hovers over or clicks. 
	 * 
	 * @param void
	 * 
	 * @return Boolean
	 * 		   true - mouse is over highscore
	 * 		   false - mouse is NOT over highscore
	 */
	drawHighScoreButtonCheckMouse() {
		this.LeftX = (windowWidth / 1.038) + (windowWidth / 16) / 2; // Left side of the box cords
		this.RightX = (windowWidth / 1.038) - (windowWidth / 16) / 2; // Right side of the box cords
		this.TopY = (windowHeight / 1.05) - (windowHeight / 14) / 2; // Top of the box cords 
		this.BotY = (windowHeight / 1.05) + (windowHeight / 14) / 2; // Bot of the box cords
		if ((mouseX >= this.RightX) && (mouseX <= this.LeftX)) { // Check if the mouse is within the X range. 
			if ((mouseY >= this.TopY) && (mouseY <= this.BotY)) { // Check if the mouse is within the Y range. 
				return true; // If the mouse is within the X bounds AND within the Y bound we know the mouse is in the box. Return true
			}
		}
		return false; // Return false if either of these things dont hold.
	}

	/**
	 * drawHighScoreButton: Draws the 3 bars at the bottom right to represent the high score
	 * 
	 * @param void
	 * 
	 * @returns void
	 */
	drawHighScoreButton() {
		push(); // Push settings
		translate(0, 0);
		let fillHighScore = "white"; // default value is white
		if (this.drawHighScoreButtonCheckMouse()) { // Checks if the mouse is over the highscore
			fillHighScore = "rgb(0,255,0)"; // if the mouse is over, it will change the boxes to green
		}
		fill(fillHighScore) // fills with whatever color from above
		rectMode(CORNER) // sets mode to CORNER, makes it easier to draw boxes
		rect(windowWidth / 1.065, windowHeight / 1.02, windowWidth / 70, -windowHeight / 35, 4); //left 
		rect(windowWidth / 1.045, windowHeight / 1.02, windowWidth / 70, -windowHeight / 20, 4); // middle
		rect(windowWidth / 1.025, windowHeight / 1.02, windowWidth / 70, -windowHeight / 55, 4); // right
		pop(); // Restore my settings
	}

	/**
	 * keyPressedStart: This function will be called whenever the user clicked on a button on the start screen. 
	 * 					@link{general/keyPressed.js} is where this function will be called. 
	 * 
	 * @param void
	 * 
	 * @returns void
	 */
	keyPressedStart() {
		console.log("keyCode: " + keyCode);
		switch (this.gameStateStartScreen) {
			case 0: // If we are on the username text box
				if (keyCode == 8) { // "pressed delete"
					if (this.usernameTextTouched == false) { //check to see if username is still default
						this.usernameText = ""; // set it to an empty string
						this.usernameTextTouched = true; // set the usernametouched to true 
					}
					/* Remove the last char from the username field */
					this.usernameText = this.usernameText.substring(0, this.usernameText.length - 1);
				} else if (this.usernameTextTouched == false) { // If they pressed something for the first time
					if ((keyCode >= 65 && keyCode <= 90) || keyCode == 32) { // checks to see if its a letter or a space
						this.usernameText = ""; // set username to an empty string
						this.usernameTextTouched = true; // set touched to true so other parts know username has been tocuhed
						this.usernameText += String.fromCharCode(keyCode); // add the pressed key to the username
					} // do nothing if its not a valid entry
				} else if (this.usernameText.length < 11) { // make sure the length of the string doesnt get too long
					if ((keyCode >= 65 && keyCode <= 90) || keyCode == 32) { // checks to see if its a letter or a space
						this.usernameText += String.fromCharCode(keyCode); // add the pressed key to the username
					}
				}
				break; // run.
			case 1: // This is when the token box is open, we need to accept input then.
				if (keyCode == 8) { // "pressed delete"
					/* Remove the last char from the username field */
					this.TokenBoxText = this.TokenBoxText.substring(0, this.TokenBoxText.length - 1);
				} else if (keyCode == 27) { // pressed esc
					this.gameStateStartScreen = 0; // go back to the username field active
					buttonList[FindButtonbyID("joinGame")].invalid = false; // reactivate buttons
					buttonList[FindButtonbyID("createGame")].invalid = false; // reactivate buttons
				} else if (this.TokenBoxText.length < 4) { // make sure the length of the string doesnt get too long
					if ((keyCode >= 65 && keyCode <= 90) || keyCode == 32) { // checks to see if its a letter or a space
						this.TokenBoxText += String.fromCharCode(keyCode); // add the pressed key to the username
					}
				}
				break;
		}
	}
}
/* This export is used for testing*/
module.exports = [StartScreen];
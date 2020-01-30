class StartScreen {
	constructor() {
		if (startscreen_constructor) console.log("Creating StartScreen Object");
		buttonList.push(new Buttons(0, windowHeight / 2.8, windowWidth / 5, windowHeight / 10, 0, "blue"));
		buttonList[buttonList.length - 1].text = "Create Game";
		buttonList[buttonList.length - 1].hoverColor = "yellow";
		buttonList[buttonList.length - 1].id = "createGame"
		buttonList.push(new Buttons(0, windowHeight/4, windowWidth / 5, windowHeight / 10, 0, "red"));
		buttonList[buttonList.length - 1].text = "Join game";
		buttonList[buttonList.length - 1].hoverColor = "yellow";
		buttonList[buttonList.length - 1].id = "joinGame"

		this.TokenBoxText = "";

		this.usernameText = "username";
		this.usernameTextTouched = false;

		this.gameStateStartScreen = 0;
	}

	draw() {
		this.drawTitle();
		this.drawUsernameBox();
		if (startscreen_draw) console.log("Drawing on Startscreen");
		switch (this.gameStateStartScreen) {
			case 0:
				break;
			case 1:
				this.drawTokenBox();
				break;
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
		rect(0, windowHeight / 15, windowWidth / 3, windowHeight / 12, 15); // drawing my username textbox
		textSize(windowWidth / 30);
		fill(0, 0, 0, 100); // Set alpha to 100
		text(this.usernameText, 0, windowHeight / 15); // draw username into text box. 
		pop(); // restore my settings
	}

	/**
	 * drawTitle: This function will draw the title (Teamtris) onto the 
	 *            launch screen
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
		rect(-windowWidth / 4.3, yStart = windowHeight / 2.6, squareSize, squareSize) // first T, top blue
		rect(-windowWidth / 4.3, yStart - (spaceBetweenSquares), squareSize, squareSize) // first T, bot blue
		fill(255, 0, 0) // fill red
		rect(-windowWidth / 4.3, yStart - (2 * spaceBetweenSquares), squareSize, squareSize) // first T, top red middle
		rect(-windowWidth / 4.3 - spaceBetweenSquares, yStart - (2 * spaceBetweenSquares), squareSize, squareSize) // first T, top red right
		rect(-windowWidth / 4.3 + spaceBetweenSquares, yStart - (2 * spaceBetweenSquares), squareSize, squareSize) // first T, top red left
		/**
		 * These recs are for the second T
		 */
		fill(0, 0, 255) // fill blue
		rect(windowWidth / 8, yStart, squareSize, squareSize) // second T, bot blue
		rect(windowWidth / 8, yStart - (spaceBetweenSquares), squareSize, squareSize) // first T, top blue
		fill(255, 0, 0) // fill red
		rect(windowWidth / 8, yStart - (2 * spaceBetweenSquares), squareSize, squareSize) // second T, top red middle
		rect(windowWidth / 8 - spaceBetweenSquares, yStart - (2 * spaceBetweenSquares), squareSize, squareSize) // second T, top red left
		rect(windowWidth / 8 + spaceBetweenSquares, yStart - (2 * spaceBetweenSquares), squareSize, squareSize) // second T, top red right
		pop(); // reset settings
	}

	drawTokenBox() {
		push();
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
			}
		}

		rect(0, windowHeight / 5.5, windowWidth / 8, windowHeight / 15, 5)
		fill(255)
		if ((mouseX - (windowWidth / 2) >= (0 - (windowWidth / 8) / 2)) && (mouseX - (windowWidth / 2) <= (0 + (windowWidth / 8) / 2))) {
			if ((mouseY - (windowHeight / 2) >= (windowHeight / 5.5 - (windowHeight / 15) / 2)) && (mouseY - (windowHeight / 2) <= (windowHeight / 5.5 + (windowHeight / 15) / 2))) {
				fill(0)
			}
		}
		textSize(30)
		text("Accept", 0, windowHeight / 5.5)
		pop();
	}

	mouseClickedStart() {
		if (this.checkTokenBox == true) {
			if ((mouseX - (windowWidth / 2) >= (0 - (windowWidth / 8) / 2)) && (mouseX - (windowWidth / 2) <= (0 + (windowWidth / 8) / 2))) {
				if ((mouseY - (windowHeight / 2) >= (windowHeight / 5.5 - (windowHeight / 15) / 2)) && (mouseY - (windowHeight / 2) <= (windowHeight / 5.5 + (windowHeight / 15) / 2))) {
					var data = JSON.stringify({
						"lobbyID": this.TokenBoxText,
						"name": "notbob",
						"playerID": "123"
					})
					socket.send(JSON.stringify({
						"type": "0",
						"data": data
					}))
					socket.onmessage = function (event) {
						console.log(event.data);
						if (event.data == "bad") {
							this.TokenBoxText = "";
						} else {
							gameState = 1;
						}
					};
				}
			}
		}
		if (startscreen_mouseClickedStart) console.log("CLICKED HERE");
		let buttonID = ClickedLoop();
		if (startscreen_mouseClickedStart) console.log("buttonID: " + buttonID);
		if (buttonID == "createGame") {
			if (startscreen_mouseClickedStart) console.log("Create Game Clicked");
			owner = true;
			mLobbyScreen.setup();
			gameState = 1;
		} else if (buttonID == "joinGame") {
			if (startscreen_mouseClickedStart) console.log("Join Game Clicked");
			owner = false;
			this.checkTokenBox = true;
			buttonList[FindButtonbyID("joinGame")].invalid = true;
			buttonList[FindButtonbyID("createGame")].invalid = true;
		}
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
				if (keyCode == 13) { //pressed "enter"
					/**
					 * Need to send player into lobby with the correct things.
					 */
					console.log("PRESSED ENTER")
				} else if (keyCode == 8) { // "pressed delete"
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
				break;
		}
		// if (this.checkTokenBox) {
		// 	if (this.TokenBoxText.length < 4 && keyCode != BACKSPACE && keyCode != ENTER) {
		// 		this.TokenBoxText += String.fromCharCode(keyCode);
		// 	}
		// 	if (keyCode == BACKSPACE) {
		// 		this.TokenBoxText = "";
		// 	}
		// 	// if(keyCode == ENTER){
		// 	//   this.checkTokenBox = true;
		// 	//   this.mouseClickedStart();
		// 	// }
		// }
	}
}
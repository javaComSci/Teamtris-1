/**
 * #class ScoreScreen |
 * @author Steven Dellamore, JavaComSci |
 * @language javascript | 
 * @desc Displays the score screen to the user when 
 * the @inline{gameState = 3}. |
 */
class ScoreScreen {

	/**
     * #function ScoreScreen::constructor |
	 * @desc ScoreScreen is going to setup all init values
     * for the screen. @link{initValuesLobbyContor1}
     * We will also have to ask the server for the scores like so:
     * @link{requestScores1} and finally, we have to listen
     * for incoming packets from the server: @link{contorLobbyScreenVar1} |
     * @header constructor() | 
	 * @returns ScoreScreen : An object of scorescreen class | 
	 */
    constructor( player, team, fromGameScreen = true ) {
        // #code initValuesLobbyContor1 javascript
        this.player = player;
        this.fromGameScreen = fromGameScreen;
        this.scoreArray = [];
        this.team = team;
        // |

        buttonList.push(new Buttons(windowWidth/2.5, windowHeight / 2.6, windowWidth / 7, windowHeight / 12, 3, "green"));
        buttonList[buttonList.length - 1].text = "Back"; // Text to put in the button
        buttonList[buttonList.length - 1].hoverColor = "yellow"; // What color to make the button on mouse hover
        buttonList[buttonList.length - 1].id = "back"; // ID of the button

        // #code requestScores1 javascript
        // var data = JSON.stringify(
        //     {"maxPlayers":"4","name": this.player.username,"playerID": this.player.id})
        // socket.send(JSON.stringify({"type": "1", "data": data}));
        // |
        socket.onmessage = (event) => {
            var e = JSON.parse(event.data);
            console.log("HERE WE GO SCORE ");
            console.log(e);
        };
    }
 
    /**
     * #function ScoreScreen::drawTitle |
	 * @desc TODO |
     * @header drawTitle() | 
	 */
    drawTitle() {

    }

    /**
     * #function ScoreScreen::renderScores |
	 * @desc TODO |
     * @header renderScores() | 
	 */
    renderScores() {

    }

    /**
     * #function ScoreScreen::mouseClickedScore |
	 * @desc Will be called whenever the user clicks and
     * @inline{gameState == 3}. Will render a new startScreen
     * if the user clicks on the back button. |
     * @header mouseClickedScore() | 
	 */
    mouseClickedScore() {
        if(ClickedLoop() == "back") {
            mStartScreen = new StartScreen();
            gameState = 0;
        }
    }

    /**
     * #function ScoreScreen::keyPressedScore |
	 * @desc TODO |
     * @header keyPressedScore() | 
	 */
    keyPressedScore() {
    }

    /**
     * #function ScoreScreen::draw |
	 * @desc This will be called 60 times a second in 
     * @inline{"sketch.js"}. It will render everything onto the 
     * screen. |
     * @header draw() | 
	 */
    draw() {
        Buttonloop();
        this.drawTitle();
        this.renderScores();
    }
}
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
     * @header constructor(player, team, fromGameScreen = true) | 
	 * @returns ScoreScreen : An object of scorescreen class | 
	 */
    constructor( player, team, fromGameScreen = true ) {
        // #code initValuesLobbyContor1 javascript
        this.player = player;
        this.fromGameScreen = fromGameScreen;
        this.scoreArray = [];
        this.team = team;
        // |

        this.scoreScreenRequest = false;

        buttonList.push(new Buttons(windowWidth/2.5, windowHeight / 2.6, windowWidth / 7, windowHeight / 12, 3, "black"));
        buttonList[buttonList.length - 1].text = "Home"; // Text to put in the button
        buttonList[buttonList.length - 1].hoverColor = "blue"; // What color to make the button on mouse hover
        buttonList[buttonList.length - 1].id = "back"; // ID of the button
        buttonList[buttonList.length - 1].round = 5;
        buttonList[buttonList.length - 1].borderWeight = 2;
        buttonList[buttonList.length - 1].border = 255;
        buttonList[buttonList.length - 1].textColor = 255;

        // var teamMembers;
        // if(this.team == undefined) {
        //     teamMembers = undefined;
        // } else {
        //     teamMembers = this.team.playersInTeam.map(player => player.username);
        // }
        // // #code requestScores1 javascript
        // var data = JSON.stringify(
        //     {"teamName": "", 
        //     "playerNames": teamMembers, 
        //     "teamScore": 0, 
        //     "timePlayed": 0})
        // // socket.send(JSON.stringify({"type": "11", "data": data}));
        // // |
        // socket.onmessage = (event) => {
        //     var e = JSON.parse(event.data);
        //     console.log("HERE WE GO SCORE ");
        //     console.log(e);
        // };
    }
 
    /**
     * #function ScoreScreen::drawTitle |
	 * @desc TODO |
     * @header drawTitle() | 
	 */
    drawTitle() {
        push();
        translate(windowWidth/2, windowHeight/7);
        fill(255,0,0);
        stroke(255);
        strokeWeight(0);
        textSize(windowHeight/5)
        text("Top Scores",0,0)
        pop();
    }

    /**
     * #function ScoreScreen::renderScores |
	 * @desc TODO |
     * @header renderScores() | 
	 */
    renderScores() {
        push();
        var yHeight = windowHeight/1.5;
        translate(windowWidth/2, windowHeight/1.6);
        fill(0,0,255);
        strokeWeight(0);
        rect(0,-yHeight/2.19,windowWidth/3,yHeight/12);
        fill(255,0,0);
        strokeWeight(0);
        rect(0,yHeight/2.19,windowWidth/3,yHeight/12);
        strokeWeight(1);
        stroke(255);
        fill(0,0,0,0);
        rect(0,0,windowWidth/3,yHeight, 3);
        line(-windowWidth/3/2,-yHeight/2 + (1 * yHeight/12),windowWidth/3/2,-yHeight/2 + (1 * yHeight/12));
        line(-windowWidth/3/2,-yHeight/2 + (11 * yHeight/12),windowWidth/3/2,-yHeight/2 + (11 * yHeight/12));

        strokeWeight(1);
        stroke(0);
        fill(0);
        textSize(windowHeight/40);
        var posOfNum = createVector(-windowWidth/3/2.4, -yHeight/2.2);
        text("No.",posOfNum.x, posOfNum.y);
        text("Team Name",posOfNum.x + windowWidth/10, posOfNum.y);
        text("Score",posOfNum.x + windowWidth/5, posOfNum.y);
        text("Time",posOfNum.x + windowWidth/4, posOfNum.y);
        fill(255);
        textSize(windowHeight/40);
        for(var i = 1; i < 11; i++){
            text(i + ".",posOfNum.x, (posOfNum.y + (i * yHeight/12.2)));
            text("MyTeamName",posOfNum.x + windowWidth/10, (posOfNum.y + (i * yHeight/12.2)));
            text(0,posOfNum.x + windowWidth/5, (posOfNum.y + (i * yHeight/12.2)));
            text(1,posOfNum.x + windowWidth/4, (posOfNum.y + (i * yHeight/12.2)));
        }
        if(this.team == undefined){
            text("-",posOfNum.x, (posOfNum.y + (11 * yHeight/12)));
            text("-",posOfNum.x + windowWidth/10, (posOfNum.y + (11 * yHeight/12)));
            text('-',posOfNum.x + windowWidth/5, (posOfNum.y + (11 * yHeight/12)));
            text('-',posOfNum.x + windowWidth/4, (posOfNum.y + (11 * yHeight/12)));
        } else {
            text("38.",posOfNum.x, (posOfNum.y + (11 * yHeight/12)));
            text("CoolPerson",posOfNum.x + windowWidth/10, (posOfNum.y + (11 * yHeight/12)));
            text(195,posOfNum.x + windowWidth/5, (posOfNum.y + (11 * yHeight/12)));
            text(48,posOfNum.x + windowWidth/4, (posOfNum.y + (11 * yHeight/12)));
        }

        pop();
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
    // \"teamScore\":3,\"timePlayed\":50}"
    requestTopScores() {
        this.scoreScreenRequest = true;
        var teamMembers;
        if(this.team == undefined) {
            socket.send(JSON.stringify({
                "teamName": "-1", 
                "playerNames": "null",
                "teamScore": "null",
                "timePlayed": "null"
            }));
        } else {
            teamMembers = this.team.playersInTeam.map(player => player.username);
            socket.send(JSON.stringify({
                "teamName": this.team.teamName, 
                "playerNames": teamMembers,
                "teamScore": this.team.teamScore,
                "timePlayed": this.team.time
            }));
        }

        socket.onmessage = (event) => {
            var e = JSON.parse(event.data);
            console.log("SCORE SCREEN GOT SOMETHING ");
            console.log(e);
        };
    }

    /**
     * #function ScoreScreen::draw |
	 * @desc This will be called 60 times a second in 
     * @inline{"sketch.js"}. It will render everything onto the 
     * screen. |
     * @header draw() | 
	 */
    draw() {
        if(!this.scoreScreenRequest)
            this.requestTopScores()
        Buttonloop();
        this.drawTitle();
        this.renderScores();
    }
}
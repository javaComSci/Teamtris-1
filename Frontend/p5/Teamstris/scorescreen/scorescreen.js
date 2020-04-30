var base64ToImage = import('base64-to-image');
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
        this.currentTeamsRank = -1;
        this.shareButtonDisplayed = false;

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
        if(this.scoreArray.length > 0){
            // if(!this.shareButtonDisplayed){
            //     buttonList.push(new Buttons(windowWidth/2.5, windowHeight / 3.5, windowWidth / 7, windowHeight / 12, 3, "black"));
            //     buttonList[buttonList.length - 1].text = "Share"; // Text to put in the button
            //     buttonList[buttonList.length - 1].hoverColor = "blue"; // What color to make the button on mouse hover
            //     buttonList[buttonList.length - 1].id = "share"; // ID of the button
            //     buttonList[buttonList.length - 1].round = 5;
            //     buttonList[buttonList.length - 1].borderWeight = 2;
            //     buttonList[buttonList.length - 1].border = 255;
            //     buttonList[buttonList.length - 1].textColor = 255;
            //     this.shareButtonDisplayed = true;
            // }
            for(var i = 1; i < this.scoreArray.length+1; i++){
                text(i + ".",posOfNum.x, (posOfNum.y + (i * yHeight/12.2)));
                text(this.scoreArray[i-1].teamName,posOfNum.x + windowWidth/10, (posOfNum.y + (i * yHeight/12.2)));
                text(this.scoreArray[i-1].teamScore,posOfNum.x + windowWidth/5, (posOfNum.y + (i * yHeight/12.2)));
                text(this.scoreArray[i-1].timePlayed,posOfNum.x + windowWidth/4, (posOfNum.y + (i * yHeight/12.2)));
            }
            if(this.team == undefined){
                text("-",posOfNum.x, (posOfNum.y + (11 * yHeight/12)));
                text("-",posOfNum.x + windowWidth/10, (posOfNum.y + (11 * yHeight/12)));
                text('-',posOfNum.x + windowWidth/5, (posOfNum.y + (11 * yHeight/12)));
                text('-',posOfNum.x + windowWidth/4, (posOfNum.y + (11 * yHeight/12)));
            } else {
                if(!this.shareButtonDisplayed){
                    buttonList.push(new Buttons(windowWidth/2.5, windowHeight / 3.5, windowWidth / 7, windowHeight / 12, 3, "black"));
                    buttonList[buttonList.length - 1].text = "Share"; // Text to put in the button
                    buttonList[buttonList.length - 1].hoverColor = "blue"; // What color to make the button on mouse hover
                    buttonList[buttonList.length - 1].id = "share"; // ID of the button
                    buttonList[buttonList.length - 1].round = 5;
                    buttonList[buttonList.length - 1].borderWeight = 2;
                    buttonList[buttonList.length - 1].border = 255;
                    buttonList[buttonList.length - 1].textColor = 255;
                    this.shareButtonDisplayed = true;
                }
                text(this.currentTeamsRank,posOfNum.x, (posOfNum.y + (11 * yHeight/12)));
                text(this.team.teamName,posOfNum.x + windowWidth/10, (posOfNum.y + (11 * yHeight/12)));
                text(this.team.score,posOfNum.x + windowWidth/5, (posOfNum.y + (11 * yHeight/12)));
                text(this.team.time,posOfNum.x + windowWidth/4, (posOfNum.y + (11 * yHeight/12)));
            }
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
        if(ClickedLoop() == "share") {
            console.log("SHARERE")
            socketShare.onmessage = (event) => {
                var e = JSON.parse(event.data);
                console.log("SHARE DATA BELOW");
                console.log(e);
                // var img;
                // var raw = new Image();
                // raw.src='data:' + e.data; // base64 data here
                // img = loadImage('data:image/png;base64, ' + e.data);
                // img = loadImage('data:image/png;base64, ' + e.data);
                // var img = loadImage('data:image/jpg;base64,'+e.data);
                // img.save('photo', 'jpg')
                var img;
                var raw = new Image();
                raw.src='data:image/jpeg;base64,' + e.data; // base64 data here
                raw.onload = function() {
                    img = createImage(raw.width, raw.height);
                    img.drawingContext.drawImage(raw, 0, 0);
                    // image(img, 0, 0); // draw the image, etc here
                    img.save('Score', 'jpg')
                }
            };
            var data = JSON.stringify({
                "teamName": this.team.teamName, 
                // "teamName": "FEA",
            });
            socketShare.send(JSON.stringify({"type": "123431", "data": data}));
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
        socketScoreWithNoTeamName.onmessage = (event) => {
            var e = JSON.parse(event.data);
            // console.log("SCORE SCREEN GOT SOMETHING ");
            // console.log(e.topTeamInfos);
            this.scoreArray = e.topTeamInfos;
        };
        socketScore.onmessage = (event) => {
            
            var e = JSON.parse(event.data);
            // console.log("SCORE SCREEN GOT SOMETHING ");
            // console.log(e);
            this.scoreArray = e.topTeamInfos;
            console.log(e.topTeamInfos)
            this.currentTeamsRank = e.currentTeamInfo.rank;
        };
        this.scoreScreenRequest = true;
        // var teamMembers = [];
        // this.team = new Team();
        // this.team.playersInTeam.push(new Player("steven", 1, true));
        // this.team.playersInTeam.push(new Player("john", 1, false));
        // this.team.playersInTeam.push(new Player("indhu", 1, false));
        // this.team.teamName = "testingmyteam";
        // this.team.lobbyToken = "1234";
        // this.team.score = 401;
        // this.team.time = 30;
        this.team = team;
        if(this.team == undefined) {
            // console.log(socketScore.readyState)
            // socket.send(JSON.stringify({"type": "7", "data": data}));
            socketScoreWithNoTeamName.send(JSON.stringify({"type": "11"}));
            /* Test */
        } else {
            // if(scoreArray)
            if(socketScore.readyState == 3){
                this.scoreArray.push({
                    "teamName": "Test1",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 824,
                    "timePlayed": 188,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test2",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 788,
                    "timePlayed": 168,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test3",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 768,
                    "timePlayed": 160,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test4",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 755,
                    "timePlayed": 158,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test5",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 745,
                    "timePlayed": 155,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test6",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 735,
                    "timePlayed": 145,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test7",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 704,
                    "timePlayed": 140,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test8",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 700,
                    "timePlayed": 139,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test9",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 689,
                    "timePlayed": 130,
                    "rank": 1
                })
                this.scoreArray.push({
                    "teamName": "Test10",
                    "playerNames": ["a", "b", "c", "d"],
                    "teamScore": 680,
                    "timePlayed": 115,
                    "rank": 1
                })
                this.currentTeamsRank = 11;
            } else {
                console.log("GHOING")
                var teamMembers = [];
                for(var i = 0; i < this.team.playersInTeam.length; i++) {
                    teamMembers.push(this.team.playersInTeam[i].username)
                }
                teamMembers.sort()
                var data = JSON.stringify({
                    "teamName": this.team.teamName, 
                    "playerNames": teamMembers,
                    "teamScore": this.team.score,
                    "timePlayed": this.team.time
                });
                console.log("data")
                console.log(data);
                socketScore.send(JSON.stringify({"type": "10", "data": data}));
            }
        }
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
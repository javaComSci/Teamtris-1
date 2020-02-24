

// const expect = require('chai').expect;

const button = require('../Teamstris/general/buttons.js');
const startScreen = require('../Teamstris/startscreen/startScreen.js');

/* vars being used for testing */
var mStartScreen;

global.gameState = 0;

var lol = false;
var numTests = 1;

/* p5 stuff */
global.windowWidth = 2560;
global.windowHeight = 1600;
global.CORNER = 0;
global.ARROW = 0;
global.mouseX = 30;
global.mouseY = 30;
global.createCanvas = function (x,y) { }
global.push = function () { }
global.pop = function () { }
global.translate = function () { }
global.fill = function () { }
global.textSize = function () { }
global.text = function () { }
global.rect = function () { }
global.rectMode = function () { }
global.cursor = function () { }

/* Debug vars */
global.startscreen_constructor       = false;
global.startscreen_draw              = false;
global.startscreen_mouseClickedStart = false;
global.buttons_constructor           = false;
global.buttons_draw                  = false;
global.buttons_checkmouse            = false;

/* Button class vars */
global.buttonList = button[0];
global.Buttons = button[1];
global.Buttonloop = button[2];
global.ClickedLoop = button[3];
global.FindButtonbyID = button[4];

/* color */
var red = '\x1b[31m%s\x1b[0m';
var green = '\x1b[32m%s\x1b[0m';
var blue = "\x1b[35m";

/* sleep */
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

testRunnerSetupStartScreen();

function CheckSame( given, expect, name, debug = false ){
    if( debug ){
        console.log(blue, "given: " + given + " expect: " + expect + " name: " + name);
    }
    if(given === expect){
        console.log(green, numTests++ + ". " + name + " passed"); 
        return true;
    } else {
        console.log(red, numTests++ + ". " + name + " failed should have been " + expect + " but was " + given);
        return false;
    }
}


async function testDefaultUsername() {
    CheckSame(mStartScreen.usernameText,"username","testDefaultUsername");
}

async function testDefaultTokenValue() {
    CheckSame(mStartScreen.TokenBoxText,"","testDefaultUsernameUserText");
}
  
async function testCheckInitStartScreenValues() {
    CheckSame(mStartScreen.usernameTextTouched,false,"checkInitStartScreenValues.usernameTextTouched");
    CheckSame(mStartScreen.titleAnimation[0],300,"checkInitStartScreenValues.titleAnimation[0]");
    CheckSame(mStartScreen.titleAnimation[1],500,"checkInitStartScreenValues.titleAnimation[1]");
    CheckSame(mStartScreen.titleAnimation[2],400,"checkInitStartScreenValues.titleAnimation[2]");
    CheckSame(mStartScreen.titleAnimation[3],700,"checkInitStartScreenValues.titleAnimation[3]");
}

async function testCheckTitlePosAfterTwoDraw() {
    mStartScreen.draw();
    mStartScreen.draw();
    CheckSame(mStartScreen.titleAnimation[0],280,"checkInitStartScreenValuesAfterTwoDraw.titleAnimation[0]");
    CheckSame(mStartScreen.titleAnimation[1],480,"checkInitStartScreenValuesAfterTwoDraw.titleAnimation[1]");
    CheckSame(mStartScreen.titleAnimation[2],380,"checkInitStartScreenValuesAfterTwoDraw.titleAnimation[2]");
    CheckSame(mStartScreen.titleAnimation[3],680,"checkInitStartScreenValuesAfterTwoDraw.titleAnimation[3]");
}

async function testChangeUserUsername() {
    global.keyCode = 65; // A
    mStartScreen.keyPressedStart();
    mStartScreen.drawUsernameBox();
    CheckSame(mStartScreen.usernameText,"A","testChangeUserUsername1");
    CheckSame(mStartScreen.usernameTextTouched,true,"testUsernameTextTouched");
    global.keyCode = 66; // A
    mStartScreen.keyPressedStart();
    mStartScreen.drawUsernameBox();
    CheckSame(mStartScreen.usernameText,"AB","testChangeUserUsername2");
}

async function testChangeMaxUsername() {
    mStartScreen.usernameText = "";
    CheckSame(mStartScreen.usernameText,"","testUsernameTextReset");
    global.keyCode = 65; // A
    var str = "";
    var strFull = "ABCDEFGHIJK"
    
    for(var i = 0; i < 15; i++) {
        if(lol) await new Promise(r => setTimeout(r, 100));
        mStartScreen.keyPressedStart();
        str += strFull.charAt(i);
        CheckSame(mStartScreen.usernameText,str,"testUsernameText" + str);
        global.keyCode++;
    }
}

async function testDeleteUsername() {
    global.keyCode = 8; // delete
    mStartScreen.keyPressedStart();
    var str = mStartScreen.usernameText;
    for(var i = 0; i < 15; i++) {
        if(lol) await new Promise(r => setTimeout(r, 200));
        mStartScreen.keyPressedStart();
        str = str.substr(0, str.length - 1);
        if (str == "") {
            CheckSame(mStartScreen.usernameText,str,"testUsernameTextNothing" + str);
        } else {
            CheckSame(mStartScreen.usernameText,str,"testUsernameText" + str);
        }
    }
}

async function testCheckSpecialChars() {
    global.keyCode = 10; // special
    mStartScreen.keyPressedStart();
    CheckSame(mStartScreen.usernameText,"","testCheckSpecialChars");

    global.keyCode = 240; // special
    mStartScreen.keyPressedStart();
    CheckSame(mStartScreen.usernameText,"","testCheckSpecialChars");

    global.keyCode = 33; // special
    mStartScreen.keyPressedStart();
    CheckSame(mStartScreen.usernameText,"","testCheckSpecialChars");
}

async function testHighScoreButton() {
    global.mouseX = mStartScreen.RightX + 1;
    global.mouseY = mStartScreen.TopY + 1;
    CheckSame(mStartScreen.gameStateStartScreen,0,"testCheckInitGameState");
    CheckSame(mStartScreen.drawHighScoreButtonCheckMouse(),true,"testDrawHighScoreButtonCheckMouse");
    mStartScreen.mouseClickedStart();
    CheckSame(mStartScreen.gameStateStartScreen,-1,"testMouseClickedScoreButton1");
    mStartScreen.gameStateStartScreen = 0; //reset gameState;
    global.mouseX += 100;
    mStartScreen.mouseClickedStart();
    if(lol) await new Promise(r => setTimeout(r, 300));
    CheckSame(mStartScreen.gameStateStartScreen,-1,"testMouseClickedScoreButton2");
    CheckSame(global.gameState,3,"testMouseClickedScoreButton2"); // 3 == score screen game state
    mStartScreen.gameStateStartScreen = 0; //reset gameState;
    global.gameState = 0; // reset gameState
    global.mouseX += 100;
    mStartScreen.mouseClickedStart();
    CheckSame(mStartScreen.gameStateStartScreen,0,"testMouseClickedScoreButtonMissed");
    if(lol) await new Promise(r => setTimeout(r, 1000));
    CheckSame(global.gameState,0,"testMouseClickedScoreButtonMissedRealGamestate");
}

async function testCreateGameButton() {
    global.mouseX = mStartScreen.RightX + 1;
    global.mouseY = mStartScreen.TopY + 1;
    CheckSame(mStartScreen.gameStateStartScreen,0,"testCheckInitGameState");
    // CheckSame(mStartScreen.drawHighScoreButtonCheckMouse(),true,"testDrawHighScoreButtonCheckMouse");
    // mStartScreen.mouseClickedStart();
    // CheckSame(mStartScreen.gameStateStartScreen,-1,"testMouseClickedScoreButton1");
}

async function testJoinLobbyButton() {
    // global.mouseX = mStartScreen.RightX + 1;
    // global.mouseY = mStartScreen.TopY + 1;
    // CheckSame(mStartScreen.gameStateStartScreen,0,"testCheckInitGameStateJoinButton");
    // CheckSame(mStartScreen.drawHighScoreButtonCheckMouse(),true,"testDrawHighScoreButtonCheckMouse");
    // mStartScreen.mouseClickedStart();
}

async function testRunnerSetupStartScreen() {
    mStartScreen = new startScreen[0];
    await testDefaultUsername();
    await testDefaultTokenValue();
    await testCheckInitStartScreenValues();
    await testCheckTitlePosAfterTwoDraw();
    await testChangeUserUsername();
    await testChangeMaxUsername();
    await testDeleteUsername();
    await testCheckSpecialChars();
    await testHighScoreButton();
    await testCreateGameButton();
    await testJoinLobbyButton();
    console.log(mStartScreen);
}



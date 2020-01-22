class StartScreen {
  constructor() {
    if(startscreen_constructor) console.log("Creating StartScreen Object");
    buttonList.push(new Buttons(0, windowHeight / 2.3, windowWidth / 16, windowHeight / 15,0));
  }

  draw() {
    if(startscreen_draw) console.log("Drawing on Startscreen");
  }

  // drawButton(){
  //   /* push all my settings */
  //   push();
  //   /* translate my postion to the center of the screen */
  //   translate(windowWidth / 2, windowHeight / 2);
  //   /* Setting my strokeweight */
  //   strokeWeight(4);
  //   /* Setting my stroke color */
  //   // stroke(0, 0, 0, mstartScreen.checkPlayButton());
  //   /* Drawing my rectangle */
  //   strokeWeight(0);
  //   textSize(windowHeight / 30);
  //   /* Setting style to Georgia because it looks good */
  //   textFont('Georgia');
  //   /* Write the back onto the box */
  //   text("Play", 0, windowHeight / 2.25);
  //   /* popping all my settings so other functions dont have to deal with them */
  //   stroke(0, 0, 0, 255);
  //   strokeWeight(1);
  //   // fill(0, 0, 0, mstartScreen.checkPlayButton())
  //   // rect(0, windowHeight / 2.3, windowWidth / 16, windowHeight / 15, 20);
  //   buttonList.push(new Buttons(0, windowHeight / 2.3, windowWidth / 16, windowHeight / 15,0));
  //   pop();
  // }
}

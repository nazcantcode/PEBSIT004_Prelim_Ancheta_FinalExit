# FINAL EXIT

**Project:** `PEBSIT004_Prelim_Ancheta_FinalExit`  
**Course:** PEBSIT 004 LAB - IT Elective 4: Mobile Application Development 2  
**Student:** Naz Aaron B. Ancheta  
**Category:** Board Games or Brain Games  
**Target:** Android mobile device, landscape gameplay  
**Engine:** Unity 6.6 (6000.6.0f1)

## Concept
FINAL EXIT is a short puzzle-escape game. The player moves a character inside a reusable room, approaches one of three answer doors, and presses **CHOOSE** to confirm an answer. Correct answers award points and reveal code digits. Rooms 1-4 provide the digits used in the final code in Room 5. Wrong answers reduce lives.

## Controls

### Desktop / Editor
- `W` / Up Arrow - move up
- `S` / Down Arrow - move down
- `A` / Left Arrow - move left
- `D` / Right Arrow - move right
- Approach/touch a door, then press **CHOOSE**.

### Mobile
- **UP / DOWN / LEFT / RIGHT** - move
- **CHOOSE** - confirm the selected/touched door
- **PAUSE** - pause gameplay

## Gameplay Rules
- 3 lives at the start.
- 30-second answer timer per room.
- Correct answer with more than 15 seconds remaining: **+200 points**.
- Correct answer with 15 seconds or less remaining: **+100 points**.
- Wrong answer: lose **1 life**.
- Rooms 1-4 reveal code digits.
- Room 5 checks the four-digit code `3489`.
- Successful completion displays **FINAL EXIT CLEARED** and the final score.

## Scenes
- `Assets/Scenes/MainMenuScene`
- `Assets/Scenes/LoadingScene`
- `Assets/Scenes/GameScene`

## Scripts
- `GameManager.cs` - game state, puzzles, scoring, timer, lives, feedback, transitions and end states.
- `PlayerMovement.cs` - keyboard/mobile movement, room bounds, door selection and CHOOSE confirmation.
- `MainMenuManager.cs` - Main Menu navigation.
- `LoadingSceneManager.cs` - scripted loading screen.
- `TitleAnimation.cs` - title presentation.

## Final Features
- Main Menu
- How To Play
- Pixel-style FINAL EXIT title
- Visible player
- Keyboard movement
- Mobile directional controls
- Three answer doors
- CHOOSE confirmation
- Five puzzle rooms
- Timer, score, lives and room status
- Correct/wrong feedback
- Pause/Resume
- Blackout room transition
- Game Over
- Game Clear
- Final score display
- Retry / Play Again
- Main Menu return
- Android APK build and phone testing

## Build Status
**SUCCESSFUL.** The Android APK was built and installed on the phone for final testing.

## Final Test Summary
Final testing covered:
- Project opening and scene loading
- Main Menu
- Player movement and mobile controls
- Door selection and CHOOSE
- Correct-answer feedback and score updates
- Room progression
- Game Over
- Game Clear
- Retry / Play Again
- Main Menu return
- Android installation

## Development Issue
A UI layering issue initially caused the player to render behind the doors/puzzle elements. The Player object was moved lower in the Canvas hierarchy so it rendered in the intended order. The correction was re-tested successfully.

## Authorized Asset Record
See `AUTHORIZED_ASSETS_FINAL.md`. The exact publisher/source for each third-party pack must match the final Unity project before submission.

## Evidence
See `Screenshots/` for the supplied final screenshots:
1. Room 2 correct answer
2. Game Over
3. Game Clear
4. Main Menu
5. Loading screen

## Submission Notes
- Fill in Student Number and Section before submission if required.
- Keep the complete Unity project inspectable.
- Keep the successful APK/build evidence.
- Do not claim an asset as authorized unless its source/permission has been verified.

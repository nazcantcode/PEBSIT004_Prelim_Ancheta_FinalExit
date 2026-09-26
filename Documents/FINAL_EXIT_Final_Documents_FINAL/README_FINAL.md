# FINAL EXIT

**Project:** `PEBSIT004_Prelim_Ancheta_FinalExit`  
**Game Title:** FINAL EXIT  
**Course:** PEBSIT 004 LAB - IT Elective 4: Mobile Application Development 2  
**Student:** Naz Aaron B. Ancheta  
**Student No.:** 23-10715  
**Section:** BSIT 4A  
**Category:** Board Games or Brain Games  
**Target:** Android mobile device, landscape gameplay  
**Engine:** Unity 6.6 (6000.6.0f1)

## Concept
FINAL EXIT is a short 2D puzzle-escape game. The player moves a character inside a reusable room, approaches one of three answer doors, and presses **CHOOSE** to confirm an answer. Correct answers award points and reveal code digits. Rooms 1–4 provide the digits used in the final code in Room 5. Wrong answers reduce lives.

## Controls
### Desktop / Editor
- W / Up Arrow - move up
- S / Down Arrow - move down
- A / Left Arrow - move left
- D / Right Arrow - move right
- Approach/touch a door, then press CHOOSE.

### Mobile
- UP / DOWN / LEFT / RIGHT - move
- CHOOSE - confirm the selected/touched door
- PAUSE - pause gameplay

## Gameplay Rules
- 3 lives at the start.
- 30-second answer timer per room.
- Correct answer with more than 15 seconds remaining: +200 points.
- Correct answer with 15 seconds or less remaining: +100 points.
- Wrong answer: -1 life.
- Rooms 1–4 reveal code digits.
- Room 5 checks the final code 3489.
- Final room completion awards +500.
- Maximum score: 1300.

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
**SUCCESSFUL.** The Android APK was built, installed on the Android phone, and tested.

A dedicated screenshot of the build-success dialog was not captured; Android phone gameplay evidence and the completed test record are retained instead.

## Development Issue
A UI layering issue initially caused the player to render behind doors/puzzle elements. The Player object was moved lower in the Canvas hierarchy so it rendered in the intended order. The correction was re-tested successfully.

## Authorized Assets
See `AUTHORIZED_ASSETS_FINAL.md`.

## Evidence
See `EVIDENCE_INDEX.md` and the `Screenshots/` folder.

## Next Development Steps
- Add more visually distinct room environments.
- Add more puzzle variations and difficulty levels.
- Improve animations, transitions, and visual feedback.
- Add optional sound effects/music.
- Test additional Android devices and screen sizes.
- Further polish the UI and presentation.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // =========================================================
    // UI
    // =========================================================

    [Header("UI Text")]
    public TMP_Text roomText;
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text feedbackText;
    public TMP_Text puzzleText;

    [Header("Door Buttons")]
    public Button doorAButton;
    public Button doorBButton;
    public Button doorCButton;

    [Header("Door Text")]
    public TMP_Text doorAText;
    public TMP_Text doorBText;
    public TMP_Text doorCText;

    [Header("Pause Menu")]
    public GameObject pausePanel;

    [Header("End Game Panels")]
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

    [Header("Room Transition")]
    public GameObject blackoutPanel;

    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("End Game UI")]
    public TMP_Text finalScoreText;

    [Header("Puzzle Panel")]
    public GameObject puzzlePanel;


    // =========================================================
    // GAME VARIABLES
    // =========================================================

    private int currentRoom = 1;
    private int lives = 3;
    private int score = 0;

    private float timeLeft = 30f;

    private bool canAnswer = true;
    private bool gameEnded = false;
    private bool isPaused = false;
    private bool memoryPhase = false;

    private int correctDoor = 2;


    // =========================================================
    // FEEDBACK ANIMATION
    // =========================================================

    [Header("Feedback Animation")]
    [SerializeField] private float feedbackEnterDuration = 0.20f;
    [SerializeField] private float feedbackHoldDuration = 0.45f;
    [SerializeField] private float feedbackExitDuration = 0.24f;
    [SerializeField] private float feedbackSlideDistance = 18f;
    [SerializeField] private float feedbackShakeAmount = 6f;

    private Coroutine feedbackCoroutine;

    private CanvasGroup feedbackCanvasGroup;
    private RectTransform feedbackRect;

    private Vector2 feedbackBasePosition;
    private Vector3 feedbackBaseScale;


    // =========================================================
    // PUZZLE PANEL ANIMATION
    // =========================================================

    [Header("Puzzle Panel Animation")]
    [SerializeField] private float puzzleEnterDuration = 0.32f;
    [SerializeField] private float puzzleSlideDistance = 26f;
    [SerializeField] private float puzzleStartScale = 0.96f;

    private Coroutine puzzleAnimationCoroutine;

    private CanvasGroup puzzleCanvasGroup;
    private RectTransform puzzlePanelRect;

    private Vector2 puzzleBasePosition;
    private Vector3 puzzleBaseScale;


    // =========================================================
    // FEEDBACK COLORS
    // =========================================================

    private static readonly Color SuccessFeedbackColor =
        new Color(0.35f, 0.95f, 0.55f);

    private static readonly Color ErrorFeedbackColor =
        new Color(1.00f, 0.32f, 0.32f);

    private static readonly Color WarningFeedbackColor =
        new Color(1.00f, 0.78f, 0.28f);

    private static readonly Color FinalFeedbackColor =
        new Color(1.00f, 0.84f, 0.38f);


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameClearPanel.SetActive(false);
        blackoutPanel.SetActive(false);

        InitializeFeedback();
        InitializePuzzlePanel();

        LoadRoom(1);
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (isPaused || gameEnded || !canAnswer || memoryPhase)
            return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;

            TimeExpired();
        }

        UpdateHUD();
    }


    // =========================================================
    // DOOR SELECTION
    // =========================================================

    public void ChooseDoorA()
    {
        CheckAnswer(1);
    }

    public void ChooseDoorB()
    {
        CheckAnswer(2);
    }

    public void ChooseDoorC()
    {
        CheckAnswer(3);
    }


    // =========================================================
    // CHECK ANSWER
    // =========================================================

    void CheckAnswer(int selectedDoor)
    {
        if (!canAnswer ||
            gameEnded ||
            isPaused ||
            memoryPhase)
        {
            return;
        }

        // =========================
        // CORRECT ANSWER
        // =========================

        if (selectedDoor == correctDoor)
        {
            canAnswer = false;

            playerMovement.SetMovement(false);

            // -------------------------
            // FINAL ROOM
            // -------------------------

            if (currentRoom == 5)
            {
                score += 500;

                ShowFeedback(
                    "FINAL CODE ACCEPTED",
                    "+500 BONUS",
                    FinalFeedbackColor,
                    false,
                    1.0f
                );

                UpdateHUD();

                StartCoroutine(GameClearAfterDelay());

                return;
            }

            // -------------------------
            // ROOMS 1 - 4
            // -------------------------

            int pointsEarned;

            if (timeLeft > 15f)
            {
                pointsEarned = 200;
            }
            else
            {
                pointsEarned = 100;
            }

            score += pointsEarned;

            ShowFeedback(
                "CORRECT",
                "+" + pointsEarned +
                " POINTS   |   CODE DIGIT " +
                GetCodeDigit(currentRoom),
                SuccessFeedbackColor
            );

            UpdateHUD();

            StartCoroutine(NextRoomAfterDelay());
        }

        // =========================
        // WRONG ANSWER
        // =========================

        else
        {
            lives--;

            ShowFeedback(
                "WRONG ANSWER",
                "-1 LIFE",
                ErrorFeedbackColor,
                true
            );

            if (lives <= 0)
            {
                StartCoroutine(GameOverAfterFeedback());
            }
            else
            {
                ResetTimer();

                playerMovement.ResetPosition();
            }

            UpdateHUD();
        }
    }


    // =========================================================
    // CODE DIGITS
    // =========================================================

    int GetCodeDigit(int room)
    {
        if (room == 1)
            return 3;

        if (room == 2)
            return 4;

        if (room == 3)
            return 8;

        if (room == 4)
            return 9;

        return 0;
    }


    // =========================================================
    // ROOM TRANSITION
    // =========================================================

    IEnumerator NextRoomAfterDelay()
    {
        // Allow feedback animation to appear first.
        yield return new WaitForSeconds(1f);

        blackoutPanel.SetActive(true);

        yield return new WaitForSeconds(1f);

        currentRoom++;

        LoadRoom(currentRoom);

        blackoutPanel.SetActive(false);
    }


    IEnumerator GameClearAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        GameClear();
    }


    IEnumerator GameOverAfterFeedback()
    {
        canAnswer = false;

        playerMovement.SetMovement(false);

        yield return new WaitForSecondsRealtime(0.8f);

        GameOver();
    }


    // =========================================================
    // FEEDBACK INITIALIZATION
    // =========================================================

    void InitializeFeedback()
    {
        if (feedbackText == null)
            return;

        feedbackRect =
            feedbackText.rectTransform;

        feedbackBasePosition =
            feedbackRect.anchoredPosition;

        feedbackBaseScale =
            feedbackRect.localScale;

        feedbackCanvasGroup =
            feedbackText.GetComponent<CanvasGroup>();

        if (feedbackCanvasGroup == null)
        {
            feedbackCanvasGroup =
                feedbackText.gameObject.AddComponent<CanvasGroup>();
        }

        feedbackCanvasGroup.alpha = 0f;

        feedbackCanvasGroup.interactable = false;
        feedbackCanvasGroup.blocksRaycasts = false;

        feedbackText.raycastTarget = false;

        feedbackText.gameObject.SetActive(false);
    }


    // =========================================================
    // SHOW FEEDBACK
    // =========================================================

    void ShowFeedback(
        string headline,
        string detail,
        Color color,
        bool shake = false,
        float customHoldDuration = -1f)
    {
        if (feedbackText == null)
            return;

        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);

            feedbackCoroutine = null;
        }

        if (feedbackRect == null ||
            feedbackCanvasGroup == null)
        {
            InitializeFeedback();
        }

        string formattedHeadline =
            "<b>" + headline + "</b>";

        if (string.IsNullOrEmpty(detail))
        {
            feedbackText.text =
                formattedHeadline;
        }
        else
        {
            feedbackText.text =
                formattedHeadline +
                "\n<size=72%>" +
                detail +
                "</size>";
        }

        feedbackText.color = color;

        feedbackText.gameObject.SetActive(true);

        feedbackCanvasGroup.alpha = 0f;

        feedbackRect.anchoredPosition =
            feedbackBasePosition +
            Vector2.down *
            feedbackSlideDistance;

        feedbackRect.localScale =
            feedbackBaseScale * 0.88f;

        float holdDuration =
            customHoldDuration >= 0f
                ? customHoldDuration
                : feedbackHoldDuration;

        feedbackCoroutine =
            StartCoroutine(
                AnimateFeedback(
                    shake,
                    holdDuration
                )
            );
    }


    // =========================================================
    // FEEDBACK ANIMATION
    // =========================================================

    IEnumerator AnimateFeedback(
        bool shake,
        float holdDuration)
    {
        float elapsed = 0f;

        Vector2 startPosition =
            feedbackBasePosition +
            Vector2.down *
            feedbackSlideDistance;

        Vector3 startScale =
            feedbackBaseScale *
            0.88f;

        // -------------------------
        // ENTER
        // -------------------------

        while (elapsed < feedbackEnterDuration)
        {
            elapsed +=
                Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed /
                    feedbackEnterDuration
                );

            float movementT =
                EaseOutCubic(t);

            float scaleT =
                EaseOutBack(t);

            feedbackCanvasGroup.alpha =
                EaseOutCubic(t);

            feedbackRect.anchoredPosition =
                Vector2.LerpUnclamped(
                    startPosition,
                    feedbackBasePosition,
                    movementT
                );

            feedbackRect.localScale =
                Vector3.LerpUnclamped(
                    startScale,
                    feedbackBaseScale,
                    scaleT
                );

            yield return null;
        }

        feedbackCanvasGroup.alpha = 1f;

        feedbackRect.anchoredPosition =
            feedbackBasePosition;

        feedbackRect.localScale =
            feedbackBaseScale;


        // -------------------------
        // SHAKE FOR WRONG ANSWER
        // -------------------------

        if (shake)
        {
            float shakeDuration = 0.14f;

            elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                elapsed +=
                    Time.unscaledDeltaTime;

                float t =
                    Mathf.Clamp01(
                        elapsed /
                        shakeDuration
                    );

                float damping =
                    1f - t;

                float offsetX =
                    Mathf.Sin(
                        t *
                        Mathf.PI *
                        8f
                    ) *
                    feedbackShakeAmount *
                    damping;

                feedbackRect.anchoredPosition =
                    feedbackBasePosition +
                    Vector2.right *
                    offsetX;

                yield return null;
            }

            feedbackRect.anchoredPosition =
                feedbackBasePosition;
        }


        // -------------------------
        // HOLD
        // -------------------------

        elapsed = 0f;

        while (elapsed < holdDuration)
        {
            elapsed +=
                Time.unscaledDeltaTime;

            yield return null;
        }


        // -------------------------
        // FADE OUT
        // -------------------------

        elapsed = 0f;

        Vector2 exitPosition =
            feedbackBasePosition +
            Vector2.up *
            (feedbackSlideDistance * 0.75f);

        Vector3 exitScale =
            feedbackBaseScale * 0.97f;

        while (elapsed < feedbackExitDuration)
        {
            elapsed +=
                Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed /
                    feedbackExitDuration
                );

            float eased =
                EaseInCubic(t);

            feedbackCanvasGroup.alpha =
                1f - eased;

            feedbackRect.anchoredPosition =
                Vector2.Lerp(
                    feedbackBasePosition,
                    exitPosition,
                    eased
                );

            feedbackRect.localScale =
                Vector3.Lerp(
                    feedbackBaseScale,
                    exitScale,
                    eased
                );

            yield return null;
        }

        feedbackCanvasGroup.alpha = 0f;

        feedbackRect.anchoredPosition =
            feedbackBasePosition;

        feedbackRect.localScale =
            feedbackBaseScale;

        feedbackText.gameObject.SetActive(false);

        feedbackCoroutine = null;
    }


    // =========================================================
    // HIDE FEEDBACK IMMEDIATELY
    // =========================================================

    void HideFeedbackImmediate()
    {
        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);

            feedbackCoroutine = null;
        }

        if (feedbackText == null)
            return;

        if (feedbackCanvasGroup != null)
        {
            feedbackCanvasGroup.alpha = 0f;
        }

        if (feedbackRect != null)
        {
            feedbackRect.anchoredPosition =
                feedbackBasePosition;

            feedbackRect.localScale =
                feedbackBaseScale;
        }

        feedbackText.gameObject.SetActive(false);
    }


    // =========================================================
    // PUZZLE PANEL INITIALIZATION
    // =========================================================

    void InitializePuzzlePanel()
    {
        if (puzzlePanel == null)
            return;

        puzzlePanelRect =
            puzzlePanel.GetComponent<RectTransform>();

        puzzleCanvasGroup =
            puzzlePanel.GetComponent<CanvasGroup>();

        if (puzzleCanvasGroup == null)
        {
            puzzleCanvasGroup =
                puzzlePanel.AddComponent<CanvasGroup>();
        }

        puzzleBasePosition =
            puzzlePanelRect.anchoredPosition;

        puzzleBaseScale =
            puzzlePanelRect.localScale;

        puzzleCanvasGroup.alpha = 1f;

        // Prevent PuzzlePanel from blocking
        // the mobile controls.
        puzzleCanvasGroup.interactable = false;
        puzzleCanvasGroup.blocksRaycasts = false;

        if (puzzleText != null)
        {
            puzzleText.raycastTarget = false;
        }
    }


    // =========================================================
    // SHOW PUZZLE
    // =========================================================

    void ShowPuzzle(string text)
    {
        if (puzzleText == null)
            return;

        if (puzzleAnimationCoroutine != null)
        {
            StopCoroutine(puzzleAnimationCoroutine);

            puzzleAnimationCoroutine = null;
        }

        puzzleText.text = text;

        if (puzzlePanelRect == null ||
            puzzleCanvasGroup == null)
        {
            InitializePuzzlePanel();
        }

        // If no panel was assigned,
        // text still appears without animation.
        if (puzzlePanelRect == null ||
            puzzleCanvasGroup == null)
        {
            return;
        }

        // Reset first so changing puzzle while
        // another animation is playing stays clean.
        puzzlePanelRect.anchoredPosition =
            puzzleBasePosition;

        puzzlePanelRect.localScale =
            puzzleBaseScale;

        puzzleCanvasGroup.alpha = 1f;

        puzzleAnimationCoroutine =
            StartCoroutine(
                AnimatePuzzlePanel()
            );
    }


    // =========================================================
    // PUZZLE PANEL ANIMATION
    // =========================================================

    IEnumerator AnimatePuzzlePanel()
    {
        float elapsed = 0f;

        Vector2 startPosition =
            puzzleBasePosition +
            Vector2.up *
            puzzleSlideDistance;

        Vector3 startScale =
            puzzleBaseScale *
            puzzleStartScale;

        puzzleCanvasGroup.alpha = 0f;

        puzzlePanelRect.anchoredPosition =
            startPosition;

        puzzlePanelRect.localScale =
            startScale;


        // Smooth cinematic entrance
        while (elapsed < puzzleEnterDuration)
        {
            elapsed +=
                Time.unscaledDeltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed /
                    puzzleEnterDuration
                );

            float smooth =
                EaseOutCubic(t);

            puzzleCanvasGroup.alpha =
                smooth;

            puzzlePanelRect.anchoredPosition =
                Vector2.Lerp(
                    startPosition,
                    puzzleBasePosition,
                    smooth
                );

            puzzlePanelRect.localScale =
                Vector3.Lerp(
                    startScale,
                    puzzleBaseScale,
                    smooth
                );

            yield return null;
        }

        puzzleCanvasGroup.alpha = 1f;

        puzzlePanelRect.anchoredPosition =
            puzzleBasePosition;

        puzzlePanelRect.localScale =
            puzzleBaseScale;

        puzzleAnimationCoroutine = null;
    }


    // =========================================================
    // EASING FUNCTIONS
    // =========================================================

    float EaseOutCubic(float t)
    {
        float inverted =
            1f - t;

        return
            1f -
            inverted *
            inverted *
            inverted;
    }


    float EaseInCubic(float t)
    {
        return
            t *
            t *
            t;
    }


    float EaseOutBack(float t)
    {
        const float c1 = 1.25f;

        const float c3 =
            c1 + 1f;

        float x =
            t - 1f;

        return
            1f +
            c3 *
            x *
            x *
            x +
            c1 *
            x *
            x;
    }


    // =========================================================
    // LOAD ROOMS
    // =========================================================

    void LoadRoom(int roomNumber)
    {
        HideFeedbackImmediate();

        memoryPhase = false;

        SetDoorsVisible(true);


        // =====================================================
        // ROOM 1
        // =====================================================

        if (roomNumber == 1)
        {
            timeLeft = 30f;

            canAnswer = true;

            playerMovement.SetMovement(true);

            ShowPuzzle(
                "<size=70%>" +
                "<color=#D6B85A>" +
                "SEQUENCE ANALYSIS" +
                "</color>" +
                "</size>\n" +

                "<b>FIND THE NEXT NUMBER</b>\n\n" +

                "<size=125%>" +
                "2   4   8   16   ?" +
                "</size>"
            );

            doorAText.text =
                "DOOR A\n\n24";

            doorBText.text =
                "DOOR B\n\n32";

            doorCText.text =
                "DOOR C\n\n40";

            correctDoor = 2;
        }


        // =====================================================
        // ROOM 2
        // =====================================================

        else if (roomNumber == 2)
        {
            StartCoroutine(
                Room2MemoryChallenge()
            );
        }


        // =====================================================
        // ROOM 3
        // =====================================================

        else if (roomNumber == 3)
        {
            StartCoroutine(
                Room3ObservationChallenge()
            );
        }


        // =====================================================
        // ROOM 4
        // =====================================================

        else if (roomNumber == 4)
        {
            timeLeft = 30f;

            canAnswer = true;

            playerMovement.SetMovement(true);

            ShowPuzzle(
                "<size=70%>" +
                "<color=#D6B85A>" +
                "LOGIC TEST" +
                "</color>" +
                "</size>\n" +

                "<b>FIND THE SAFE NUMBER</b>\n\n" +

                "<size=82%>" +
                "DIVISIBLE BY 6\n" +
                "GREATER THAN 30\n" +
                "NOT DIVISIBLE BY 7\n" +
                "DIGITS ADD UP TO 9" +
                "</size>"
            );

            doorAText.text =
                "DOOR A\n\n24";

            doorBText.text =
                "DOOR B\n\n36";

            doorCText.text =
                "DOOR C\n\n42";

            correctDoor = 2;
        }


        // =====================================================
        // ROOM 5
        // =====================================================

        else if (roomNumber == 5)
        {
            timeLeft = 30f;

            canAnswer = true;

            playerMovement.SetMovement(true);

            ShowPuzzle(
                "<size=70%>" +
                "<color=#D6B85A>" +
                "FINAL ACCESS" +
                "</color>" +
                "</size>\n" +

                "<b>FINAL EXIT</b>\n\n" +

                "<size=82%>" +
                "ENTER THE 4-DIGIT CODE\n" +
                "COLLECTED FROM ROOMS 1-4" +
                "</size>"
            );

            doorAText.text =
                "CODE A\n\n3489";

            doorBText.text =
                "CODE B\n\n3498";

            doorCText.text =
                "CODE C\n\n3849";

            // 3 + 4 + 8 + 9 = 3489
            correctDoor = 1;
        }

        UpdateHUD();
    }


    // =========================================================
    // ROOM 2 - MEMORY CHALLENGE
    // =========================================================

    IEnumerator Room2MemoryChallenge()
    {
        playerMovement.SetMovement(false);

        memoryPhase = true;

        canAnswer = false;

        SetDoorsVisible(false);


        // -------------------------
        // MEMORIZE
        // -------------------------

        ShowPuzzle(
            "<size=70%>" +
            "<color=#D6B85A>" +
            "MEMORY TEST" +
            "</color>" +
            "</size>\n" +

            "<b>MEMORIZE THE SEQUENCE</b>\n\n" +

            "<size=135%>" +
            "▲   ●   ■   ★" +
            "</size>"
        );

        timerText.text =
            "MEMORIZE: 3";

        yield return
            new WaitForSeconds(1f);

        timerText.text =
            "MEMORIZE: 2";

        yield return
            new WaitForSeconds(1f);

        timerText.text =
            "MEMORIZE: 1";

        yield return
            new WaitForSeconds(1f);


        // -------------------------
        // QUESTION
        // -------------------------

        ShowPuzzle(
            "<size=70%>" +
            "<color=#D6B85A>" +
            "MEMORY TEST" +
            "</color>" +
            "</size>\n" +

            "<b>WHICH SYMBOL WAS THIRD?</b>"
        );

        doorAText.text =
            "DOOR A\n\n●";

        doorBText.text =
            "DOOR B\n\n■";

        doorCText.text =
            "DOOR C\n\n★";

        correctDoor = 2;

        timeLeft = 30f;

        SetDoorsVisible(true);

        memoryPhase = false;

        canAnswer = true;

        playerMovement.SetMovement(true);

        UpdateHUD();
    }


    // =========================================================
    // ROOM 3 - OBSERVATION CHALLENGE
    // =========================================================

    IEnumerator Room3ObservationChallenge()
    {
        playerMovement.SetMovement(false);

        memoryPhase = true;

        canAnswer = false;

        SetDoorsVisible(false);


        // -------------------------
        // OBSERVE ORIGINAL GRID
        // -------------------------

        ShowPuzzle(
            "<size=70%>" +
            "<color=#D6B85A>" +
            "OBSERVATION TEST" +
            "</color>" +
            "</size>\n" +

            "<b>OBSERVE THE GRID CAREFULLY</b>\n\n" +

            "<size=95%>" +
            "A1     B2     C3\n" +
            "D4     E5     F6\n" +
            "G7     H8     I9" +
            "</size>"
        );

        timerText.text =
            "OBSERVE: 4";

        yield return
            new WaitForSeconds(1f);

        timerText.text =
            "OBSERVE: 3";

        yield return
            new WaitForSeconds(1f);

        timerText.text =
            "OBSERVE: 2";

        yield return
            new WaitForSeconds(1f);

        timerText.text =
            "OBSERVE: 1";

        yield return
            new WaitForSeconds(1f);


        // -------------------------
        // CHANGED GRID
        // -------------------------

        ShowPuzzle(
            "<size=70%>" +
            "<color=#D6B85A>" +
            "OBSERVATION TEST" +
            "</color>" +
            "</size>\n" +

            "<b>ONE TILE WAS CHANGED</b>\n\n" +

            "<size=92%>" +
            "A1     B2     C3\n" +
            "D4     E5     F6\n" +
            "G7     H3     I9" +
            "</size>\n\n" +

            "<size=78%>" +
            "WHAT WAS THE ORIGINAL TILE?" +
            "</size>"
        );

        doorAText.text =
            "DOOR A\n\nH8";

        doorBText.text =
            "DOOR B\n\nF6";

        doorCText.text =
            "DOOR C\n\nC3";

        correctDoor = 1;

        timeLeft = 30f;

        SetDoorsVisible(true);

        memoryPhase = false;

        canAnswer = true;

        playerMovement.SetMovement(true);

        UpdateHUD();
    }


    // =========================================================
    // DOORS
    // =========================================================

    void SetDoorsVisible(bool visible)
    {
        doorAButton.gameObject.SetActive(visible);
        doorBButton.gameObject.SetActive(visible);
        doorCButton.gameObject.SetActive(visible);
    }


    // =========================================================
    // TIMER
    // =========================================================

    void ResetTimer()
    {
        timeLeft = 30f;
    }


    void TimeExpired()
    {
        lives--;

        ShowFeedback(
            "TIME EXPIRED",
            "-1 LIFE",
            ErrorFeedbackColor,
            true
        );

        if (lives <= 0)
        {
            StartCoroutine(
                GameOverAfterFeedback()
            );
        }
        else
        {
            ResetTimer();

            playerMovement.ResetPosition();
        }

        UpdateHUD();
    }


    // =========================================================
    // GAME OVER
    // =========================================================

    void GameOver()
    {
        gameEnded = true;

        canAnswer = false;

        playerMovement.SetMovement(false);

        SetDoorsVisible(false);

        HideFeedbackImmediate();

        gameOverPanel.SetActive(true);
    }


    // =========================================================
    // GAME CLEAR
    // =========================================================

    void GameClear()
    {
        gameEnded = true;

        canAnswer = false;

        playerMovement.SetMovement(false);

        SetDoorsVisible(false);

        HideFeedbackImmediate();

        finalScoreText.text =
            "FINAL SCORE: " +
            score;

        gameClearPanel.SetActive(true);

        timerText.text =
            "CLEARED";
    }


    // =========================================================
    // PAUSE
    // =========================================================

    public void PauseGame()
    {
        if (gameEnded)
            return;

        if (memoryPhase || !canAnswer)
        {
            ShowFeedback(
                "CHALLENGE ACTIVE",
                "PAUSE IS TEMPORARILY DISABLED",
                WarningFeedbackColor,
                false,
                0.75f
            );

            return;
        }

        isPaused = true;

        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }


    public void ResumeGame()
    {
        isPaused = false;

        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }


    // =========================================================
    // RETRY
    // =========================================================

    public void RetryGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            "GameScene"
        );
    }


    // =========================================================
    // MAIN MENU
    // =========================================================

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            "MainMenuScene"
        );
    }


    // =========================================================
    // HUD
    // =========================================================

    void UpdateHUD()
    {
        roomText.text =
            "ROOM " +
            currentRoom +
            "/5";

        livesText.text =
            "LIVES: " +
            lives;

        scoreText.text =
            "SCORE: " +
            score;

        if (!memoryPhase)
        {
            timerText.text =
                "TIME: " +
                Mathf.CeilToInt(timeLeft);
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("Loading Settings")]
    [SerializeField] private string sceneToLoad = "GameScene";
    [SerializeField] private float minimumLoadingTime = 1.5f;

    private Text loadingText;
    private Text percentText;
    private Image progressFill;

    private Font uiFont;

    void Start()
    {
        uiFont = Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );

        CreateLoadingScreen();

        StartCoroutine(LoadGame());
    }

    // =========================================================
    // CREATE ENTIRE LOADING SCREEN
    // =========================================================

    void CreateLoadingScreen()
    {
        // -----------------------------------------------------
        // CANVAS
        // -----------------------------------------------------

        GameObject canvasObject =
            new GameObject(
                "LoadingCanvas",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster)
            );

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.matchWidthOrHeight = 0.5f;


        // -----------------------------------------------------
        // BACKGROUND
        // -----------------------------------------------------

        GameObject background =
            CreateImage(
                "Background",
                canvasObject.transform,
                new Color32(
                    9,
                    14,
                    25,
                    255
                )
            );

        StretchFullScreen(
            background.GetComponent<RectTransform>()
        );


        // -----------------------------------------------------
        // SUBTLE CENTER PANEL
        // -----------------------------------------------------

        GameObject centerPanel =
            CreateImage(
                "CenterPanel",
                canvasObject.transform,
                new Color32(
                    15,
                    22,
                    36,
                    245
                )
            );

        RectTransform centerRect =
            centerPanel.GetComponent<RectTransform>();

        centerRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        centerRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        centerRect.pivot =
            new Vector2(0.5f, 0.5f);

        centerRect.sizeDelta =
            new Vector2(
                850f,
                390f
            );

        centerRect.anchoredPosition =
            new Vector2(
                0f,
                10f
            );


        // -----------------------------------------------------
        // GOLD TOP LINE
        // -----------------------------------------------------

        GameObject topLine =
            CreateImage(
                "GoldLine",
                centerPanel.transform,
                new Color32(
                    190,
                    150,
                    55,
                    255
                )
            );

        RectTransform lineRect =
            topLine.GetComponent<RectTransform>();

        lineRect.anchorMin =
            new Vector2(0.5f, 1f);

        lineRect.anchorMax =
            new Vector2(0.5f, 1f);

        lineRect.pivot =
            new Vector2(0.5f, 1f);

        lineRect.sizeDelta =
            new Vector2(
                420f,
                4f
            );

        lineRect.anchoredPosition =
            new Vector2(
                0f,
                -35f
            );


        // -----------------------------------------------------
        // TITLE
        // -----------------------------------------------------

        Text title =
            CreateText(
                "Title",
                centerPanel.transform,
                "FINAL EXIT",
                72,
                FontStyle.Bold,
                new Color32(
                    245,
                    240,
                    220,
                    255
                )
            );

        RectTransform titleRect =
            title.GetComponent<RectTransform>();

        titleRect.anchorMin =
            new Vector2(0.5f, 1f);

        titleRect.anchorMax =
            new Vector2(0.5f, 1f);

        titleRect.pivot =
            new Vector2(0.5f, 1f);

        titleRect.sizeDelta =
            new Vector2(
                700f,
                100f
            );

        titleRect.anchoredPosition =
            new Vector2(
                0f,
                -65f
            );


        // -----------------------------------------------------
        // SUBTITLE
        // -----------------------------------------------------

        Text subtitle =
            CreateText(
                "Subtitle",
                centerPanel.transform,
                "PREPARE TO FIND THE FINAL EXIT",
                22,
                FontStyle.Normal,
                new Color32(
                    165,
                    175,
                    190,
                    255
                )
            );

        RectTransform subtitleRect =
            subtitle.GetComponent<RectTransform>();

        subtitleRect.anchorMin =
            new Vector2(0.5f, 1f);

        subtitleRect.anchorMax =
            new Vector2(0.5f, 1f);

        subtitleRect.pivot =
            new Vector2(0.5f, 1f);

        subtitleRect.sizeDelta =
            new Vector2(
                700f,
                50f
            );

        subtitleRect.anchoredPosition =
            new Vector2(
                0f,
                -155f
            );


        // -----------------------------------------------------
        // LOADING TEXT
        // -----------------------------------------------------

        loadingText =
            CreateText(
                "LoadingText",
                centerPanel.transform,
                "LOADING",
                27,
                FontStyle.Bold,
                new Color32(
                    218,
                    183,
                    90,
                    255
                )
            );

        RectTransform loadingRect =
            loadingText.GetComponent<RectTransform>();

        loadingRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        loadingRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        loadingRect.pivot =
            new Vector2(0.5f, 0.5f);

        loadingRect.sizeDelta =
            new Vector2(
                500f,
                50f
            );

        loadingRect.anchoredPosition =
            new Vector2(
                0f,
                -20f
            );


        // -----------------------------------------------------
        // PROGRESS BAR BACKGROUND
        // -----------------------------------------------------

        GameObject progressBackground =
            CreateImage(
                "ProgressBackground",
                centerPanel.transform,
                new Color32(
                    35,
                    43,
                    57,
                    255
                )
            );

        RectTransform progressBackgroundRect =
            progressBackground
                .GetComponent<RectTransform>();

        progressBackgroundRect.anchorMin =
            new Vector2(0.5f, 0f);

        progressBackgroundRect.anchorMax =
            new Vector2(0.5f, 0f);

        progressBackgroundRect.pivot =
            new Vector2(0.5f, 0f);

        progressBackgroundRect.sizeDelta =
            new Vector2(
                600f,
                18f
            );

        progressBackgroundRect.anchoredPosition =
            new Vector2(
                0f,
                90f
            );


        // -----------------------------------------------------
        // PROGRESS FILL
        // -----------------------------------------------------

        GameObject fill =
            CreateImage(
                "ProgressFill",
                progressBackground.transform,
                new Color32(
                    203,
                    164,
                    65,
                    255
                )
            );

        progressFill =
            fill.GetComponent<Image>();

        RectTransform fillRect =
            fill.GetComponent<RectTransform>();

        fillRect.anchorMin =
            new Vector2(0f, 0f);

        fillRect.anchorMax =
            new Vector2(0f, 1f);

        fillRect.pivot =
            new Vector2(0f, 0.5f);

        fillRect.offsetMin =
            Vector2.zero;

        fillRect.offsetMax =
            Vector2.zero;

        fillRect.sizeDelta =
            new Vector2(
                0f,
                0f
            );


        // -----------------------------------------------------
        // PERCENT
        // -----------------------------------------------------

        percentText =
            CreateText(
                "PercentText",
                centerPanel.transform,
                "0%",
                20,
                FontStyle.Bold,
                new Color32(
                    220,
                    225,
                    230,
                    255
                )
            );

        RectTransform percentRect =
            percentText.GetComponent<RectTransform>();

        percentRect.anchorMin =
            new Vector2(0.5f, 0f);

        percentRect.anchorMax =
            new Vector2(0.5f, 0f);

        percentRect.pivot =
            new Vector2(0.5f, 0f);

        percentRect.sizeDelta =
            new Vector2(
                300f,
                40f
            );

        percentRect.anchoredPosition =
            new Vector2(
                0f,
                35f
            );
    }


    // =========================================================
    // LOAD GAME
    // =========================================================

    IEnumerator LoadGame()
    {
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(
                sceneToLoad
            );

        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.unscaledDeltaTime;

            float realProgress =
                Mathf.Clamp01(
                    operation.progress / 0.9f
                );

            float timeProgress =
                Mathf.Clamp01(
                    timer /
                    minimumLoadingTime
                );

            float displayedProgress =
                Mathf.Min(
                    realProgress,
                    timeProgress
                );

            UpdateLoadingUI(
                displayedProgress,
                timer
            );

            if (operation.progress >= 0.9f &&
                timer >= minimumLoadingTime)
            {
                UpdateLoadingUI(
                    1f,
                    timer
                );

                yield return
                    new WaitForSecondsRealtime(
                        0.2f
                    );

                operation.allowSceneActivation =
                    true;
            }

            yield return null;
        }
    }


    // =========================================================
    // UPDATE LOADING SCREEN
    // =========================================================

    void UpdateLoadingUI(
        float progress,
        float timer)
    {
        int percent =
            Mathf.RoundToInt(
                progress * 100f
            );

        percentText.text =
            percent + "%";

        int dots =
            Mathf.FloorToInt(
                timer * 2.5f
            ) % 4;

        loadingText.text =
            "LOADING" +
            new string('.', dots);

        RectTransform fillRect =
            progressFill.rectTransform;

        fillRect.anchorMax =
            new Vector2(
                progress,
                1f
            );
    }


    // =========================================================
    // HELPERS
    // =========================================================

    GameObject CreateImage(
        string objectName,
        Transform parent,
        Color color)
    {
        GameObject obj =
            new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

        obj.transform.SetParent(
            parent,
            false
        );

        Image image =
            obj.GetComponent<Image>();

        image.color = color;
        image.raycastTarget = false;

        return obj;
    }


    Text CreateText(
        string objectName,
        Transform parent,
        string textValue,
        int fontSize,
        FontStyle style,
        Color color)
    {
        GameObject obj =
            new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Text)
            );

        obj.transform.SetParent(
            parent,
            false
        );

        Text text =
            obj.GetComponent<Text>();

        text.font =
            uiFont;

        text.text =
            textValue;

        text.fontSize =
            fontSize;

        text.fontStyle =
            style;

        text.color =
            color;

        text.alignment =
            TextAnchor.MiddleCenter;

        text.raycastTarget =
            false;

        return text;
    }


    void StretchFullScreen(
        RectTransform rect)
    {
        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;
    }
}
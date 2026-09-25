using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleAnimation : MonoBehaviour
{
    [Header("Pixel Logo Settings")]
    [SerializeField] private Color mainColor =
        new Color32(242, 239, 220, 255);

    [SerializeField] private Color outlineColor =
        new Color32(20, 31, 40, 255);

    [SerializeField] private Color shadowColor =
        new Color32(8, 12, 18, 180);

    [SerializeField] private int pixelSize = 4;

    private TMP_Text originalText;
    private RawImage pixelLogo;
    private Texture2D titleTexture;

    // 5x7 PIXEL FONT
    private readonly Dictionary<char, string[]> letters =
        new Dictionary<char, string[]>()
    {
        {
            'F',
            new[]
            {
                "11111",
                "10000",
                "10000",
                "11110",
                "10000",
                "10000",
                "10000"
            }
        },

        {
            'I',
            new[]
            {
                "11111",
                "00100",
                "00100",
                "00100",
                "00100",
                "00100",
                "11111"
            }
        },

        {
            'N',
            new[]
            {
                "10001",
                "11001",
                "11001",
                "10101",
                "10011",
                "10011",
                "10001"
            }
        },

        {
            'A',
            new[]
            {
                "01110",
                "10001",
                "10001",
                "11111",
                "10001",
                "10001",
                "10001"
            }
        },

        {
            'L',
            new[]
            {
                "10000",
                "10000",
                "10000",
                "10000",
                "10000",
                "10000",
                "11111"
            }
        },

        {
            'E',
            new[]
            {
                "11111",
                "10000",
                "10000",
                "11110",
                "10000",
                "10000",
                "11111"
            }
        },

        {
            'X',
            new[]
            {
                "10001",
                "10001",
                "01010",
                "00100",
                "01010",
                "10001",
                "10001"
            }
        },

        {
            'T',
            new[]
            {
                "11111",
                "00100",
                "00100",
                "00100",
                "00100",
                "00100",
                "00100"
            }
        }
    };

    void Start()
    {
        originalText = GetComponent<TMP_Text>();

        // Hide normal TMP lettering
        if (originalText != null)
        {
            originalText.enabled = false;
        }

        CreatePixelTitle();
    }

    void CreatePixelTitle()
    {
        // Remove old generated title if it exists
        Transform oldLogo =
            transform.Find("PixelTitleLogo");

        if (oldLogo != null)
        {
            Destroy(oldLogo.gameObject);
        }

        GameObject logoObject =
            new GameObject(
                "PixelTitleLogo",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(RawImage)
            );

        logoObject.transform.SetParent(
            transform,
            false
        );

        RectTransform rect =
            logoObject.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        // Large enough for menu
        rect.sizeDelta =
            new Vector2(720f, 125f);

        rect.anchoredPosition =
            Vector2.zero;

        pixelLogo =
            logoObject.GetComponent<RawImage>();

        pixelLogo.raycastTarget = false;

        GenerateTexture();

        pixelLogo.texture =
            titleTexture;
    }

    void GenerateTexture()
    {
        string title = "FINAL EXIT";

        int characterWidth = 5;


        int spacing = 1;
        int wordSpacing = 3;

        int totalWidth = 0;

        foreach (char character in title)
        {
            if (character == ' ')
            {
                totalWidth += wordSpacing;
            }
            else
            {
                totalWidth +=
                    characterWidth + spacing;
            }
        }

        totalWidth += 4;

        int textureWidth =
            totalWidth * pixelSize;

        int textureHeight =
            12 * pixelSize;

        titleTexture =
            new Texture2D(
                textureWidth,
                textureHeight,
                TextureFormat.RGBA32,
                false
            );

        titleTexture.filterMode =
            FilterMode.Point;

        titleTexture.wrapMode =
            TextureWrapMode.Clamp;

        // Transparent background
        Color[] clearPixels =
            new Color[
                textureWidth *
                textureHeight
            ];

        for (int i = 0;
             i < clearPixels.Length;
             i++)
        {
            clearPixels[i] =
                Color.clear;
        }

        titleTexture.SetPixels(
            clearPixels
        );

        int cursorX = 2;

        int startY = 3;

        foreach (char character in title)
        {
            if (character == ' ')
            {
                cursorX += wordSpacing;

                continue;
            }

            if (!letters.ContainsKey(character))
                continue;

            string[] pattern =
                letters[character];

            // Shadow first
            DrawCharacter(
                pattern,
                cursorX + 1,
                startY - 1,
                shadowColor
            );

            // Pixel outline
            DrawCharacter(
                pattern,
                cursorX - 1,
                startY,
                outlineColor
            );

            DrawCharacter(
                pattern,
                cursorX + 1,
                startY,
                outlineColor
            );

            DrawCharacter(
                pattern,
                cursorX,
                startY - 1,
                outlineColor
            );

            DrawCharacter(
                pattern,
                cursorX,
                startY + 1,
                outlineColor
            );

            // Main lettering
            DrawCharacter(
                pattern,
                cursorX,
                startY,
                mainColor
            );

            cursorX +=
                characterWidth +
                spacing;
        }

        titleTexture.Apply();
    }

    void DrawCharacter(
        string[] pattern,
        int startX,
        int startY,
        Color color)
    {
        for (int row = 0;
             row < pattern.Length;
             row++)
        {
            for (int column = 0;
                 column < pattern[row].Length;
                 column++)
            {
                if (pattern[row][column]
                    != '1')
                {
                    continue;
                }

                int pixelX =
                    startX + column;

                int pixelY =
                    startY +
                    (pattern.Length - 1 - row);

                DrawPixelBlock(
                    pixelX,
                    pixelY,
                    color
                );
            }
        }
    }

    void DrawPixelBlock(
        int gridX,
        int gridY,
        Color color)
    {
        int startX =
            gridX * pixelSize;

        int startY =
            gridY * pixelSize;

        for (int x = 0;
             x < pixelSize;
             x++)
        {
            for (int y = 0;
                 y < pixelSize;
                 y++)
            {
                int finalX =
                    startX + x;

                int finalY =
                    startY + y;

                if (finalX < 0 ||
                    finalY < 0 ||
                    finalX >=
                    titleTexture.width ||
                    finalY >=
                    titleTexture.height)
                {
                    continue;
                }

                titleTexture.SetPixel(
                    finalX,
                    finalY,
                    color
                );
            }
        }
    }

    void OnDestroy()
    {
        if (titleTexture != null)
        {
            Destroy(titleTexture);
        }
    }
}
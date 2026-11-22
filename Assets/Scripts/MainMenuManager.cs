
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject howToPlayPanel;
    public GameObject playerProfilePanel;

    void Start()
    {
        howToPlayPanel.SetActive(false);

        bool hasProfile =
            PlayerProfileManager.Instance != null &&
            !string.IsNullOrEmpty(
                PlayerProfileManager.Instance.PlayerName
            );

        mainMenuPanel.SetActive(hasProfile);
        playerProfilePanel.SetActive(!hasProfile);
    }

    public void ShowMainMenu()
    {
        playerProfilePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenHowToPlay()
    {
        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }


public void ResetPlayerProfile()
{
    if (PlayerProfileManager.Instance == null)
    {
        Debug.LogError("Player Profile Manager not found.");
        return;
    }

    PlayerProfileManager.Instance.ResetProfile();

    mainMenuPanel.SetActive(false);
    howToPlayPanel.SetActive(false);
    playerProfilePanel.SetActive(true);

    PlayerProfileUI profileUI =
        playerProfilePanel.GetComponent<PlayerProfileUI>();

    if (profileUI != null)
    {
        profileUI.ClearProfileForm();
    }

    Debug.Log("Player profile has been reset.");
}


}

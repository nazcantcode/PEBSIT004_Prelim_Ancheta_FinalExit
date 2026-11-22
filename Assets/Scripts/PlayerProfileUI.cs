
using UnityEngine;
using TMPro;

public class PlayerProfileUI : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text profileMessageText;
    public MainMenuManager mainMenuManager;


public void ClearProfileForm()
{
    playerNameInput.text = "";
    profileMessageText.text = "";
    profileMessageText.color = Color.white;
}


    public void SaveProfile()
    {
        if (PlayerProfileManager.Instance == null)
        {
            profileMessageText.text =
                "Profile manager not found.";
            return;
        }

        string name = playerNameInput.text;

        if (PlayerProfileManager.Instance.SetPlayerName(name))
        {
            profileMessageText.color = Color.green;
            profileMessageText.text =
                "Profile saved successfully!";

            mainMenuManager.ShowMainMenu();
        }
        else
        {
            profileMessageText.color = Color.red;
            profileMessageText.text =
                "Enter 2-20 letters or numbers.";
        }
    }
}

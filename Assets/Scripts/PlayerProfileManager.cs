
using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager Instance;

    public string PlayerName { get; private set; }
    public int BestScore { get; private set; }
    public int HighestRoom { get; private set; }

    private const string NameKey = "FE_PlayerName";
    private const string ScoreKey = "FE_BestScore";
    private const string RoomKey = "FE_HighestRoom";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadProfile();
    }

    public bool SetPlayerName(string name)
    {
        name = name.Trim();

        if (name.Length < 2 || name.Length > 20)
            return false;

        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c) && c != ' ')
                return false;
        }

        PlayerName = name;
        PlayerPrefs.SetString(NameKey, PlayerName);
        PlayerPrefs.Save();
        return true;
    }

    public void SaveProgress(int score, int room)
    {
        BestScore = Mathf.Max(BestScore, score);
        HighestRoom = Mathf.Max(HighestRoom, room);

        PlayerPrefs.SetInt(ScoreKey, BestScore);
        PlayerPrefs.SetInt(RoomKey, HighestRoom);
        PlayerPrefs.Save();
    }

    public void LoadProfile()
    {
        PlayerName = PlayerPrefs.GetString(NameKey, "");
        BestScore = PlayerPrefs.GetInt(ScoreKey, 0);
        HighestRoom = PlayerPrefs.GetInt(RoomKey, 0);
    }

    public void ResetProfile()
    {
        PlayerPrefs.DeleteKey(NameKey);
        PlayerPrefs.DeleteKey(ScoreKey);
        PlayerPrefs.DeleteKey(RoomKey);
        PlayerPrefs.Save();

        LoadProfile();
    }
}

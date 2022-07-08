using TMPro;

public class UICont : CoreCont<UICont>
{
    private int currentPage;
    public TextMeshProUGUI TextLevelInGame;
    public TextMeshProUGUI TextLevelInGameOver;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }


    public void Init()
    {
        string level = (LevelCont.LevelIndex + 1).ToString();
        TextLevelInGame.SetText("LEVEL " + level);
    }

    public void ShowPage(int id)
    {
        HidePage(currentPage);
        currentPage = id;
        transform.GetChild(id).gameObject.SetActive(true);
    }

    public void ShowGameOver(int score)
    {
        string level = (LevelCont.LevelIndex + 1).ToString();
        TextLevelInGameOver.SetText(level);
        ShowPage(2);
    }


    public void HidePage(int id)
    {
        transform.GetChild(currentPage).gameObject.SetActive(false);
    }
}
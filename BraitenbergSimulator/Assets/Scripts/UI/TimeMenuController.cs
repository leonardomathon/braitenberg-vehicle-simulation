using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeMenuController : MonoBehaviour
{
    [SerializeField] private GameObject timeMenu;

    [SerializeField] private GameObject timeMenuControls;

    [SerializeField] private Sprite pauseImage;

    [SerializeField] private Sprite playImage;

    private TextMeshProUGUI currentSpeedTag;

    private Button[] timeButtons = new Button[3];

    private TimeManager timeManager;

    private float gameSpeed = 1;

    void Start()
    {
        timeManager = TimeManager.Instance;

        // Get used components
        currentSpeedTag = timeMenu.transform.Find("CurrentSpeedTag").GetComponent<TextMeshProUGUI>();

        // Setup time buttons
        SetupTimeButtons();
    }

    void Update()
    {
        float _gameSpeed = timeManager.GetCurrentGameSpeed();

        // Check if gamespeed has changed
        if (gameSpeed != _gameSpeed)
        {
            if (gameSpeed == 0 && _gameSpeed != 0)
            {
                ChangePauseButton(false);
            }

            gameSpeed = _gameSpeed;

            // Check if paused
            if (gameSpeed == 0)
            {
                currentSpeedTag.SetText("Currently paused");
                ChangePauseButton(true);
            }
            else
            {
                currentSpeedTag.SetText("Current speed: " + gameSpeed.ToString());
            }
        }
    }

    private void SetupTimeButtons()
    {
        // Get the grid in which the buttons are situated
        GameObject grid = timeMenuControls.transform.Find("Grid").gameObject;

        // Get all buttons
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            timeButtons[i] = grid.transform.GetChild(i).GetComponent<Button>();
        }

        // Setup listeners
        timeButtons[0].onClick.AddListener(ClickOnSpeedDownButton);
        timeButtons[1].onClick.AddListener(ClickOnPauseButton);
        timeButtons[2].onClick.AddListener(ClickOnSpeedUpButton);
    }

    private void ChangePauseButton(bool paused)
    {
        Image pauseButtonImage = timeButtons[1].transform.GetChild(0).GetComponent<Image>();

        if (paused)
        {
            timeButtons[1].GetComponent<Image>().color = new Color32(147, 147, 147, 255);
            pauseButtonImage.sprite = playImage;
        }


        if (!paused)
        {
            timeButtons[1].GetComponent<Image>().color = new Color32(84, 84, 84, 255);
            pauseButtonImage.sprite = pauseImage;
        }

    }

    private void ClickOnSpeedDownButton()
    {
        timeManager.GameSpeedDown();
    }

    private void ClickOnPauseButton()
    {
        timeManager.TogglePause();
    }

    private void ClickOnSpeedUpButton()
    {
        timeManager.GameSpeedUp();
    }
}

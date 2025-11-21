using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("ButtonsRefs")]
    public Button nextLvlBtn; 
    public Button mainMenuBtn; 
    public Button mainMenuBtnFromEndScreen; 
    public Button retryBtn; 
    public Button StartGameBtn; 
    public Button settingsBtn; 
    public Button quitBtn; 

    [Header("Top Counter")]
    public TextMeshProUGUI foodText;

    [Header("Level Complete Panel")]
    [SerializeField] GameObject LevelCompletepanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI timerText;
    public Image[] stars; // 3 star images

    public Sprite starOn;
    public Sprite starOff;

    

    [Header("otherRefds")]
    public GameObject endGamePanel;
    [SerializeField] private GameObject mainMenuPanel;
    public TutorialPanel tutorialPanel;


    private void Awake()
    {
        if (nextLvlBtn) nextLvlBtn.onClick.AddListener(() => GameManager.Instance.NextLevel());
        if (mainMenuBtn) mainMenuBtn.onClick.AddListener(() => GameManager.Instance.LoadDesiredScene(0));
        if (mainMenuBtnFromEndScreen) mainMenuBtnFromEndScreen.onClick.AddListener(() => GameManager.Instance.LoadDesiredScene(0));
        if (retryBtn)  retryBtn.onClick.AddListener(() => GameManager.Instance.Retry());

        if (StartGameBtn) StartGameBtn.onClick.AddListener(StartGame);
        if (settingsBtn) settingsBtn.onClick.AddListener(ShowSettingsPanel);
        if (quitBtn) quitBtn.onClick.AddListener(() => GameManager.Instance.Quit());
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) { mainMenuPanel.gameObject.SetActive(true); endGamePanel.SetActive(false); }
        else mainMenuPanel.gameObject.SetActive(false);
    }
    private void StartGame()
    {
        mainMenuPanel.gameObject.SetActive(false);
        GameManager.Instance.LoadDesiredScene(1);
    }

    private void ShowSettingsPanel()
    {
        //EnableSettingsPanel
    }
    public void UpdateFoodUI(int current, int total)
    {
        foodText.text = current + "/" + total;
    }

    public void ShowLevelComplete(int current, int total, float time)
    {
        LevelCompletepanel.SetActive(true);

        finalScoreText.text = "Collected: " + current + "/" + total;

       // timerText.text = "Time: " + time.ToString("0.0") + "s";

        SetStars(current, total);
    }

    void SetStars(int current, int total)
    {
        int starCount = 0;

        if (current >= total * 0.33f) starCount = 1;
        if (current >= total * 0.66f) starCount = 2;
        if (current >= total) starCount = 3;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = (i < starCount) ? starOn : starOff;
        }
    }


    internal void ShowEndGamePanel()
    {
        LevelCompletepanel.SetActive(false);
        endGamePanel.SetActive(true);
    }
}

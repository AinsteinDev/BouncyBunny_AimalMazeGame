using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Food Tracking")]
    public int totalFood;
    public int collectedFood;

    [Header("UI")]
    [HideInInspector]public UIManager ui;

    [Header("Timer")]
    private float timer;
    private bool timerRunning;


    private AudioSource audioSource;
    [SerializeField] private AudioClip bgMusicClip;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;      // Disable vsync
        Application.targetFrameRate = 60;    // Cap FPS



        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgMusicClip;
        audioSource.Play();

        ui = FindFirstObjectByType<UIManager>();

        if (SceneManager.GetActiveScene().buildIndex == 0) { return; }
        timerRunning = true;
        timer = 0f;
        totalFood = GetTotalFood();
        ui.UpdateFoodUI(collectedFood, totalFood);
    }

    private int GetTotalFood()
    {
        PickableFood[] foodInThisLevel = FindObjectsByType<PickableFood>(FindObjectsSortMode.None);
        return foodInThisLevel.Length;
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }
    }

    public void CollectFood(int val)
    {
        collectedFood += val;
        ui.UpdateFoodUI(collectedFood, totalFood);
    }

    public void CompleteLevel()
    {
        timerRunning = false;
        ui.ShowLevelComplete(collectedFood, totalFood, timer);

        var bushmonster = FindFirstObjectByType<BushMonster>();
        if (bushmonster != null)
        {
            bushmonster.StopMonster();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next >= SceneManager.sceneCountInBuildSettings)
        {
            ui.ShowEndGamePanel();
            return;
        }

        SceneManager.LoadScene(next);
        SceneManager.LoadScene(next);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void LoadDesiredScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        //if (buildIndex == 1) { ui.tutorialPanel.gameObject.SetActive(true); ui.tutorialPanel.ShowTutForLvl1(); }
       // if (buildIndex == 2) { ui.tutorialPanel.gameObject.SetActive(true); ui.tutorialPanel.ShowTutForLvl2(); }
    }
}

using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Spawner References")]
    public LevelGenerator levelGenerator;
    public CollectibleSpawner collectibleSpawner;
    public EnemySpawner enemySpawner;
    public UICollect uiCollect;

    [Header("UI References")]
    public GameObject popupPanel;
    public TMP_Text stageText;

    [Header("Progress")]
    public int currentLevel = 1;
    public int extraCollectiblePerLevel = 2;

    [HideInInspector] public int totalJob;
    [HideInInspector] public int totalCoffee;
    [HideInInspector] public int collectedJob;
    [HideInInspector] public int collectedCoffee;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterCollectibleTargets(int jobTarget, int coffeeTarget)
    {
        totalJob = jobTarget;
        totalCoffee = coffeeTarget;
        collectedJob = 0;
        collectedCoffee = 0;

        if (popupPanel != null) popupPanel.SetActive(false);

        Time.timeScale = 1f;
        uiCollect.UpdateUI();
    }

    public void AddJob()
    {
        collectedJob++;
        CheckCompletion();
    }

    public void AddCoffee()
    {
        collectedCoffee++;
        CheckCompletion();
    }

    void CheckCompletion()
    {
        if (collectedJob >= totalJob && collectedCoffee >= totalCoffee)
        {
            if (popupPanel != null && stageText != null)
            {
                popupPanel.SetActive(true);
                stageText.text = "Stage " + currentLevel;
                Debug.Log("pop up jalan");
            }

            Time.timeScale = 0f;
        }
    }

    public void NextStage()
    {
        Time.timeScale = 1f;
        currentLevel++;
        Debug.Log("Naik ke stage " + currentLevel);

        if (enemySpawner != null)
            enemySpawner.ClearAllEnemies();
        if (collectibleSpawner != null)
            collectibleSpawner.ClearAllCollectibles();

        int baseJob = 5;
        int baseCoffee = 5;
        int jobTarget = baseJob + ((currentLevel - 1) * extraCollectiblePerLevel);
        int coffeeTarget = baseCoffee + ((currentLevel - 1) * extraCollectiblePerLevel);

        RegisterCollectibleTargets(jobTarget, coffeeTarget);

        if (levelGenerator != null)
        {
            levelGenerator.ResetLevel();
            levelGenerator.GenerateNewStage();
        }

        if (popupPanel != null) popupPanel.SetActive(false);
    }







}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public Canvas popupCanvas;
    public GameObject popupPanel;
    public GameObject nextButton;

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

        if (popupCanvas != null) popupCanvas.enabled = false;
        if (popupPanel != null) popupPanel.SetActive(false);
        if (nextButton != null) nextButton.SetActive(false);

        Time.timeScale = 1f;
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
            if (popupCanvas != null) popupCanvas.enabled = true;
            if (popupPanel != null) {
                popupPanel.SetActive(true);
                Debug.Log("pop up jalan");
            } 
            if (nextButton != null)
            {
                nextButton.SetActive(true);
                Debug.Log("next button jalan");
            }

            Time.timeScale = 0f;
        }
    }

    public void NextStage()
    {
        Time.timeScale = 1f;
        currentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

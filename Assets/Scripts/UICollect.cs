using TMPro;
using UnityEngine;

public class UICollect : MonoBehaviour
{
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI coffeeText;

    void Start()
    {
        int jobTarget = 5 + (GameManager.instance.currentLevel - 1) * GameManager.instance.extraCollectiblePerLevel;
        int coffeeTarget = 5 + (GameManager.instance.currentLevel - 1) * GameManager.instance.extraCollectiblePerLevel;

        GameManager.instance.RegisterCollectibleTargets(jobTarget, coffeeTarget);
        UpdateUI();
    }

    public void AddJob()
    {
        GameManager.instance.AddJob();
        UpdateUI();
    }

    public void AddCoffee()
    {
        GameManager.instance.AddCoffee();
        UpdateUI();
    }

    void UpdateUI()
    {
        jobText.text = $"{GameManager.instance.collectedJob}/{GameManager.instance.totalJob}";
        coffeeText.text = $"{GameManager.instance.collectedCoffee}/{GameManager.instance.totalCoffee}";
    }
}

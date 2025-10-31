using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICollect : MonoBehaviour
{
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI coffeeText;

    private int jobCount = 0;
    private int coffeeCount = 0;
    private int maxJob = 5;
    private int maxCoffee = 5;


    private void Start()
    {
        jobText.text = $"{jobCount}/{maxJob}";
        coffeeText.text = $"{coffeeCount}/{maxCoffee}";
    }
    public void AddJob()
    {
        jobCount++;
        UpdateUI();
    }

    public void AddCoffee()
    {
        coffeeCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        jobText.text = $"{jobCount}/{maxJob}";
        coffeeText.text = $"{coffeeCount}/{maxCoffee}";
        if ( jobCount >= maxJob && coffeeCount >= maxCoffee)
        {
            print("Lets go! you completed level 1! go find a door to go to the next level");
        }
    }
}

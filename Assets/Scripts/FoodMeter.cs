using UnityEngine;
using UnityEngine.UI;

public class FoodMeter : MonoBehaviour
{
    [Header("Food Settings")]
    public int maxFood = 100;
    public int currentFood;

    [Header("UI")]
    public Slider foodSlider;
    public Text foodText;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Start()
    {
        currentFood = maxFood;
        UpdateUI();
        Debug.Log("Food Meter: " + currentFood + "/" + maxFood);
    }

    public void LoseFood(int amount)
    {
        if (isGameOver) return;

        currentFood -= amount;
        currentFood = Mathf.Max(0, currentFood);
        UpdateUI();

        Debug.Log("Food lost! Remaining: " + currentFood);

        if (currentFood <= 0)
        {
            GameOver();
        }
    }

    public void GainFood(int amount)
    {
        if (isGameOver) return;

        currentFood += amount;
        currentFood = Mathf.Min(maxFood, currentFood);
        UpdateUI();

        Debug.Log("Food gained! Now: " + currentFood);
    }

    void UpdateUI()
    {
        if (foodSlider != null)
        {
            foodSlider.value = (float)currentFood / maxFood;
        }

        if (foodText != null)
        {
            foodText.text = "FOOD: " + currentFood;
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER! No food left!");
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}

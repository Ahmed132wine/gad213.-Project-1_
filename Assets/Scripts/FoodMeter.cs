using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FoodMeter : MonoBehaviour
{
    [Header("Food Settings")]
    public float maxFood = 300f;
    public float currentFood;

    [Header("UI References")]
    public Slider foodSlider;
    public TextMeshProUGUI foodText;
    public GameObject gameOverPanel;

    [Header("Flash Effect")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private bool isGameOver = false;
    private Color originalTextColor;

    void Start()
    {
        currentFood = maxFood;
        if (foodText != null)
            originalTextColor = foodText.color;
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void LoseFood(float amount)
    {
        if (isGameOver) return;

        currentFood -= amount;
        currentFood = Mathf.Clamp(currentFood, 0, maxFood);
        UpdateUI();

        // Flash red on text
        if (foodText != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(FlashText());
        }

        if (currentFood <= 0)
        {
            TriggerGameOver();
        }
    }

    IEnumerator FlashText()
    {
        foodText.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        foodText.color = originalTextColor;
    }

    void UpdateUI()
    {
        if (foodSlider != null) foodSlider.value = currentFood / maxFood;
        if (foodText != null) foodText.text = " " + Mathf.CeilToInt(currentFood).ToString();
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        // Stop player movement
        GetComponent<LaneMovement3D>().forwardSpeed = 0;
        
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null) ui.GameOver();
    }
}

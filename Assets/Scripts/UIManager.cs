using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // if using TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI deliveredText; // Drag the DeliveredText here

    [Header("Game References")]
    public FoodMeter foodMeter; // Drag Player's FoodMeter
    public Transform winTrigger; // Drag your win trigger object

    private bool isGameOver = false;
    private bool isWin = false;
    private int startFood;

    void Start()
    {
        // Store starting food (max)
        if (foodMeter != null) startFood = (int)foodMeter.maxFood;

        // Hide all panels
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver || isWin) return;

        // Escape for pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = Time.timeScale == 0f;
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        // Game over check (food <= 0)
        if (foodMeter != null && foodMeter.currentFood <= 0 && !isGameOver)
        {
            GameOver();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        // Calculate delivered packages = maxFood - currentFood (since currentFood is 0 at game over, but safe)
        int remaining = (int)foodMeter.currentFood;
        deliveredText.text = "Packages remaining: " + remaining;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void WinGame()
    {
        if (isWin || isGameOver) return;
        isWin = true;
        Time.timeScale = 0f;

        // All packages delivered = maxFood
        if (deliveredText != null && winPanel != null)
        {
            // You could also show delivered count on win panel if you add a text there
            TextMeshProUGUI winDelivered = winPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (winDelivered != null) winDelivered.text = "Packages delivered: " + startFood;
        }
        if (winPanel != null) winPanel.SetActive(true);
    }

    // --- Button functions ---
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            LoadMainMenu(); // no next level, back to menu
    }
}

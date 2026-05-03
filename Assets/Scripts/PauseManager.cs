using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuPanel;
    public CanvasGroup pauseCanvasGroup;

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);  
            SetPauseMenuVisibility(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public static void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        // Find the pause manager and update UI
        PauseManager pm = FindObjectOfType<PauseManager>();
        if (pm != null)
        {
            pm.SetPauseMenuVisibility(isPaused);
        }
    }

    void SetPauseMenuVisibility(bool visible)
    {
        if (pauseMenuPanel == null) return;

        
        CanvasGroup cg = pauseMenuPanel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = pauseMenuPanel.AddComponent<CanvasGroup>();
        }

        cg.alpha = visible ? 1f : 0f;
        cg.interactable = visible;
        cg.blocksRaycasts = visible;
    }

    // Called by Resume Button
    public void ResumeGame()
    {
        if (isPaused)
        {
            TogglePause();
        }
    }

    // Called by Main Menu Button
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    // Called by Quit Button
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

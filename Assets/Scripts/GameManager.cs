using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Public static reference
    public static GameManager instance;

    // Player reference
    public PlayerController playerController;
    public PlayerStatSO currPlayerStat;
    public PlayerStatSO lastSavedPlayerStat;

    // Reference to UI text

    [Space(5)]
    public TMP_Text statText;
    public TMP_Text levelText;

    // Reference to the UI Panels
    [Space(5)]
    public GameObject pauseMenu;
    public GameObject gameoverMenu;

    // Next level
    public string nextLevelName;

    // Start is called before the first frame update
    private void Start()
    {
        // assign self to instance and destroy any duplicate
        if (instance != null)
            Destroy(instance);
        instance = this;

        levelText.text = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Updates the ui showing the current stat of the player
    /// </summary>
    public void UpdateStatText()
    {
        // set text to show current shot type
        if (currPlayerStat.currShotType == PlayerController.ShotType.SPREAD)
            statText.text = "Spread";
        else if (currPlayerStat.currShotType == PlayerController.ShotType.LASER)
            statText.text = "Laser";
        else
            statText.text = "Basic";

        // update the rest of the text according to player stat
        statText.text += "\n" + currPlayerStat.currPower + 
            "\n" + currPlayerStat.currLives + 
            "\n" + currPlayerStat.currScore;
    }

    /// <summary>
    /// Adds value to the current player score
    /// </summary>
    /// <param name="val">integer value to add</param>
    public void AddScore(int val)
    {
        // add score and update the ui
        currPlayerStat.currScore += val;
        UpdateStatText();
    }

    /// <summary>
    /// Revert progress and restart level
    /// </summary>
    public void LevelRestart()
    {
        // Revert player stats to the previous state
        currPlayerStat.SetStat(lastSavedPlayerStat);

        // Reset timescale to 1 and reloads current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Save progress and load next level
    /// </summary>
    public void LevelCleared()
    {
        // Save the stat of the player at the end of the level
        ++currPlayerStat.currLevel;
        lastSavedPlayerStat.SetStat(currPlayerStat);  

        // Load the next level
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelName);
    }

    /// <summary>
    /// Return to the main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        // Change scene to the main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    private void Update()
    {
        // Toggle the pause menu on/off on escape key press
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1f)
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
            }
            else
            {
                ResumeGame();
            }
        }
    }

    /// <summary>
    /// Resumes the game and closes the pause menu. Also for resume button in pause menu
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }


    /// <summary>
    /// Sets the game timescale to 0 and show the game over menu
    /// </summary>
    public void SetGameOver()
    {
        // Pause timescale
        Time.timeScale = 0;

        // Open the gameover menu
        gameoverMenu.SetActive(true);
    }
}

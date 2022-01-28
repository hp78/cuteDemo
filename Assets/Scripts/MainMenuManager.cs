using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // reference to player stat SO
    public PlayerStatSO savedPlayerStat;
    public PlayerStatSO currPlayerStat;

    // reference to continue button
    public Button continueBtn;

    // multiplier for enemy health depending on difficulty
    float difficultyModifier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // set the continue active if the player has previously played
        continueBtn.interactable = (savedPlayerStat.currLevel > 0);
    }

    /// <summary>
    /// Sets the difficulty modifier for the next new game. Enemy health is multiplied by this modifier
    /// </summary>
    /// <param name="modVal">the value of the modifier</param>
    public void SetDifficultyMod(float modVal)
    {
        difficultyModifier = modVal;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ContinueGame()
    {
        currPlayerStat.SetStat(savedPlayerStat);
        string sceneName = "Level" + currPlayerStat.currLevel;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Start a game from the beginning
    /// </summary>
    public void StartNewGame()
    {
        currPlayerStat.difficultyModifier = difficultyModifier;
        currPlayerStat.ResetStat();
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    ///  Quits the game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}

////
// Description : Simple manager to handle TMP text and button function
////

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenManager : MonoBehaviour
{
    public TMP_Text scoreTmp;
    public PlayerStatSO savedPlayerStat;

    // Start is called before the first frame update
    private void Start()
    {
        scoreTmp.text = "High Score : " + savedPlayerStat.currScore;
    }

    // Function for return to main menu button
    public void ReturnToMainMenu()
    {
        // Set current level to -1 so continue button doesn't work in main menu
        savedPlayerStat.currLevel = -1;

        // Change scene to the main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

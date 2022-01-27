using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // public static reference
    public static GameManager instance;

    // player reference
    public PlayerController playerController;
    public PlayerStatSO currPlayerStat;
    public PlayerStatSO lastSavedPlayerStat;

    // reference to right side of stat text
    public TMP_Text statText;

    // Start is called before the first frame update
    private void Start()
    {
        // assign self to instance and destroy any duplicate
        if (instance != null)
            Destroy(instance);
        instance = this;
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
}

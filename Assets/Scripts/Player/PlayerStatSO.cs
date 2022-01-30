////
// Description : Data storage for player stats. 
//                  Keeps track of current level, power, lives, score and difficulty.
//                  Also keeps track of previous in-progress game.
////

using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObject/PlayerStat", order = 1)]
[System.Serializable]
public class PlayerStatSO : ScriptableObject
{
    public PlayerController.ShotType currShotType;
    public int currPower;
    public int currLives;
    public int currScore;
    public int currLevel;
    public float difficultyModifier;

    /// <summary>
    /// Resets the player stats
    /// </summary>
    public void ResetStat()
    {
        currShotType = PlayerController.ShotType.BASIC;
        currPower = 0;
        currLives = 3;
        currScore = 0;
        currLevel = 0;
    }

    /// <summary>
    /// Set the player state to be the same as the referenced stat
    /// </summary>
    /// <param name="targetStat">the player stat used for reference</param>
    public void SetStat(PlayerStatSO targetStat)
    {
        currShotType = targetStat.currShotType;
        currPower = targetStat.currPower;
        currLives = targetStat.currLives;
        currScore = targetStat.currScore;
        currLevel = targetStat.currLevel;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //
    public static GameManager instance;
    public PlayerController playerController;
    public TMP_Text statText;


    private void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    public void UpdateStatText(int lives, int power, PlayerController.ShotType shotType)
    {
        statText.text = "" + lives + "\n" + power + "\n";

        if (shotType == PlayerController.ShotType.BASIC)
            statText.text += "Basic";

        if (shotType == PlayerController.ShotType.SPREAD)
            statText.text += "Spread";

        if (shotType == PlayerController.ShotType.LASER)
            statText.text += "Laser";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SavePlayerData
{
    public static SavepPlayerData instance = new SavepPlayerData();
    public static SavepPlayerData Instance => instance;

    public int CurrentPlayer = 0;



    public int finalPlayer
    {
        get
        {
            return PlayerPrefs.GetInt("selectedPlayer", 0);
        }
        set
        {
            PlayerPrefs.SetInt("selectedPlayer", value);
        }
    }
}

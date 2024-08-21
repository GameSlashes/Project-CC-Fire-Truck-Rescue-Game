using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SavepPlayerData
{
    public static SavepPlayerData instance = new SavepPlayerData();
    public static SavepPlayerData Instance => instance;

    public int CurrentPlayer = 0;



    public int finalPlayer
    {
        get
        {
            return PlayerPrefs.GetInt("selectedCharacter", 0);
        }
        set
        {
            PlayerPrefs.SetInt("selectedCharacter", value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{

    private static float volumeLevel;
    public static float VolumeLevel
    {
        get
        {
            return PlayerPrefs.GetFloat(volumeKey);

        }

        set
        {
            volumeLevel = value;
            PlayerPrefs.SetFloat(volumeKey, volumeLevel);
        }
    }

    public static string volumeKey;
    public static string fullScreenKey;

}

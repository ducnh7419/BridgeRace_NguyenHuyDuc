using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static int CurrentLevel=1;
    public static void SaveGame(){
        CurrentLevel++;
        PlayerPrefs.SetInt("level", CurrentLevel);
    }

    public static int LoadGame(int level){
        CurrentLevel=level;
        PlayerPrefs.SetInt("level", CurrentLevel);
        return CurrentLevel;
    }

    public static int LoadGame(){
        return PlayerPrefs.GetInt("level");
    }
}

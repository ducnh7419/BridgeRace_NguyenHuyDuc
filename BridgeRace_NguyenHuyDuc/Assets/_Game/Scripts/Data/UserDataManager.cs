using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public  int CurrentLevel=1;
    public  void SaveGame(){
        CurrentLevel++;
        PlayerPrefs.SetInt("level", CurrentLevel);
    }

    public  void LoadGame(int level){
        CurrentLevel=level;
        PlayerPrefs.SetInt("level", CurrentLevel);
    }

    public  int LoadGame(){
        return PlayerPrefs.GetInt("level");
    }
}

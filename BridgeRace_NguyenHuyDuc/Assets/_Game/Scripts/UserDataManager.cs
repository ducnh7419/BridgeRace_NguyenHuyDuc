using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Ins { get; private set; }
    public int CurrentLevel=1;
    public int Score;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public  void SaveGame(){
        CurrentLevel++;
        PlayerPrefs.SetInt("level", CurrentLevel);
    }

    public  void LoadGame(int level){
        CurrentLevel=level;
        PlayerPrefs.SetInt("level", CurrentLevel);
    }

    public  void LoadGame(){
        CurrentLevel= PlayerPrefs.GetInt("level");
    }
}

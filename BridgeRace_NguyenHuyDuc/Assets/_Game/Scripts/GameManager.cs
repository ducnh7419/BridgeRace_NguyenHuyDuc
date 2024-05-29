using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager ins;
    public static GameManager Ins=>ins;
    
    public enum State{
        None=0,
        LevelSelection=1,
        MainMenu=2,
        StartGame=3,
        OngoingGame=4,
        EndGame=5
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (ins != null && ins != this)
        {
            Destroy(this);
        }
        else
        {
            ins = this;
        }
        OnStartGame();
        
    }

    public void ChangeState(State state){
        switch (state)
        {
            case State.None:
                break;
            case State.LevelSelection:
                break;
            case State.MainMenu:
                break;
            case State.StartGame:
                OnStartGame();
                break;
            case State.OngoingGame:
                break;
            case State.EndGame:
                break;
        }
    }

    private void OnStartGame(){
        LevelManager.Ins.GenerateLevel();
        
    }

}

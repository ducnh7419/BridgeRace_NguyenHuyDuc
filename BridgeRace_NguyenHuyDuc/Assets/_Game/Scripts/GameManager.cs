using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager ins;
    private State currState;

    private GameResult currentResult;

    public static GameManager Ins=>ins;

    public GameResult CurrentResult { get => currentResult; set => currentResult = value; }
    public State CurrState { get => currState; }

    public enum State{
        None=0,
        StartScreen=1,
        MainMenu=2,
        LevelSelection=3, 
        StartGame=4,
        OngoingGame=5,
        EndGame=6
    }

    public enum GameResult{
        None=0,
        Win=1,
        Lose=2
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
        
        OnInit();
        
    }

    public void ChangeState(State state){
        currState=state;
        switch (state)
        {
            case State.None:
                break;
            case State.StartScreen:
                OnStartScreen();
                break;
            case State.MainMenu:
                OnMainMenu();
                break;
            case State.LevelSelection:
                OnLevelSelectionMenu();
                break;
            case State.StartGame:
                OnStartGame();
                break;
            case State.OngoingGame:
                OnGoingGame();
                break;
            case State.EndGame:
                OnEndGame();
                break;
        }
    }

    private void OnLevelSelectionMenu()
    {
        UIManager.Ins.OpenUI<LevelSelection>();
    }

    private void OnMainMenu()
    {
        UIManager.Ins.OpenUI<MainMenu>();
    }

    private void OnStartScreen(){
        UIManager.Ins.OpenUI<OpeningScreen>();

    }

    private void OnGoingGame(){
        UIManager.Ins.OpenUI<IngameUI>();
    }

    private void OnStartGame(){
        Time.timeScale=1;
        LevelManager.Ins.GenerateLevel();
        GameManager.Ins.ChangeState(State.OngoingGame);
        
    }

    IEnumerator DelayShowingEndGameCanvas(){
        yield return new WaitForSeconds(7);
        Time.timeScale=0;
        if(currentResult==GameResult.Win){
            UIManager.Ins.OpenUI<Win>();
        }else{
            UIManager.Ins.OpenUI<Lose>();
        }
    }

    private void OnEndGame(){
        StartCoroutine(DelayShowingEndGameCanvas());

    }

    protected void OnInit()
    {
        //base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
        //csv.OnInit();
        //userData?.OnInitData();

        //ChangeState(GameState.MainMenu);

        OnStartScreen();
    }

    public void GoBackward()
    {
        Debug.Log(CurrState);
        currState--;
        ChangeState(CurrState);
    }
}

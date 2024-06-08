using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    public Text score;

    [SerializeField]private Button retryButton;
    [SerializeField]private Button mainMenuButton;

    private void Start() {
        retryButton.onClick.AddListener(RetryButtonOnClicked);
        mainMenuButton.onClick.AddListener(MainMenuButtonOnClicked);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            LevelManager.Ins.IncreseLevel();
            LevelManager.Ins.DestroyLevel();
            GameManager.Ins.ChangeState(GameManager.State.StartGame);
            Close(0);
        }
        
    }


    public void MainMenuButtonOnClicked()
    {
        GameManager.Ins.ChangeState(GameManager.State.MainMenu);
        Close(0);
    }

    public void RetryButtonOnClicked()
    {
        LevelManager.Ins.DestroyLevel();
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        Close(0);
    }
}

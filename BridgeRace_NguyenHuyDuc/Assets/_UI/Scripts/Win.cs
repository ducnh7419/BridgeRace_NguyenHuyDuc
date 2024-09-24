using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    public Text score;

    [SerializeField]private Button retryButton;
    [SerializeField]private Button nextLevelButton;
    [SerializeField]private Button mainMenuButton;

    private void Start() {
        retryButton.onClick.AddListener(RetryButtonOnClicked);
        mainMenuButton.onClick.AddListener(MainMenuButtonOnClicked);
        nextLevelButton.onClick.AddListener(NextLevelButtonOnClicked);
    }

    private void OnEnable(){
        score.text=UserDataManager.Ins.Score.ToString();
    }

    public void MainMenuButtonOnClicked()
    {
        LevelManager.Ins.DestroyLevel();
        GameManager.Ins.ChangeState(GameManager.State.MainMenu);
        CameraFollow.Ins.ResetCam();
        Close(0);
    }

    public void NextLevelButtonOnClicked(){
        UserDataManager.Ins.SaveGame();
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        CameraFollow.Ins.ResetCam();
        Close(0);
    }

    public void RetryButtonOnClicked()
    {
        GameManager.Ins.ChangeState(GameManager.State.StartGame);
        CameraFollow.Ins.ResetCam();
        Close(0);
    }
}
